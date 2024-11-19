using System;

namespace FormsLibrary
{
    /// <summary>
    /// Specifies the aspects of a form's state that can persist accross sessions.
    /// </summary>
    [Flags]
    public enum FormStateAspects
    {
        /// <summary>
        /// Specifies that no aspects of the form's state should be persistent.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// Specifies that the form's size should be persistent.
        /// </summary>
        Size = 0x1,

        /// <summary>
        /// Specifies that the form's location should be persistent.
        /// </summary>
        Location = 0x2,

        /// <summary>
        /// Specifies that the window state of the form (i.e. maximised or normal) should be persistent.
        /// </summary>
        WindowState = 0x4,

        /// <summary>
        /// Specifies that all aspects of the form's state should be persistent.
        /// </summary>
        All = Size | Location | WindowState
    }
}