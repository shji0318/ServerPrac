using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class PacketHandler
{
	public static void S_EnterGameHandler(PacketSession session, IMessage packet)
	{
		S_EnterGame enterPacket = packet as S_EnterGame;			
	}
	public static void S_LeaveGameHandler(PacketSession session, IMessage packet)
	{
		S_LeaveGame leavePacket = packet as S_LeaveGame;
	}
	public static void S_MatchingHandler(PacketSession session, IMessage packet)
	{
		S_Matching matchingPacket = packet as S_Matching;
	}
	public static void S_CancleMatchingHandler(PacketSession session, IMessage packet)
	{
		S_CancleMatching canclePacket = packet as S_CancleMatching;
	}
	public static void S_StartBanPickHandler(PacketSession session, IMessage packet)
	{
		S_StartBanPick startBanPacket = packet as S_StartBanPick;

		MatchingSystem.StartBanPick(startBanPacket.Key,startBanPacket.Dir,startBanPacket.MyTurn);
	}
	public static void S_EndBanPickHandler(PacketSession session, IMessage packet)
	{
		S_EndBanPick endBanPacket = packet as S_EndBanPick;

		UI_BattleScene battle = UIMananger.UI.GetSceneUI("BattleScene").GetComponent<UI_BattleScene>();
		battle.gameObject.SetActive(true);

		battle.ChangeImage(endBanPacket.MyChamp, endBanPacket.EnemyChamp);
	}
	public static void S_SelectChampHandler(PacketSession session, IMessage packet)
	{
		S_SelectChamp selectChampPacket = packet as S_SelectChamp;

		UI_BanPickScene banPick = UIMananger.UI.GetSceneUI("BanPickScene").GetComponent<UI_BanPickScene>();

		if (banPick == null)
		{
			Debug.Log($"Failed SelectChamp PacketHadler.cs");
			return;
		}

		if(!selectChampPacket.Champ.Name.Equals(string.Empty))
			banPick.SelectedChamp(selectChampPacket.Champ.Name, false);

		banPick.MyTurn = selectChampPacket.MyTurn;
	}
	public static void S_SelectBanHandler(PacketSession session, IMessage packet)
	{
		S_SelectBan selectBanPacket = packet as S_SelectBan;
		ServerSession serverSession = session as ServerSession;

		UI_BanPickScene banPick = UIMananger.UI.GetSceneUI("BanPickScene").GetComponent<UI_BanPickScene>();
		banPick.SelectBan();
	}
	public static void S_EndGameHandler(PacketSession session, IMessage packet)
	{
		S_EndGame endGamePacket = packet as S_EndGame;
		ServerSession serverSession = session as ServerSession;
		
	}
}
