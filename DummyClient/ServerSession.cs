using ServerCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DummyClient
{
    class Packet
    {
        public ushort size;
        public ushort packetId;
    }

    class PlayerInfoReq : Packet
    {
        public long playerId;
    }

    class PlayerInfoOk : Packet
    {
        public int hp;
        public int attack;
    }

    public enum PacketID
    {
        PlayerInfoReq = 1,
        PlayerInfoOk =2,
    }

    class ServerSession : Session
    {
        public override void OnConnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnConnected : {endPoint}");

            PlayerInfoReq packet = new PlayerInfoReq() { size = 4, packetId = (ushort)PacketID.PlayerInfoReq, playerId = 1001};

            // 보낸다
            //for (int i = 0; i < 5; i++)
            {
                ArraySegment<byte> s = SendBufferHelper.Open(4096);
                bool success = true;

                BitConverter.TryWriteBytes()

                byte[] size = BitConverter.GetBytes(packet.size);
                byte[] packetId = BitConverter.GetBytes(packet.packetId);
                byte[] playerId = BitConverter.GetBytes(packet.playerId);

                ushort count = 0;
                Array.Copy(size, 0, s.Array, s.Offset + count, size.Length);
                count += 2;
                Array.Copy(packetId, 0, s.Array, s.Offset + count, packetId.Length);
                count += 2;
                Array.Copy(packetId, 0, s.Array, s.Offset + count, packetId.Length);
                count += 8;

                ArraySegment<byte> sendBuff = SendBufferHelper.Close(count);

                Send(sendBuff);
            }
        }

        public override void OnDisconnected(EndPoint endPoint)
        {
            Console.WriteLine($"OnDisconnected : {endPoint}");
        }

        public override int OnRecv(ArraySegment<byte> buffer)
        {
            string recvData = Encoding.UTF8.GetString(buffer.Array, buffer.Offset, buffer.Count);
            Console.WriteLine($"[From Server] {recvData}");
            return buffer.Count;
        }

        public override void OnSend(int numOfBytes)
        {
            Console.WriteLine($"Transferred bytes : {numOfBytes}");
        }
    }
}
