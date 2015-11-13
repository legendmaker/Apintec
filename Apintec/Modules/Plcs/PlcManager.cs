using Apintec.Core.APCoreLib;
using Apintec.Modules.Plcs.Protocols;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace Apintec.Modules.Plcs
{
    public class PlcManager : IDisposable 
    {
        private static APXDoc PlcsCfg { get; set; }
        private const string PlcsCfgFile = "Plcs.xml";
        private static List<PlcInfo> _plcInfo { get; set; }
        public static Dictionary<int, PlcInstanceInfo> PlcDict { get; internal set; }
        static PlcManager()
        {
            if (!File.Exists(PlcsCfgFile))
            {
                try
                {
                    PlcsCfg = new APXDoc();
                    PlcsCfg.Create(PlcsCfgFile);
                    PlcsCfg.Save();
                }
                catch (Exception e)
                {
                    throw new APXExeception(e.ToString());
                }
            }
            else
            {
                PlcsCfg = new APXDoc(PlcsCfgFile);
            }
            _plcInfo = new List<PlcInfo>();
            PlcDict = new Dictionary<int, PlcInstanceInfo>();
            try
            {
                Load();
                ReinInstance();
            }
            catch (Exception e)
            {
                APXlog.Write(APXlog.BuildLogMsg(e.Message));
            }
        }

        private static void ReinInstance()
        {
            foreach (var item in _plcInfo)
            {
                try
                {
                    Type type = Type.GetType(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace
                        + "." + "Vendors" + "." + item.Vendor, true, true);
                    PlcDict.Add(item.Sequence, new PlcInstanceInfo(item, type));
                }
                catch (Exception e)
                {
                    APXlog.Write(APXlog.BuildLogMsg(e.ToString()));
                }
            }
        }

        private static bool Load()
        {
            string vendor = "";
            string ipAddr = "";
            int sequence = 0;
            byte network = 0;
            byte unit = 0;
            byte node = 0;
            int timeout = 0;
            int port = 0;
            string comKey = "";
            PlcProtocolType pt=PlcProtocolType.None;
            int localHostEth = 0;
            if (PlcsCfg == null)
                return false;
            try
            {
                var queryVendor = PlcsCfg.Doc.Descendants(PlcsCfg.NameSpace + "Plc").Descendants().Select(n => new { n.Name, n});
                foreach (var item in queryVendor)
                {
                    vendor = item.Name.LocalName;
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
                            case "Network":
                                network = byte.Parse(attr.Value);
                                break;
                            case "Port":
                                port = int.Parse(attr.Value);
                                break;
                            case "Unit":
                                unit = byte.Parse(attr.Value);
                                break;
                            case "Node":
                                node = byte.Parse(attr.Value);
                                break;
                            case "Protocol":
                                pt = (PlcProtocolType)Enum.Parse(typeof(PlcProtocolType), attr.Value);
                                break;
                            case "Timeout":
                                timeout = int.Parse(attr.Value);
                                break;
                            case "LocalHostEthIndex":
                                localHostEth = int.Parse(attr.Value);
                                break;
                            case "ComKey":
                                comKey = attr.Value;
                                break;
                            default:
                                break;
                        }
                    }
                    _plcInfo.Add(new PlcInfo(null, vendor, null, null, sequence, ipAddr, port,
                        network, unit, node, pt, timeout, localHostEth, comKey));
                }
            }
            catch (Exception e)
            {
                throw new APXExeception(e.ToString());
            }
            return true;
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }
                
                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                disposedValue = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~PlcManager() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
