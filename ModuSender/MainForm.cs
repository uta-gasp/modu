using System;
using System.Drawing;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace ModuSender
{
    public partial class MainForm : Form
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

        public MainForm()
        {
            InitializeComponent();

            iTimer = new System.Timers.Timer(50);
            iTimer.Elapsed += ITimer_Elapsed;

            txbIP.Text = ModuSender.Properties.Settings.Default.IP;
            txbPort.Text = ModuSender.Properties.Settings.Default.Port.ToString();
        }

        ~MainForm()
        {
            if (iSocket?.Client != null)
            {
                iSocket.Close();
            }
        }

        private void SetControlState(ControlState aState)
        {
            int port;

            txbIP.Enabled = aState == ControlState.Disconnected;
            txbPort.Enabled = aState == ControlState.Disconnected;
            if (aState == ControlState.Disconnected)
                btnStartStop.Enabled = txbIP.Text.Length > 0 && int.TryParse(txbPort.Text, out port);
            else
                btnStartStop.Enabled = aState == ControlState.Connected;
        }

        private void CloseConnection()
        {
            if (iSocket != null && iSocket.Connected)
            {
                iSocket.Close();
            }

            iSocket = null;

            iTimer.Stop();

            btnStartStop.Text = "Start";

            SetControlState(ControlState.Disconnected);
        }

        private void ITimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (iSocket == null)
                return;

            if (!iSocket.Connected && iTimer.Enabled)
            {
                CloseConnection();
                lblStatus.Text = "Disconnected";
                return;
            }

            Point mouseLocation = Cursor.Position;
            if (iMouseLocation.X == mouseLocation.X && iMouseLocation.Y == mouseLocation.Y)
                return;

            string payload = string.Format("{0} {1}\r\n", mouseLocation.X, mouseLocation.Y);
            byte[] bytes = Encoding.ASCII.GetBytes(payload);

            try
            {
                iSocket.Client.Send(bytes);
            }
            catch (Exception ex)
            {
                lblStatus.Text = ex.Message;
                CloseConnection();
            }
        }

        private void txbIP_TextChanged(object sender, EventArgs e)
        {
            SetControlState(ControlState.Disconnected);
        }

        private void txbPort_TextChanged(object sender, EventArgs e)
        {
            SetControlState(ControlState.Disconnected);
        }

        private async void btnStartStop_Click(object sender, EventArgs e)
        {
            SetControlState(ControlState.Disconnecting);

            lblStatus.Text = "Connecting...";

            if (iSocket == null)
            {
                iSocket = new TcpClient();
                iSocket.NoDelay = true;

                string ip = txbIP.Text;
                try
                {
                    IPAddress addr = IPAddress.Parse(txbIP.Text);
                    int port = int.Parse(txbPort.Text);
                    await iSocket.ConnectAsync(addr, port);

                    btnStartStop.Text = "Stop";
                    lblStatus.Text = "Connected";
                    iTimer.Start();

                    SetControlState(ControlState.Connected);
                }
                catch (Exception ex)
                {
                    lblStatus.Text = ex.Message;
                    CloseConnection();
                }
            }
            else
            {
                lblStatus.Text = "Disconnected";
                CloseConnection();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            ModuSender.Properties.Settings.Default.IP = txbIP.Text;
            int port;
            if (int.TryParse(txbPort.Text, out port))
            {
                ModuSender.Properties.Settings.Default.Port = port;
            }
            ModuSender.Properties.Settings.Default.Save();
        }
    }
}
