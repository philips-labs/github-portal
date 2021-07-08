using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Services
{
    public interface IRepositoryService
    {
        public Task<CrawlerResult[]> GetAllRepositories();
        public Task<List<string>> GetAllLanguagesFromRepos(IEnumerable<CrawlerResult> repositories);
        public Task<List<CrawlerResult>> FilterRepositoryOnString(IEnumerable<CrawlerResult> repositories, string textToSearch);
        public Task<List<CrawlerResult>> FilterRepositoryOnLanguage(IEnumerable<CrawlerResult> repositories, string languageToMatch);
        public Task<List<CrawlerResult>> SortRepository(IEnumerable<CrawlerResult> repositories, string sortType);

        public Task<List<CrawlerResult>> SortOnBusinessCategory(IEnumerable<CrawlerResult> repositories,
            string businessCategory);
        public Task<List<string>> GetAllBusinessCategories(IEnumerable<CrawlerResult> repoCrawlerResults);
    }
}
