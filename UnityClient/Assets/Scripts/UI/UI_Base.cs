using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_Base : MonoBehaviour
{
    [SerializeField]
    protected Dictionary<Type, UnityEngine.Object[]> _dic = new Dictionary<Type, UnityEngine.Object[]>();

    protected void Bind<T> (Type type) where T : UnityEngine.Object
    {
        string[] names = type.GetEnumNames();

        UnityEngine.Object[] objs = new UnityEngine.Object[names.Length];

        if(typeof(T) == typeof(GameObject))
        {
            for (int i = 0; i < names.Length; i++)
            {
                objs[i] = gameObject.transform.Find(names[i]).gameObject;
            }
        }
        else
        {
            for (int i = 0; i < names.Length; i++)
            {
                objs[i] = gameObject.Find<T>(names[i], true);
            }
        }        

        _dic.Add(typeof(T), objs);
    }
    

    protected T Get<T>(int idx) where T : UnityEngine.Object
    {
        return _dic[typeof(T)][idx] as T;
    }

    protected T Get<T>(Type type,string name) where T : UnityEngine.Object
    {
        string[] names = type.GetEnumNames();
        
        for(int i = 0; i < names.Length; i++)
        {
            if(names[i].Equals(name))
            {
                return _dic[typeof(T)][i] as T;
            }
        }
        return null;
    }



    public enum UIEvent
    {
        Click,
        Drag,
    }

    protected void AddUIEvent(GameObject go, Action<PointerEventData> action, UIEvent type = UIEvent.Click ) 
    {
        UI_EventHandler uiEvent = go.GetOrAddComponent<UI_EventHandler>();
        
        switch (type)
        {
            case UIEvent.Click:
                uiEvent._clickEventHandler -= action;
                uiEvent._clickEventHandler += action;
                break;
            case UIEvent.Drag:
                uiEvent._dragEventHandler -= action;
                uiEvent._dragEventHandler += action;
                break;
        }
    }

    protected void InitUIEvent(GameObject go , UIEvent type = UIEvent.Click)
    {
        UI_EventHandler uiEvent = go.GetOrAddComponent<UI_EventHandler>();

        switch (type)
        {
            case UIEvent.Click:
                uiEvent._clickEventHandler = null;
                break;
            case UIEvent.Drag:
                uiEvent._dragEventHandler = null;
                break;
        }
    }
}
