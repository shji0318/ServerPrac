using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MatchingSystem 
{
    public static void Matching()
    {
        C_Matching matchingPacket = new C_Matching();
        NetworkManager.Network.Send(matchingPacket);
    }
    public static void CancleMatching()
    {
        C_CancleMatching canclePacket = new C_CancleMatching();
        NetworkManager.Network.Send(canclePacket);
    }

    public static void StartBanPick(int key, bool dir,bool myTurn)
    {
        UI_BanPickScene banPick = UIMananger.UI.GetSceneUI("BanPickScene").GetComponent<UI_BanPickScene>();        

        if(banPick == null)
        {
            Debug.Log($"Failed StartBanPick MathchingSystem.cs");
            return;
        }

        banPick.gameObject.SetActive(true);
        banPick.Key = key;
        banPick.Dir = dir;
        banPick.MyTurn = myTurn;
    }   

    public static void SelectChamp(string name,int key, bool dir)
    {
        C_SelectChamp selectPacket = new C_SelectChamp() { Champ = new Champion() };
        Champion champ = null;

        if(ChampData.Data._dic.TryGetValue(name,out champ) == false)
        {
            Debug.Log("Failed SelectChamp MatchingSystem.cs Data Get Value");
            return;
        }

        selectPacket.Champ.Speed = champ.Speed;
        selectPacket.Champ.Attack = champ.Attack;
        selectPacket.Champ.Hp = champ.Hp;
        selectPacket.Champ.Name = champ.Name;
        selectPacket.Champ.Path = champ.Path;
        selectPacket.Key = key;
        selectPacket.Dir = dir;

        NetworkManager.Network.Send(selectPacket);
    }    

    public static void SelectBan(int banNum)
    {        
        UI_BanPickScene banPick = UIMananger.UI.GetSceneUI("BanPickScene").GetComponent<UI_BanPickScene>();

        if (banPick == null)
        {
            Debug.Log($"Failed StartBanPick MathchingSystem.cs");
            return;
        }
        C_SelectBan selectBanPacket = new C_SelectBan();

        selectBanPacket.Ban = banNum;
        selectBanPacket.Key = banPick.Key;
        selectBanPacket.Dir = banPick.Dir;

        NetworkManager.Network.Send(selectBanPacket);
    }
}
