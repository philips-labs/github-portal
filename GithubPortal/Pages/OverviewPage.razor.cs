using System;
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
        private List<CrawlerResult> _repositories;
        private IEnumerable<CrawlerResult> _originalRepositories;
        private List<string> _languages;
        private bool _isModalOpen;
        private CrawlerResult _selectedRepo;
        private string _text;
        private bool _queued;
        private bool _loading;
        private int _totalItems;
        private int _totalRepoCount;
        // TODO: Add Missing Metadata Implementation
        // private List<string> _businessCategories;

        private List<string>
            _sortTypes = new List<string>
                {
                    "Name",
                    // "Activity",
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

        // TODO: Add Missing Metadata Implementation
        // private string _businessCategory;

        // TODO: Add Missing Metadata Implementation
        // private string BusinessCategory
        // {
        //     get => _businessCategory;
        //     set
        //     {
        //         if (value != _businessCategory)
        //         {
        //             _businessCategory = value;
        //             InvokeAsync(LoadRepositoriesBasedOnBusinessCategory);
        //         }
        //     }
        // }


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

        private void ShowDetailedRepositoryModal(CrawlerResult repo)
        {
            _isModalOpen = true;
            _selectedRepo = repo;
        }

        protected override async Task OnInitializedAsync()
        {
            // _originalRepositories = await _httpClient.GetFromJsonAsync<CrawlerResult[]>("inner-source/repositories?format=full");
            _originalRepositories = await _repositoryService.GetAllRepositories().ConfigureAwait(false);
            _repositories = _originalRepositories.ToList();
            _languages = await _repositoryService.GetAllLanguagesFromRepos(_repositories).ConfigureAwait(false);
            // TODO: Add Missing Metadata Implementation 
            // _businessCategories =
            //     await _repositoryService.GetAllBusinessCategories(_repositories).ConfigureAwait(false);
            // _businessCategories.Insert(0, "--Select Category--");
            _languages.Insert(0, "--Select Language--");
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
                    _repositories = await _repositoryService.FilterRepositoryOnString(_originalRepositories, textToSearch);
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
            if (_selectedLanguage.Equals("--Select Language--"))
            {
                _repositories = _originalRepositories.ToList();
            }
            else
            {
                _repositories = await _repositoryService.FilterRepositoryOnLanguage(_originalRepositories, _selectedLanguage);
            }
            await InvokeAsync(StateHasChanged);
        }

        private async Task LoadRepositoriesBasedOnSortType()
        {
            _repositories = await _repositoryService.SortRepository((_originalRepositories), _sortType);
            await InvokeAsync(StateHasChanged);
        }

        // TODO: Add Missing Metadata Implementation
        // private async Task LoadRepositoriesBasedOnBusinessCategory()
        // {
        //     if (_businessCategory.Equals("--Select Category--"))
        //     {
        //         _repositories = _originalRepositories.ToList();
        //     }
        //     else
        //     {
        //         _repositories = await _repositoryService.SortOnBusinessCategory((_originalRepositories), _businessCategory);
        //
        //     }
        //     await InvokeAsync(StateHasChanged);
        // }
    }
}
