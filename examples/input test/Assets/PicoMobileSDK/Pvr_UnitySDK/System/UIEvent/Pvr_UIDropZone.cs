using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Pvr_UIDropZone : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    protected Pvr_UIDraggableItem droppableItem;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag)
        {
            var dragItem = eventData.pointerDrag.GetComponent<Pvr_UIDraggableItem>();
            if (dragItem && dragItem.restrictToDropZone)
            {
                dragItem.validDropZone = gameObject;
                droppableItem = dragItem;
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (droppableItem)
        {
            droppableItem.validDropZone = null;
        }
        droppableItem = null;
    }
    
}
