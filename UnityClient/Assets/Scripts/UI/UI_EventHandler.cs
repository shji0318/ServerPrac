using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_EventHandler : MonoBehaviour,IPointerClickHandler, IPointerEnterHandler, IDragHandler
{
    public Action<PointerEventData> _clickEventHandler;
    public Action<PointerEventData> _enterEventHandler;
    public Action<PointerEventData> _dragEventHandler;
    

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_clickEventHandler != null)
            _clickEventHandler.Invoke(eventData);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_enterEventHandler != null)
            _enterEventHandler.Invoke(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if(_dragEventHandler != null)
            _dragEventHandler.Invoke(eventData);
    }

    
}
