using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragDrop : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake(){
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        if(canvas == null){ 
            canvas = FindObjectOfType<Canvas>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData){
        DetachFromSlot();
    }

    public void OnDrag(PointerEventData eventData){
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData){
        EndDrag();
    }

    public void DetachFromSlot(){
        DragDropSlot slot = GetComponentInParent<DragDropSlot>();
        if(slot) { slot.itemInSlot = null; }
        this.gameObject.transform.SetParent(canvas.gameObject.transform);
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void EndDrag(){
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
}
