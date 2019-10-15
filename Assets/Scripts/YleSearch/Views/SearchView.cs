using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YleSearch
{
    public class SearchView : MonoBehaviour
    {
        private const int ScrollUpdateIndexTreshold = 3;
        private float ScrollDownUpdateInterval = 1.0f;
        
        public event Action<string> OnSearch;
        public event Action OnScrolledForUpdate;

        [SerializeField] private ScrollLoadingSpinnerDecorator _spinner;
        [SerializeField] private LoopScrollRect _scrollRect;
        [SerializeField] private TMP_InputField _searchInputField;

        private string _searchTerm;
        private bool _isAwaitingForResponse;
        private float LastScrollDownUpdateTime;
        private void Awake() 
        {
            InitView();
        }

        private void InitView()
        {
            _scrollRect.ScrolledDownToIndex += OnScrolledToItem;
            _scrollRect.onValueChanged.AddListener(OnResultsScrolled);
        }
        
        public void OnSearchSubmit()
        {
            if (!ValidateSearchString() || _isAwaitingForResponse) return;
            _isAwaitingForResponse = true;
            _searchTerm = _searchInputField.text;
            OnSearch?.Invoke(_searchTerm);
            ClearSearchResults();
            EnableSpinner(true);
        }
        
        private void OnScrolledToItem(int index)
        {
            if (_isAwaitingForResponse || _scrollRect.dataSource.Count - index > ScrollUpdateIndexTreshold)
            {
                return;
            }
            
            _isAwaitingForResponse = true;
            OnScrolledForUpdate?.Invoke();
        }

        private void OnResultsScrolled(Vector2 normalizedScrollPos)
        {
            if (normalizedScrollPos.y < 1f ||
                Time.realtimeSinceStartup - LastScrollDownUpdateTime < ScrollDownUpdateInterval)
            {
                return;
            }

            if (!_isAwaitingForResponse)
            {
                LastScrollDownUpdateTime = Time.realtimeSinceStartup;
                _isAwaitingForResponse = true;
                OnScrolledForUpdate?.Invoke();
            }
            
            EnableSpinner(true);
        }

        public void OnDataSourceUpdated(bool isSuccess)
        {
            _isAwaitingForResponse = false;
            EnableSpinner(false);
            
            if (!isSuccess)
            {
                return;
            }
            
            _scrollRect.RefreshCells();
        }
        
        public void OnDataSourceRecreated(bool isSuccess, IList dataSource)
        {
            _isAwaitingForResponse = false;
            EnableSpinner(false);

            if (!isSuccess)
            {
                return;
            }
            
            _scrollRect.dataSource = dataSource;
            _scrollRect.RefillCells();
        }

        private void ClearSearchResults()
        {
            _scrollRect.ClearCells();
            _scrollRect.dataSource = null;
        }

        private void EnableSpinner(bool isEnabled)
        {
            _spinner.SetEnabled(isEnabled);
        }
        
        private bool ValidateSearchString()
        {
            return !string.IsNullOrEmpty(_searchInputField.text);
        }
    }
}