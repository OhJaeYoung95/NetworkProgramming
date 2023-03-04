using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ServerCore;

namespace NetworkServer
{
    class Knight
    {
        public int hp;
        public int attack;
    }
    class GameSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            Knight knight = new Knight() { hp = 100, attack = 10 };

            byte[] sendBuff = new byte[1024];
            byte[] buffer = BitConverter.GetBytes(knight.hp);
            byte[] buffer2 = BitConverter.GetBytes(knight.attack);
            Array.Copy(buffer, 0, sendBuff, 0, buffer.Length);
            Array.Copy(buffer2, 0, sendBuff, buffer.Length, buffer2.Length);
            // Encoding.UTF8.GetBytes("Welcome to MMORPG Server !");
            
            Send(sendBuff);
            Thread.Sleep(1000);
            Disconnect();

        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Client] {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }

    class Program
    {
        static Listener _listener = new Listener();

        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // www.rookies.com -> 123.123.123.12 / 도메인 이름을 사용하면 IP만 바꿀때 접근하기 용이함

            _listener.Init(endPoint, () => { return new GameSession(); });
            Console.WriteLine("Listening...");


            while (true)
            {
                ;
            }
        }
    }
}
