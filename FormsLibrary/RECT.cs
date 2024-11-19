using System.Drawing;
using System.Runtime.InteropServices;

namespace FormsLibrary
{
    /// <summary>
    /// Represents a Win32 RECT structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet=CharSet.Auto)]
    public struct RECT
    {
        /// <summary>
        /// Gets a <see cref="FormsLibrary.RECT"/> from a normal rectangle.
        /// </summary>
        /// <param name="rectangle">The <see cref="System.Drawing.Rectangle"/> from which to construct the RECT.</param>
        /// <returns>An equivalent RECT structure.</returns>
        public static RECT FromRectangle(Rectangle rectangle)
        {
            return new RECT(rectangle.Left, rectangle.Top, rectangle.Right, rectangle.Bottom);
        }

        /// <summary>
        /// Gets the x-coordinate of the top-left edge of this RECT.
        /// </summary>
        public int Left;

        /// <summary>
        /// Gets the y-coordinate of the top edge of this RECT.
        /// </summary>
        public int Top;

        /// <summary>
        /// Gets the x-coordinate of the top-right edge of this RECT.
        /// </summary>
        public int Right;

        /// <summary>
        /// Gets the y-coordinate of the bottom edge of this RECT.
        /// </summary>
        public int Bottom;

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.POINT"/> instance.
        /// </summary>
        /// <param name="left">The x-coordinate of the top-left edge of the rectangle.</param>
        /// <param name="top">The y-coordinate of the top edge of the rectangle.</param>
        /// <param name="right">The x-coordinate of the top-right edge of the rectangle.</param>
        /// <param name="bottom">The y-coordinate of the bottom edge of the rectangle.</param>
        public RECT(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// Constructs a <see cref="System.Drawing.Rectangle"/> from the current RECT.
        /// </summary>
        /// <returns>A <see cref="System.Drawing.Rectangle"/> equivalent to the current RECT.</returns>
        public Rectangle ToRectangle()
        {
            return new Rectangle(Left, Top, Right - Left, Bottom - Top);
        }
    }
}