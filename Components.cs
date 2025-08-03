using System;
using System.Runtime.InteropServices;

using WinDirector.Devices;

namespace WinDirector
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Location : IEquatable<Location>
    {
        public int X, Y;

        public static Location Empty = new Location(0, 0);
        public Location(int x, int y) { X = x; Y = y; }
        public override string ToString() => string.Format("X: {0}, Y: {1}", X, Y);
        public override bool Equals(object obj) => obj is Location location && Equals(location);
        public bool Equals(Location other) => other.X == X && other.Y == Y;
        public override int GetHashCode()
        {
            int hash = 17;

            hash = (hash * 31) + X.GetHashCode();
            hash = (hash * 31) + Y.GetHashCode();

            return hash;
        }
        public static Location operator +(Location l1, Location l2) => new Location(l1.X + l2.X, l1.Y + l2.Y);
        public static Location operator -(Location l1, Location l2) => new Location(l1.X - l2.X, l1.Y - l2.Y);
        public static bool operator ==(Location l1, Location l2) => l1.Equals(l2);
        public static bool operator !=(Location l1, Location l2) => !l1.Equals(l2);
        public static Location operator *(Location l, float f)
        {
            int newX = Convert.ToInt32(l.X * f);
            int newY = Convert.ToInt32(l.Y * f);

            return new Location(newX, newY);
        }
        public static Location operator /(Location l, float f)
        {
            int newX = Convert.ToInt32(l.X / f);
            int newY = Convert.ToInt32(l.Y / f);

            return new Location(newX, newY);
        }
        public Location ToVisualPoint()
        {
            return this * Display.Settings.ScalingFactor;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle : IEquatable<Rectangle>
    {
        public int Left, Top, Right, Bottom;

        public static Rectangle Empty = new Rectangle(0, 0, 0, 0);
        public Location Location => new Location(Left, Top);
        public int X => Left;
        public int Y => Top;
        public int Width => Right - Left;
        public int Height => Bottom - Top;

        public Rectangle(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
        public Rectangle(Location loc, int width, int height)
        {
            Left = loc.X;
            Top = loc.Y;
            Right = Left + width;
            Bottom = Top + height;
        }
        public override string ToString() => string.Format("Width: {0} to {1}, Height: {2} to {3}", Left, Right, Top, Bottom);
        public override bool Equals(object obj) => obj is Rectangle location && Equals(location);
        public bool Equals(Rectangle other) => other.Left == Left && other.Top == Top && other.Right == Right && other.Bottom == Bottom;

        public bool Contains(Location pt)
        {
            return Left < pt.X && pt.X < Right && Top < pt.Y && pt.Y < Bottom;
        }

        public static bool operator ==(Rectangle c1, Rectangle c2) => c1.Equals(c2);

        public static bool operator !=(Rectangle c1, Rectangle c2) => !c1.Equals(c2);

        public static Rectangle operator *(Rectangle c, float f) =>
            new Rectangle(Convert.ToInt32(c.Left * f), Convert.ToInt32(c.Top * f), Convert.ToInt32(c.Right * f), Convert.ToInt32(c.Bottom * f));
        public override int GetHashCode()
        {
            int hash = 29;

            hash = (hash * 31) + Left.GetHashCode();
            hash = (hash * 31) + Right.GetHashCode();
            hash = (hash * 31) + Top.GetHashCode();
            hash = (hash * 31) + Bottom.GetHashCode();

            return hash;
        }
    }
}
