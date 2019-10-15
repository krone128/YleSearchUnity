using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace YleSearch
{
    public class SearchResultListItem : MonoBehaviour, IListItemDataReciever
    {
        private const float ExpandedButtonRotation = -90f;
        private const float CollapsedButtonRotation = 0f;
        private const float CollapsedItemHeight = 100f;
        private const float BottomTextPadding = 10f;

        private const float ExpandAimDuration = 0.3f;

        [SerializeField] private TMP_Text _labelText;
        [SerializeField] private TMP_Text _infoText;

        [SerializeField] private RectTransform _expandButton;
        [SerializeField] private LayoutElement _layoutElement;
        [SerializeField] private RectTransform _infoTextRect;

        [SerializeField] private bool _isExpanded;

        private SearchListItemData _itemData;

        public void OnExpandClick()
        {
            _isExpanded = !_isExpanded;

            if (_itemData != null)
                _itemData.IsExpanded = _isExpanded;
            
            if (_isExpanded)
            {
                AnimateExpand();
            }
            else
            {
                AnimateCollapse();
            }
        }

        private void AnimateExpand()
        {
            _expandButton.DOLocalRotate(new Vector3(0f, 0f, ExpandedButtonRotation), ExpandAimDuration);
            _layoutElement.DOPreferredSize(
                new Vector2(0f, CollapsedItemHeight + _infoTextRect.rect.height + BottomTextPadding),
                ExpandAimDuration);
        }
        private void AnimateCollapse()
        {
            _expandButton.DOLocalRotate(new Vector3(0f, 0f, CollapsedButtonRotation), ExpandAimDuration);
            _layoutElement.DOPreferredSize(new Vector2(0f, CollapsedItemHeight), ExpandAimDuration);
        }
        private async void SetExpanded()
        {
            _isExpanded = true;
            _expandButton.localEulerAngles = new Vector3(0f, 0f, ExpandedButtonRotation);
            LayoutRebuilder.ForceRebuildLayoutImmediate(_infoTextRect);
            _layoutElement.preferredHeight = CollapsedItemHeight + _infoTextRect.rect.height + BottomTextPadding;
        }
        private void SetCollapsed()
        {
            _isExpanded = false;
            _expandButton.localEulerAngles = Vector3.zero;
            _layoutElement.preferredHeight = CollapsedItemHeight;
        }
        public void Recieve(object data)
        {
            _itemData = data as SearchListItemData;

            if (_itemData == null)
            {
                Debug.LogError("Could not cast data to destination type");
                return;
            }

            _labelText.text = _itemData.Title;
            _infoText.text = _itemData.Info;
            _isExpanded = _itemData.IsExpanded;

            if (_isExpanded)
            {
                SetExpanded();
            }
            else
            {
                SetCollapsed();
            }
        }
    }
}
