using UnityEngine;

namespace YleSearch
{
    [DisallowMultipleComponent]
    public class Context : MonoBehaviour
    {
        public static Context instance { get; private set; }

        [SerializeField]
        private SearchView _searchView;
        private SearchController _searchController = new SearchController();
        
        void Awake()
        {
           InitializeContext();
        }

        private void InitializeContext()
        {
            instance = this;
            
            _searchView.OnSearch += _searchController.StartSearch;
            _searchView.OnScrolledForUpdate += _searchController.LoadNextSearchPage;
            
            _searchController.OnNewSearchLoaded += _searchView.OnDataSourceRecreated;
            _searchController.OnSearchPageLoaded += _searchView.OnDataSourceUpdated;
        }
    }
}