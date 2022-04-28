using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common
{
    /// <summary>
    /// 捕获在RectTransform上的拖动事件
    /// </summary>
    public class InputDrag : UIBehaviour, IDragHandler
    {
        public event Action<Vector2> OnDragging;
        public void OnDrag(PointerEventData eventData)
        {
            OnDragging?.Invoke(eventData.delta);
        }
    }
}