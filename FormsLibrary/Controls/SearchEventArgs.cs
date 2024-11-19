using System;

namespace FormsLibrary.Controls
{
    /// <summary>
    /// Contains data pertaining to a <see cref="FormsLibrary.Controls.SearchTextBox.Search"/> event.
    /// </summary>
    public class SearchEventArgs : EventArgs
    {
        private string m_SearchText = string.Empty;

        /// <summary>
        /// Gets or sets the text for the search.
        /// </summary>
        public virtual string SearchText
        {
            get
            {
                return m_SearchText;
            }
            set
            {
                m_SearchText = value;
            }
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.Controls.SearchEventArgs"/> instance.
        /// </summary>
        /// <param name="searchText">The text entered by the user.</param>
        public SearchEventArgs(string searchText)
        {
            m_SearchText = searchText;
        }
    }
}