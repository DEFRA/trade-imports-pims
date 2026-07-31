namespace Defra.Imports.IntegrationTests.LogicApps.Helpers
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    public class XmlHelper
    {
        private XmlDocument _doc;

        public XmlHelper(string fileName)
        {
            this._doc = new XmlDocument();
            this._doc.Load(fileName);
        }

        public List<string> GetNodesAsStrings(string elementName)
        {
            XmlNodeList nodeList = this._doc.GetElementsByTagName(elementName);
            return this.GetNodesAsStrings(nodeList);
        }

        public List<string> GetNodesAsStrings(XmlNodeList nodes)
        {
            List<string> outputNodeStrings = new List<string>();
            using (StringWriter stringWriter = new StringWriter())
            {
                using (XmlTextWriter xmlWriter = new XmlTextWriter(stringWriter))
                {
                    xmlWriter.Formatting = Formatting.Indented;
                    xmlWriter.Indentation = 2;
                    foreach (XmlNode node in nodes)
                    {
                        // Clear the string writer
                        StringBuilder sb = stringWriter.GetStringBuilder();
                        sb.Remove(0, sb.Length);

                        // Write node and add to list
                        node.WriteTo(xmlWriter);
                        outputNodeStrings.Add(stringWriter.ToString());
                    }
                }
            }

            return outputNodeStrings;
        }
    }
}
