using ServerCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using Google.Protobuf;

public class NetworkManager : MonoBehaviour
{
	static NetworkManager _instance;
	public static NetworkManager Network { get {return _instance;} }

	ServerSession _session = new ServerSession();

	public void Send(IMessage packet)
	{
		_session.Send(packet);
	}
    public void Awake()
    {
		if (_instance == null)
		{
			_instance = this;
			DontDestroyOnLoad(this);
			Init();
		}
		else
			Destroy(this.gameObject);	
    }

	public void Update()
	{
		List<PacketMessage> list = PacketQueue.Instance.PopAll();
		foreach (PacketMessage packet in list)
		{
			Action<PacketSession, IMessage> handler = PacketManager.Instance.GetPacketHandler(packet.Id);
			if (handler != null)
				handler.Invoke(_session, packet.Message);
		}
	}

	public void Init()
	{
		// DNS (Domain Name System)
		string host = Dns.GetHostName();
		IPHostEntry ipHost = Dns.GetHostEntry(host);
		IPAddress ipAddr = ipHost.AddressList[0];
		IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

		Connector connector = new Connector();

		connector.Connect(endPoint,
			() => { return _session; },
			1);
	}

}
