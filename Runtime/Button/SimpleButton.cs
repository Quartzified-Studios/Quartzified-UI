using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Quartzified.UI
{
    [RequireComponent(typeof(RectTransform))]
    public class SimpleButton : Selectable, IPointerClickHandler
    {
        [Space]
        public UnityEvent OnClick;
        public UnityEvent OnRightClick;

        public void PressLeft()
        {
            if (!IsActive() || !IsInteractable())
                return;

            OnClick?.Invoke();
        }

        public void PressRight()
        {
            if (!IsActive() || !IsInteractable())
                return;

            OnRightClick?.Invoke();
        }

        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                PressLeft();
                return;
            }

            if (eventData.button == PointerEventData.InputButton.Right)
            {
                PressRight();
                return;
            }

        }

    }
}