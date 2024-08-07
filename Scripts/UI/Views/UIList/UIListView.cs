using System.Collections.Generic;
using UnityEngine;

namespace IT.CoreLib.UI
{
    /// <summary>
    /// Class for creating lists
    /// </summary>
    /// <typeparam name="T">Type for cell view</typeparam>
    /// <typeparam name="P">Type for cell data</typeparam>
    public abstract class UIListView<T, P> : MonoBehaviour where T : MonoBehaviour, IUIListItem
    {
        [SerializeField] protected Transform _itemsContainer;
        [SerializeField] protected T _itemPrefab;

        protected List<T> _items = new();
        private int _poolIndex = 0;

        public void UpdateView(P[] dataItems)
        {
            ClearViewItems();

            for (int i=0; i<dataItems.Length; i++)
            {
                P item = dataItems[i];
                T viewItem = CreateViewItem(item);
                viewItem.transform.SetSiblingIndex(i);
            }
        }

        public void ClearViewItems()
        {
            foreach (T item in _items)
            {
                item.PrepareToDeactivate();
                item.gameObject.SetActive(false);
            }
        }

        protected virtual T CreateViewItem(P itemData)
        {

            T viewItem = GetViewItemFromPool();
            if (viewItem == null)
            {
                viewItem = Instantiate(_itemPrefab, _itemsContainer);
                _items.Add(viewItem);
            } 
            else
            {
                viewItem.gameObject.SetActive(true);
            }

            return SetupViewItem(viewItem, itemData);
        }

        protected abstract T SetupViewItem(T viewItem, P itemData);

        private T GetViewItemFromPool()
        {
            if (_items.Count == 0 || _poolIndex >= _items.Count)
                return null;

            T item = _items[_poolIndex];

            if (!item.gameObject.activeSelf)
            {
                _poolIndex++;
                return item;
            }

            for (_poolIndex = 0; _poolIndex < _items.Count; _poolIndex++)
            {
                item = _items[_poolIndex];
                if (!item.gameObject.activeSelf)
                {
                    _poolIndex++;
                    return item;
                }
            }

            return null;
        }

    }

}
