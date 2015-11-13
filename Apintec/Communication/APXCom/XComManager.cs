using Apintec.Communiction.APXCom.Instances.Net;
using Apintec.Communiction.APXCom.Instances.Serial;
using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Xml.Linq;

namespace Apintec.Communiction.APXCom
{
    public class XComManager
    {
        private static APXDoc _xComCfg;

        public static APXDoc XComCfg
        {
            get { return _xComCfg; }
            protected set { _xComCfg = value; }
        }
        private const string IOCardCfgFile = "XCom.xml";

        private static List<XComInfo> _xComInfo;
        public static Dictionary<string,XComInstanceInfo> XComsDict { get; protected set; }
        static XComManager()
        {
            if (!File.Exists(IOCardCfgFile))
            {
                try
                {
                    XComCfg = new APXDoc();
                    XComCfg.Create(IOCardCfgFile);
                    XComCfg.Save();
                }
                catch (Exception e)
                {
                    throw new APXExeception(e.ToString());
                }
            }
            else
            {
                XComCfg = new APXDoc(IOCardCfgFile);
                _xComInfo = new List<XComInfo>();
                XComsDict = new Dictionary<string, XComInstanceInfo>();
            }
            Load();
            ReinInstance();
        }
        private static void ReinInstance()
        {
            string xcomSubClass="";
            string key = "";
            foreach (var item in _xComInfo)
            {
                try
                {
                    if (item.Type == "XSerialPort")
                    {
                        xcomSubClass = "Serial";
                        key = SerialKey.BuildSerialKey(item.Key, item.Parameter as XSerialParameter);
                    }
                    else if ((item.Type == "XUdpClient") || (item.Type == "XTcpClient")) 
                    {
                        xcomSubClass = "Net";
                        key = NetKey.BuildNetKey(item.Key, item.Parameter as NetParameter);
                    }
                    Type type = Type.GetType(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace
                        + "." + "Instances" + "." + xcomSubClass + "." + item.Type, true, true);
                    XComsDict.Add(key, new XComInstanceInfo(item, type));
                }
                catch (Exception e)
                {
                    APXlog.Write(APXlog.BuildLogMsg(e.ToString()));
                }
            }
        }

        private static bool Load()
        {
            if (XComCfg == null)
                return false;
            try
            {
                var queryVendor = XComCfg.Doc.Descendants(XComCfg.NameSpace + "XCom").Descendants().Select(n => new { n.Name, n });
                foreach (var item in queryVendor)
                {
                    XComInfo xComInfo = new XComInfo();
                    xComInfo.Type = item.Name.LocalName;
                    if (xComInfo.Type == "XSerialPort")
                    {
                        XSerialParameter para = new XSerialParameter();
                        for (XAttribute attr = item.n.FirstAttribute; attr != null; attr = attr.NextAttribute)
                        {
                            switch (attr.Name.LocalName)
                            {
                                case "PortName":
                                    para.PortName = attr.Value;
                                    break;
                                case "Baudrate":
                                    para.Baudrate = int.Parse(attr.Value);
                                    break;
                                case "Parity":
                                    para.Parity = (Parity)Enum.Parse(typeof(Parity), attr.Value);
                                    break;
                                case "DataBits":
                                    para.DataBits = int.Parse(attr.Value);
                                    break;
                                case "StopBits":
                                    para.StopBits = (StopBits)Enum.Parse(typeof(StopBits), attr.Value);
                                    break;
                                case "Handshake":
                                    para.Handshake = (Handshake)Enum.Parse(typeof(Handshake), attr.Value);
                                    break;
                                case "Key":
                                    xComInfo.Key = attr.Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                        xComInfo.Parameter = para;
                    }
                    else if((xComInfo.Type == "XUdpClient") || (xComInfo.Type == "XTcpClient"))
                    {
                        NetParameter para = new NetParameter();
                        for (XAttribute attr = item.n.FirstAttribute; attr != null; attr = attr.NextAttribute)
                        {
                            switch (attr.Name.LocalName)
                            {
                                case "ServerIP":
                                    para.ServerIp = attr.Value;
                                    break;
                                case "ServerPort":
                                    para.ServerPort = int.Parse(attr.Value);
                                    break;
                                case "Timeout":
                                    para.Timeout = int.Parse(attr.Value);
                                    break;
                                case "LocalEthIndex":
                                    para.LocalEthIndex = int.Parse(attr.Value);
                                    break;
                                case "LocalPort":
                                    para.LocalPort = int.Parse(attr.Value);
                                    break;
                                case "Key":
                                    xComInfo.Key = attr.Value;
                                    break;
                                default:
                                    break;
                            }
                        }
                        xComInfo.Parameter = para;
                    }
                    _xComInfo.Add(xComInfo);
                }
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
            return true;
        }
    }
}
