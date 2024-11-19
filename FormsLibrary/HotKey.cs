using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FormsLibrary
{
    /// <summary>
    /// Represents a hot key combination.
    /// </summary>
    [Serializable]
    public class HotKey : IXmlSerializable, IEquatable<HotKey>
    {
        private Keys m_Key;
        private KeyModifiers m_Modifiers;

        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        public Keys Key
        {
            get
            {
                return m_Key;
            }
            set
            {
                m_Key = ((value & ~Keys.Control) & ~Keys.Alt) & ~Keys.Shift;
            }
        }

        /// <summary>
        /// Gets or sets the key modifiers.
        /// </summary>
        public KeyModifiers Modifiers
        {
            get
            {
                return m_Modifiers;
            }
            set
            {
                m_Modifiers = value;
            }
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.HotKey"/> instance.
        /// </summary>
        public HotKey()
        {
        }

        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.HotKey"/> instance.
        /// </summary>
        /// <param name="key">The primary key of the hot key combination.</param>
        /// <param name="modifiers">The key modifiers for the hot key combination.</param>
        public HotKey(Keys key, KeyModifiers modifiers)
        {
            m_Key = ((key & ~Keys.Control) & ~Keys.Alt) & ~Keys.Shift;
            m_Modifiers = modifiers;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="FormsLibrary.HotKey"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="FormsLibrary.HotKey"/>.</returns>
        public override string ToString()
        {
            if (m_Key == Keys.ControlKey || m_Key == Keys.ShiftKey || m_Key == Keys.Menu || m_Key == Keys.None)
            {
                return "None";
            }

            List<string> keys = new List<string>();
            if ((m_Modifiers & KeyModifiers.Alt) == KeyModifiers.Alt)
            {
                keys.Add("Alt");
            }
            if ((m_Modifiers & KeyModifiers.Control) == KeyModifiers.Control)
            {
                keys.Add("Ctrl");
            }
            if ((m_Modifiers & KeyModifiers.Shift) == KeyModifiers.Shift)
            {
                keys.Add("Shift");
            }

            keys.Add(m_Key.ToString());
            string keyString = string.Join(" + ", keys.ToArray());

            return keyString;
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
            reader.ReadStartElement("hotKey");

            // Read main key.
            string keyString = reader.ReadElementString("key");
            m_Key = (Keys)Enum.Parse(typeof(Keys), keyString);

            // Read hot key modifiers.
            reader.ReadStartElement("modifiers");
            m_Modifiers = KeyModifiers.None;
            while (reader.NodeType != XmlNodeType.EndElement)
            {
                string modifierString = reader.ReadElementString("modifier");
                KeyModifiers modifier = (KeyModifiers)Enum.Parse(typeof(KeyModifiers),
                                                                 modifierString);
                m_Modifiers |= modifier;

                reader.MoveToContent();
            }
            reader.ReadEndElement();

            reader.ReadEndElement();
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("hotKey");

            // Write the main key.
            writer.WriteElementString("key", m_Key.ToString());

            // Write the modifiers for the hot key.
            writer.WriteStartElement("modifiers");
            foreach (object value in Enum.GetValues(typeof(KeyModifiers)))
            {
                KeyModifiers modifier = (KeyModifiers)value;
                if ((m_Modifiers & modifier) == modifier)
                {
                    writer.WriteElementString("modifier", modifier.ToString());
                }
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
        }

        #endregion

        #region IEquatable<HotKey> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">An object to compare with this object.</param>
        /// <returns><c>true</c> if the current object is equal to the other parameter; <c>false</c> otherwise.</returns>
        public bool Equals(HotKey other)
        {
            return m_Key == other.m_Key && m_Modifiers == other.m_Modifiers;
        }

        #endregion
    }
}