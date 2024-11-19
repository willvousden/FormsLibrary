using System;

namespace FormsLibrary.Controls
{
    /// <summary>
    /// Specifies the possible actions that can trigger a <see cref="FormsLibrary.Controls.SearchTextBox.Search"/> event.
    /// </summary>
    [Flags]
    public enum SearchTriggerActions
    {
        /// <summary>
        /// Specifies that the event should not be raised.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Specifies that the event should be raised when ENTER or RETURN is pressed.
        /// </summary>
        EnterPressed = 0x1,

        /// <summary>
        /// Specifies that the event should be raised when the control loses focus.
        /// </summary>
        FocusLost = 0x2,

        /// <summary>
        /// Specifies that the event should be raised when the control's search text changes.
        /// </summary>
        TextChanged = 0x4
    }
}