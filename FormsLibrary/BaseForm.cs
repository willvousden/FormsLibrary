using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FormsLibrary
{
    /// <summary>
    /// A base form providing additional utilities and functionality to derived forms.
    /// </summary>
    public partial class BaseForm : Form
    {
        private static FormSettings m_FormSettings = null;

        /// <summary>
        /// Gets or sets the <see cref="FormsLibrary.FormSettings"/> used to save form locations.
        /// </summary>
        public static FormSettings FormSettings
        {
            get
            {
                return m_FormSettings;
            }
            set
            {
                m_FormSettings = value;
            }
        }

        private EventHandler<HotKeyEventArgs> m_HotKeyPressed = null;
        private EventHandler m_Maximised = null;
        private EventHandler m_Minimised = null;
        private EventHandler m_Restored = null;
        private Dictionary<int, bool> m_RegisteredHotKeys = new Dictionary<int, bool>();
        private Dictionary<int, MethodInvoker> m_HotKeyCallbacks = new Dictionary<int, MethodInvoker>();
        private bool m_LoadStateOnFormLoad = true;
        private bool m_StartInTray;

        #region Form state tracking

        private string m_FormId;
        private FormStateAspects m_PersistenceAspects;
        private bool m_IsMaximised = false;

        #endregion

        #region Window state event tracking

        private FormWindowState m_PreviousWindowState;

        #endregion

        #region Tray minimisation tracking

        private bool m_IsMinimisedToTray;
        private FormWindowState m_NonTrayWindowState; // The form's window state before being minimised to the tray.

        #endregion

        #region 'BringForward' tracking

        private FormWindowState m_NonMinimisedWindowState;

        #endregion

        /// <summary>
        /// Occurs when a registered hot key is pressed.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Occurs when the form is maximised.")]
        public virtual event EventHandler Maximised
        {
            add
            {
                m_Maximised += value;
            }
            remove
            {
                m_Maximised -= value;
            }
        }

        /// <summary>
        /// Occurs when a registered hot key is pressed.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Occurs when the form is minimised.")]
        public virtual event EventHandler Minimised
        {
            add
            {
                m_Minimised += value;
            }
            remove
            {
                m_Minimised -= value;
            }
        }

        /// <summary>
        /// Occurs when a registered hot key is pressed.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Occurs when the form is restored from minimised or maximised state.")]
        public virtual event EventHandler Restored
        {
            add
            {
                m_Restored += value;
            }
            remove
            {
                m_Restored -= value;
            }
        }

        /// <summary>
        /// Occurs when a registered hot key is pressed.
        /// </summary>
        [Browsable(true)]
        [Category("Behavior")]
        [Description("Occurs when a hot key registered with the form is pressed.")]
        public virtual event EventHandler<HotKeyEventArgs> HotKeyPressed
        {
            add
            {
                m_HotKeyPressed += value;
            }
            remove
            {
                m_HotKeyPressed -= value;
            }
        }

        /// <summary>
        /// Gets or sets a value specifying whether the form's previous state should be loaded from persistant storage when the form loads.
        /// </summary>
        [Browsable(false)]
        public virtual bool LoadStateOnFormLoad
        {
            get
            {
                return m_LoadStateOnFormLoad;
            }
            set
            {
                m_LoadStateOnFormLoad = value;
            }
        }

        /// <summary>
        /// Gets or sets a value specifying whether the form should be minimised to the system tray when its state is loaded from persistant storage.
        /// </summary>
        [Browsable(false)]
        public virtual bool StartInTray
        {
            get
            {
                return m_StartInTray;
            }
            set
            {
                m_StartInTray = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the form's location should persist across sessions.
        /// </summary>
        [Browsable(false)]
        public virtual bool RememberLocation
        {
            get
            {
                return (m_PersistenceAspects & FormStateAspects.Location) == FormStateAspects.Location;
            }
            set
            {
                if (value)
                {
                    m_PersistenceAspects |= FormStateAspects.Location;
                }
                else
                {
                    m_PersistenceAspects &= ~FormStateAspects.Location;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the form's size should persist across sessions.
        /// </summary>
        [Browsable(false)]
        public virtual bool RememberSize
        {
            get
            {
                return (m_PersistenceAspects & FormStateAspects.Size) == FormStateAspects.Size;
            }
            set
            {
                if (value)
                {
                    m_PersistenceAspects |= FormStateAspects.Size;
                }
                else
                {
                    m_PersistenceAspects &= ~FormStateAspects.Size;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the form's window state (i.e. whether the form is minimised, maximised or normal) should persist across sessions.
        /// </summary>
        [Browsable(false)]
        public virtual bool RememberWindowState
        {
            get
            {
                return (m_PersistenceAspects & FormStateAspects.WindowState) == FormStateAspects.WindowState;
            }
            set
            {
                if (value)
                {
                    m_PersistenceAspects |= FormStateAspects.WindowState;
                }
                else
                {
                    m_PersistenceAspects &= ~FormStateAspects.WindowState;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value specifying whether the form is visible or minimised and invisible (i.e. minimised to the system tray).
        /// </summary>
        [Browsable(false)]
        public virtual bool MinimisedToTray
        {
            get
            {
                return m_IsMinimisedToTray;
            }
            set
            {
                if (value != m_IsMinimisedToTray)
                {
                    if (value)
                    {
                        WindowState = FormWindowState.Minimized;
                        Visible = false;
                        m_IsMinimisedToTray = true;
                    }
                    else
                    {
                        Visible = true;

                        // Check that the window state hasn't been changed since it was minimised.
                        if (WindowState == FormWindowState.Minimized)
                        {
                            WindowState = m_NonTrayWindowState;
                        }

                        m_IsMinimisedToTray = false;
                    }
                }
            }
        }

        /// <summary>
        /// Gets or sets a string used to identify the form.
        /// </summary>
        protected virtual string FormId
        {
            get
            {
                return m_FormId;
            }
            set
            {
                m_FormId = value;
            }
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.BaseForm"/> instance.
        /// </summary>
        public BaseForm()
            : this(false)
        {
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.BaseForm"/> instance.
        /// </summary>
        /// <param name="startInTray"><c>true</c> if the form should initially be minimised to the system tray; <c>false</c> otherwise.</param>
        public BaseForm(bool startInTray)
            : this(true, startInTray)
        {
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.BaseForm"/> instance.
        /// </summary>
        /// <param name="loadStateOnFormLoad"><c>true</c> if the form's previous state should be loaded automatically from persistant storage when the form loads; <c>false</c> otherwise.</param>
        /// <param name="startInTray"><c>true</c> if the form should initially be minimised to the system tray; <c>false</c> otherwise.</param>
        public BaseForm(bool loadStateOnFormLoad, bool startInTray)
        {
            InitializeComponent();
            LoadPersistenceSettings();
            m_StartInTray = startInTray;
            m_LoadStateOnFormLoad = loadStateOnFormLoad;
        }

        /// <summary>
        /// Invokes a method on the thread that created the current form.
        /// </summary>
        /// <param name="method">The method to be called.</param>
        public virtual void UIInvoke(MethodInvoker method)
        {
            Utility.UIInvoke(this, method);
        }

        /// <summary>
        /// Causes the form's taskbar button to flash.
        /// </summary>
        public virtual void Flash()
        {
            Utility.Flash(this);
        }

        /// <summary>
        /// Brings the form to the front and ensures that it is visible.
        /// </summary>
        /// <param name="flash"><c>true</c> to cause the form's taskbar entry to flash; <c>false</c>
        /// otherwise.</param>
        public virtual void BringForward(bool flash)
        {
            if (InvokeRequired)
            {
                MethodInvoker callback = () =>
                                         {
                                             BringForward(flash);
                                         };
                UIInvoke(callback);
                return;
            }

            // Restore form to last non-minimised window state.
            MinimisedToTray = false;
            if (WindowState == FormWindowState.Minimized)
            {
                WindowState = m_NonMinimisedWindowState;
            }

            // Bring form forward and flash if necessary.
            Activate();
            if (flash)
            {
                Flash();
            }
        }

        /// <summary>
        /// Registers a hot key with the current form.
        /// </summary>
        /// <param name="hotKey">The hot key to be registered.</param>
        /// <param name="callback">A <see cref="System.Windows.Forms.MethodInvoker"/> delegate representing the method to be called when the hot key is pressed.</param>
        /// <returns><c>true</c> if successful; <c>false</c> otherwise.</returns>
        protected virtual bool RegisterHotKey(HotKey hotKey, MethodInvoker callback)
        {
            return RegisterHotKey(hotKey, callback, false);
        }

        /// <summary>
        /// Registers a hot key with the current form.
        /// </summary>
        /// <param name="hotKey">The hot key to be registered.</param>
        /// <param name="callback">A <see cref="System.Windows.Forms.MethodInvoker"/> delegate representing the method to be called when the hot key is pressed.</param>
        /// <param name="whenInactive"><c>true</c> to raise an event even if the form is not active; <c>false</c> otherwise.</param>
        /// <returns><c>true</c> if successful; <c>false</c> otherwise.</returns>
        protected virtual bool RegisterHotKey(HotKey hotKey, MethodInvoker callback, bool whenInactive)
        {
            int id;
            bool success = RegisterHotKey(hotKey, out id, whenInactive);
            m_HotKeyCallbacks.Add(id, callback);
            return success;
        }

        /// <summary>
        /// Registers a hot key with the current form.
        /// </summary>
        /// <param name="hotKey">The hot key to be registered.</param>
        /// <param name="hotKeyId">A 32-bit signed integer used to identify the hot key.</param>
        /// <returns><c>true</c> if successful; <c>false</c> otherwise.</returns>
        protected virtual bool RegisterHotKey(HotKey hotKey, out int hotKeyId)
        {
            return RegisterHotKey(hotKey, out hotKeyId, false);
        }

        /// <summary>
        /// Registers a hot key with the current form.
        /// </summary>
        /// <param name="hotKey">The hot key to be registered.</param>
        /// <param name="hotKeyId">A 32-bit integer used to identify the hot key.</param>
        /// <param name="whenInactive"><c>true</c> to raise an event even if the form is not active; <c>false</c> otherwise.</param>
        /// <returns><c>true</c> if successful; <c>false</c> otherwise.</returns>
        protected virtual bool RegisterHotKey(HotKey hotKey, out int hotKeyId, bool whenInactive)
        {
            if (HotKeyRegistrar.AvailableDllIds.Count == 0)
            {
                throw new ArgumentException("No more hot keys can be registered; the limit has been reached.");
            }

            hotKeyId = HotKeyRegistrar.AvailableDllIds.Peek();
            bool success = HotKeyRegistrar.Register(this, hotKeyId, hotKey);
            if (success)
            {
                HotKeyRegistrar.AvailableDllIds.Pop();
                m_RegisteredHotKeys.Add(hotKeyId, whenInactive);
            }

            return success;
        }

        /// <summary>
        /// Unregisters a hot key with the current form.
        /// </summary>
        /// <param name="callback">A <see cref="System.Windows.Forms.MethodInvoker"/> delegate with which the hot key to be unregistered is associated.</param>
        /// <returns><c>true</c> if successful; <c>false</c> otherwise.</returns>
        protected virtual bool UnregisterHotKey(MethodInvoker callback)
        {
            // Determine the hot key ID associated with the given callback.
            int hotKeyId = -1;
            bool exists = false;
            foreach (int id in m_HotKeyCallbacks.Keys)
            {
                bool isCallbackId = object.ReferenceEquals(m_HotKeyCallbacks[id], callback);
                if (isCallbackId)
                {
                    hotKeyId = id;
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                throw new ArgumentException("Given delegate is not a registered callback.", "callback");
            }

            m_HotKeyCallbacks.Remove(hotKeyId);
            return UnregisterHotKey(hotKeyId);
        }

        /// <summary>
        /// Unregisters a hot key with the current form.
        /// </summary>
        /// <param name="hotKeyId">A 32-bit integer used to identify the hot key.</param>
        /// <returns><c>true</c> if successful; <c>false</c> otherwise.</returns>
        protected virtual bool UnregisterHotKey(int hotKeyId)
        {
            bool exists = HotKeyRegistrar.AvailableDllIds.Contains(hotKeyId);
            if (exists)
            {
                throw new ArgumentException("Hot key id was not found.", "hotKeyId");
            }

            bool success = HotKeyRegistrar.Unregister(this, hotKeyId);
            if (success)
            {
                HotKeyRegistrar.AvailableDllIds.Push(hotKeyId);
                m_RegisteredHotKeys.Remove(hotKeyId);
            }

            return success;
        }

        /// <summary>
        /// Draws a horizontal rule on the form.
        /// </summary>
        /// <param name="surface">The GDI+ surface on which to draw the rule.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rule.</param>
        /// <param name="y">The y-coordiante of the upper-left corner of the rule.</param>
        /// <param name="width">The width of the rule.</param>
        protected virtual void DrawHorizontalRule(Graphics surface, int x, int y, int width)
        {
            Utility.DrawHorizontalRule(surface, x, y, width);
        }

        /// <summary>
        /// Draws a horizontal rule on the form.
        /// </summary>
        /// <param name="surface">The GDI+ surface on which to draw the rule.</param>
        /// <param name="x">The x-coordinate of the upper-left corner of the rule.</param>
        /// <param name="y">The y-coordiante of the upper-left corner of the rule.</param>
        /// <param name="width">The width of the rule.</param>
        /// <param name="topColour">The colour of the top part of the rule.</param>
        /// <param name="bottomColour">The colour of the bottom part of the rule.</param>
        protected virtual void DrawHorizontalRule(Graphics surface, int x, int y, int width, Color topColour, Color bottomColour)
        {
            Utility.DrawHorizontalRule(surface, x, y, width, topColour, bottomColour);
        }

        /// <summary>
        /// Saves the form's current state to persistant storage.
        /// </summary>
        protected virtual void SaveState()
        {
            if (m_FormSettings != null && m_PersistenceAspects != FormStateAspects.None)
            {
                FormState state = GetFormState(true);

                if (WindowState != FormWindowState.Normal)
                {
                    // Save bounds as they were prior to form being maximised or minimised.
                    state.Bounds = RestoreBounds;
                }
                else
                {
                    // Save current bounds.
                    state.Bounds = Bounds;
                }

                state.Maximised = m_IsMaximised;
            }
        }

        /// <summary>
        /// Loads the form's previous state from persistant storage.
        /// </summary>
        protected virtual void LoadState(bool startInTray)
        {
            FormWindowState windowState = WindowState;

            // Restore settings from persistent storage.
            FormState state = GetFormState();
            if (state != null)
            {
                if (RememberLocation)
                {
                    Location = state.Bounds.Location;
                }

                if (RememberSize)
                {
                    Size = state.Bounds.Size;
                }

                if (RememberWindowState)
                {
                    m_IsMaximised = state.Maximised;
                    if (state.Maximised)
                    {
                        windowState = FormWindowState.Maximized;
                    }
                    else
                    {
                        windowState = FormWindowState.Normal;
                    }
                }
            }

            m_NonMinimisedWindowState = windowState;
            if (startInTray)
            {
                // Start minimised and store saved window state for later restoration.
                MinimisedToTray = true;
                m_NonTrayWindowState = windowState;
            }
            else
            {
                // Start with saved window state.
                WindowState = windowState;
            }

            m_PreviousWindowState = WindowState;
        }

        /// <summary>
        /// Gets the form state information for the current instance, or <c>null</c> if there is none.
        /// </summary>
        private FormState GetFormState()
        {
            return GetFormState(false);
        }

        /// <summary>
        /// Gets the form state information for the current instance, or <c>null</c> if there is none.
        /// </summary>
        /// <param name="createIfNotFound"><c>true</c> to create a new instance and return it if no state is found; <c>false</c> to return null in this case.</param>
        private FormState GetFormState(bool createIfNotFound)
        {
            bool stateExists = m_FormSettings != null &&
                               m_FormSettings.FormStates.ContainsKey(m_FormId);
            if (stateExists)
            {
                return m_FormSettings.FormStates[m_FormId];
            }
            else if (createIfNotFound)
            {
                if (m_FormSettings == null)
                {
                    m_FormSettings = new FormSettings();
                }

                FormState state = new FormState();
                m_FormSettings.FormStates.Add(m_FormId, state);
                return state;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Checks the form's current border style to determine whether the size of the form should be saved.
        /// </summary>
        /// <returns><c>true</c> if the form's size should persist; <c>false</c> otherwise.</returns>
        private bool CheckBorderStyle()
        {
            return FormBorderStyle == FormBorderStyle.Sizable ||
                   FormBorderStyle == FormBorderStyle.SizableToolWindow;
        }

        /// <summary>
        /// Loads the current type's default form persistence settings from its <see cref="FormsLibrary.FormPersistenceAttribute"/> attribute (if any).
        /// </summary>
        private void LoadPersistenceSettings()
        {
            FormPersistenceAttribute attribute = Utility.GetAttribute<FormPersistenceAttribute>(GetType(), false);

            if (attribute != null)
            {
                m_FormId = attribute.FormId != null ? attribute.FormId : GetType().FullName;
                m_PersistenceAspects = attribute.Aspects;
            }
            else
            {
                m_FormId = GetType().FullName;
                m_PersistenceAspects = FormStateAspects.None;
            }
        }

        #region Event handlers

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Form.Load"/> event;
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnLoad(EventArgs e)
        {
            if (m_LoadStateOnFormLoad && m_FormSettings != null && m_FormSettings.RememberFormStates)
            {
                LoadState(m_StartInTray);
            }

            base.OnLoad(e);
        }

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Control.SizeChanged"/> event;
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (WindowState == FormWindowState.Normal && m_PreviousWindowState != FormWindowState.Normal)
            {
                OnRestored(EventArgs.Empty);
            }

            if (WindowState == FormWindowState.Maximized && m_PreviousWindowState != FormWindowState.Maximized)
            {
                OnMaximised(EventArgs.Empty);
            }

            if (WindowState == FormWindowState.Minimized && m_PreviousWindowState != FormWindowState.Minimized)
            {
                OnMinimised(EventArgs.Empty);
            }

            if (WindowState != FormWindowState.Minimized)
            {
                m_NonMinimisedWindowState = WindowState;
                m_NonTrayWindowState = WindowState;
            }

            m_PreviousWindowState = WindowState;

            if (WindowState == FormWindowState.Maximized)
            {
                m_IsMaximised = true;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                m_IsMaximised = false;
            }
        }

        /// <summary>
        /// Raises the <see cref="System.Windows.Forms.Form.FormClosed"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.Windows.Forms.FormClosedEventArgs"/> that contains the event data.</param>
        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            SaveState();

            // Unregister hot keys.
            int[] hotKeyIds = new int[m_RegisteredHotKeys.Keys.Count];
            m_RegisteredHotKeys.Keys.CopyTo(hotKeyIds, 0);
            foreach (int hotKeyId in hotKeyIds)
            {
                UnregisterHotKey(hotKeyId);
            }

            base.OnFormClosed(e);
        }

        /// <summary>
        /// Processes a Windows message.
        /// </summary>
        /// <param name="m">The Windows message.</param>
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == (int)WindowMessage.WM_HOTKEY)
            {
                int hotKeyId = m.WParam.ToInt32();
                bool whenInactive = m_RegisteredHotKeys.ContainsKey(hotKeyId) &&
                                    m_RegisteredHotKeys[hotKeyId];
                bool isActive = object.ReferenceEquals(this, ActiveForm);
                if (isActive || whenInactive)
                {
                    bool callbackSpecified = m_HotKeyCallbacks.ContainsKey(hotKeyId) &&
                                             m_HotKeyCallbacks[hotKeyId] != null;
                    if (callbackSpecified)
                    {
                        m_HotKeyCallbacks[hotKeyId]();
                    }
                    else
                    {
                        OnHotKeyPressed(new HotKeyEventArgs(hotKeyId));
                    }
                }
            }

            base.WndProc(ref m);
        }

        /// <summary>
        /// Raises the <see cref="FormsLibrary.BaseForm.Maximised"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected virtual void OnMaximised(EventArgs e)
        {
            if (m_Maximised != null)
            {
                m_Maximised(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="FormsLibrary.BaseForm.Minimised"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected virtual void OnMinimised(EventArgs e)
        {
            if (m_Minimised != null)
            {
                m_Minimised(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="FormsLibrary.BaseForm.Restored"/> event.
        /// </summary>
        /// <param name="e">A <see cref="System.EventArgs"/> that contains the event data.</param>
        protected virtual void OnRestored(EventArgs e)
        {
            if (m_Restored != null)
            {
                m_Restored(this, e);
            }
        }

        /// <summary>
        /// Raises the <see cref="FormsLibrary.BaseForm.HotKeyPressed"/> event.
        /// </summary>
        /// <param name="e">A <see cref="FormsLibrary.HotKeyEventArgs"/> that contains the event data.</param>
        protected virtual void OnHotKeyPressed(HotKeyEventArgs e)
        {
            if (m_HotKeyPressed != null)
            {
                m_HotKeyPressed(this, e);
            }
        }

        #endregion
    }
}