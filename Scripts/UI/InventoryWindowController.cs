using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class InventoryWindowController : MonoBehaviour
{

    public RectTransform scrollRectTransform;
    public RectTransform contentPanel;
    public RectTransform selectedRectTransform;
    public GameObject curSelected;
    public GameObject lastSelected;

    public float selectedPositionY;
    public float scrollViewMinY;
    public float scrollViewMaxY;
    public float newY;

    void Start()
    {

    }

    void Update()
    {
        curSelected = EventSystem.current.currentSelectedGameObject;

        selectedRectTransform = curSelected.GetComponent<RectTransform>();

        // The position of the selected UI element is the absolute anchor position,
        // ie. the local position within the scroll rect + its height if we're
        // scrolling down. If we're scrolling up it's just the absolute anchor position.
        selectedPositionY = Mathf.Abs(selectedRectTransform.anchoredPosition.y) + selectedRectTransform.rect.height;
        // The upper bound of the scroll view is the anchor position of the content we're scrolling.
        scrollViewMinY = contentPanel.anchoredPosition.y;
        // The lower bound is the anchor position + the height of the scroll rect.
        scrollViewMaxY = contentPanel.anchoredPosition.y + scrollRectTransform.rect.height;

        // If the selected position is below the current lower bound of the scroll view we scroll down.
        if (selectedPositionY > scrollViewMaxY) {
            newY = selectedPositionY - scrollRectTransform.rect.height;
            contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, newY);
        }
        // If the selected position is above the current upper bound of the scroll view we scroll up.
        else if (Mathf.Abs(selectedRectTransform.anchoredPosition.y) < scrollViewMinY) {
            contentPanel.anchoredPosition = new Vector2(contentPanel.anchoredPosition.x, Mathf.Abs(selectedRectTransform.anchoredPosition.y));
        }

        lastSelected = curSelected;
    }
}