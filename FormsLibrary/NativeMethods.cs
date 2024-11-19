using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FormsLibrary
{
    /// <summary>
    /// Contains P/Invoke methods.
    /// </summary>
    internal static class NativeMethods
    {
        /// <summary>
        /// Sends the specified message to a window or windows.
        /// </summary>
        /// <param name="hWnd">The handle to the window whose window procedure will receive the message.</param>
        /// <param name="Msg">Specifies the message to be sent.</param>
        /// <param name="wParam">Specifies additional message-specific information.</param>
        /// <param name="lParam">Specifies additional message-specific information.</param>
        /// <returns></returns>
        [DllImport("user32.dll", CharSet=CharSet.Auto, SetLastError=false)]
        public static extern IntPtr SendMessage(HandleRef hWnd,
                                                uint Msg,
                                                IntPtr wParam,
                                                IntPtr lParam);

        /// <summary>
        /// Validates the client area within a rectangle by removing the rectangle from the update region of the specified window. 
        /// </summary>
        /// <param name="handle">The handle to the window whose update region is to be modified.</param>
        /// <param name="lpRect">A RECT structure that contains the client coordinates of the rectangle to be removed from the update region.</param>
        /// <returns><c>true</c> of successful; <c>false</c> otherwise.</returns>
        [DllImport("user32.dll", CharSet=CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool ValidateRect(HandleRef handle,
                                               ref RECT lpRect);

        /// <summary>
        /// Flashes the specified window. It does not change the active state of the window.
        /// </summary>
        /// <param name="hWnd">The handle of the form to flash.</param>
        /// <param name="bInvert"><c>true</c> to flash once before before staying highlighted; <c>false</c>
        /// otherwise.</param>
        /// <returns><c>true</c> if the window was active before the call; otherwise <c>false</c>.</returns>
        [DllImport("User32.dll")]
        public static extern bool FlashWindow(IntPtr hWnd,
                                              [MarshalAs(UnmanagedType.Bool)] bool bInvert);

        /// <summary>
        /// Flashes the specified window. It does not change the active state of the window.
        /// </summary>
        /// <param name="hWnd">The handle of the form to flash.</param>
        /// <param name="bInvert"><c>true</c> to flash once before before staying highlighted; <c>false</c>
        /// otherwise.</param>
        /// <returns><c>true</c> if the window was active before the call; otherwise <c>false</c>.</returns>
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool FlashWindow(HandleRef hWnd,
                                              [MarshalAs(UnmanagedType.Bool)] bool bInvert);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="id"></param>
        /// <param name="fsModifiers"></param>
        /// <param name="vlc"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int RegisterHotKey(IntPtr hWnd,
                                                int id,
                                                [MarshalAs(UnmanagedType.U4)] KeyModifiers fsModifiers,
                                                [MarshalAs(UnmanagedType.U4)] Keys vlc);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hWnd"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [DllImport("User32.dll")]
        public static extern int UnregisterHotKey(IntPtr hWnd,
                                                  int id);
    }
}