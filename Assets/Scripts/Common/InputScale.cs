using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common
{
    /// <summary>
    /// 捕获在RectTransform上的缩放事件，win_editor:鼠标滚轮；android:两个手指缩放
    /// todo 需要限定在rectTransform区域内
    /// </summary>
    public class InputScale : UIBehaviour
    {
        public event Action<float> OnScale;

        private RectTransform rectTransform;
        private Rect rect;

#if UNITY_EDITOR
#elif UNITY_ANDROID
        private Touch oldTouch1, oldTouch2;
#endif

        private new void Start()
        {
            rectTransform = transform as RectTransform;
            rect = rectTransform.rect;
        }

        private void Update()
        {
#if UNITY_EDITOR
            // var position = Input.mousePosition;
            // if (position.InRect(rect))
            // {
            float wheel = Input.GetAxis("Mouse ScrollWheel");
            if (wheel != 0)
            {
                if (EventSystem.current.IsPointerOverGameObject(-1))
                {
                    OnScale?.Invoke(wheel);
                }
            }
            // }
#elif UNITY_ANDROID
            //没有触摸  
            if (Input.touchCount <= 0)
                return;

            if (!EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            if (Input.touchCount == 2)
            {
                //多点触摸, 放大缩小  
                Touch newTouch1 = Input.GetTouch(0);
                Touch newTouch2 = Input.GetTouch(1);

                //第2点刚开始接触屏幕, 只记录，不做处理  
                if (newTouch2.phase == TouchPhase.Began)
                {
                    oldTouch2 = newTouch2;
                    oldTouch1 = newTouch1;
                    return;
                }

                //计算老的两点距离和新的两点间距离，变大要放大模型，变小要缩放模型
                float oldDistance = Vector2.Distance(oldTouch1.position, oldTouch2.position);
                float newDistance = Vector2.Distance(newTouch1.position, newTouch2.position);

                //两个距离之差，为正表示放大手势， 为负表示缩小手势
                float offset = newDistance - oldDistance;
                OnScale?.Invoke(offset);

                //记住最新的触摸点，下次使用
                oldTouch1 = newTouch1;
                oldTouch2 = newTouch2;
            }
#endif
        }
    }
}