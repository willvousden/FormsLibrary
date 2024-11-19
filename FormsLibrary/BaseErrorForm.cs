using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Security;

namespace FormsLibrary
{
    /// <summary>
    /// Displays information to a user about an error.
    /// </summary>
    public partial class BaseErrorForm : BaseForm
    {
        private bool m_VerboseOutput = true;
        private Exception m_Exception = null;
        private bool m_ShowHeading = true;

        /// <summary>
        /// Gets or sets a value indicating whether a heading panel is displayed on the form.
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint)]
        [EditorBrowsable]
        [Category("Appearance")]
        [Description("Controls whether a heading panel is displayed on the form.")]
        public virtual bool ShowHeading
        {
            get
            {
                return m_ShowHeading;
            }
            set
            {
                if (value && !m_ShowHeading)
                {
                    headingPanel.Visible = true;
                    AdjustBodyVerticalLocation(headingPanel.Height);

                }
                else if (!value && m_ShowHeading)
                {
                    headingPanel.Visible = false;
                    AdjustBodyVerticalLocation(-headingPanel.Height);
                }

                m_ShowHeading = value;
            }
        }

        /// <summary>
        /// Gets or sets the image displayed in the upper-right corner of the form. <c>null</c> can be specified for no image.
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint)]
        [EditorBrowsable]
        [Category("Appearance")]
        [Description("The image that is displayed in the upper-right corner of the form.")]
        public virtual Image HeadingImage
        {
            get
            {
                return headingPictureBox.Image;
            }
            set
            {
                headingPictureBox.Image = value;

                if (value == null)
                {
                    // Hide the image and allow the subheading label to take up the slack.
                    headingPictureBox.Visible = false;
                    subHeadingLabel.Width = subHeadingLabel.Parent.Width - 15;
                }
                else
                {
                    headingPictureBox.Visible = true;
                    subHeadingLabel.Width = subHeadingLabel.Parent.Width - 69;
                }
            }
        }

        /// <summary>
        /// Gets or sets the heading displayed in the upper part of the form.
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint)]
        [EditorBrowsable]
        [Category("Appearance")]
        [Description("The heading that is displayed in the upper part of the form.")]
        public virtual string HeadingText
        {
            get
            {
                return headingLabel.Text;
            }
            set
            {
                headingLabel.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets the text displayed in the upper part of the form, beneath the heading. <c>null</c>
        /// can be specified for no text.
        /// </summary>
        [RefreshProperties(RefreshProperties.Repaint)]
        [EditorBrowsable]
        [Category("Appearance")]
        [Description("The text that is displayed in the upper part of the form, beneath the heading.")]
        public virtual string SubHeadingText
        {
            get
            {
                return subHeadingLabel.Text;
            }
            set
            {
                subHeadingLabel.Text = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether technical information is displayed on the form.
        /// </summary>
        [RefreshProperties(RefreshProperties.None)]
        [EditorBrowsable]
        [Category("Appearance")]
        [Description("Controls whether technical information is displayed on the form.")]
        public virtual bool VerboseOutput
        {
            get
            {
                return m_VerboseOutput;
            }
            set
            {
                m_VerboseOutput = value;
            }
        }

        /// <summary>
        /// Gets or sets the exception whose information to display on the form.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual Exception Exception
        {
            get
            {
                return m_Exception;
            }
            set
            {
                m_Exception = value;
                DisplayException();
            }
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.BaseErrorForm"/> instance.
        /// </summary>
        public BaseErrorForm()
        {
            InitializeComponent();

            HeadingImage = null;
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.BaseErrorForm"/> instance.
        /// </summary>
        /// <param name="exception">The exception whose information to display on the form.</param>
        public BaseErrorForm(Exception exception)
            : this()
        {
            m_Exception = exception;
        }

        /// <summary>
        /// Displays information about an exception on the form.
        /// </summary>
        private void DisplayException()
        {
            if (m_Exception != null)
            {
                if (m_VerboseOutput)
                {
                    detailsTextBox.Clear();

                    detailsTextBox.AppendText("Message:" + Environment.NewLine);
                    detailsTextBox.AppendText(m_Exception.Message + Environment.NewLine);
                    detailsTextBox.AppendText(Environment.NewLine);

                    detailsTextBox.AppendText("Stack trace:" + Environment.NewLine);
                    detailsTextBox.AppendText(m_Exception.StackTrace);

                    detailsTextBox.SelectionStart = 0;
                    detailsTextBox.ScrollToCaret();
                }
                else
                {
                    detailsTextBox.Text = m_Exception.Message;
                }
            }
            else
            {
                detailsTextBox.Clear();
            }
        }

        /// <summary>
        /// Adjusts the vertical location of the main elements of the form.
        /// </summary>
        /// <param name="adjustment">The number of pixels by which to adjust the elements' vertical locations.</param>
        private void AdjustBodyVerticalLocation(int adjustment)
        {
            Height += adjustment;
            detailsLabel.Top += adjustment;
            detailsTextBox.Height -= adjustment;
            detailsTextBox.Top += adjustment;
        }

        #region Event handlers

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.Paint"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (m_ShowHeading)
            {
                // Draw a horizontal rule below the white heading panel.
                int x = ClientRectangle.Left;
                int y = ClientRectangle.Top + headingPanel.Height;
                int width = ClientRectangle.Width;
                DrawHorizontalRule(e.Graphics, x, y, width);
            }
        }
        
        #endregion

        #region Form event handlers

        private void ErrorForm_Load(object sender, EventArgs e)
        {
            DisplayException();
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        #endregion
    }
}