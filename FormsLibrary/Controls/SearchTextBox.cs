using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace FormsLibrary.Controls
{
    /// <summary>
    /// Represents a Windows text box control that displays default, greyed out text when empty and unfocused.
    /// </summary>
    [DefaultEvent("Search")]
    [DefaultProperty("SearchText")]
    [Description("A TextBox that displays default, greyed out text when empty and unfocused.")]
    public class SearchTextBox : TextBox
    {
        private EventHandler<SearchEventArgs> m_Search = null;
        private bool m_IgnoreTextChange = false;
        private bool m_IgnoreForeColorChange = false;
        private bool m_IsFocused = false; // We keep track of focus ourselves because the Focused property is rather unreliable.
        private bool m_IsFocused2 = false; // A separate variable is needed to keep track of focus for the OnMouseDown method.
        private bool m_SelectTextOnFocus = false;
        private string m_PreviousSearchText = null;
        private string m_SearchText = string.Empty;
        private string m_EmptyText = string.Empty;
        private Color m_EmptyColour = SystemColors.GrayText;
        private Color m_SearchColour = SystemColors.WindowText;
        private SearchTriggerActions m_TriggerAction = SearchTriggerActions.TextChanged;

        /// <summary>
        /// Raised when a search is made.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when a search is made.")]
        public virtual event EventHandler<SearchEventArgs> Search
        {
            add
            {
                m_Search += value;
            }
            remove
            {
                m_Search -= value;
            }
        }

        /// <summary>
        /// Gets or sets the text associated with the control.
        /// </summary>
        [ReadOnly(true)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the foreground colour of this component, which is used to display text.
        /// </summary>
        [ReadOnly(true)]
        public override Color ForeColor
        {
            get
            {
                return base.ForeColor;
            }
            set
            {
                base.ForeColor = value;
            }
        }

        /// <summary>
        /// Gets or sets the text that will be displayed in the <see cref="FormsLibrary.Controls.SearchTextBox"/> when it is empty and does not have focus.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [Category("Appearance")]
        [Description("The text that will be displayed when the SearchTextBox is empty and does not have focus.")]
        public virtual string EmptyText
        {
            get
            {
                return m_EmptyText;
            }
            set
            {
                m_EmptyText = value;

                Display();
            }
        }

        /// <summary>
        /// Gets or sets the text colour that will be displayed in the <see cref="FormsLibrary.Controls.SearchTextBox"/> when it is empty and does not have focus.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [Category("Appearance")]
        [Description("The colour that will be displayed when the SearchTextBox is empty and does not have focus")]
        public virtual Color EmptyColour
        {
            get
            {
                return m_EmptyColour;
            }
            set
            {
                m_EmptyColour = value;

                Display();
            }
        }

        /// <summary>
        /// Gets or sets the text that the user has entered (ignoring placeholder text).
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [Category("Appearance")]
        [DefaultValue("")]
        [Description("The text that will be displayed when the SearchTextBox is not empty or has focus.")]
        public virtual string SearchText
        {
            get
            {
                return m_SearchText;
            }
            set
            {
                if (m_SearchText != value)
                {
                    m_SearchText = value;
                    Display();
                }
            }
        }

        /// <summary>
        /// Gets or sets the text colour that will be displayed in the <see cref="FormsLibrary.Controls.SearchTextBox"/> when it is not empty or has have focus.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [Category("Appearance")]
        [Description("Gets or sets the text colour that will be displayed in the SearchTextBox when it is not empty or has have focus.")]
        public virtual Color SearchColor
        {
            get
            {
                return m_SearchColour;
            }
            set
            {
                m_SearchColour = value;
                Display();
            }
        }

        /// <summary>
        /// Gets or sets the action that triggers a <see cref="FormsLibrary.Controls.SearchTextBox.Search"/>.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(SearchTriggerActions.EnterPressed)]
        [Category("Behavior")]
        [Description("The action that triggers a Search event.")]
        public virtual SearchTriggerActions TriggerAction
        {
            get
            {
                return m_TriggerAction;
            }
            set
            {
                m_TriggerAction = value;
            }
        }

        /// <summary>
        /// Gets or sets a value specifying whether any text present should be selected when the control gains focus.
        /// </summary>
        [Browsable(true)]
        [DefaultValue(true)]
        [Category("Behavior")]
        [Description("Whether any text present should be selected when the control gains focus.")]
        public virtual bool SelectTextOnFocus
        {
            get
            {
                return m_SelectTextOnFocus;
            }
            set
            {
                m_SelectTextOnFocus = value;
            }
        }

        /// <summary>
        /// Initialises a <see cref="FormsLibrary.Controls.SearchTextBox"/> instance.
        /// </summary>
        public SearchTextBox()
        {
            Display();
        }

        /// <summary>
        /// Generates a <see cref="FormsLibrary.Controls.SearchTextBox.Search"/> event for the current search text box.
        /// </summary>
        public virtual void PerformSearch()
        {
            OnSearch(new SearchEventArgs(m_SearchText), true);
        }

        /// <summary>
        /// Updates the <see cref="FormsLibrary.Controls.SearchTextBox"/> according to its current state.
        /// </summary>
        private void Display()
        {
            // We don't want events to be raised while the colours and text are being changed.
            m_IgnoreForeColorChange = true;
            m_IgnoreTextChange = true;

            bool isEmpty = string.IsNullOrEmpty(m_SearchText);
            if (!m_IsFocused && isEmpty)
            {
                // Set text and colour for when the box is empty.
                Text = m_EmptyText;
                ForeColor = m_EmptyColour;
            }
            else
            {
                // Set the text and colour to what the user has entered and to the normal colour.
                Text = m_SearchText;
                ForeColor = m_SearchColour;
            }

            // Events can now be raised again.
            m_IgnoreForeColorChange = false;
            m_IgnoreTextChange = false;
        }

        #region Event handlers

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.Enter"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnEnter(EventArgs e)
        {
            base.OnEnter(e);

            m_IsFocused = true;
            Display();
            if (m_SelectTextOnFocus)
            {
                SelectAll();
            }
        }

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.Leave"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnLeave(EventArgs e)
        {
            base.OnLeave(e);

            m_IsFocused = false;
            m_IsFocused2 = false;
            Display();

            if ((m_TriggerAction & SearchTriggerActions.FocusLost) == SearchTriggerActions.FocusLost)
            {
                OnSearch(new SearchEventArgs(m_SearchText));
            }
        }

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.TextChanged"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnTextChanged(EventArgs e)
        {
            if (!m_IgnoreTextChange)
            {
                SearchText = Text;
                base.OnTextChanged(e);

                if ((m_TriggerAction & SearchTriggerActions.TextChanged) == SearchTriggerActions.TextChanged)
                {
                    OnSearch(new SearchEventArgs(m_SearchText));
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.ForeColorChanged"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnForeColorChanged(EventArgs e)
        {
            if (!m_IgnoreForeColorChange)
            {
                base.OnForeColorChanged(e);
            }
        }

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.OnKeyDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.KeyEventArgs"/> that contains the event data.</param>
        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);

            if ((m_TriggerAction & SearchTriggerActions.EnterPressed) == SearchTriggerActions.EnterPressed)
            {
                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Return)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    OnSearch(new SearchEventArgs(m_SearchText));
                }
            }
        }

        /// <summary>
        /// Raises the <see cref="FormsLibrary.Controls.SearchTextBox.Search"/> event.
        /// </summary>
        /// <param name="e">A <see cref="FormsLibrary.Controls.SearchEventArgs"/> that contains the event data.</param>
        protected virtual void OnSearch(SearchEventArgs e)
        {
            OnSearch(e, false);
        }

        /// <summary>
        /// Raises the <see cref="FormsLibrary.Controls.SearchTextBox.Search"/> event.
        /// </summary>
        /// <param name="e">A <see cref="FormsLibrary.Controls.SearchEventArgs"/> that contains the event data.</param>
        /// <param name="ignorePrevious"><c>true</c> to raise the event regardless of the previous search text; <c>false</c> to raise only when the current search text is different.</param>
        protected virtual void OnSearch(SearchEventArgs e, bool ignorePrevious)
        {
            if ((m_Search != null) && ((m_SearchText != m_PreviousSearchText) || ignorePrevious))
            {
                m_Search(this, e);
            }

            m_PreviousSearchText = m_SearchText;
        }

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.OnMouseDown"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (!m_IsFocused2 && m_SelectTextOnFocus)
            {
                SelectAll();
            }

            m_IsFocused2 = true;
            base.OnMouseDown(e);
        }

        #endregion
    }
}