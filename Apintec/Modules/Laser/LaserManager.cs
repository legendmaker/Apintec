using Apintec.Core.APCoreLib;
using Apintec.Modules.Laser.Protocol;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Apintec.Modules.Laser
{
    public class LaserManager : IDisposable
    {
        private static APXDoc LaserCfg { get; set; }
        private const string LaserCfgFile = "Lasers.xml";
        private static List<LaserInfo> _laserInfo { get; set; }
        public static Dictionary<int, LaserInstanceInfo> LaserDict { get; internal set; }
        static LaserManager()
        {
            if (!File.Exists(LaserCfgFile))
            {
                try
                {
                    LaserCfg = new APXDoc();
                    LaserCfg.Create(LaserCfgFile);
                    LaserCfg.Save();
                }
                catch (Exception e)
                {
                    throw new APXExeception(e.ToString());
                }
            }
            else
            {
                LaserCfg = new APXDoc(LaserCfgFile);
            }
            _laserInfo = new List<LaserInfo>();
            LaserDict = new Dictionary<int, LaserInstanceInfo>();
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
            foreach (var item in _laserInfo)
            {
                try
                {
                    Type type = Type.GetType(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace
                        + "." + "Vendors" + "." + item.Vendor, true, true);
                    LaserDict.Add(item.Sequence, new LaserInstanceInfo(item, type));
                }
                catch (Exception e)
                {
                    APXlog.Write(APXlog.BuildLogMsg(e.ToString()));
                }
            }
        }

        private static void Load()
        {
            string vendor = "";
            int sequence = 0;
            string ipAddr = "";
            int port = 0;
            LaserProtocolType protocolType = LaserProtocolType.None;
            string comKey = "";
            try
            {
                var queryVendor = LaserCfg.Doc.Descendants(LaserCfg.NameSpace + "Laser").Descendants().Select(n => new { n.Name, n });
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
                            case "Port":
                                port = int.Parse(attr.Value);
                                break;
                            case "ProtocolType":
                                protocolType = (LaserProtocolType)Enum.Parse(typeof(LaserProtocolType), attr.Value);
                                break;
                            case "ComKey":
                                comKey = attr.Value;
                                break;
                            default:
                                break;
                        }
                    }
                    _laserInfo.Add(new LaserInfo(null, vendor, null, null, sequence, ipAddr, port, protocolType, comKey));
                }
            }
            catch (Exception e)
            {
                throw new APXExeception(e.ToString());
            }
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
        // ~LaserManager() {
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
