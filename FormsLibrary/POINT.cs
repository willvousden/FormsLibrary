using System.Drawing;
using System.Runtime.InteropServices;

namespace FormsLibrary
{
    /// <summary>
    /// Represents a Win32 POINT structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
    public struct POINT
    {
        /// <summary>
        /// Gets a <see cref="FormsLibrary.POINT"/> from a normal point.
        /// </summary>
        /// <param name="point">The <see cref="System.Drawing.Point"/> from which to construct the POINT.</param>
        /// <returns>An equivalent POINT structure.</returns>
        public static POINT FromPoint(Point point)
        {
            return new POINT(point.X, point.Y);
        }

        /// <summary>
        /// The x-coordinate of this point.
        /// </summary>
        public int X;

        /// <summary>
        /// The y-coordinate of this point.
        /// </summary>
        public int Y;

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.POINT"/> instance.
        /// </summary>
        /// <param name="x">The x-coordinate of the point.</param>
        /// <param name="y">The y-coordinate of the point.</param>
        public POINT(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Constructs a <see cref="System.Drawing.Point"/> from the current POINT.
        /// </summary>
        /// <returns>A <see cref="System.Drawing.Point"/> equivalent to the current POINT.</returns>
        public Point ToPoint()
        {
            return new Point(X, Y);
        }
    }
}