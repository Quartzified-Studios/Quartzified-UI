using UnityEngine;
using UnityEngine.EventSystems;

namespace Quartzified.UI
{
    public class UIWindow : UIDraggable, IPointerDownHandler
    {
        public Transform titleBar;
        public WindowCloseOption onClose = WindowCloseOption.DeactivateWindow;
        [Space]
        [Tooltip("Should the [Window Object] be moved to the front of the UI on being selected?")]
        public bool moveToFrontClick;
        [Tooltip("Should the [Window Object] be moved to the front of the UI on being dragged?")]
        public bool moveToFrontDrag;
        public Transform windowObject;

        protected override void Awake()
        {
            window = titleBar != null ? titleBar : transform.parent;
        }

        public void OnClose()
        {
            window.SendMessage("OnWindowClose", SendMessageOptions.DontRequireReceiver);

            if (onClose == WindowCloseOption.DeactivateWindow)
                window.gameObject.SetActive(false);

            if (onClose == WindowCloseOption.DestroyWindow)
                Destroy(window.gameObject);
        }

        public override void HandleDrag(PointerEventData d)
        {
            if (moveToFrontDrag && windowObject != null)
            {
                windowObject.SetAsLastSibling();
            }

            base.HandleDrag(d);
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            if (moveToFrontClick && windowObject != null)
            {
                windowObject.SetAsLastSibling();
            }
        }
    }
}