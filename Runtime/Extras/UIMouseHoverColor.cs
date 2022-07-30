using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Quartzified.UI
{
    public class UIMouseHoverColor : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public Image image;
        public Color highlightColor = Color.white;
        Color defaultColor;

        public void OnPointerEnter(PointerEventData pointerEventData)
        {
            defaultColor = image.color;
            image.color = highlightColor;
        }

        public void OnPointerExit(PointerEventData pointerEventData)
        {
            image.color = defaultColor;
        }
    }
}
