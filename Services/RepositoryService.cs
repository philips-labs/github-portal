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
    }
}
