using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace GazeNetClient
{
    public partial class PointerWidget : Form
    {
        private const double HUE_STEP = 68.5; // recommended:   64.166, 68.5, 77, 86.25
        private const float DARKNESS = 0.3f;

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams createParams = base.CreateParams;
                createParams.ExStyle |= Utils.WinAPI.WS_EX.TRANSPARENT | Utils.WinAPI.WS_EX.TOOLWINDOW;
                return createParams;
            }
        }

        public PointerWidget()
        {
            InitializeComponent();

            BackColor = Color.White;
            TransparencyKey = Color.White;
        }

        public void setImage(Bitmap aBitmap, int aColorIndex)
        {
            if (aColorIndex >= 0)
            {
                int hueRotationAngle = (int)(HUE_STEP * aColorIndex);
                float darkness = (hueRotationAngle / 360) % 4 == 3 ? DARKNESS : 0.0f;
                pcbImage.Image = TransformImage(aBitmap, hueRotationAngle % 360, darkness);
            }
            else
            {
                pcbImage.Image = aBitmap;
            }
        }

        private Bitmap TransformImage(Bitmap aBitmap, int aHueRotationAngle, float aDarkness)
        {
            Bitmap bmp = (Bitmap)aBitmap.Clone();
            if (aHueRotationAngle != 0)
            {
                ImageAttributes imageAttributes = new ImageAttributes();
                imageAttributes.SetColorMatrix(GetHueShiftColorMatrix(aHueRotationAngle, aDarkness),
                    ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

                using (Graphics graphics = Graphics.FromImage(bmp))
                {
                    graphics.DrawImage(aBitmap, new Rectangle(new Point(0, 0), bmp.Size),
                        0, 0, bmp.Width, bmp.Height, GraphicsUnit.Pixel, imageAttributes);
                    graphics.Flush();
                }
            }

            return bmp;
        }

        private ColorMatrix GetHueShiftColorMatrix(double aHueRotationDegrees, float aDarkness, bool aPreserveLuminosity = false)
        {
            double theta = -aHueRotationDegrees / 180 * Math.PI;
            float cos = (float)Math.Cos(theta);
            float sin = (float)Math.Sin(theta);
            float[][] m = new float[3][]
            {
                new float[3],
                new float[3],
                new float[3]
            };

            float brightness = 1.0f - aDarkness;

            if (aPreserveLuminosity)
            {
                // luminanace constants
                float lr = 0.213f;
                float lg = 0.715f;
                float lb = 0.072f;

                float a = 0.143f;
                float b = 0.140f;
                float c = -0.283f;

                m[0][0] = lr + cos * (1f - lr) + sin * (-lr);
                m[0][1] = lr + cos * (-lr) + sin * (a);
                m[0][2] = lr + cos * (-lr) + sin * (-(1f - lr));

                m[1][0] = lg + cos * (-lg) + sin * (-lg);
                m[1][1] = lg + cos * (1f - lg) + sin * (b);
                m[1][2] = lg + cos * (-lg) + sin * (lg);

                m[2][0] = lb + cos * (-lb) + sin * (1f - lb);
                m[2][1] = lb + cos * (-lb) + sin * (c);
                m[2][2] = lb + cos * (1f - lb) + sin * (lb);
            }
            else
            {
                /* According to PIXIJS.ColorMatrixFilter.hue:
                    
                This matrix is far better than the versions with magic luminance constants
                formerly used here, but also used in the starling framework (flash) and known from this
                old part of the internet: quasimondo.com/archives/000565.php
                    
                This new matrix is based on rgb cube rotation in space. Look here for a more descriptive
                implementation as a shader not a general matrix:
                https://github.com/evanw/glfx.js/blob/58841c23919bd59787effc0333a4897b43835412/src/filters/adjust/huesaturation.js
                    
                This is the source for the code:
                see http://stackoverflow.com/questions/8507885/shift-hue-of-an-rgb-color/8510751#8510751
                */

                float w = 1f / 3;
                float sqrW = (float)Math.Sqrt(w);

                m[0][0] = cos + (1f - cos) * w;
                m[0][1] = w * (1f - cos) - sqrW * sin;
                m[0][2] = w * (1f - cos) + sqrW * sin;

                m[1][0] = w * (1f - cos) + sqrW * sin;
                m[1][1] = cos + w * (1f - cos);
                m[1][2] = w * (1 - cos) - sqrW * sin;

                m[2][0] = w * (1 - cos) - sqrW * sin;
                m[2][1] = w * (1 - cos) + sqrW * sin;
                m[2][2] = cos + w * (1 - cos);
            }

            for (int row = 0; row < 3; row++)
                for (int col = 0; col < 3; col++)
                    m[row][col] *= brightness;

            float[][] matrix = {
                new float[] { m[0][0], m[0][1], m[0][2], 0, 0 },
                new float[] { m[1][0], m[1][1], m[1][2], 0, 0 },
                new float[] { m[2][0], m[2][1], m[2][2], 0, 0 },
                new float[] { 0,       0,       0,       1, 0 },
                new float[] { 0,       0,       0,       0, 1 }
            };

            return new ColorMatrix(matrix);
        }

        private void tmrPositionInspector_Tick(object sender, EventArgs e)
        {
            if (Visible)
            {
                Utils.WinAPI.SetWindowPos(Handle, Utils.WinAPI.HWND.TOPMOST, 0, 0, 0, 0,
                    Utils.WinAPI.SWP.NOMOVE | Utils.WinAPI.SWP.NOSIZE | Utils.WinAPI.SWP.NOACTIVATE);
            }
        }
    }
}
