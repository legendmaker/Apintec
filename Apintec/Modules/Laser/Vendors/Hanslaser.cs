using Apintec.Core.APCoreLib;
using Apintec.Modules.Laser.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Apintec.Modules.Laser.Vendors
{
    public class Hanslaser : Laser
    {
        public Hanslaser(LaserInfo info):base(info)
        {
        }
        public override event EventHandler OnClose;
        public override event EventHandler OnOpen;
        public override event EventHandler OnStart;
        public override event EventHandler OnStop;

        public override bool Close()
        {
            return Stop();
        }

        public override bool Open()
        {
            return Start();
        }

        public override bool Start()
        {
            return base.Start();
        }

        public override bool Stop()
        {
            return base.Stop();
        }
        public override bool Run(LaserCommand cmd, params object[] para)
        {
            bool bRet = false;
            switch (cmd)
            {
                case LaserCommand.Config:
                    string fileName = "";
                    try
                    {
                        fileName = para[0].ToString();

                    }
                    catch (Exception e)
                    {
                        throw new APXExeception(e.ToString());
                    }
                    bRet = ChangeConfigByFileNameFunc(fileName);
                    break;
                case LaserCommand.Burn:
                    try
                    {
                        byte[] content =(byte[])para[0];
                        bRet = BurnFunc(content);
                    }
                    catch(Exception e)
                    {
                        throw new APXExeception(e.Message);
                    }
                    break;
                default:
                    break;
            }
            return bRet;
        }

        private bool BurnFunc(byte[] content)
        {
            bool isOK = false;
            try
            {
                byte[] frame = new byte[0];
                isOK = Protocol.SendMessage(LaserProtocolMessageType.Data, LaserProtocolMessageAck.Initial, content, ref frame);
                if(isOK)
                {
                    Thread.Sleep(200);
                    byte[] cmd = new byte[4];
                    cmd[4] = Convert.ToByte(Command.Burn);
                    isOK = Protocol.SendMessage(LaserProtocolMessageType.Command, LaserProtocolMessageAck.Initial, cmd, ref frame);
                    if (isOK)
                    {
                        LaserProtocolMessageAck ack = Protocol.CheckResult(frame, 3000);
                        if (ack != LaserProtocolMessageAck.CommandSucceed)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
        }

        private bool ChangeConfigByFileNameFunc(string fileName)
        {
            bool isOK = false;
            if (fileName == null)
            {    
                return false;
            }
            else
            {
                try
                {
                    byte[] cmd = new byte[4];
                    cmd[3] = Convert.ToByte(Command.Config);
                    byte[] file = Encoding.ASCII.GetBytes(fileName);
                    byte[] content = Gadget.ArrayAppend(cmd, file);
                    byte[] message = new byte[0];
                    isOK = Protocol.SendMessage(LaserProtocolMessageType.Command, LaserProtocolMessageAck.Initial, content, ref message);
                    if (isOK)
                    {
                        LaserProtocolMessageAck ack = Protocol.CheckResult(message, 200);
                        if (ack != LaserProtocolMessageAck.CommandSucceed)
                        {
                            return false;
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                catch(Exception e)
                {
                    throw new APXExeception(e.Message);
                }
            }
        }

      

        enum Command
        {
            None = 0xFF,
            Burn = 0x01,
            Config=0x02
        }
    }
}
