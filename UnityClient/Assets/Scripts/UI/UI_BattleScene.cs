using Google.Protobuf.Protocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattleScene : UI_Base
{
    public enum Images
    {
        MyPlayerCharacter,
        EnemyPlayerCharacter,
    }

    public void Awake()
    {
        Bind<Image>(typeof(Images));
    }

    public void ChangeImage(Champion myChamp, Champion enemyChamp)
    {
        Get<Image>((int)Images.MyPlayerCharacter).sprite = Resources.Load<Sprite>(myChamp.Path);
        Get<Image>((int)Images.EnemyPlayerCharacter).sprite = Resources.Load<Sprite>(enemyChamp.Path);
    }
}
