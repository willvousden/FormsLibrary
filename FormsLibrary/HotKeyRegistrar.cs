using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FormsLibrary
{
    /// <summary>
    /// Facilitates registration and unregistration of hot keys with a Windows form.
    /// </summary>
    internal static class HotKeyRegistrar
    {
        private static Stack<int> m_AvailableDllIds;
        private static Stack<int> m_AvailableApplicationIds;

        /// <summary>
        /// The minimum ID for a hot key when registered from a application. This field is read-only.
        /// </summary>
        public static readonly int ApplicationMinimumId = 0x0000;

        /// <summary>
        /// The maximum ID for a hot key when registered from an application. This field is read-only.
        /// </summary>
        public static readonly int ApplicationMaximumId = 0xBFFF;

        /// <summary>
        /// The minimum ID for a hot key when registered from a DLL. This field is read-only.
        /// </summary>
        public static readonly int DllMinimumId = 0xC000;

        /// <summary>
        /// The maximum ID for a hot key when registered from a DLL. This field is read-only.
        /// </summary>
        public static readonly int DllMaximumId = 0xFFFF;

        /// <summary>
        /// Gets a stack containing the available hot key IDs for use by a DLL.
        /// </summary>
        public static Stack<int> AvailableDllIds
        {
            get
            {
                if (m_AvailableDllIds == null)
                {
                    GenerateDllIdStack();
                }

                return m_AvailableDllIds;
            }
        }

        /// <summary>
        /// Gets a stack containing the available hot key IDs for use by an application.
        /// </summary>
        public static Stack<int> AvailableApplicationIds
        {
            get
            {
                if (m_AvailableApplicationIds == null)
                {
                    GenerateApplicationIdStack();
                }

                return m_AvailableApplicationIds;
            }
        }

        /// <summary>
        ///  Registers a hot key with the given Windows form.
        /// </summary>
        /// <param name="targetWindow">The window that should receive notification of the hotkey being pressed.</param>
        /// <param name="id">A 32-bit integer used to identify the hot key.</param>
        /// <param name="hotKey">The hot key to be registered.</param>
        /// <returns><c>true</c> if successful; <c>false</c> otherwise.</returns>
        public static bool Register(IWin32Window targetWindow, int id, HotKey hotKey)
        {
            int result = NativeMethods.RegisterHotKey(targetWindow.Handle, id, hotKey.Modifiers, hotKey.Key);
            return result != 0;
        }

        /// <summary>
        /// Unregisters a hot key with the given Windows form.
        /// </summary>
        /// <param name="targetWindow">The window with which the hot key is currently registerd.</param>
        /// <param name="id">A 32-bit integer used to identify the hot key.</param>
        /// <exception cref="System.InvalidOperationException">The hot key has already been registered.</exception>
        /// <returns><c>true</c> if successful; <c>false</c> otherwise.</returns>
        public static bool Unregister(IWin32Window targetWindow, int id)
        {
            int result = NativeMethods.UnregisterHotKey(targetWindow.Handle, id);
            return result != 0;
        }

        /// <summary>
        /// Generates a stack containing all valid hot key IDs for a DLL to register.
        /// </summary>
        private static void GenerateDllIdStack()
        {
            m_AvailableDllIds = new Stack<int>(DllMaximumId - DllMinimumId);
            for (int id = DllMinimumId; id <= DllMaximumId; id++)
            {
                m_AvailableDllIds.Push(id);
            }
        }

        /// <summary>
        /// Gets a stack containing all valid hot key IDs for an application to register.
        /// </summary>
        private static void GenerateApplicationIdStack()
        {
            m_AvailableApplicationIds = new Stack<int>(ApplicationMaximumId - ApplicationMinimumId);
            for (int id = ApplicationMinimumId; id <= ApplicationMaximumId; id++)
            {
                m_AvailableApplicationIds.Push(id);
            }
        }
    }
}