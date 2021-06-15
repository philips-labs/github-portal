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
    }
}
