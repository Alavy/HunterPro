using Algine.FPS.MobileInput;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Algine.MobileInput
{
    public class TouchField : MonoBehaviour,
        IPointerDownHandler, IPointerUpHandler
    {
        [HideInInspector]
        public static Vector2 TouchDist;
        [HideInInspector]
        public Vector2 PointerOld;
        [HideInInspector]
        protected int PointerId;
        [HideInInspector]
        public bool Pressed;

        void Update()
        {
            if (Pressed)
            {
                if (PointerId >= 0 && PointerId < Input.touches.Length)
                {
                    TouchDist = Input.touches[PointerId].position - PointerOld;
                    PointerOld = Input.touches[PointerId].position;
                }
                else
                {
                    TouchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - PointerOld;
                    PointerOld = Input.mousePosition;
                }
            }
            else
            {
                TouchDist = new Vector2();
            }
            InputEventsHandler.Current.Touch(TouchDist);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Pressed = true;
            PointerId = eventData.pointerId;
            PointerOld = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Pressed = false;
        }
    }
}