using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Quartzified.UI
{
    public class UITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public GameObject tooltipPrefab;
        [TextArea(1, 30)] public string text = "";

        GameObject current;

        /// <summary>
        /// Returns true if the tooltip is currently active
        /// </summary>
        /// <returns></returns>
        public bool IsActive() => current != null;

        /// <summary>
        /// Returns true if the tooltip is currently active and visible
        /// </summary>
        /// <returns></returns>
        public bool IsVisible() => IsActive() && current.gameObject.activeInHierarchy;


        void ShowToolTip(float delay)
        {
            if(current == null)
            {
                StartCoroutine(CreateToolTip(delay));
            }
        }

        public void OnPointerEnter(PointerEventData d)
        {
            ShowToolTip(0.5f);
        }

        IEnumerator CreateToolTip(float delay)
        {
            yield return new WaitForSeconds(delay);

            if (tooltipPrefab == null)
            {
                Debug.LogWarning("Tooltip Prefab is not set!");
                yield return null;
            }
            else
            {
                current = Instantiate(tooltipPrefab, transform.position, Quaternion.identity);
                current.SetActive(false);
                Transform uiParent = transform.root;

                current.transform.SetParent(uiParent, true);
                current.transform.SetAsLastSibling();

                current.GetComponentInChildren<TextMeshProUGUI>().text = text;

                yield return new WaitForEndOfFrame();

                StartCoroutine(PositionTooltip(current));

                Debug.Log("Enable");

                current.SetActive(true);
            }
        }

        // Reposition the tooltip after its Content Sizer did its magic
        // Wait For Next Frame required to detect changes in rect transform
        IEnumerator PositionTooltip(GameObject current)
        {
            yield return new WaitForEndOfFrame();

            RectTransform rect = current.GetComponent<RectTransform>();
            Vector2 myRectSize = transform.GetComponent<RectTransform>().GetSize();

            float offsetX = (myRectSize.x / 2f) + (rect.GetWidth() / 2f) - 4f;
            float offsetY = (myRectSize.y / 2f) + (rect.GetHeight() / 2f) - 4f;

            rect.position = new Vector3(current.transform.position.x + offsetX, current.transform.position.y - offsetY, 0);

            Debug.Log("Move UI");

            yield return null;
        }

        #region Destroy

        void DestroyToolTip()
        {
            StopAllCoroutines();

            if (current != null)
            {
                Destroy(current);
            }
        }

        public void OnPointerExit(PointerEventData d)
        {
            DestroyToolTip();
        }

        void OnDisable()
        {
            DestroyToolTip();
        }

        void OnDestroy()
        {
            DestroyToolTip();
        }

        #endregion
    }

}
