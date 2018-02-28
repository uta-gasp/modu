using System.Collections.Generic;
using System.Net.Sockets;

namespace Modu
{
    public class ReceiverClient
    {
        public TcpClient Socket { get; private set; }
        public int ID { get; private set; }
        public GazeNetClient.Pointer.Pointer Pointer { get; set; } = null;

        public ReceiverClient(TcpClient aSocket, int aID)
        {
            Socket = aSocket;
            ID = aID;
        }

        public static int GetNextID(ReceiverClient[] aList)
        {
            int result = 0;
            List<int> existingIDs = new List<int>(aList.Length);

            foreach (var client in aList)
            {
                existingIDs.Add(client.ID);
            }

            existingIDs.Sort();

            foreach (var client in aList)
            {
                if (client.ID > result)
                    break;
                result++;
            }

            return result;
        }
    }
}
