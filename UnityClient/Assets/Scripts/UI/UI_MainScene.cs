using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_MainScene : UI_Base
{
    public enum Buttons
    {
        StartMatchingButton,
        CancleMatchingButton,
    }
    void Awake()
    {
        Bind<Button>(typeof(Buttons));
    }

    public void Start()
    {        
        AddUIEvent(Get<Button>(
            (int)Buttons.StartMatchingButton).gameObject,
            (PointerEventData p) =>
            {
                MatchingSystem.Matching();
                Get<Button>((int)Buttons.StartMatchingButton).gameObject.SetActive(false);
                Get<Button>((int)Buttons.CancleMatchingButton).gameObject.SetActive(true);
            },
            UIEvent.Click);
        AddUIEvent(Get<Button>((int)Buttons.CancleMatchingButton).gameObject,
            (PointerEventData p) =>
            {
                MatchingSystem.CancleMatching();                
                Get<Button>((int)Buttons.CancleMatchingButton).gameObject.SetActive(false);
                Get<Button>((int)Buttons.StartMatchingButton).gameObject.SetActive(true);
            },
            UIEvent.Click);
    }


}
