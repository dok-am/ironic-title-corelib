using IT.CoreLib.Application;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace IT.CoreLib.UI
{
    public class SceneUIBase : MonoBehaviour
    {
        public UITransitionBase AppUITransition => _appUIContainer.UITransition;


        protected Dictionary<Type, UIWindowBase> _createdWindows;


        [SerializeField] private List<UIWindowBase> _windowsPrefabs;

        private ApplicationUIContainer _appUIContainer;
        private SceneBootstrap _scene;

        public T ShowWindow<T>() where T : UIWindowBase
        {
            if (_createdWindows.TryGetValue(typeof(T), out var window))
            {
                window.OnBeforeShowWindow();
                //TODO: add transition
                window.gameObject.SetActive(true);
                window.OnAfterShowWindow();
                return window as T;
            }

            UIWindowBase windowPrefab = _windowsPrefabs.Find(window => window is T);

            if (windowPrefab == null)
                throw new Exception($"[UI] Can' create window of type {typeof(T).Name}: no prefab available");

            T windowInstance = Instantiate(windowPrefab, transform) as T;
            windowInstance.Initialize(_scene);

            windowInstance.OnBeforeShowWindow();
            //TODO: and transition here
            _createdWindows.Add(typeof(T), windowInstance);
            windowInstance.OnAfterShowWindow();

            return windowInstance;
        }

        public void HideWindow<T>() where T : UIWindowBase
        {
            if (_createdWindows.TryGetValue(typeof(T), out var window))
            {
                window.OnBeforeHideWindow();
                window.gameObject.SetActive(false);
                window.OnAfterHideWindow();
                return;
            }

            throw new Exception($"[UI] Can't hide window {typeof(T).Name}: it doesnt' exist!");
        }


        public virtual void Initialize(SceneBootstrap scene, ApplicationUIContainer appUIContainer)
        {
            _appUIContainer = appUIContainer;
            _scene = scene;
            _createdWindows = new Dictionary<Type, UIWindowBase>();
        }

        public virtual void Deinitialize()
        {
            foreach (UIWindowBase window in _createdWindows.Values)
            {
                window.OnBeforeHideWindow();
                Destroy(window.gameObject);
            }

            _createdWindows.Clear();
        }
    }
}
