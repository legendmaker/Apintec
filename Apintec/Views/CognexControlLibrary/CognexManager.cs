using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Apintec.views.CognexControlLibrary
{
    internal class CognexManager
    {
        private static APXDoc CognexClientCfg { get; set; }
        private const string CognexClientCfgFile = "CognexControlLibrary.xml";
        public static Dictionary<int, CognexClientInfo> CognexCilentDict { get; private set; }

        static CognexManager()
        {
            if (!File.Exists(CognexClientCfgFile))
            {
                try
                {
                    CognexClientCfg = new APXDoc();
                    CognexClientCfg.Create(CognexClientCfgFile);
                    CognexClientCfg.Save();
                }
                catch (Exception e)
                {
                    throw new APXExeception(e.ToString());
                }
            }
            else
            {
                CognexClientCfg = new APXDoc(CognexClientCfgFile);
            }
            CognexCilentDict = new Dictionary<int, CognexClientInfo>();
            Load();
        }
        public CognexManager()
        {
        }

        private static void Load()
        {
            int sequence = 0;
            string ipAddr = "";
            int port = 0;
            string userName = "";
            string passWord = "";
            string protocolType = "";
            string comKey = "";
            try
            {
                var queryVendor = CognexClientCfg.Doc.Descendants(CognexClientCfg.NameSpace + "InSight").Descendants().Select(n => new { n.Name, n });
                foreach (var item in queryVendor)
                {
                    for (XAttribute attr = item.n.FirstAttribute; attr != null; attr = attr.NextAttribute)
                    {
                        switch (attr.Name.LocalName)
                        {
                            case "Sequence":
                                sequence = int.Parse(attr.Value);
                                break;
                            case "IPAddress":
                                ipAddr = attr.Value;
                                break;
                            case "Port":
                                port = int.Parse(attr.Value);
                                break;
                            case "UserName":
                                userName = attr.Value;
                                break;
                            case "PassWord":
                                passWord = attr.Value;
                                break;
                            case "ProtocolType":
                                protocolType = attr.Value;
                                break;
                            case "ComKey":
                                comKey = attr.Value;
                                break;
                            default:
                                break;
                        }
                    }
                    CognexCilentDict.Add(sequence, new CognexClientInfo(sequence, ipAddr, 
                        port, userName, passWord, protocolType, comKey));
                }
            }
            catch (Exception e)
            {
                throw new APXExeception(e.ToString());
            }
        }
    }
}
