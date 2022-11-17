using UnityEngine;

public class Panel : MonoBehaviour
{
    [Header("Panel")]
    [Tooltip("Panel Transform that will be handled")]
    public Transform mainPanel;
    [Space]
    [Tooltip("Whether the Object is allowed to moved up and down the hierarchy.")]
    public bool canMove;

    /// <summary>
    /// Moves the Main Panel to the front of the UI
    /// </summary>
    public void MoveToTop()
    {
        if(canMove)
        {
            mainPanel.SetAsLastSibling();
        }
    }

    /// <summary>
    /// Moves the Main Panel to the back of the UI
    /// </summary>
    public void MoveToBottom()
    {
        if (canMove)
        {
            this.transform.SetAsFirstSibling();
        }
    }


#if UNITY_EDITOR

    private void OnValidate()
    {
        if(mainPanel == null)
        {
            mainPanel = this.transform;
        }
    }
#endif
}
