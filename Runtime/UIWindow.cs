using UnityEngine;

namespace Clockwork.UI
{
    public class UIWindow : UIDraggable
    {
        public Transform titleBar;
        public WindowCloseOption onClose = WindowCloseOption.DeactivateWindow;

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
    }
}