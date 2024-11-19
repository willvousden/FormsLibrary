namespace FormsLibrary.Controls
{
    /// <summary>
    /// Specifies the possible states for a <see cref="FormsLibrary.Controls.Throbber"/>
    /// </summary>
    public enum ThrobberState
    {
        /// <summary>
        /// Specifies that the throbber is animated.
        /// </summary>
        Active,

        /// <summary>
        /// Specifies that the throbber is alternating between the inactive state and complete transparency.
        /// </summary>
        Blink,

        /// <summary>
        /// Specifies that the throbber is not animated.
        /// </summary>
        Inactive
    }
}