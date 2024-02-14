using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollView : MonoBehaviour, IBeginDragHandler, IEndDragHandler
{
    public ScrollRect scrollRect;

    public void OnBeginDrag(PointerEventData eventData)
    {
        scrollRect.enabled = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        scrollRect.enabled = true;
    }

    private void Awake() {
        scrollRect.GetComponent<ScrollRect>();
    }
}
