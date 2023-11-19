﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Protobuf;
using Google.Protobuf.Protocol;
using Google.Protobuf.WellKnownTypes;
using Server.InGameContents;
using ServerCore;

namespace Server
{	
	class Program
	{
		static Listener _listener = new Listener();
		static List<System.Timers.Timer> _timers = new List<System.Timers.Timer>();

		static void TickFlush(int tick = 100)
		{
			var timer = new System.Timers.Timer();
			timer.Interval = tick;
			timer.Elapsed += ((s, e) => { MatchingManager.Instance.Update(); });
			timer.AutoReset = true;
			timer.Enabled = true;

			_timers.Add(timer);
		}
		static void Main(string[] args)
		{
			TickFlush(50);

			// DNS (Domain Name System)
			string host = Dns.GetHostName();
			IPHostEntry ipHost = Dns.GetHostEntry(host);
			IPAddress ipAddr = ipHost.AddressList[0];
			IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

			_listener.Init(endPoint, () => { return SessionManager.Instance.Generate(); });
			Console.WriteLine("Listening...");

			//FlushRoom();
			//JobTimer.Instance.Push(FlushRoom);

			while (true)
			{
				Thread.Sleep(100);
			}
		}
	}
}
