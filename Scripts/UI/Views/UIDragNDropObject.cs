using UnityEngine;
using UnityEngine.EventSystems;

namespace IT.CoreLib.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent (typeof(CanvasGroup))]
    public abstract class UIDragNDropObject<T> : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public bool IsDragging => _isDragging;

        private RectTransform _rectTransform;
        private CanvasGroup _canvasGroup;

        private Canvas _mainCanvas;
        private Transform _gridTransform;
        private int _index;
        private bool _isDragging;

        private static Transform CellEmptyCopy
        {
            get
            {
                if (_cellEmptyCopy == null)
                {
                    _cellEmptyCopy = new GameObject("_DUMMY", typeof(RectTransform));
                }

                return _cellEmptyCopy.transform;
            }
        }
        private static GameObject _cellEmptyCopy;

        public abstract T GetValue();

        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// You should run this method in children classs
        /// </summary>
        /// <param name="mainCanvas">Main canvas</param>
        protected void InitializeDragNDrop(Canvas mainCanvas)
        {
            _mainCanvas = mainCanvas;
        }

        public void StopDragginForced()
        {
            if (!_isDragging)
                return;

            CellEmptyCopy.SetParent(null);
            CellEmptyCopy.gameObject.SetActive(false);

            transform.SetParent(_gridTransform, false);
            transform.SetSiblingIndex(_index);

            _canvasGroup.blocksRaycasts = true;

            _isDragging = false;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _index = transform.GetSiblingIndex();
            _gridTransform = transform.parent;
            transform.SetParent(_gridTransform.parent, true);
            CellEmptyCopy.SetParent(_gridTransform, false);
            CellEmptyCopy.SetSiblingIndex(_index);
            CellEmptyCopy.gameObject.SetActive(true);

            _canvasGroup.blocksRaycasts = false;

            _isDragging = true;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            StopDragginForced();
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (_isDragging)
                _rectTransform.anchoredPosition += eventData.delta / _mainCanvas.scaleFactor;
        }
               
    }

}
