using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace FormsLibrary.Controls
{
    /// <summary>
    /// Displays a series of images as repeating animation.
    /// </summary>
    [Description("Displays a series of images as a repeating animation.")]
    public partial class Throbber : PictureBox
    {
        private static readonly int DefaultAnimationInterval = 100;
        private static readonly int DefaultBlinkInterval = 500;

        private Image[] m_FrameImages = new Image[0];
        private Image m_InactiveImage;
        private ThrobberState m_State = ThrobberState.Inactive;
        private int m_CurrentFrame;
        private int m_AnimationInterval = DefaultAnimationInterval;
        private int m_BlinkInterval = DefaultBlinkInterval;

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.Controls.Throbber"/> instance.
        /// </summary>
        public Throbber()
        {
            InitializeComponent();

            Image = m_InactiveImage;
        }

        /// <summary>
        /// Gets or sets an array of images to be used as the throbber's frames.
        /// </summary>
        [Browsable(false)]
        public virtual Image[] FrameImages
        {
            get
            {
                return m_FrameImages;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException();
                }
                else if (value.Length == 0)
                {
                    throw new ArgumentException("Frame array must contain at least one image.");
                }

                m_FrameImages = value;
            }
        }

        /// <summary>
        /// Gets or sets the image to be displayed when the throbber is inactive.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [Description("The image to be used when the throbber is inactive.")]
        public virtual Image InactiveImage
        {
            get
            {
                return m_InactiveImage;
            }
            set
            {
                m_InactiveImage = value;
            }
        }

        /// <summary>
        /// Gets or sets the interval to be used between frames when the throbber is active.
        /// </summary>
        [Browsable(true)]
        [Description("The interval to be used between frames when the throbber is active.")]
        public virtual int AnimationInterval
        {
            get
            {
                return m_AnimationInterval;
            }
            set
            {
                m_AnimationInterval = value;

                if (m_State == ThrobberState.Active)
                {
                    animationTimer.Interval = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the interval to be used between frames when the throbber is blinking.
        /// </summary>
        [Browsable(true)]
        [Description("The interval to be used between frames when the throbber is blinking.")]
        public virtual int BlinkInterval
        {
            get
            {
                return m_BlinkInterval;
            }
            set
            {
                m_BlinkInterval = value;

                if (m_State == ThrobberState.Blink)
                {
                    animationTimer.Interval = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the state of the throbber.
        /// </summary>
        [RefreshProperties(RefreshProperties.All)]
        [Browsable(true)]
        [Description("The state of the throbber.")]
        public virtual ThrobberState State
        {
            get
            {
                return m_State;
            }
            set
            {
                switch (value)
                {
                    case ThrobberState.Active:
                        Start();
                        break;
                    case ThrobberState.Blink:
                        Blink();
                        break;
                    case ThrobberState.Inactive:
                        Stop();
                        break;
                    default:
                        Debug.Fail("Unknown enumeration member.");
                        break;
                }
            }
        }

        /// <summary>
        /// Starts the throbber's animation.
        /// </summary>
        public virtual void Start()
        {
            Image = null;
            m_State = ThrobberState.Active;
            m_CurrentFrame = 1;
            animationTimer.Interval = m_AnimationInterval;
            animationTimer.Start();
        }

        /// <summary>
        /// Stops the throbber's animation.
        /// </summary>
        public virtual void Stop()
        {
            m_State = ThrobberState.Inactive;
            animationTimer.Stop();
            m_CurrentFrame = 0;
            Image = m_InactiveImage;
        }

        /// <summary>
        /// Sets the throbber's animation mode to blink.
        /// </summary>
        public virtual void Blink()
        {
            m_CurrentFrame = 0;
            m_State = ThrobberState.Blink;
            animationTimer.Interval = m_BlinkInterval;
            animationTimer.Start();
        }

        /// <summary>
        /// Advances the throbber's animation to the next frame.
        /// </summary>
        protected virtual void NextFrame()
        {
            if (m_State == ThrobberState.Active)
            {
                if (m_FrameImages.Length != 0)
                {
                    Image = m_FrameImages[m_CurrentFrame];
                    m_CurrentFrame = (m_CurrentFrame + 1) % m_FrameImages.Length;
                }
            }
            else if (m_State == ThrobberState.Blink)
            {
                if (m_CurrentFrame == 1)
                {
                    Image = InactiveImage;
                }
                else if (m_CurrentFrame == 0)
                {
                    Image = null;
                }
                m_CurrentFrame = 1 - m_CurrentFrame;
            }
        }

        #region Control handlers

        private void animationTimer_Tick(object sender, EventArgs e)
        {
            NextFrame();
        }

        #endregion
    }
}