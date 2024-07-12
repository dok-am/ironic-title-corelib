using IT.CoreLib.Application;
using UnityEngine;

namespace IT.CoreLib.UI
{

    public abstract class UIWindowBase : MonoBehaviour
    {
        public abstract void Initialize(SceneBootstrap scene);

        public abstract void OnBeforeShowWindow();
        public abstract void OnAfterShowWindow();
        public abstract void OnBeforeHideWindow();
        public abstract void OnAfterHideWindow();
    }

}
