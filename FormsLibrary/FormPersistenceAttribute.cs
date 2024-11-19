using System;

namespace FormsLibrary
{
    /// <summary>
    /// Specifies the extent to which the form's state should be saved to settings and restored at the beginning of the next session. This class cannot be inherited.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited=false, AllowMultiple=false)]
    public sealed class FormPersistenceAttribute : Attribute
    {
        private string m_FormId = null;
        private FormStateAspects m_Aspects = FormStateAspects.None;

        /// <summary>
        /// Gets or sets a string used to identify the form.
        /// </summary>
        public string FormId
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
        /// Gets or sets the aspects of the form that should persist accross sessions.
        /// </summary>
        public FormStateAspects Aspects
        {
            get
            {
                return m_Aspects;
            }
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.FormPersistenceAttribute"/> instance.
        /// </summary>
        /// <param name="aspects">The aspects of the form that should persist across sessions.</param>
        public FormPersistenceAttribute(FormStateAspects aspects)
        {
            m_Aspects = aspects;
        }
    }
}