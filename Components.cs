using System;
using System.Runtime.InteropServices;

using WinDirector.Devices;

namespace WinDirector
{
    [StructLayout(LayoutKind.Sequential)]
    public struct Location : IEquatable<Location>
    {
        public int X;
        public int Y;

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
}
