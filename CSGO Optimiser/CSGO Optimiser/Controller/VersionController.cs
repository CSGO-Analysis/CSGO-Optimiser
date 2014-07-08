using System;
using System.Xml;

namespace Controller
{
    public class VersionController
    {
        // Find newest version from ftp and return it
        public Version GetNewestVersion()
        {
            Version newVersion = null;
            string url = "";
            XmlTextReader reader = null;
            try
            {
                string xmlURL = "http://peips.dk/csgo.xml";
                reader = new XmlTextReader(xmlURL);
                reader.MoveToContent();
                string elementName = "";
                if ((reader.NodeType == XmlNodeType.Element) &&
                    (reader.Name == "CSGO_Optimiser"))
                {
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                            elementName = reader.Name;
                        else
                        {
                            if ((reader.NodeType == XmlNodeType.Text) &&
                                (reader.HasValue))
                            {
                                switch (elementName)
                                {
                                    case "version":
                                        newVersion = new Version(reader.Value);
                                        break;
                                    case "url":
                                        url = reader.Value;
                                        break;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                if (reader != null) reader.Close();
            }
            return newVersion;
        }
    }
}
