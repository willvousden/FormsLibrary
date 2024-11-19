using System;

namespace FormsLibrary
{
    /// <summary>
    /// Contains data pertaining to a <see cref="FormsLibrary.BaseForm.HotKeyPressed"/> event.
    /// </summary>
    public class HotKeyEventArgs : EventArgs
    {
        private int m_HotKeyId;

        /// <summary>
        /// Gets a 32-bit integer used to identify the hot key.
        /// </summary>
        public int HotKeyId
        {
            get
            {
                return m_HotKeyId;
            }
            internal set
            {
                m_HotKeyId = value;
            }
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.HotKeyEventArgs"/> instance.
        /// </summary>
        /// <param name="hotKeyId">A 32-bit integer used to identify the hot key.</param>
        internal HotKeyEventArgs(int hotKeyId)
        {
            m_HotKeyId = hotKeyId;
        }
    }
}