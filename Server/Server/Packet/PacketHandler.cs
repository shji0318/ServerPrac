using Google.Protobuf;
using Google.Protobuf.Protocol;
using Server;
using Server.InGameContents;
using ServerCore;
using System;
using System.Collections.Generic;
using System.Text;

class PacketHandler
{
	public static void C_MatchingHandler(PacketSession session, IMessage packet)
	{
		C_Matching matchingPacket = packet as C_Matching;
		ClientSession clientSession = session as ClientSession;

		int id = clientSession.SessionId;

		MatchingManager.Instance.Push(MatchingManager.Instance.Matching, id);

	}
	public static void C_CancleMatchingHandler(PacketSession session, IMessage packet)
	{
		C_CancleMatching canclePacket = packet as C_CancleMatching;
		ClientSession clientSession = session as ClientSession;

		int id = clientSession.SessionId;

		MatchingManager.Instance.Push(MatchingManager.Instance.CancleMatching,id);
	}
	public static void C_StartBanPickHandler(PacketSession session, IMessage packet)
	{
		C_StartBanPick startBanPacket = packet as C_StartBanPick;
		ClientSession clientSession = session as ClientSession;
		
	}
	public static void C_EndBanPickHandler(PacketSession session, IMessage packet)
	{
		C_EndBanPick endBanPacket = packet as C_EndBanPick;
		ClientSession clientSession = session as ClientSession;

	}
	public static void C_SelectChampHandler(PacketSession session, IMessage packet)
	{
		C_SelectChamp selectChampPacket = packet as C_SelectChamp;
		ClientSession clientSession = session as ClientSession;

		int key = selectChampPacket.Key;
		Champion champion = selectChampPacket.Champ;
		

		bool dir = selectChampPacket.Dir;

		MatchingManager.Instance.Push(MatchingManager.Instance.SelectChamp, key, champion, dir);
	}

	public static void C_SelectBanHandler(PacketSession session, IMessage packet)
	{
		C_SelectBan selectBanPacket = packet as C_SelectBan;
		ClientSession clientSession = session as ClientSession;
		int key = selectBanPacket.Key;
		int ban = selectBanPacket.Ban;
		bool dir = selectBanPacket.Dir;
		MatchingManager.Instance.Push(MatchingManager.Instance.SelectBan, key, ban, dir);
	}
}
