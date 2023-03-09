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

            _listener.Init(endPoint, () => { return new ClientSession(); });
            Console.WriteLine("Listening...");


            while (true)
            {
                ;
            }
        }
    }
}
