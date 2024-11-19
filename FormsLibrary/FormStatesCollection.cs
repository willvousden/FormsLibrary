using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace FormsLibrary
{
    /// <summary>
    /// A collection of <see cref="FormsLibrary.FormState"/> objects that describe the state of a windows form.
    /// </summary>
    [Serializable]
    internal class FormStatesCollection : Dictionary<string, FormState>, IXmlSerializable
    {
        /// <summary>
        /// Initialises a new <see cref="FormsLibrary.FormStatesCollection"/> instance.
        /// </summary>
        internal FormStatesCollection()
        {
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
            reader.ReadStartElement("formStates");

            if (reader.NodeType != XmlNodeType.None)
            {
                while (reader.NodeType != XmlNodeType.EndElement)
                {
                    // Read forms's key.
                    string id = reader.GetAttribute("id", string.Empty);
                    FormState formState = new FormState();

                    // Read form's information.
                    reader.ReadStartElement("form");
                    ((IXmlSerializable)formState).ReadXml(reader);
                    Add(id, formState);

                    reader.ReadEndElement();
                    reader.MoveToContent();
                }

                reader.ReadEndElement();
            }
        }

        void IXmlSerializable.WriteXml(XmlWriter writer)
        {
            writer.WriteStartElement("formStates");
            foreach (string id in Keys)
            {
                writer.WriteStartElement("form");

                // Write form's key.
                writer.WriteAttributeString("id", id);

                // Write form's information.
                ((IXmlSerializable)this[id]).WriteXml(writer);

                writer.WriteEndElement();
            }
            writer.WriteEndElement();
        }

        #endregion
    }
}