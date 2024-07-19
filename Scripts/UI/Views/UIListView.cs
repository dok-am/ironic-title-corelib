
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

        private List<T> _items = new();
        

        public void UpdateView(P[] dataItems)
        {
            ClearViewItems();

            foreach (P item in dataItems)
            {
                T viewItem = CreateViewItem(item);
                viewItem.transform.SetParent(_itemsContainer);

                _items.Add(CreateViewItem(item));

            }
        }

        public void ClearViewItems()
        {
            foreach (T item in _items)
            {
                item.PrepareToDestroy();
            }

            _items.Clear();

            for (int i = 0; i < _itemsContainer.childCount; i++)
            {
                Destroy(_itemsContainer.GetChild(i).gameObject);
            }
        }

        protected virtual T CreateViewItem(P itemData)
        {
            //TODO: Create proper pool
            T viewItem = Instantiate(_itemPrefab.gameObject).GetComponent<T>();
            return SetupViewItem(viewItem, itemData);
        }

        protected abstract T SetupViewItem(T viewItem, P itemData);

    }

}
