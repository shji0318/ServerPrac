using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace Server.InGameContents
{
    class BattleStage : JobSerializer
    {
        public ClientSession[] Players { get; set; } = new ClientSession[2];
        public int LeftCount { get; set; } = 0;
        public int RightCount { get; set; } = 0;
        public Champion[] FirstChampions { get; set; } = new Champion[4];
        public Champion[] SecondChampions { get; set; } = new Champion[4];
        public bool LeftBan { get; set; } = false;
        public bool RightBan { get; set; } = false;
    }
}
