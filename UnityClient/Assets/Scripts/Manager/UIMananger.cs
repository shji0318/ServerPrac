using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMananger : UI_Base
{
    static UIMananger _instance;
    public static UIMananger UI { get { return _instance; } }

    public GameObject GetSceneUI(string name)
    {
        string[] names = typeof(Canvases).GetEnumNames();

        for(int i = 0; i < names.Length; i++)
        {
            if(names[i].Equals(name))
                return Get<Canvas>(i).gameObject;
        }
        return null;
    }

    public enum Canvases
    {
        MainScene,
        BanPickScene,
        BattleScene,
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this);
        }
        else
            Destroy(this.gameObject);

        Bind<Canvas>(typeof(Canvases));
    }

}
