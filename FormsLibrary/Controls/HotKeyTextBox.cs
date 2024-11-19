using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace FormsLibrary.Controls
{
    /// <summary>
    /// Allows the user to enter key combinations for hot keys.
    /// </summary>
    public partial class HotKeyTextBox : TextBox
    {
        private HotKey m_HotKey;

        /// <summary>
        /// Gets or sets the current hot key combination.
        /// </summary>
        public virtual HotKey HotKey
        {
            get
            {
                return m_HotKey;
            }
            set
            {
                m_HotKey = value;
                DisplayKeyCombination();
            }
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.Controls.HotKeyTextBox"/> instance.
        /// </summary>
        public HotKeyTextBox()
        {
            ReadOnly = true;
            BackColor = SystemColors.Window;
            DisplayKeyCombination();
        }

        /// <summary>
        /// Displays the current hot key on the control.
        /// </summary>
        private void DisplayKeyCombination()
        {
            Text = m_HotKey != null ? m_HotKey.ToString() : "None";
        }

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.KeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            Keys key = e.KeyCode;
            KeyModifiers modifiers = KeyModifiers.None;
            if ((e.Modifiers & Keys.Alt) == Keys.Alt)
            {
                modifiers |= KeyModifiers.Alt;
            }
            if ((e.Modifiers & Keys.Control) == Keys.Control)
            {
                modifiers |= KeyModifiers.Control;
            }
            if ((e.Modifiers & Keys.Shift) == Keys.Shift)
            {
                modifiers |= KeyModifiers.Shift;
            }

            m_HotKey = new HotKey(key, modifiers);

            e.Handled = true;
            e.SuppressKeyPress = true;
            base.OnKeyDown(e);
        }

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.KeyUp"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyUp(KeyEventArgs e)
        {
            DisplayKeyCombination();

            e.Handled = true;
            e.SuppressKeyPress = true;
            base.OnKeyUp(e);
        }
    }
}