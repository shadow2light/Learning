using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common
{
    /// <summary>
    /// 捕获在RectTransform上的拖动完成事件
    /// </summary>
    public class InputBeginEndDrag : UIBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public event Action<Vector2> OnDragged;

        private Vector2 beginDragPosition;
        private Vector2 endDragPosition;

        public void OnBeginDrag(PointerEventData eventData)
        {
            beginDragPosition = eventData.position;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            endDragPosition = eventData.position;
            OnDragged?.Invoke(endDragPosition - beginDragPosition);
        }

        //使用IBeginDragHandler, IEndDragHandler时，需要带上IDragHandler才能触发Begin/End事件
        public void OnDrag(PointerEventData eventData)
        {
        }
    }
}