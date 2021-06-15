using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public interface IRepositoryService
    {
        public Task<Repository[]> GetAllRepositories();
        public Task<List<string>> GetAllLanguagesFromRepos(IEnumerable<Repository> repositories);
        public Task<List<Repository>> FilterRepositoryOnString(IEnumerable<Repository> repositories, string textToSearch);
        public Task<List<Repository>> FilterRepositoryOnLanguage(IEnumerable<Repository> repositories, string languageToMatch);
        public Task<List<Repository>> SortRepository(IEnumerable<Repository> repositories, string sortType);
    }
}
