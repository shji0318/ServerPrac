using Google.Protobuf;
using Google.Protobuf.Protocol;
using ServerCore;
using System;
using System.Collections.Generic;

class PacketManager
{
	#region Singleton
	static PacketManager _instance = new PacketManager();
	public static PacketManager Instance { get { return _instance; } }
	#endregion

	PacketManager()
	{
		Register();
	}

	Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>> _onRecv = new Dictionary<ushort, Action<PacketSession, ArraySegment<byte>, ushort>>();
	Dictionary<ushort, Action<PacketSession, IMessage>> _handler = new Dictionary<ushort, Action<PacketSession, IMessage>>();
		
	public Action<PacketSession, IMessage, ushort> CustomHandler { get; set; }
	public void Register()
	{		
		_onRecv.Add((ushort)MsgId.SEnterGame, MakePacket<S_EnterGame>);
		_handler.Add((ushort)MsgId.SEnterGame, PacketHandler.S_EnterGameHandler);		
		_onRecv.Add((ushort)MsgId.SLeaveGame, MakePacket<S_LeaveGame>);
		_handler.Add((ushort)MsgId.SLeaveGame, PacketHandler.S_LeaveGameHandler);		
		_onRecv.Add((ushort)MsgId.SMatching, MakePacket<S_Matching>);
		_handler.Add((ushort)MsgId.SMatching, PacketHandler.S_MatchingHandler);		
		_onRecv.Add((ushort)MsgId.SCancleMatching, MakePacket<S_CancleMatching>);
		_handler.Add((ushort)MsgId.SCancleMatching, PacketHandler.S_CancleMatchingHandler);		
		_onRecv.Add((ushort)MsgId.SStartBanPick, MakePacket<S_StartBanPick>);
		_handler.Add((ushort)MsgId.SStartBanPick, PacketHandler.S_StartBanPickHandler);		
		_onRecv.Add((ushort)MsgId.SEndBanPick, MakePacket<S_EndBanPick>);
		_handler.Add((ushort)MsgId.SEndBanPick, PacketHandler.S_EndBanPickHandler);		
		_onRecv.Add((ushort)MsgId.SSelectChamp, MakePacket<S_SelectChamp>);
		_handler.Add((ushort)MsgId.SSelectChamp, PacketHandler.S_SelectChampHandler);		
		_onRecv.Add((ushort)MsgId.SSelectBan, MakePacket<S_SelectBan>);
		_handler.Add((ushort)MsgId.SSelectBan, PacketHandler.S_SelectBanHandler);		
		_onRecv.Add((ushort)MsgId.SEndGame, MakePacket<S_EndGame>);
		_handler.Add((ushort)MsgId.SEndGame, PacketHandler.S_EndGameHandler);
	}

	public void OnRecvPacket(PacketSession session, ArraySegment<byte> buffer)
	{
		ushort count = 0;

		ushort size = BitConverter.ToUInt16(buffer.Array, buffer.Offset);
		count += 2;
		ushort id = BitConverter.ToUInt16(buffer.Array, buffer.Offset + count);
		count += 2;

		Action<PacketSession, ArraySegment<byte>, ushort> action = null;
		if (_onRecv.TryGetValue(id, out action))
			action.Invoke(session, buffer, id);
	}

	void MakePacket<T>(PacketSession session, ArraySegment<byte> buffer, ushort id) where T : IMessage, new()
	{
		T pkt = new T();
		pkt.MergeFrom(buffer.Array, buffer.Offset + 4, buffer.Count - 4);
		if(CustomHandler != null)
        {
			CustomHandler.Invoke(session, pkt, id);
        }
		else
        {
			Action<PacketSession, IMessage> action = null;
			if (_handler.TryGetValue(id, out action))
				action.Invoke(session, pkt);
        }
	}

	public Action<PacketSession, IMessage> GetPacketHandler(ushort id)
	{
		Action<PacketSession, IMessage> action = null;
		if (_handler.TryGetValue(id, out action))
			return action;
		return null;
	}
}