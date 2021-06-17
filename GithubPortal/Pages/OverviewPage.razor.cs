using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Components;
using Services;

namespace GithubPortal.Pages
{
    public partial class OverviewPage
    {
        [Inject]
        private IRepositoryService _repositoryService { get; set; }
        private List<Repository> _repositories;
        private IEnumerable<Repository> _originalRepositories;
        private List<string> _languages;
        private bool _isModalOpen;
        private Repository _selectedRepo;
        private string _text;
        private bool _queued;
        private bool _loading;
        private int _totalItems;
        private int _totalRepoCount;

        private List<string>
            _sortTypes = new List<string>
                {
                    "Name",
                    "Activity",
                    "Stars",
                    "Watchers",
                    "Issues"
                };

        private string _sortType;

        private string SortType
        {
            get => _sortType;
            set
            {
                if (value != _sortType)
                {
                    _sortType = value;
                    InvokeAsync(LoadRepositoriesBasedOnSortType);
                }
            }
        }


        private string _selectedLanguage;

        private string SelectedLanguage
        {
            get => _selectedLanguage;
            set
            {
                if (value != _selectedLanguage)
                {
                    _selectedLanguage = value;
                    InvokeAsync(LoadRepositoriesForSpecificLanguageAsync);
                }
            }
        }

        private void ShowDetailedRepositoryModal(Repository repo)
        {
            _isModalOpen = true;
            _selectedRepo = repo;
        }

        protected override async Task OnInitializedAsync()
        {
            _originalRepositories = await _repositoryService.GetAllRepositories().ConfigureAwait(false);
            _repositories = _originalRepositories.ToList();
            _languages = await _repositoryService.GetAllLanguagesFromRepos(_repositories).ConfigureAwait(false);
            _totalRepoCount = _originalRepositories.Count();
        }

        private string Text
        {
            get => _text;
            set
            {
                if (value != _text)
                {
                    _text = value;
                    InvokeAsync(OnSearchAsync);
                }
            }
        }

        private void Reset()
        {
            _totalItems = 0;
            _text = string.Empty;
            _repositories = _originalRepositories.ToList();
            StateHasChanged();
        }

        private async Task OnSearchAsync()
        {
            if (!string.IsNullOrWhiteSpace(_text))
            {
                if (_loading)
                {
                    _queued = true;
                    return;
                }

                do
                {
                    string textToSearch = _text.ToLowerInvariant();
                    _loading = true;
                    _queued = false;
                    _repositories = await _repositoryService.FilterRepositoryOnString(_repositories, textToSearch);
                    _totalItems = _repositories.Count;
                    _loading = false;
                } while (_queued);
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                Reset();
            }
        }

        private async Task LoadRepositoriesForSpecificLanguageAsync()
        {
            _repositories = await _repositoryService.FilterRepositoryOnLanguage(_repositories, _selectedLanguage);
            await InvokeAsync(StateHasChanged);
        }

        private async Task LoadRepositoriesBasedOnSortType()
        {
            _repositories = await _repositoryService.SortRepository(_repositories, _sortType);
            await InvokeAsync(StateHasChanged);
        }
    }
}
