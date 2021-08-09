using Algine.FPS.MobileInput;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Algine.MobileInput
{
    public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
    {
        [Header("Options")]
        [Range(0f, 2f)] public float handleLimit = 1f;

        public static Vector2 inputVector = Vector2.zero;

        [Header("Components")]
        public RectTransform background;
        public RectTransform handle;

        public float Horizontal { get { return inputVector.x; } }
        public float Vertical { get { return inputVector.y; } }
        public Vector2 Direction { get { return new Vector2(Horizontal, Vertical); } }

        Vector2 joystickPosition = Vector2.zero;

        void Start()
        {
            joystickPosition = RectTransformUtility.WorldToScreenPoint(new Camera(),background.position);
        }

        public virtual void OnDrag(PointerEventData eventData)
        {
            Vector2 direction = eventData.position - joystickPosition;
            inputVector = (direction.magnitude > background.sizeDelta.x / 2f) 
                ? direction.normalized : direction / (background.sizeDelta.x / 2f);
            handle.anchoredPosition = (inputVector * background.sizeDelta.x / 2f) * handleLimit;
            InputEventsHandler.Current.JoyStick(inputVector);
        }

        public virtual void OnPointerDown(PointerEventData eventData)
        {
            OnDrag(eventData);

            LeanTween.scale(gameObject, new Vector2 { x = 1.3f, y = 1.3f}, .05f);
        }

        public virtual void OnPointerUp(PointerEventData eventData)
        {
            inputVector = Vector2.zero;

            InputEventsHandler.Current.JoyStick(inputVector);

            handle.anchoredPosition = Vector2.zero;

            LeanTween.scale(gameObject, new Vector2 { x = 1f, y = 1f }, .05f);
        }
    }
}
