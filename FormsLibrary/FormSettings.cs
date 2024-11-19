using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FormsLibrary
{
    /// <summary>
    /// Contains settings describing the saved windows forms and allows them to be serialised, either as XML or binary.
    /// </summary>
    [Serializable]
    public class FormSettings : IXmlSerializable
    {
        private bool m_RememberFormStates;
        private FormStatesCollection m_FormStates = new FormStatesCollection();

        /// <summary>
        /// Gets or sets a value specifying whether form states should be saved and restored at the beginning of each session.
        /// </summary>
        public bool RememberFormStates
        {
            get
            {
                return m_RememberFormStates;
            }
            set
            {
                m_RememberFormStates = value;
            }
        }

        /// <summary>
        /// Gets a collection containing form state descriptors.
        /// </summary>
        internal FormStatesCollection FormStates
        {
            get
            {
                return m_FormStates;
            }
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.FormSettings"/> instance.
        /// </summary>
        public FormSettings()
            : this(true)
        {
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.FormSettings"/> instance.
        /// </summary>
        /// <param name="rememberFormStates">A value specifying whether form states should be saved and restored at the beginning of each session.</param>
        public FormSettings(bool rememberFormStates)
        {
            m_RememberFormStates = rememberFormStates;
        }

        #region IXmlSerializable Members

        XmlSchema IXmlSerializable.GetSchema()
        {
            return null;
        }

        void IXmlSerializable.ReadXml(XmlReader reader)
        {
            if (reader.NodeType == XmlNodeType.Element && reader.LocalName == GetType().Name)
            {
                reader.Read();
            }
            reader.ReadStartElement("formSettings");

            // Read form data.
            m_FormStates = new FormStatesCollection();
            if (reader.NodeType != XmlNodeType.EndElement)
            {
                m_RememberFormStates = true;
                ((IXmlSerializable)m_FormStates).ReadXml(reader);
            }
            else
            {
                m_RememberFormStates = false;
            }

            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("formSettings");

            // Write form data.
            if (m_RememberFormStates)
            {
                ((IXmlSerializable)m_FormStates).WriteXml(writer);
            }

            writer.WriteEndElement();
        }

        #endregion
    }
}