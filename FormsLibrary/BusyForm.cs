using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace FormsLibrary
{
    /// <summary>
    /// Displayed to the user when the application is busy.
    /// </summary>
    public partial class BusyForm : BaseForm
    {
        private Exception m_Error = null;
        private bool m_ShowCancelButton;

        /// <summary>
        /// Gets or sets the form's <see cref="System.Windows.Forms.Form.Text"/> property. Allows cross-thread access.
        /// </summary>
        public string Title
        {
            get
            {
                string title = null;
                UIInvoke(() => title = Text);
                return title;
            }
            set
            {
                UIInvoke(() => Text = value);
            }
        }

        /// <summary>
        /// Gets or sets the message displayed on the form. Allows cross-thread access.
        /// </summary>
        public string Message
        {
            get
            {
                string message = null;
                UIInvoke(() => message = messageLabel.Text);
                return message;
            }
            set
            {
                UIInvoke(() => messageLabel.Text = value);
            }
        }

        /// <summary>
        /// Gets or sets the style of the progress bar. Allows cross-thread access.
        /// </summary>
        public ProgressBarStyle ProgressBarStyle
        {
            get
            {
                ProgressBarStyle style = ProgressBarStyle.Marquee;
                UIInvoke(() => style = progressBar.Style);
                return style;
            }
            set
            {
                UIInvoke(() => progressBar.Style = value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the progress bar.
        /// </summary>
        public int ProgressBarValue
        {
            get
            {
                int value = 0;
                UIInvoke(() => value = progressBar.Value);
                return value;
            }
            set
            {
                UIInvoke(() => progressBar.Value = value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the progress bar.
        /// </summary>
        public int ProgressBarMaximum
        {
            get
            {
                int maximum = 0;
                UIInvoke(() => maximum = progressBar.Maximum);
                return maximum;
            }
            set
            {
                UIInvoke(() => progressBar.Maximum = value);
            }
        }

        /// <summary>
        /// Gets or sets the value of the progress bar.
        /// </summary>
        public int ProgressBarMinimum
        {
            get
            {
                int minimum = 0;
                UIInvoke(() => minimum = progressBar.Minimum);
                return minimum;
            }
            set
            {
                UIInvoke(() => progressBar.Value = value);
            }
        }

        /// <summary>
        /// Gets the error that occurred during the execution of the task, or <c>null</c> if none occurred.
        /// </summary>
        public Exception Error
        {
            get
            {
                return m_Error;
            }
        }

        /// <summary>
        /// Gets or sets a value specifying whether to display a cancel button on the form.
        /// </summary>
        public bool ShowCancelButton
        {
            get
            {
                return m_ShowCancelButton;
            }
            set
            {
                MethodInvoker method = () =>
                                       {
                                           if (!m_ShowCancelButton && value)
                                           {
                                               cancelButton.Show();
                                               Height += 29;
                                           }
                                           else if (m_ShowCancelButton && !value)
                                           {
                                               cancelButton.Hide();
                                               Height -= 29;
                                           }

                                           m_ShowCancelButton = value;
                                       };
                UIInvoke(method);
            }
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.BusyForm"/> instance.
        /// </summary>
        public BusyForm()
        {
            InitializeComponent();

            m_ShowCancelButton = true;
            ShowCancelButton = false;
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.BusyForm"/> instance.
        /// </summary>
        /// <param name="showCancelButton"><c>true</c> to show a cancel button on the form; <c>false</c>
        /// otherwise.</param>
        public BusyForm(bool showCancelButton)
            : this()
        {
            ShowCancelButton = showCancelButton;
        }

        /// <summary>
        /// Displays the form and starts working on the given task.
        /// </summary>
        public void PerformTask(MethodInvoker task, string message)
        {
            PerformTask(task, message, () => ShowDialog());
        }

        /// <summary>
        /// Displays the form and starts working on the given task.
        /// </summary>
        /// <param name="task">A callback to be used to display the form.</param>
        /// <param name="message">A message to be displayed on the form.</param>
        /// <param name="displayCallback">A custom callback used to display the form.</param>
        public void PerformTask(MethodInvoker task, string message, MethodInvoker displayCallback)
        {
            if (task == null || message == null || displayCallback == null)
            {
                throw new ArgumentNullException();
            }
            else if (backgroundWorker.IsBusy)
            {
                throw new InvalidOperationException("Cannot perform multiple tasks simultaneously.");
            }

            Message = message;
            StartMarquee();
            backgroundWorker.RunWorkerAsync(task);
            displayCallback();
            if (m_Error != null)
            {
                throw m_Error;
            }
        }

        /// <summary>
        /// Sets the progress bar to marquee mode.
        /// </summary>
        private void StartMarquee()
        {
            progressBar.Style = ProgressBarStyle.Marquee;
        }

        /// <summary>
        /// Stops a progress bar that is in marquee mode.
        /// </summary>
        private void StopMarquee()
        {
            if (progressBar.Style == ProgressBarStyle.Marquee)
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = progressBar.Minimum;
            }
        }

        #region Form event handlers

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            MethodInvoker task = e.Argument as MethodInvoker;
            if (task != null)
            {
                try
                {
                    task();
                }
                catch (Exception exception)
                {
                    m_Error = exception;
                }
            }
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                m_Error = e.Error;
            }

            Close();
        }

        #endregion
    }
}