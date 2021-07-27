using Extension.Utilities.Exceptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Extension.Utilities.Serialization
{
    public static class CommonSerializer
    {
        /// <summary>
        /// The Logger for this class
        /// </summary>
        public static ILogger _log;

        /// <summary>
        /// Saves a .net object as an xml file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="persistanceObject"></param>
        public static void SaveAsXMLFile<T>(string fileName, T persistanceObject)
        {
            if (!fileName.EndsWith(".xml"))
            {
                throw new InvalidFileExtension("The filename requires the extension .xml");
            }

            var tmpPath = Path.GetTempFileName();
            //Create a backup of the file, if it already exists
            if (File.Exists(fileName))
            {
                File.Move(fileName, tmpPath);
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (var stream = new FileStream(fileName, FileMode.Create))
                {
                    XmlWriterSettings xmlWriterSettings = new XmlWriterSettings
                    {
                        Indent = true,
                        Encoding = Encoding.UTF8
                    };
                    XmlWriter xmlWriter = XmlWriter.Create(stream, xmlWriterSettings);

                    // Serializes the project, and closes the TextWriter.
                    serializer.Serialize(xmlWriter, persistanceObject);
                    xmlWriter.Close();
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                if (File.Exists(tmpPath))
                {
                    File.Move(tmpPath, fileName);
                }
                //Rethrow the exception
                throw ex;
            }
        }

        /// <summary>
        /// Load a .net object from a xml file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T LoadXMLFile<T>(string fileName)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));

            //Hanlde unknown nodes and attributes
            serializer.UnknownNode += new XmlNodeEventHandler(Serializer_UnknownNode);
            serializer.UnknownAttribute += new XmlAttributeEventHandler(Serializer_UnknownAttribute);

            using (var stream = new FileStream(fileName, 
                                                FileMode.Open, 
                                                FileAccess.Read, 
                                                FileShare.ReadWrite))
            {
                var obj = (T)serializer.Deserialize(stream);
                return obj;
            }
        }

        /// <summary>
        /// Saves the object T as a file with the given fileName (without extension)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <param name="persistanceObject"></param>
        public static void SaveAsJsonFile<T>(string fileName, T persistanceObject)
        {
            if (!fileName.EndsWith(".xml"))
            {
                throw new InvalidFileExtension("The filename requires the extension .xml");
            }

            var tmpPath = Path.GetTempFileName();
            //Create a backup of the file, if it already exists
            if (File.Exists(fileName))
            {
                File.Move(fileName, tmpPath);
            }
            try
            {
                var fileContent = JsonConvert.SerializeObject(persistanceObject, Newtonsoft.Json.Formatting.Indented);
                using (var sw = new StreamWriter(fileName))
                {
                    sw.Write(fileContent);
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }
                if (File.Exists(tmpPath))
                {
                    File.Move(tmpPath, fileName);
                }
                //Rethrow the exception
                throw ex;
            }
        }

        /// <summary>
        /// Load a .net object from a json file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static T LoadJsonFile<T>(string fileName)
        {
            using (var fileStream = new FileStream(fileName,
                                                    FileMode.Open,
                                                    FileAccess.Read,
                                                    FileShare.ReadWrite))
            {
                using (var sr = new StreamReader(fileStream))
                {
                    var stringContent = sr.ReadToEnd();
                    return JsonConvert.DeserializeObject<T>(stringContent);
                }
            }
        }

        /// <summary>
        /// Handle an unknow xml node
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Serializer_UnknownNode(object sender, XmlNodeEventArgs e)
        {
            _log?.LogWarning("Unknown Node:" + e.Name + "\t" + e.Text);
        }

        /// <summary>
        /// Handle an unknown xml attribute
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void Serializer_UnknownAttribute(object sender, XmlAttributeEventArgs e)
        {
            XmlAttribute attr = e.Attr;
            _log?.LogWarning("Unknown attribute " + attr.Name + "='" + attr.Value + "'");
        }
    }
}
