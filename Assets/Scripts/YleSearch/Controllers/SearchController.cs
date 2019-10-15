using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace YleSearch
{
    public class SearchController
    {
        public event Action<bool, IList> OnNewSearchLoaded;
        public event Action<bool> OnSearchPageLoaded;
        
        private const int PageSize = 10;

        private string _cachedSearchTerm;
        private int _cachedPage;

        private List<SearchListItemData> _cachedSearchResults;

        public void StartSearch(string searchTerm)
        {
            _cachedSearchTerm = searchTerm;
            _cachedPage = 0;
            Context.instance.StartCoroutine(YleAPI.GetPrograms(_cachedSearchTerm, 0, PageSize, OnSearchLoaded));
        }

        public void LoadNextSearchPage()
        {
            Context.instance.StartCoroutine(YleAPI.GetPrograms(_cachedSearchTerm, (_cachedPage + 1) * PageSize, PageSize, OnPageLoaded));
        }

        private void OnSearchLoaded(bool isSuccess, IList<YleProgramData> searchData)
        {
            if (isSuccess)
            {
                _cachedSearchResults = searchData.Select(programData => new SearchListItemData(programData)).ToList();
                OnNewSearchLoaded?.Invoke(true, _cachedSearchResults);
            }
            else
            {
                OnNewSearchLoaded?.Invoke(false, null);
            }
        }
        
        private void OnPageLoaded(bool isSuccess, IList<YleProgramData> searchData)
        {
            if (isSuccess)
            {
                _cachedSearchResults.AddRange(searchData.Select(programData => new SearchListItemData(programData)));
                ++_cachedPage;
            }

            OnSearchPageLoaded?.Invoke(isSuccess);
        }
    }
}