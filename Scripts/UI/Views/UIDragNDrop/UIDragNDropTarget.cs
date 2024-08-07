using IT.CoreLib.UI;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace IT.CoreLib
{
    /// <summary>
    /// Drag'n'drop drop target object class
    /// </summary>
    /// <typeparam name="T">Drag'n'drop data value type</typeparam>
    public abstract class UIDragNDropTarget<T> : MonoBehaviour, IDropHandler
    {
        public event Action<T> OnValueDropped;

        public void OnDrop(PointerEventData eventData)
        {
            UIDragNDropObject<T> dndObject = eventData.pointerDrag.GetComponent<UIDragNDropObject<T>>();
            if (dndObject != null)
            {
                dndObject.StopDragginForced();
                OnValueDropped?.Invoke(dndObject.GetValue());
            }
        }
    }
}
