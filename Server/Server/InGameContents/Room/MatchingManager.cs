using Google.Protobuf.Protocol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Google.Protobuf;

namespace Server.InGameContents
{
    public class MatchingManager : JobSerializer
    {
        Dictionary<int, BattleStage> _stages = new Dictionary<int, BattleStage>();
        
        public static MatchingManager Instance { get; } = new MatchingManager();
        Queue<ClientSession> _matchingQueue = new Queue<ClientSession>();

        public void Update()
        {            
            Flush();
        }

        //매칭
        public void Matching(int id)
        {
            ClientSession client = SessionManager.Instance.Find(id);
            
            _matchingQueue.Enqueue(client);
            Console.WriteLine($"Matching {id}, Count {_matchingQueue.Count}");
            //매칭 큐에 플레이어가 2명 이상일 때만 방에 입장
            if (_matchingQueue.Count < 2)
                return;

            Push(StartBanPick);
        }

        //매칭 취소
        public void CancleMatching(int id)
        {
            ClientSession client = SessionManager.Instance.Find(id);

            if (client == null)
                return;

            _matchingQueue = new Queue<ClientSession>(_matchingQueue.Where((x) => client != x));
            Console.WriteLine($"CancleMatching {client.SessionId} Player, {_matchingQueue.Count}");
        }

        //밴픽 방에 입장 
        //첫번째 플레이어의 Id를 기준으로 방 생성
        public void StartBanPick()
        {   
            ClientSession player1 = _matchingQueue.Dequeue();
            ClientSession player2 = _matchingQueue.Dequeue();

            Console.WriteLine($"Start Matching {player1.SessionId}, {player2.SessionId}");
            BattleStage stage = new BattleStage();
            stage.Players[0] = player1;
            stage.Players[1] = player2;

            _stages.Add(player1.SessionId, stage);

            S_StartBanPick startPacket1 = new S_StartBanPick();
            S_StartBanPick startPacket2 = new S_StartBanPick();

            startPacket1.Dir = false;
            startPacket1.Key = player1.SessionId;
            startPacket1.MyTurn = true;

            startPacket2.Dir = true;
            startPacket2.Key = player1.SessionId;
            startPacket2.MyTurn = false;

            player1.Send(startPacket1);
            player2.Send(startPacket2);
        }

        //영웅 선택
        public void SelectChamp(int key, Champion champ, bool dir)
        {
            BattleStage stage = null;
            if (_stages.TryGetValue(key, out stage) == false)
            {
                Console.WriteLine($"SelectChamp Failed{key}");
                return;
            }

            S_SelectChamp selectPacket1 = new S_SelectChamp() { Champ = new Champion() };
            S_SelectChamp selectPacket2 = new S_SelectChamp() { Champ = new Champion() };

            if (dir == false)
            {
                stage.FirstChampions[stage.LeftCount++] = champ;
                selectPacket2.Champ = champ;                
            }
            else
            {
                stage.SecondChampions[stage.RightCount++] = champ;
                selectPacket1.Champ = champ;
            }

            selectPacket1.Dir = false;
            selectPacket1.Key = key;
            selectPacket1.MyTurn = stage.LeftCount < stage.RightCount;

            selectPacket2.Dir = true;
            selectPacket2.Key = key;
            selectPacket2.MyTurn = !selectPacket1.MyTurn;

            stage.Players[0].Send(selectPacket1);
            stage.Players[1].Send(selectPacket2);


            if (stage.LeftCount == 2 && stage.RightCount == 2)
                Push(StartBan,key);
        }

        // 영웅 밴
        public void StartBan(int key)
        {
            S_SelectBan selectBanPacket = new S_SelectBan();
            BattleStage stage = null;
            if (_stages.TryGetValue(key, out stage) == false)
            {
                Console.WriteLine($"SelectChamp Failed{key}");
                return;
            }

            stage.Players[0].Send(selectBanPacket);
            stage.Players[1].Send(selectBanPacket);
        }

        public void SelectBan(int key, int ban, bool dir)
        {
            BattleStage stage = null;
            if (_stages.TryGetValue(key, out stage) == false)
            {
                Console.WriteLine($"SelectChamp Failed{key}");
                return;
            }

            if(dir == false)
            {
                stage.FirstChampions[ban - 1] = null;
                stage.LeftBan = true;
            }
            else
            {
                stage.SecondChampions[ban - 1] = null;
                stage.RightBan = true;
            }

            if (stage.LeftBan && stage.RightBan)
                Push(EndBanPick,key);
        }

        public void EndBanPick(int key)
        {
            BattleStage stage = null;
            if (_stages.TryGetValue(key, out stage) == false)
            {
                Console.WriteLine($"SelectChamp Failed{key}");
                return;
            }

            S_EndBanPick firstEndBanPacket = new S_EndBanPick() { MyChamp = new Champion(), EnemyChamp = new Champion() };
            S_EndBanPick secondEndBanPacket = new S_EndBanPick() { MyChamp = new Champion(), EnemyChamp = new Champion() };

            for(int i = 0; i<stage.FirstChampions.Length;i++)
            {
                if (stage.FirstChampions[i] == null)
                    continue;

                firstEndBanPacket.MyChamp = stage.FirstChampions[i];
                secondEndBanPacket.EnemyChamp = stage.FirstChampions[i];
            }

            for (int i = 0; i < stage.SecondChampions.Length; i++)
            {
                if (stage.SecondChampions[i] == null)
                    continue;

                firstEndBanPacket.EnemyChamp = stage.SecondChampions[i];
                secondEndBanPacket.MyChamp = stage.SecondChampions[i];
            }

            stage.Players[0].Send(firstEndBanPacket);
            stage.Players[1].Send(secondEndBanPacket);
        }
    }
}
