using System;

namespace FormsLibrary
{
    /// <summary>
    /// Specifies the possible modifiers for a hotkey.
    /// </summary>
    /// <remarks>A combination of these modifiers can be specified using bitwise logic operators.</remarks>
    [Flags]
    public enum KeyModifiers
    {
        /// <summary>
        /// No modifier keys are pressed.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The ALT key is pressed.
        /// </summary>
        Alt = 0x1,

        /// <summary>
        /// The CTRL key is pressed.
        /// </summary>
        Control = 0x2,

        /// <summary>
        /// The SHIFT key is pressed.
        /// </summary>
        Shift = 0x4,

        /// <summary>
        /// The WIN key is pressed.
        /// </summary>
        Win = 0x8
    }
}