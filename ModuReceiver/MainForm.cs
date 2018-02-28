using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Drawing;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ModuReceiver
{
    public partial class MainForm : Form
    {
        private static int sClientID = 0;

        private GazeNetClient.Pointer.Collection iPointers = new GazeNetClient.Pointer.Collection();
        private TcpListener iServer = null;
        private List<TcpClient> iClients = new List<TcpClient>();

        private Thread iServerThread;

        private delegate void UpdateConnectionCount();
        private delegate void CreatePointer();
        private delegate void UpdatePointerlocation(GazeNetClient.Pointer.Pointer aPointer, PointF aLocation);

        public MainForm()
        {
            InitializeComponent();

            GazeNetClient.Pointer.Collection.VisilityFollowsDataAvailability = false;

            txbPort.Text = ModuReceiver.Properties.Settings.Default.Port.ToString();
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
                        lblConnectionCount.Invoke(new UpdateConnectionCount(() =>
                        {
                            lblConnectionCount.Text = iClients.Count.ToString();
                        }));

                        new Thread(new ParameterizedThreadStart(ClientRunner)).Start(client);
                    }
                }
                catch { }
            }
        }

        private void ClientRunner(object aClient)
        {
            TcpClient client = aClient as TcpClient;

            lock (iClients)
            {
                iClients.Add(client);
            }

            NetworkStream stream = client.GetStream();

            int id = ++sClientID;

            txbDebug.Invoke(new UpdateConnectionCount(() => {
                txbDebug.AppendText(string.Format("Client {0} connected\r\n", id));
            }));

            GazeNetClient.Pointer.Pointer pointer = null;
            this.Invoke(new CreatePointer(() => {
                pointer = new GazeNetClient.Pointer.Pointer(id - 1);
                pointer.show();
            }));

            try
            {
                byte[] buffer = new byte[256];
                int i;
                while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string data = Encoding.ASCII.GetString(buffer, 0, i);
                    string[] coords = data.Split(' ');

                    try
                    {
                        int x = int.Parse(coords[0]);
                        int y = int.Parse(coords[1]);
                        if (pointer == null)
                            continue;
                        else
                            this.Invoke(new UpdatePointerlocation((p, location) => {
                                p.moveTo(location);
                            }), pointer, new PointF(x, y));
                    }
                    catch { }
                }
            }
            catch { }

            lock (client)
            {
                if (client.Client != null)
                {
                    client.Close();
                }
            }

            lock (iClients)
            {
                iClients.Remove(client);
            }

            if (iServerThread.ThreadState == ThreadState.Running)
            {
                this.Invoke(new CreatePointer(() => {
                    pointer.hide();
                }));

                txbDebug.Invoke(new UpdateConnectionCount(() =>
                {
                    txbDebug.AppendText(string.Format("Client {0} disconnected\r\n", id));
                }));

                lblConnectionCount.Invoke(new UpdateConnectionCount(() =>
                {
                    lblConnectionCount.Text = iClients.Count.ToString();
                }));
            }
        }

        private void StartServer(int aPort)
        {
            try
            {
                iServer = TcpListener.Create(aPort);
            }
            catch (Exception ex)
            {
                txbDebug.AppendText(ex.Message + "\r\n");
                return;
            }

            iServerThread = new Thread(new ThreadStart(ListenerRunner));
            iServerThread.Start();

            btnStartClose.Text = "Close";
            txbPort.Enabled = false;

            this.ClientSize = new Size(this.ClientSize.Width, this.ClientSize.Height + pnlInfo.Height + 8);
            pnlInfo.Visible = true;
            pnlInfo.Anchor |= AnchorStyles.Bottom | AnchorStyles.Right;
        }

        private void btnStartClose_Click(object sender, EventArgs e)
        {
            if (iServer == null)
            {
                try
                {
                    StartServer(int.Parse(txbPort.Text));
                }
                catch (Exception ex)
                {
                    txbDebug.AppendText(ex.Message + "\r\n");
                }
            }
            else
            {
                MainForm_FormClosed(null, null);
                Close();
            }
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            iServer.Stop();

            lock (iClients)
            {
                foreach (var client in iClients)
                {
                    lock (client)
                    {
                        if (client.Client != null)
                        {
                            client.Close();
                        }
                    }
                }
            }

            iServerThread.Abort();
            iServerThread.Join();

            int port;
            if (int.TryParse(txbPort.Text, out port))
            {
                ModuReceiver.Properties.Settings.Default.Port = port;
            }
            ModuReceiver.Properties.Settings.Default.Save();
        }
    }
}
