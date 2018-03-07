using System;
using System.Drawing;
using System.Windows.Forms;

namespace GazeNetClient.Pointer
{
    public class Pointer : IDisposable
    {
        #region Internal members

        private bool iDisposed = false;

        private PointerWidget iWidget;
        private Style iAppearance;
        private int iColorIndex;
        private double iOpacity = 1;
        private double iDataAvailability;

        private long iLastDataTimestamp = 0;
        private Utils.HiResTimestamp iHRTimestamp;
        private Timer iDataAvailabilityTimer;

        #endregion

        #region Properties

        // Settings

        public Style Appearance
        {
            get { return iAppearance; }
            set
            {
                iAppearance = value;
                if (Collection.StyleImages.ContainsKey(value))
                {
                    iWidget.setImage(Collection.StyleImages[value], iColorIndex);
                }
            }
        }

        public double Opacity
        {
            get { return iOpacity; }
            set
            {
                iOpacity = value;
                UpdateWidgetOpacity();
            }
        }

        public int Size
        {
            get { return iWidget.Width; }
            set
            {
                iWidget.Width = value;
                iWidget.Height = value;
            }
        }

        public bool Visible
        {
            get { return iWidget.Visible; }
        }

        public long FadingInterval { get; set; }

        public long NoDataVisibilityInterval { get; set; }

        #endregion

        #region Events

        public event EventHandler OnHide = delegate { };
        public event EventHandler OnShow = delegate { };

        #endregion

        #region Public methods

        public Pointer(int aColorIndex)
        {
            iColorIndex = aColorIndex;

            iWidget = new PointerWidget();

            iDataAvailabilityTimer = new Timer();
            iDataAvailabilityTimer.Interval = 30;
            iDataAvailabilityTimer.Tick += DataAvailabilityTimer_Tick;

            iHRTimestamp = new Utils.HiResTimestamp();

            Appearance = Style.Simple;
        }

        public void show()
        {
            int size = Size;
            iDataAvailability = 1.0;
            UpdateWidgetOpacity();
            iWidget.Show();
            Size = size;

            iDataAvailabilityTimer.Start();

            iLastDataTimestamp = 0;
        }

        public void hide()
        {
            iWidget.Hide();
            iDataAvailabilityTimer.Stop();
        }

        public void moveTo(PointF aLocation)
        {
            iLastDataTimestamp = iHRTimestamp.Milliseconds;

            if (iWidget.Visible)
            {
                iWidget.Invoke(new Action(() =>
                {
                    iWidget.Location = new Point((int)(aLocation.X - iWidget.Width / 2), (int)(aLocation.Y - iWidget.Height / 2));
                }));
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region Event handlers

        private void DataAvailabilityTimer_Tick(object sender, EventArgs e)
        {
            double dataAvailability = Collection.VisilityFollowsDataAvailability ? -1.0 : 1.0;
            double prevDataAvailability = iDataAvailability;

            if (Collection.VisilityFollowsDataAvailability)
            {
                long dataNotAvailableInterval = iHRTimestamp.Milliseconds - iLastDataTimestamp;
                if (dataNotAvailableInterval > NoDataVisibilityInterval)
                {
                    dataAvailability = 1.0 - Math.Min(1.0, (double)(dataNotAvailableInterval - NoDataVisibilityInterval) / FadingInterval);
                }
                else
                {
                    double opacityIncreaseStep = (double)(iDataAvailabilityTimer.Interval) / FadingInterval;
                    dataAvailability = Math.Min(1.0, iDataAvailability + opacityIncreaseStep);
                }
            }

            if (dataAvailability != iDataAvailability)
            {
                iDataAvailability = dataAvailability;
                UpdateWidgetOpacity();

                if (iDataAvailability == 0.0)
                {
                    if (prevDataAvailability != 1.0)
                    {
                        OnHide(this, new EventArgs());
                    }
                }
                else if (iDataAvailability == 1.0)
                {
                    OnShow(this, new EventArgs());
                }
            }
        }

        #endregion

        #region Internal methods

        protected virtual void Dispose(bool aDisposing)
        {
            if (iDisposed)
                return;

            if (aDisposing)
            {
            }

            iDisposed = true;
        }

        private void UpdateWidgetOpacity()
        {
            iWidget.Opacity = iOpacity * iDataAvailability;
        }

        #endregion
    }
}
