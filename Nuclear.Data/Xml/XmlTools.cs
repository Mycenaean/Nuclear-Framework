// Copyright © Nikola Milinkovic 
// Licensed under the MIT License (MIT).
// See License.md in the repository root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Serialization;

namespace Nuclear.Data.Xml
{
    /// <summary>
    /// Helper class for InputStream Deserialization from incoming HttpRequest
    /// </summary>
    public static class XmlTools
    {
        /// <summary>
        /// Deserializes Complex Type Entites
        /// </summary>
        /// <param name="input">Http request body string</param>
        /// <param name="asm">Wanted types assembly</param>
        /// <param name="paramType">Wanted type name</param>
        /// <returns>Instantiated object</returns>
        public static object DeserializeComplex(string input, string asm, string paramType)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName == asm);
            Type param = assembly.GetType(paramType);
            object entity = Activator.CreateInstance(param);
            string[] splitted = paramType.Split('.');
            string root = splitted[splitted.Length - 1];
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(input);
            XmlNodeList entities = xml.GetElementsByTagName(root);
            XmlDocument newXml = new XmlDocument();
            foreach (XmlNode xentity in entities)
            {
                XmlNode import = newXml.ImportNode(xentity, true);
                newXml.AppendChild(import);
            }
            string inputObjectString = newXml.OuterXml;

