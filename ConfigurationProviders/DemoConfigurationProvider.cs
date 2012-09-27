using System;
using System.Xml;
using System.IO;
using System.Text;
using System.Configuration;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Web.Hosting;



namespace Renascence
{
    /// <summary>
    /// RENASCENCE.NET
    /// (c) 2009 Dmitrii Voitsik
    /// </summary>

    
    
    public class DemoConfigurationProvider : ProtectedConfigurationProvider
    {



        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="config"></param>
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection config)
        {
            base.Initialize(name, config);
        }

		

        /// <summary>
        /// 
        /// </summary>
        /// <param name="encryptedNode"></param>
        /// <returns></returns>
        public override XmlNode Decrypt(XmlNode encryptedNode)
        {
            string
                applicationName = HostingEnvironment.ApplicationVirtualPath.Substring(1);

            XmlNode
                defaultSettings = encryptedNode.SelectSingleNode("defaultSettings");
            
            if (applicationName != String.Empty)
            {
                XmlNodeList
                    settings = encryptedNode.SelectNodes(applicationName + "/*");

                if (settings != null)
                    foreach (XmlNode node in settings)
                    {
                        // connectionStrings
                        if (node.Name == "add")
                            if (node.Attributes["name"] != null)
                            {
                                string
                                    name = node.Attributes["name"].Value;

                                string
                                    connectionString = node.Attributes["connectionString"].Value;

                                string
                                    providerName = "";

                                if (node.Attributes["providerName"] != null)
                                    providerName = node.Attributes["providerName"].Value;

                                if (defaultSettings != null)
                                {
                                    XmlNode
                                        defaultNode = defaultSettings.SelectSingleNode("add[@name = \"" + name + "\"]");

                                    if (defaultNode != null)
                                        defaultNode.Attributes["connectionString"].Value = connectionString;

                                    if (defaultNode != null)
                                        if (defaultNode.Attributes["providerName"] != null)
                                            defaultNode.Attributes["providerName"].Value = providerName;
                                }
                            }

                        // appSettings
                        if (node.Name == "add")
                            if (node.Attributes["key"] != null)
                            {
                                string
                                    key = node.Attributes["key"].Value;

                                string
                                    value = node.Attributes["value"].Value;

                                if (defaultSettings != null)
                                {
                                    XmlNode
                                        defaultNode = defaultSettings.SelectSingleNode("add[@key = \"" + key + "\"]");

                                    if (defaultNode != null)
                                        defaultNode.Attributes["value"].Value = value;
                                }
                            }
                    }
            }
            
            if (defaultSettings != null)
                return
                    defaultSettings;
            else
                return
                    encryptedNode;
        }

		
		
        /// <summary>
        /// 
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
		public override XmlNode Encrypt(XmlNode node)
        {
            return
                null;
        }



    }
}
