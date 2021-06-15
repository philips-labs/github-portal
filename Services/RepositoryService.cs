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
        public async Task<Repository[]> GetAllRepositories()
        {
            return await _httpClient.GetFromJsonAsync<Repository[]>("sample-data/repos.json");
        }

        /// <summary>
        /// Find the Languages from the repositories.
        /// </summary>
        /// <param name="repositories"></param>
        /// <returns>List of strings that are the languages</returns>
        public async Task<List<string>> GetAllLanguagesFromRepos(IEnumerable<Repository> repositories)
        {
            return repositories.Select(repo => repo.Language).Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
        }

        /// <summary>
        /// Filter a list of repositories based on string
        /// </summary>
        /// <param name="repositories">List of repositories to perform the search on</param>
        /// <param name="textToSearch">String to base the search on</param>
        /// <returns>Filtered list of repositories based on matches with the specified string</returns>
        public async Task<List<Repository>> FilterRepositoryOnString(IEnumerable<Repository> repositories, string textToSearch)
        {
            return repositories.Where(r =>
                r.Name.ToLowerInvariant().Contains(textToSearch) ||
                r.InnerSourceMetadata.Topics.Any(s => s.ToLowerInvariant().Contains(textToSearch)) ||
                r.Homepage != null && r.Homepage.ToLowerInvariant().Contains(textToSearch) ||
                r.Description != null && r.Description.ToLowerInvariant().Contains(textToSearch) ||
                r.FullName != null && r.FullName.ToLowerInvariant().Contains(textToSearch) ||
                r.HtmlUrl != null && r.HtmlUrl.ToLowerInvariant().Contains(textToSearch) ||
                r.License is {Name: { }} && r.License.Name.ToLowerInvariant().Contains(textToSearch) ||
                r.Language != null && r.Language.ToLowerInvariant().Contains(textToSearch)).ToList();
        }

        /// <summary>
        /// Filter Repository list based on matched language
        /// </summary>
        /// <param name="repositories">List of repositories to match against</param>
        /// <param name="languageToMatch">Examples of languages are Java, Javascript, C#</param>
        /// <returns>A filtered list based on the Language that matched the provided string</returns>
        public async Task<List<Repository>> FilterRepositoryOnLanguage(IEnumerable<Repository> repositories, string languageToMatch)
        {
            return repositories.Where(r => r.Language != null && r.Language.ToLowerInvariant().Contains(languageToMatch.ToLowerInvariant())).ToList();
        }

        /// <summary>
        /// Sort the Repositories based on their sort type
        /// </summary>
        /// <param name="repositories">List of repositories</param>
        /// <param name="sortType">Valid sort types include: Name, Stars, Issues, Activity, Watchers</param>
        /// <returns>A sorted list of repositories based on the specified sort type</returns>
        public async Task<List<Repository>> SortRepository(IEnumerable<Repository> repositories, string sortType)
        {
            return sortType switch
            {
                "Name" => repositories.OrderByDescending(r => r.Name).ToList(),
                "Stars" => repositories.OrderByDescending(r => r.StargazersCount).ToList(),
                "Issues" => repositories.OrderByDescending(r => r.OpenIssues).ToList(),
                "Activity" => repositories.OrderByDescending(r => r.InnerSourceMetadata.Score).ToList(),
                "Watchers" => repositories.OrderByDescending(r => r.WatchersCount).ToList(),
                _ => new List<Repository>(repositories)
            };
        }
    }
}
