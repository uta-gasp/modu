using System;
using System.Windows.Forms;

namespace Modu
{
    public partial class MainForm : Form
    {
        private Receiver iReceiver;
        private Sender iSender;

        private enum ControlState
        {
            Disconnected,
            Connecting,
            Connected,
            Disconnecting
        }

        private enum InfoSource
        {
            RCV,
            SND
        }

        public MainForm()
        {
            InitializeComponent();

            txbIP.Text = Modu.Properties.Settings.Default.IP;

            iSender = new Sender();
            iSender.ConnectionAttempt += Sender_ConnectionAttempt;
            iSender.ConnectionLost += Sender_ConnectionLost;

            iReceiver = new Receiver(this);
            iReceiver.ClientCountChanged += Receiver_ClientCountChanged;
            iReceiver.Debug += Receiver_Debug;

            try
            {
                iReceiver.Start();
            }
            catch (Exception ex)
            {
                Log(InfoSource.RCV, ex.Message);
            }
        }

        private void Log(InfoSource aSource, string aText)
        {
            txbDebug.AppendText(string.Format("{0}: {1}\r\n", aSource.ToString(), aText));
        }

        private void SetControlState(ControlState aState)
        {
            txbIP.Enabled = aState == ControlState.Disconnected;

            if (aState == ControlState.Disconnected)
                btnToggleSender.Enabled = txbIP.Text.Length > 7;
            else
                btnToggleSender.Enabled = aState == ControlState.Connected;

            if (aState == ControlState.Disconnected)
                btnToggleSender.Text = "Start";
            else if (aState == ControlState.Connected)
                btnToggleSender.Text = "Stop";
        }

        private void Receiver_Debug(object sender, Receiver.DebugArgs e)
        {
            Log(InfoSource.RCV, e.Message);
        }

        private void Receiver_ClientCountChanged(object sender, Receiver.ClientCountChangedArgs e)
        {
            lblConnectionCount.Text = e.Count.ToString();
        }

        private void Sender_ConnectionAttempt(object sender, Sender.ConnectionAttemptArgs e)
        {
            if (e.Success)
            {
                Log(InfoSource.SND, "Connected");
                SetControlState(ControlState.Connected);
            }
            else
            {
                Log(InfoSource.SND, e.Message);
            }
        }

        private void Sender_ConnectionLost(object sender, EventArgs e)
        {
            Log(InfoSource.SND, "Disconnected");
            SetControlState(ControlState.Disconnected);
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            iReceiver.Stop();
            iSender.Stop();

            Modu.Properties.Settings.Default.IP = txbIP.Text;
            Modu.Properties.Settings.Default.Save();
        }

        private void txbIP_TextChanged(object sender, EventArgs e)
        {
            SetControlState(ControlState.Disconnected);
        }

        private void btnToggleSender_Click(object sender, EventArgs e)
        {
            if (!iSender.IsConnected)
            {
                Log(InfoSource.SND, "Connecting...");
                SetControlState(ControlState.Connecting);

                iSender.Start(txbIP.Text);
            }
            else
            {
                iSender.Stop();

                Log(InfoSource.SND, "Disconnected");
                SetControlState(ControlState.Disconnected);
            }
        }
    }
}
