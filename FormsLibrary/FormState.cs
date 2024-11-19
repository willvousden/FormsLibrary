using System;
using System.Diagnostics;
using System.Drawing;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FormsLibrary
{
    /// <summary>
    /// Contains saved information about a form's state.
    /// </summary>
    [Serializable]
    internal class FormState : IXmlSerializable
    {
        private Rectangle m_Bounds = new Rectangle();
        private bool m_Maximised = false;

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.FormState"/> instance.
        /// </summary>
        public FormState()
        {
        }

        /// <summary>
        /// Gets or sets a <see cref="System.Drawing.Rectangle"/> representing the form's bounds.
        /// </summary>
        /// <remarks>
        /// This will always contain the bounds of the form in its normal window state; minimising or maximising the form will have no effect.
        /// </remarks>
        public Rectangle Bounds
        {
            get
            {
                return m_Bounds;
            }
            set
            {
                m_Bounds = value;
            }
        }

        /// <summary>
        /// Gets or sets a value specifying whether the form's unminimised state is maximised as opposed to normal.
        /// </summary>
        public bool Maximised
        {
            get
            {
                return m_Maximised;
            }
            set
            {
                m_Maximised = value;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="FormsLibrary.FormState"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="FormsLibrary.FormState"/>.</returns>
        public override string ToString()
        {
            return string.Format("{0}, {1}", m_Maximised ? "Maximised" : "Not maximised", m_Bounds);
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
            reader.ReadStartElement("formState");

            // Read form's bounds.
            m_Bounds = new Rectangle();
            reader.ReadStartElement("bounds");
            m_Bounds.X = reader.ReadElementContentAsInt("x", string.Empty);
            m_Bounds.Y = reader.ReadElementContentAsInt("y", string.Empty);
            m_Bounds.Width = reader.ReadElementContentAsInt("width", string.Empty);
            m_Bounds.Height = reader.ReadElementContentAsInt("height", string.Empty);
            reader.ReadEndElement();

            // Read form's state.
            try
            {
                m_Maximised = reader.ReadElementContentAsBoolean("maximised", string.Empty);
            }
            catch (Exception)
            {
                Debugger.Break();
                throw;
            }

            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("formState");

            // Write form's bounds.
            writer.WriteStartElement("bounds");
            writer.WriteElementString("x", m_Bounds.X.ToString());
            writer.WriteElementString("y", m_Bounds.Y.ToString());
            writer.WriteElementString("width", m_Bounds.Width.ToString());
            writer.WriteElementString("height", m_Bounds.Height.ToString());
            writer.WriteEndElement();

            // Write form's state.
            writer.WriteStartElement("maximised");
            writer.WriteValue(m_Maximised);
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        #endregion
    }
}