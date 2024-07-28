using IT.CoreLib.Application;
using UnityEngine;

namespace IT.CoreLib.UI
{

    public abstract class UIWindowBase : MonoBehaviour
    {
        public abstract void Initialize(SceneBootstrap scene);

        public virtual void OnBeforeShowWindow() { }
        public virtual void OnAfterShowWindow() { }
        public virtual void OnBeforeHideWindow() { }
        public virtual void OnAfterHideWindow() { }
    }

}
