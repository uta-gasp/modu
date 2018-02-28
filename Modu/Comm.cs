using System.Drawing;

namespace Modu
{
    public sealed class Comm
    {
        public static readonly int PORT = 18296;

        public static string Pack(Point aPoint)
        {
            return string.Format("{0} {1}\r\n", aPoint.X, aPoint.Y);
        }

        public static Point Unpack(string aString)
        {
            string[] coords = aString.Split(' ');
            int x = int.Parse(coords[0]);
            int y = int.Parse(coords[1]);
            return new Point(x, y);
        }
    }
}
