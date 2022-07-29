using UnityEngine;
using UnityEngine.EventSystems;

namespace Clockwork.UI
{
    public partial class UIDraggable : MonoBehaviour, IDraggable
    {
        protected Transform window;

        protected virtual void Awake()
        {
            window = transform;
        }

        public void HandleDrag(PointerEventData d)
        {
            window.SendMessage("OnWindowDrag", d, SendMessageOptions.DontRequireReceiver);

            window.Translate(d.delta);
        }
    }
}