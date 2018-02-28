using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Reflection;

namespace GazeNetClient.Pointer
{
    public class Collection : IDisposable
    {
        public static Dictionary<Style, Bitmap> StyleImages { get; private set; }
        public static bool VisilityFollowsDataAvailability { get; set; } = true;

        private Stack<Settings> iSettingsBuffer = new Stack<Settings>();
        private Dictionary<string, Pointer> iPointers = new Dictionary<string, Pointer>();
        private bool iVisible = true;

        private bool iDisposed = false;

        public Settings Settings { get; private set; }
        public Pointer[] Items { get { return iPointers.Values.ToArray(); } }

        public bool Visible
        {
            get { return iVisible; }
            set
            {
                iVisible = value;
                if (iVisible)
                    foreach (Pointer pointer in iPointers.Values) pointer.show();
                else
                    foreach (Pointer pointer in iPointers.Values) pointer.hide();
            }
        }

        public Collection()
        {
            LoadStyleImages();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void pushSettings()
        {
            iSettingsBuffer.Push(Settings.copy());
        }

        public void popSettings(bool aRestore)
        {
            if (iSettingsBuffer.Count == 0)
                return;

            Settings settings = iSettingsBuffer.Pop();

            if (aRestore)
            {
                Settings = settings;
            }
            else
            {
                foreach (Pointer pointer in iPointers.Values)
                    Settings.saveTo(pointer);
            }
        }

        public void movePointer(string aID, PointF aLocation)
        {
            Pointer pointer;
            if (iPointers.ContainsKey(aID))
            {
                pointer = iPointers[aID];
            }
            else
            {
                pointer = new Pointer(iPointers.Count);
                if (iVisible)
                    pointer.show();
                else
                    pointer.hide();

                iPointers.Add(aID, pointer);
            }

            pointer.moveTo(aLocation);
        }

        public void scale(PointF aScale)
        {
            Settings.Scale = aScale;
            foreach (Pointer pointer in iPointers.Values)
                Settings.saveTo(pointer);
        }

        private void LoadStyleImages()
        {
            if (StyleImages != null)
                return;

            StyleImages = new Dictionary<Style, Bitmap>();

            Type resourceManagerType = typeof(Modu.Properties.Resources);
            foreach (Style style in Enum.GetValues(typeof(Style)))
            {
                string imageName = "pointer" + style.ToString();
                PropertyInfo property = resourceManagerType.GetProperty(imageName, BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
                if (property == null)
                    throw new Exception("Internal error: missing some images for Pointer");

                object image = property.GetValue(null, null);
                if (image == null)
                    throw new Exception("Internal error: null image for Pointer");

                StyleImages.Add(style, (Bitmap)image);
            }
        }

        protected virtual void Dispose(bool aDisposing)
        {
            if (iDisposed)
                return;

            if (aDisposing)
            {
                foreach (Pointer pointer in iPointers.Values)
                    pointer.Dispose();

                iPointers.Clear();
            }

            iDisposed = true;
        }
    }
}
