using UnityEngine;
using UnityEngine.EventSystems;

namespace Quartzified.UI
{
    public partial class UIDraggable : MonoBehaviour, IDraggable
    {
        protected Transform window;

        protected virtual void Awake()
        {
            window = transform;
        }

        public virtual void HandleDrag(PointerEventData d)
        {
            window.SendMessage("OnWindowDrag", d, SendMessageOptions.DontRequireReceiver);

            window.Translate(d.delta);
        }
    }
}