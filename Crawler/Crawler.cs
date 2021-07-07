using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Octokit;
using YamlDotNet.Serialization;

namespace Crawler
{
    public class Crawler : BackgroundService
    {
        private readonly string _metaDataFileName;
        private readonly bool _yamlMode = true;
        private readonly List<CrawlerRepositoryResult> _crawlerRepositoryResults = new List<CrawlerRepositoryResult>();
        private GitHubClient Client { get; set; }
        private readonly Config _config;

        public Crawler(Config config)
        {
            _config = config;
            _metaDataFileName = _config.Self.MetaDataFileName;
            _yamlMode = _config.Self.YamlMode;
            PrepareGithubClient();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                await StartCrawler();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
        }

        private void PrepareGithubClient()
        {
            Client = new GitHubClient(new ProductHeaderValue("Github-Portal-Crawler"))
            {
                Credentials = new Credentials(_config.Self.GithubToken)
            };
        }


        /// <summary>
        ///     Initiates the crawler
        /// </summary>
        /// <returns></returns>
        public async Task StartCrawler()
        {
            // Initialize a new instance of the SearchRepositoriesRequest class
            SearchRepositoriesRequest request = new SearchRepositoriesRequest($"org:{_config.Self.GithubOrganization} is:internal");
            SearchRepositoryResult result = await Client.Search.SearchRepo(request);

            foreach (Repository repo in result.Items)
            {
                dynamic data = await GetContentMetadataFile(repo);

                CommitActivity weeklyRepoActivity = await FetchWeeklyRepoActivity(repo);
                string contributorFileUrl = await GetContributorFileUrl(repo.FullName);


                // TODO: Get repository topics when new version is released (OctoKit) - or manually REST call

                CrawlerRepositoryResult crawlerRepositoryResult =
                    new CrawlerRepositoryResult(repo, data, contributorFileUrl, weeklyRepoActivity);
                // Repository score should always be done at the end to ensure all information is correct.
                crawlerRepositoryResult.RepositoryScore = CalculateRepoScore(crawlerRepositoryResult);
                _crawlerRepositoryResults.Add(crawlerRepositoryResult);
            }

            await WriteResultsToJsonFile();
        }

        /// <summary>
        /// Calculate a score based on various factors for a specific repository
        /// More information about the activity score is found at https://patterns.innersourcecommons.org/p/repository-activity-score
        /// </summary>
        /// <param name="crawlerResult"></param>
        /// <returns>Integer that indicates the score</returns>
        private int CalculateRepoScore(CrawlerRepositoryResult crawlerResult)
        {
            // initial score is 50 to give active repos with low GitHub KPIs (forks, watchers, stars) a better starting point
            double repoScore = 50;
            // weighting: forks and watches count most, then stars, add some little score for open issues, too
            repoScore += (crawlerResult.Repository.ForksCount * 5) + (crawlerResult.Repository.WatchersCount / 3) +
                         (crawlerResult.Repository.StargazersCount / 3) +
                         (crawlerResult.Repository.OpenIssuesCount / 5);

            int daysSinceLastUpdate = (DateTimeOffset.Now - crawlerResult.Repository.UpdatedAt).Days;
            // updated in last 3 months: adds a bonus multiplier between 0..1 to overall score (1 = updated today, 0 = updated more than 100 days ago)
            repoScore *= (1.0 + (100.0 - Math.Min(daysSinceLastUpdate, 100.0)) / 100.0);
            int averageCommitsPerWeek = 0;

            if (crawlerResult.CommitActivity.Activity.Count > 0)
            {
                averageCommitsPerWeek = (int) crawlerResult.CommitActivity.Activity.Average(s => s.Total);
            }

            // average commits: adds a bonus multiplier between 0..1 to overall score (1 = >10 commits per week, 0 = less than 3 commits per week)
            repoScore *= (1.0 + (Math.Min(Math.Max(averageCommitsPerWeek - 3.0, 0.0), 7.0)) / 7.0);

            // boost calculation:
            // all repositories updated in the previous year will receive a boost of maximum 1000 declining by days since last update
            double boostScore = (1000.0 - Math.Min(daysSinceLastUpdate, 365.0) * 2.74);

            int daysSinceRepoCreation = (DateTimeOffset.Now - crawlerResult.Repository.CreatedAt).Days;

            // gradually scale down boost according to repository creation date to mix with "real" engagement stats
            boostScore *= (365.0 - Math.Min(daysSinceRepoCreation, 365.0)) / 365.0;

            // add boost to score
            repoScore += boostScore;

            repoScore = ApplyFlatBonussesToRepoScore(crawlerResult, repoScore);

            // build in a logarithmic scale for very active projects (open ended but stabilizing around 5000)
            if (repoScore > 3000.0)
            {
                repoScore = 3000.0 + Math.Log(repoScore) * 100.0;
            }

            // final score is a rounded value starting from 0 (subtract the initial value)
            repoScore = Math.Round(repoScore - 50.0);

            return Convert.ToInt32(repoScore);
        }

