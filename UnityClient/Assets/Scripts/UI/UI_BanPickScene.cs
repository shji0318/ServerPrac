using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_BanPickScene : UI_Base
{
    bool _myTurn;
    ChampionSlot _selectSlot;
    int _banNum;
    
    public int Key { get; set; }
    public bool Dir { get; set; }
    public int MySlotCount { get; set; }
    public int EnemySlotCount { get; set; }
    public bool MyTurn 
    {
        get { return _myTurn; } 
        set 
        {
            ChangeTurn(value);
            _myTurn = value;
        } 
    }
    public enum Buttons
    {
        SelectButton,
        WaitButton,
        SelectBanButton,
        AlenciaSlot,
        RasSlot,
        SenyaSlot,
        VioletSlot,
        Slot1,
        Slot2,
    }

    public enum Images
    {
        MySlot1,
        MySlot2,
        Slot1,
        Slot2,
        MySlotIcon1,
        MySlotIcon2,
        SlotIcon1,
        SlotIcon2,
        AlenciaSlot,
        RasSlot,
        SenyaSlot,
        VioletSlot,
        PopUpImage,
        VSImage,
    }

    public enum ChampionSlots
    {
        AlenciaSlot,
        RasSlot,
        SenyaSlot,
        VioletSlot,
    }

    public void Awake()
    {
        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<ChampionSlot>(typeof(ChampionSlots));
        Init();
    }

    public void OnEnable()
    {
        ReFresh();
    }

    public void Start()
    {
        AddUIEvent(Get<Button>((int)Buttons.AlenciaSlot).gameObject,
            (p) =>
            {
                _selectSlot = Get<ChampionSlot>((int)ChampionSlots.AlenciaSlot);                
                PopUpImage("Alencia");
            },
            UIEvent.Click);
        AddUIEvent(Get<Button>((int)Buttons.RasSlot).gameObject,
            (p) =>
            {
                _selectSlot = Get<ChampionSlot>((int)ChampionSlots.RasSlot);
                PopUpImage("Ras");
            },
            UIEvent.Click);
        AddUIEvent(Get<Button>((int)Buttons.SenyaSlot).gameObject,
            (p) =>
            {
                _selectSlot = Get<ChampionSlot>((int)ChampionSlots.SenyaSlot);
                PopUpImage("Senya");
            },
            UIEvent.Click);
        AddUIEvent(Get<Button>((int)Buttons.VioletSlot).gameObject,
            (p) =>
            {
                _selectSlot = Get<ChampionSlot>((int)ChampionSlots.VioletSlot);
                PopUpImage("Violet");
            },
            UIEvent.Click);
        AddUIEvent(Get<Button>((int)Buttons.SelectButton).gameObject,
            (p) =>
            {
                if (_selectSlot == null)
                    return;

                if (_selectSlot.Selected)
                    return;

                SelectedChamp(_selectSlot.Name, true);
                MatchingSystem.SelectChamp(_selectSlot.Name,Key,Dir);
            });
    }

    public void Init()
    {
        Get<ChampionSlot>(typeof(ChampionSlots), $"AlenciaSlot").Name = "Alencia";
        Get<ChampionSlot>(typeof(ChampionSlots), $"RasSlot").Name = "Ras";
        Get<ChampionSlot>(typeof(ChampionSlots), $"SenyaSlot").Name = "Senya";
        Get<ChampionSlot>(typeof(ChampionSlots), $"VioletSlot").Name = "Violet";
    }

  
    public void ChangeTurn(bool myTurn)
    {        
        Get<Image>((int)Images.AlenciaSlot).raycastTarget = myTurn;
        Get<Image>((int)Images.AlenciaSlot).raycastTarget = !Get<ChampionSlot>(typeof(ChampionSlots), $"AlenciaSlot").Selected; 
        Get<Image>((int)Images.RasSlot).raycastTarget = myTurn;
        Get<Image>((int)Images.RasSlot).raycastTarget = !Get<ChampionSlot>(typeof(ChampionSlots), $"RasSlot").Selected;
        Get<Image>((int)Images.SenyaSlot).raycastTarget = myTurn;
        Get<Image>((int)Images.SenyaSlot).raycastTarget = !Get<ChampionSlot>(typeof(ChampionSlots), $"SenyaSlot").Selected;
        Get<Image>((int)Images.VioletSlot).raycastTarget = myTurn;
        Get<Image>((int)Images.VioletSlot).raycastTarget = !Get<ChampionSlot>(typeof(ChampionSlots), $"VioletSlot").Selected;

        if (myTurn == true)
        {
            Get<Button>((int)Buttons.SelectButton).gameObject.SetActive(true);
            Get<Button>((int)Buttons.WaitButton).gameObject.SetActive(false);
        }
        else
        {
            Get<Button>((int)Buttons.SelectButton).gameObject.SetActive(false);
            Get<Button>((int)Buttons.WaitButton).gameObject.SetActive(true);
        }
    }

    public void PopUpImage(string name)
    {
        Sprite sprite = Resources.Load<Sprite>($"Model/{name}Image");
        Image img = Get<Image>((int)Images.PopUpImage);
        img.gameObject.SetActive(true);
        img.sprite = sprite;
    }

    public void SelectedChamp(string name, bool selectSelf)
    {
        Image img = Get<Image>(typeof(Images), $"{name}Slot");
        if(img == null)
        {
            Debug.Log($"Image Error UI_BanPickScene.cs SelectedChamp Name = {name} , selectSelf = {selectSelf}");
        }
        img.color = Color.red;        

        Image IconSlot = null;
        
        if(selectSelf == true)
        {
            IconSlot = Get<Image>(typeof(Images), $"MySlotIcon{MySlotCount++}");            
        }
        else
        {
            IconSlot = Get<Image>(typeof(Images), $"SlotIcon{EnemySlotCount++}");
        }        

        IconSlot.gameObject.SetActive(true);
        IconSlot.sprite = Resources.Load<Sprite>($"Icon/{name}");

        ChampionSlot cs = Get<ChampionSlot>(typeof(ChampionSlots), $"{name}Slot");
        cs.Selected = true;
    }

    public void SelectBan()
    {
        Get<Button>((int)Buttons.SelectButton).gameObject.SetActive(false);
        Get<Button>((int)Buttons.WaitButton).gameObject.SetActive(false);
        Get<Button>((int)Buttons.SelectBanButton).gameObject.SetActive(true);
        Get<Image>((int)Images.Slot1).raycastTarget = true;
        Get<Image>((int)Images.Slot2).raycastTarget = true;
        AddUIEvent(Get<Button>((int)Buttons.Slot1).gameObject,
            (p) =>
            {
                RefreshBanSlotUI();
                Get<Image>((int)Images.Slot1).color = Color.red;
                _banNum = 1;
            },
            UIEvent.Click);

        AddUIEvent(Get<Button>((int)Buttons.Slot2).gameObject,
            (p) =>
            {
                RefreshBanSlotUI();
                Get<Image>((int)Images.Slot2).color = Color.red;
                _banNum = 2;
            },
            UIEvent.Click);

        AddUIEvent(Get<Button>((int)Buttons.SelectBanButton).gameObject,
            (p) =>
            {
                if (_banNum == 0)
                    return;

                MatchingSystem.SelectBan(_banNum);
            },
            UIEvent.Click);
    }

    public void RefreshBanSlotUI()
    {
        Get<Image>((int)Images.Slot1).color = Color.white;
        Get<Image>((int)Images.Slot2).color = Color.white;
    }
    
    public void ReFresh()
    {
        Key = 0;
        Dir = false;
        MyTurn = true;
        MySlotCount = 1;
        EnemySlotCount = 1;
        _banNum = 0;
    }

}
