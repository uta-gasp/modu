using System;
using System.Drawing;

namespace GazeNetClient.Pointer
{
    [Serializable]
    public class Settings
    {
        public const int DEFAULT_SIZE = 100; // should match sizes of the pointer widget and resource images

        public Style Appearance { get; set; } = Style.Simple;
        public double Opacity { get; set; } = 0.3;
        public int Size { get; set; } = DEFAULT_SIZE;
        public long FadingInterval { get; set; } = 300;
        public long NoDataVisibilityInterval { get; set; } = 1000;

        [System.Xml.Serialization.XmlIgnore]
        public PointF Scale
        {
            get { return new PointF(Size / DEFAULT_SIZE, Size / DEFAULT_SIZE); }
            set { Size = (int)(value.Y * DEFAULT_SIZE); }
        }

        public Settings() { }

        public Settings(Pointer aPointer)
        {
            loadFrom(aPointer);
        }

        public Settings copy()
        {
            Settings result = new Settings();
            result.Appearance = Appearance;
            result.Opacity = Opacity;
            result.Size = Size;
            result.FadingInterval = FadingInterval;
            result.NoDataVisibilityInterval = NoDataVisibilityInterval;
            return result;
        }

        public void loadFrom(Pointer aPointer)
        {
            Appearance = aPointer.Appearance;
            Opacity = aPointer.Opacity;
            Size = aPointer.Size;
            FadingInterval = aPointer.FadingInterval;
            NoDataVisibilityInterval = aPointer.NoDataVisibilityInterval;
        }

        public void saveTo(Pointer aPointer)
        {
            aPointer.Appearance = Appearance;
            aPointer.Opacity = Opacity;
            aPointer.Size = Size;
            aPointer.FadingInterval = FadingInterval;
            aPointer.NoDataVisibilityInterval = NoDataVisibilityInterval;
        }
    }
}