        /// <summary>
        /// Apply a flat bonus score to a repository if it includes certain things.
        /// This is the place to add bonus points for having a contributor file, philips file, etc.
        /// </summary>
        /// <param name="crawlerResult"></param>
        /// <param name="repoScore"></param>
        /// <returns></returns>
        private static double ApplyFlatBonussesToRepoScore(CrawlerRepositoryResult crawlerResult, double repoScore)
        {
            // give projects with a description a static boost of 50
            if (!string.IsNullOrEmpty(crawlerResult.Repository.Description))
            {
                if (crawlerResult.Repository.Description.Length > 30)
                {
                    repoScore += 50;
                }
            }

            // give projects with a business category in their meta data file a static boost of 50
            if (crawlerResult.MetaData != null)
            {
                if (!string.IsNullOrEmpty(crawlerResult.MetaData["business-category"]))
                {
                    repoScore += 50;
                }
            }

            // give projects with contribution guidelines (CONTRIBUTING.md) file a static boost of 100
            if (!string.IsNullOrEmpty(crawlerResult.ContributorFileUrl))
            {
                repoScore += 100;
            }

            return repoScore;
        }

        /// <summary>
        /// Finds a contributing.md and returns URL
        /// </summary>
        /// <param name="repoFullName"></param>
        /// <returns>If a contributing.md file exists, it will return the URL</returns>
        private async Task<string> GetContributorFileUrl(string repoFullName)
        {
            SearchCodeResult contributorFileSearchResults = await SearchRepoForFile(repoFullName, "CONTRIBUTING.md");
            if (contributorFileSearchResults.TotalCount != 0)
            {
                return contributorFileSearchResults.Items[0].HtmlUrl;
            }

            return null;
        }

        /// <summary>
        /// Dump the projects to a file with proper formatting.
        /// </summary>
        /// <returns></returns>
        private async Task WriteResultsToJsonFile()
        {
            await using StreamWriter file = File.CreateText("repos.json");
            JsonSerializer serializer = new JsonSerializer {Formatting = Formatting.Indented};
            serializer.Serialize(file, _crawlerRepositoryResults);
        }

        /// <summary>
        /// Retrieve weekly Commit activity from a specific repository.
        /// </summary>
        /// <param name="repo"></param>
        /// <returns></returns>
        private async Task<CommitActivity> FetchWeeklyRepoActivity(Repository repo)
        {
            CommitActivity weeklyRepoActivity = await Client.Repository.Statistics.GetCommitActivity(repo.Id);
            return weeklyRepoActivity;
        }

        /// <summary>
        ///     Gets the content of the Meta data file if possible
        /// </summary>
        /// <param name="repo"></param>
        /// <returns>null if no meta data is found</returns>
        /// <returns>dynamic object filled with contents of the file (dictionary)</returns>
        private async Task<dynamic> GetContentMetadataFile(Repository repo)
        {
            SearchCodeResult codeResult = await SearchRepoForFile(repo.FullName, _metaDataFileName);
            if (codeResult.TotalCount != 0)
                foreach (SearchCode codeResultItem in codeResult.Items)
                {
                    Console.WriteLine(codeResultItem.Path);
                    IReadOnlyList<RepositoryContent> contentOfMetadataFile =
                        await Client.Repository.Content.GetAllContents(repo.Id, codeResultItem.Path);

                    using TextReader sr = new StringReader(contentOfMetadataFile[0].Content);
                    dynamic metaDataFile;
                    if (_yamlMode)
                    {
                        Deserializer deserializer = new Deserializer();
                        metaDataFile = deserializer.Deserialize(sr);
                    }
                    else
                    {
                        metaDataFile = JsonConvert.DeserializeObject<ExpandoObject>(contentOfMetadataFile[0].Content,
                            new ExpandoObjectConverter());
                    }


                    return metaDataFile;
                }

            return null;
        }


        /// <summary>
        /// Search the specified repository for a specific file
        /// </summary>
        /// <param name="repositoryFullName">Full name of the repository</param>
        /// <param name="fileName">name of the file to search for</param>
        /// <returns></returns>
        private async Task<SearchCodeResult> SearchRepoForFile(string repositoryFullName, string fileName)
        {
            SearchCodeRequest searchCodeForFile =
                new SearchCodeRequest($"repo:{repositoryFullName} filename:{fileName}");
            SearchCodeResult codeResult = await Client.Search.SearchCode(searchCodeForFile);

            if (codeResult.TotalCount == 0)
            {
                Console.WriteLine($"Unable to find {fileName} file.");
            }

            return codeResult;
        }
    }
}