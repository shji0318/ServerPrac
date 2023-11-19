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
		_onRecv.Add((ushort)MsgId.CMatching, MakePacket<C_Matching>);
		_handler.Add((ushort)MsgId.CMatching, PacketHandler.C_MatchingHandler);		
		_onRecv.Add((ushort)MsgId.CCancleMatching, MakePacket<C_CancleMatching>);
		_handler.Add((ushort)MsgId.CCancleMatching, PacketHandler.C_CancleMatchingHandler);		
		_onRecv.Add((ushort)MsgId.CStartBanPick, MakePacket<C_StartBanPick>);
		_handler.Add((ushort)MsgId.CStartBanPick, PacketHandler.C_StartBanPickHandler);		
		_onRecv.Add((ushort)MsgId.CEndBanPick, MakePacket<C_EndBanPick>);
		_handler.Add((ushort)MsgId.CEndBanPick, PacketHandler.C_EndBanPickHandler);		
		_onRecv.Add((ushort)MsgId.CSelectChamp, MakePacket<C_SelectChamp>);
		_handler.Add((ushort)MsgId.CSelectChamp, PacketHandler.C_SelectChampHandler);		
		_onRecv.Add((ushort)MsgId.CSelectBan, MakePacket<C_SelectBan>);
		_handler.Add((ushort)MsgId.CSelectBan, PacketHandler.C_SelectBanHandler);
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