            XmlSerializer serializer = new XmlSerializer(entity.GetType());
            using (StringReader reader = new StringReader(inputObjectString))
                return serializer.Deserialize(reader);
        }

        /// <summary>
        /// Deserialize single string input parameter
        /// </summary>
        /// <param name="document">InputStream in form of XML</param>
        /// <param name="name">Name of the string parameter</param>
        /// <returns>Parameter value</returns>
        public static string DeserializeString(XmlDocument document, string name)
        {
            XmlNode root = document.FirstChild;
            foreach (XmlNode node in root.ChildNodes)
            {
                if (node.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return node.InnerText;
            }

            return null;
        }

        /// <summary>
        /// Deserialize single int input parameter
        /// </summary>
        /// <param name="document">InputStream in form of XML</param>
        /// <param name="name">Name of the int parameter</param>
        /// <returns>Parameter value</returns>
        public static int DeserializeInt(XmlDocument document, string name)
        {
            return int.Parse(DeserializeString(document, name));
        }

        /// <summary>
        /// Deserialize single bool input parameter
        /// </summary>
        /// <param name="document">InputStream in form of XML</param>
        /// <param name="name">Name of the bool parameter</param>
        /// <returns>Parameter value</returns>
        public static bool DeserializeBool(XmlDocument document, string name)
        {
            return Convert.ToBoolean(DeserializeString(document, name));
        }

        /// <summary>
        /// Deserialize single double input parameter
        /// </summary>
        /// <param name="document">InputStream in form of XML</param>
        /// <param name="name">Name of the double parameter</param>
        /// <returns>Parameter value</returns>
        public static double DeserializeDouble(XmlDocument document, string name)
        {
            return double.Parse(DeserializeString(document, name));
        }

        /// <summary>
        /// Deserialize single float input parameter
        /// </summary>
        /// <param name="document">InputStream in form of XML</param>
        /// <param name="name">Name of the float parameter</param>
        /// <returns>Parameter value</returns>
        public static float DeserializeFloat(XmlDocument document, string name)
        {
            return float.Parse(DeserializeString(document, name));
        }

        /// <summary>
        /// Deserialize single decimal input parameter
        /// </summary>
        /// <param name="document">InputStream in form of XML</param>
        /// <param name="name">Name of the decimal parameter</param>
        /// <returns>Parameter value</returns>
        public static decimal DeserializeDecimal(XmlDocument document, string name)
        {
            return decimal.Parse(DeserializeString(document, name));
        }


        #region OBSOLETE
        /// <summary>
        /// Deserialize custom entity
        /// </summary>
        /// <typeparam name="T">Type of entity to be deserialized</typeparam>
        /// <param name="doc">InputStream in form of XML</param>
        /// <param name="Entity">Instance of T entity</param>
        /// <returns>Instantiated object</returns>
        [Obsolete("Unused please for complex types use DeserializeComplex")]
        public static object DeserializeEntity(XmlDocument doc, string asm, string paramType, Dictionary<string, string> original, string named = null)
        {
            Assembly assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName == asm);
            Type param = assembly.GetType(paramType);
            object Entity = Activator.CreateInstance(param);

            PropertyInfo[] properties = param.GetProperties();

            foreach (var prop in properties)
            {
                if (doc.GetElementsByTagName(prop.Name.ToLower())[0] == null)
                {
                    //throw new XmlDeserializationException($"{prop.Name} element not found in the request");
                    continue;
                }

                string originalText = string.Empty;
                //Check if property is of type System.Colletions.Generics
                Type colInterface = prop.PropertyType.GetInterface("IEnumerable");

                // Check that property is not of string type since strings also implement IEnumerable
                if (prop.PropertyType != typeof(string) && colInterface != null)
                {
                    Type[] colMembers = prop.PropertyType.GenericTypeArguments;
                    foreach (var colMember in colMembers)
                    {
                        string genericClTypeXmlName = string.Empty; // needed for recursion
                        IList<object> genericList = new List<object>();

                        // Go through all keys/texts
                        for (int i = -1; i < original.Count(); i++)
                        {
                            try
                            {
                                genericClTypeXmlName = prop.Name.Remove(prop.Name.Length - 1);
                                genericList.Add(DeserializeForCollections(doc, colMember.Assembly.FullName, colMember.FullName, original, genericClTypeXmlName, i));
                            }
                            catch (Exception ex)
                            {
                                goto AssingCollection;
                            }
                        }

                    AssingCollection: // break out of loop if exception occures
                        var collection = Activator.CreateInstance(prop.PropertyType);
                        collection = genericList;
                        prop.SetValue(Entity, collection);

                    }
                }

                //Manipulations if property is of custom type
                string[] inspected = paramType.Split('.'); //paramType splitted array
                int index = inspected.Count(); // find last index
                string inspectedName = inspected[index - 1]; // class name is last index
                if (!String.IsNullOrEmpty(named))
                    originalText = original[$"{named} {prop.Name.ToLower()}"];
                else
                {
                    try
                    {
                        originalText = original[$"{inspectedName} {prop.Name.ToLower()}"]; //concatinate to match input request format
                    }
                    catch (Exception ex)
                    {
                        originalText = null;
                    }
                }

                XmlNode element = doc.GetElementsByTagName(prop.Name.ToLower())[0];
                if (prop.PropertyType == typeof(int))
                    prop.SetValue(Entity, int.Parse(originalText));
                else if (prop.PropertyType == typeof(string))
                    prop.SetValue(Entity, originalText);
                else if (prop.PropertyType == typeof(double))
                    prop.SetValue(Entity, double.Parse(originalText));
                else if (prop.PropertyType == typeof(float))
                    prop.SetValue(Entity, float.Parse(originalText));
                else if (prop.PropertyType == typeof(decimal))
                    prop.SetValue(Entity, decimal.Parse(originalText));
                else if (prop.PropertyType == typeof(bool))
                    prop.SetValue(Entity, bool.Parse(originalText));
                else if (prop.PropertyType == typeof(char))
                    prop.SetValue(Entity, char.Parse(originalText));
                else
                {
                    object dependEntity = DeserializeEntity(doc, prop.PropertyType.Assembly.FullName, prop.PropertyType.FullName, original, prop.Name);

                    prop.SetValue(Entity, dependEntity);
                }

            }

            return Entity;
        }

        /// <summary>
        /// Deserializes entity from System.Collections.Generics
        /// </summary>
        /// <param name="doc">XML Request</param>
        /// <param name="asm">Assembly name</param>
        /// <param name="paramType">Type name</param>
        /// <param name="original">Dictionary of original keys/texts</param>
        /// <param name="genericClTypeXmlName">XmlName for the property</param>
        /// <param name="i">count</param>
        /// <returns>Instantiated object</returns>
        [Obsolete("Unused please for complex types use DeserializeComplex")]
        public static object DeserializeForCollections(XmlDocument doc, string asm, string paramType, Dictionary<string, string> original, string genericClTypeXmlName, int i)
        {
            if (i == -1)
                return DeserializeEntity(doc, asm, paramType, original, genericClTypeXmlName);
            else
                return DeserializeEntity(doc, asm, paramType, original, $"{genericClTypeXmlName} {i}");
        }

        /// <summary>
        /// Method that will save all original texts and remove capitalizations from the input parameters
        /// </summary>
        /// <param name="inputBodyString">XML Body request string</param>
        /// <returns>XML without capitalization , XmlNodes and their original Text format</returns>
        [Obsolete("Unused please for complex types use DeserializeComplex")]
        public static KeyValuePair<XmlDocument, Dictionary<string, string>> ProccessRequestParameters(string inputBodyString)
        {
            XmlDocument requestXml = new XmlDocument();
            requestXml.LoadXml(inputBodyString);
            XmlNode requestNode = requestXml.FirstChild;
            XmlNodeList childRequestNodes = requestNode.ChildNodes;
            Dictionary<string, string> originalTexts = new Dictionary<string, string>();
            foreach (XmlNode child in childRequestNodes)
            {
                if (child.HasChildNodes)
                    LoadChilds(originalTexts, child);
                else
                    originalTexts.Add(child.ParentNode.Name + child.Name.ToLower(), child.InnerText);
            }

            requestXml.RemoveAll();
            requestXml.LoadXml(inputBodyString.ToLower());

            return new KeyValuePair<XmlDocument, Dictionary<string, string>>(requestXml, originalTexts);

        }

        /// <summary>
        /// Deserialize nested entities inside the request entity
        /// </summary>
        /// <param name="original">Returning dictionary</param>
        /// <param name="node">Insepcted Node</param>
        [Obsolete("Unused please for complex types use DeserializeComplex")]
        public static void LoadChilds(Dictionary<string, string> original, XmlNode node)
        {
            XmlNodeList childRequestNodes = node.ChildNodes;
            foreach (XmlNode child in childRequestNodes)
            {
                int i = 0;
                if (child.FirstChild != child.LastChild)
                    LoadChilds(original, child);
                else
                {
                    try
                    {
                        original.Add($"{child.ParentNode.Name} {child.Name.ToLower()}", child.InnerText);
                    }
                    catch (Exception ex)
                    {
                        original.Add($"{child.ParentNode.Name} {i} {child.Name.ToLower()}", child.InnerText);
                        i++;
                    }

                }
            }
        }
        #endregion
    }
}
