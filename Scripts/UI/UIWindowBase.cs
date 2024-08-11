using IT.CoreLib.Application;
using UnityEngine;

namespace IT.CoreLib.UI
{
    public abstract class UIWindowBase : MonoBehaviour
    {
        public RectTransform UIRectTransfrom
        {
            get
            {
                if (_rectTransform == null) 
                    _rectTransform = transform as RectTransform;

                return _rectTransform;
            }
        }


        private RectTransform _rectTransform;



        public abstract void Initialize(SceneContext scene);

        public virtual void UpdateData(IWindowData data) { }
        public virtual void OnBeforeShowWindow() { }
        public virtual void OnAfterShowWindow() { }
        public virtual void OnBeforeHideWindow() { }
        public virtual void OnAfterHideWindow() { }
    }
}
