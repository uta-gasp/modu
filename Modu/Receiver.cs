using System;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Threading;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Modu
{
    public class Receiver
    {
        private GazeNetClient.Pointer.Collection iPointers = new GazeNetClient.Pointer.Collection();
        private TcpListener iServer = null;
        private List<ReceiverClient> iClients = new List<ReceiverClient>();
        private Control iParent;

        private Thread iServerThread;

        private delegate void UISync();
        private delegate void UpdatePointerlocation(GazeNetClient.Pointer.Pointer aPointer, Point aLocation);

        public class ClientCountChangedArgs : EventArgs
        {
            public int Count { get; private set; }
            public ClientCountChangedArgs(int aCount) : base()
            {
                Count = aCount;
            }
        }

        public class DebugArgs
        {
            public string Message { get; private set; }
            public DebugArgs(string aMessage)
            {
                Message = aMessage;
            }
        }

        public event EventHandler<ClientCountChangedArgs> ClientCountChanged = delegate { };
        public event EventHandler<DebugArgs> Debug = delegate { };

        public Receiver(Control aParent)
        {
            iParent = aParent;
            GazeNetClient.Pointer.Collection.VisilityFollowsDataAvailability = false;
        }

        public void Start()
        {
            try
            {
                iServer = TcpListener.Create(Comm.PORT);
            }
            catch (Exception ex)
            {
                Debug(this, new DebugArgs(ex.Message));
                return;
            }

            iServerThread = new Thread(new ThreadStart(ListenerRunner));
            iServerThread.Start();
        }

        public void Stop()
        {
            iServer.Stop();

            lock (iClients)
            {
                foreach (var client in iClients)
                {
                    lock (client)
                    {
                        if (client.Socket.Client != null)
                        {
                            client.Socket.Close();
                        }
                    }
                }
            }

            iServerThread.Abort();
            iServerThread.Join();
        }

        private void ListenerRunner()
        {
            iServer.Start();

            while (iServerThread.ThreadState == ThreadState.Running)
            {
                try
                {
                    TcpClient client = iServer.AcceptTcpClient();
                    if (client != null)
                    {
                        new Thread(new ParameterizedThreadStart(ClientRunner)).Start(client);
                    }
                }
                catch { }
            }
        }

        private void ClientRunner(object aClient)
        {
            TcpClient socket = aClient as TcpClient;

            int clientCount;
            int id;
            ReceiverClient client;
            GazeNetClient.Pointer.Pointer pointer = null;

            lock (iClients)
            {
                id = ReceiverClient.GetNextID(iClients.ToArray());
                client = new ReceiverClient(socket, id);
                iClients.Add(client);
                clientCount = iClients.Count;
            }

            iParent.Invoke(new UISync(() =>
            {
                ClientCountChanged(this, new ClientCountChangedArgs(clientCount));
                Debug(this, new DebugArgs(string.Format("Client {0} connected", id + 1)));

                pointer = new GazeNetClient.Pointer.Pointer(id - 1);
                pointer.show();

                client.Pointer = pointer;
            }));

            NetworkStream stream = socket.GetStream();

            try
            {
                byte[] buffer = new byte[256];
                int i;
                while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    if (pointer == null)
                        continue;

                    string data = Encoding.ASCII.GetString(buffer, 0, i);

                    try
                    {
                        iParent.Invoke(new UpdatePointerlocation((p, location) =>
                        {
                            p.moveTo(location);
                        }), pointer, Comm.Unpack(data));
                    }
                    catch { }
                }
            }
            catch { }

            lock (client)
            {
                if (client.Socket.Client != null)
                {
                    client.Socket.Close();
                }
            }

            lock (iClients)
            {
                iClients.Remove(client);
                clientCount = iClients.Count;
            }

            if (iServerThread.ThreadState == ThreadState.Running)
            {
                iParent.Invoke(new UISync(() => {
                    pointer.hide();
                    Debug(this, new DebugArgs(string.Format("Client {0} disconnected", id + 1)));
                    ClientCountChanged(this, new ClientCountChangedArgs(clientCount));
                }));
            }
        }
    }
}
