using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Modu
{
    public class Sender
    {
        private TcpClient iSocket;
        private System.Timers.Timer iTimer;
        private Point iMouseLocation = new Point();

        private enum ControlState
        {
            Disconnected,
            Connecting,
            Connected,
            Disconnecting
        }

        public bool IsConnected { get { return iSocket != null; } }

        public class ConnectionAttemptArgs : EventArgs
        {
            public bool Success { get; private set; }
            public string Message { get; private set; }
            public ConnectionAttemptArgs(bool aSuccess, string aMessage = "") : base()
            {
                Success = aSuccess;
                Message = aMessage;
            }
        }

        public event EventHandler<ConnectionAttemptArgs> ConnectionAttempt = delegate { };

        public class ConnectionLostArgs : EventArgs
        {
            public string Message { get; private set; }
            public ConnectionLostArgs(string aMessage) : base()
            {
                Message = aMessage;
            }
        }
        public event EventHandler<ConnectionLostArgs> ConnectionLost = delegate { };

        public Sender()
        {
            iTimer = new System.Timers.Timer(50);
            iTimer.Elapsed += Timer_Elapsed;
        }

        ~Sender()
        {
            if (iSocket?.Client != null)
            {
                iSocket.Close();
            }
        }

        public async void Start(string aIP)
        {
            iSocket = new TcpClient();
            iSocket.NoDelay = true;

            try
            {
                IPAddress addr = IPAddress.Parse(aIP);
                await iSocket.ConnectAsync(addr, Comm.PORT);

                iTimer.Start();

                ConnectionAttempt(this, new ConnectionAttemptArgs(true));
            }
            catch (Exception ex)
            {
                Stop();
                ConnectionAttempt(this, new ConnectionAttemptArgs(false, ex.Message));
            }
        }

        public void Stop()
        {
            if (iSocket != null && iSocket.Connected)
            {
                iSocket.Close();
            }

            iSocket = null;

            iTimer.Stop();
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (iSocket == null)
                return;

            if (!iSocket.Connected && iTimer.Enabled)
            {
                Stop();
                return;
            }

            Point mouseLocation = System.Windows.Forms.Cursor.Position;
            if (iMouseLocation.X == mouseLocation.X && iMouseLocation.Y == mouseLocation.Y)
                return;

            string payload = Comm.Pack(mouseLocation);
            byte[] bytes = Encoding.ASCII.GetBytes(payload);

            try
            {
                iSocket.Client.Send(bytes);
            }
            catch (Exception ex)
            {
                Stop();
                ConnectionLost(this, new ConnectionLostArgs(ex.Message));
            }
        }
    }
}
