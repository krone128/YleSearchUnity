using UnityEngine;
using UnityEngine.UI;

namespace YleSearch
{

    [RequireComponent(typeof(LoopScrollRect))]
    public class ScrollLoadingSpinnerDecorator : MonoBehaviour
    {
        private const float ScrollEndVerticalNormalizedPos = 1f;

        [SerializeField] private RectTransform _spinner;

        private LoopScrollRect _scrollRect;

        private bool _isEnabled;

        private void Awake()
        {
            _scrollRect = GetComponent<LoopScrollRect>();
        }

        public void SetEnabled(bool isEnabled)
        {
            _isEnabled = isEnabled;

            if (_isEnabled)
            {
                _spinner.SetParent(_scrollRect.content);
                _spinner.SetAsLastSibling();
                _scrollRect.verticalNormalizedPosition = ScrollEndVerticalNormalizedPos;

            }
            else
            {
                _spinner.SetParent(this.transform);
            }

            _spinner.gameObject.SetActive(_isEnabled);
        }
    }
}
