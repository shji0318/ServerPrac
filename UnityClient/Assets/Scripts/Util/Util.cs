using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util
{
    public const string objectFormat = @"------ [{0}]";
    public static T GetOrAddComponent<T>(this GameObject go) where T : UnityEngine.Component
    {
        T component = go.GetComponent<T>();

        if(component == null)
        {
            component = go.AddComponent<T>();
        }

        return component as T;
    }

    public static T Find<T> (this GameObject go, string name, bool recursive = false) where T : UnityEngine.Object
    {
        T component = null;

        if(typeof(T) == typeof(GameObject))
        {
            return GameObject.Find(name) as T;
        }
        else
        {
            foreach (T t in go.transform.GetComponentsInChildren<T>(recursive))
            {
                if (t.name.Equals(name))
                {
                    component = t;

                    return component;
                }
            }
        }        

        return component;
    }    

}
