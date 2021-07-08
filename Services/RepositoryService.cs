using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public class RepositoryService : IRepositoryService
    {
        private readonly HttpClient _httpClient;
        public RepositoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Retrieve all repositories from a data source.
        /// </summary>
        /// <returns>Array of repositories</returns>
        public async Task<CrawlerResult[]> GetAllRepositories()
        {
            return await _httpClient.GetFromJsonAsync<CrawlerResult[]>("sample-data/repos.json");
        }

        /// <summary>
        /// Find the Languages from the repositories.
        /// </summary>
        /// <param name="repositories"></param>
        /// <returns>List of strings that are the languages</returns>
        public async Task<List<string>> GetAllLanguagesFromRepos(IEnumerable<CrawlerResult> repositories)
        {
            return repositories.Select(repo => repo.Repository.Language).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
        }

        /// <summary>
        /// Filter a list of repositories based on string
        /// </summary>
        /// <param name="repositories">List of repositories to perform the search on</param>
        /// <param name="textToSearch">String to base the search on</param>
        /// <returns>Filtered list of repositories based on matches with the specified string</returns>
        public async Task<List<CrawlerResult>> FilterRepositoryOnString(IEnumerable<CrawlerResult> repositories, string textToSearch)
        {
            return repositories.Where(r =>
                r.Repository.Name.ToLowerInvariant().Contains(textToSearch) ||
                // r.Repository.Topics.Any(s => s.ToLowerInvariant().Contains(textToSearch)) ||
                r.Repository.Homepage != null && r.Repository.Homepage.ToLowerInvariant().Contains(textToSearch) ||
                r.Repository.Description != null && r.Repository.Description.ToLowerInvariant().Contains(textToSearch) ||
                r.Repository.FullName != null && r.Repository.FullName.ToLowerInvariant().Contains(textToSearch) ||
                r.Repository.HtmlUrl != null && r.Repository.HtmlUrl.ToLowerInvariant().Contains(textToSearch) ||
                r.Repository.License is {Name: { }} && r.Repository.License.Name.ToLowerInvariant().Contains(textToSearch) ||
                r.Repository.Language != null && r.Repository.Language.ToLowerInvariant().Contains(textToSearch)).ToList();
        }

        /// <summary>
        /// Filter Repository list based on matched language
        /// </summary>
        /// <param name="repositories">List of repositories to match against</param>
        /// <param name="languageToMatch">Examples of languages are Java, Javascript, C#</param>
        /// <returns>A filtered list based on the Language that matched the provided string</returns>
        public async Task<List<CrawlerResult>> FilterRepositoryOnLanguage(IEnumerable<CrawlerResult> repositories, string languageToMatch)
        {
            return repositories.Where(r => r.Repository.Language != null && r.Repository.Language.ToLowerInvariant().Contains(languageToMatch.ToLowerInvariant())).ToList();
        }

        /// <summary>
        /// Sort the Repositories based on their sort type
        /// </summary>
        /// <param name="repositories">List of repositories</param>
        /// <param name="sortType">Valid sort types include: Name, Stars, Issues, Activity, Watchers</param>
        /// <returns>A sorted list of repositories based on the specified sort type</returns>
        public async Task<List<CrawlerResult>> SortRepository(IEnumerable<CrawlerResult> repositories, string sortType)
        {
            return sortType switch
            {
                "Name" => repositories.OrderByDescending(r => r.Repository.Name).ToList(),
                "Stars" => repositories.OrderByDescending(r => r.Repository.StargazersCount).ToList(),
                "Issues" => repositories.OrderByDescending(r => r.Repository.OpenIssuesCount).ToList(),
                "Activity" => repositories.OrderByDescending(r => r.RepositoryScore).ToList(),
                "Watchers" => repositories.OrderByDescending(r => r.Repository.WatchersCount).ToList(),
                _ => new List<CrawlerResult>(repositories)
            };
        }
    }
}
