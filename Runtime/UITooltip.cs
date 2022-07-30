using Quartzified.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UITooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject tooltipPrefab;
    [TextArea(1, 30)] public string text = "";

    GameObject current;

    public bool IsVisible() => current != null;

    void Update()
    {
        if (current)
            current.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    void ShowToolTip(float delay)
    {
        Invoke(nameof(CreateToolTip), delay);
    }

    public void OnPointerEnter(PointerEventData d)
    {
        ShowToolTip(0.5f);
    }

    void CreateToolTip()
    {
        if (tooltipPrefab == null)
        {
            Debug.LogWarning("Tooltip Prefab is not set!");
            return;
        }

        current = Instantiate(tooltipPrefab, transform.position, Quaternion.identity);

        Transform uiParent = transform.root;

        current.transform.SetParent(uiParent, true);
        current.transform.SetAsLastSibling();

        current.GetComponentInChildren<TextMeshProUGUI>().text = text;

        StartCoroutine(PositionTooltip(current));
    }

    // Reposition the tooltip after its Content Sizer did its magic
    // Wait For Next Frame required to detect changes in rect transform
    IEnumerator PositionTooltip(GameObject current)
    {
        yield return new WaitForEndOfFrame();

        RectTransform rect = current.GetComponent<RectTransform>();

        float offsetX = (transform.GetComponent<RectTransform>().GetSize().x / 2f) + (rect.GetWidth() / 2f) - 4f;
        float offsetY = (transform.GetComponent<RectTransform>().GetSize().y / 2f) + (rect.GetHeight() / 2f) - 4f;

        rect.position = new Vector3(current.transform.position.x + offsetX, current.transform.position.y - offsetY, 0);
    }

    #region Destroy

    void DestroyToolTip()
    {
        CancelInvoke(nameof(CreateToolTip));

        Destroy(current);
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
