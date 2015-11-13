using Apintec.Communiction.APXCom;
using Apintec.Core.APCoreLib;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Apintec.Modules.Robots.Vendors
{
    public class YamahaRobot : Robot
    {
        private Thread _updateCoord;
        private string _cmdRead = "@?WHRXY\r\n";

        private Thread _receiveParse;
        private string _cmdWrite = "@WRITE";
        private byte[] buffer=new byte[0];

        private bool _stopUpdateCoord = false;



        public YamahaRobot()
        {
            
        }

        private void UpdateCoordFun()
        {
            byte[] cmd = new byte[16];
            Array.Copy(Encoding.ASCII.GetBytes(_cmdRead), cmd, _cmdRead.Length);
            while (true)
            {
                if (ComModule == null)
                {
                    return;
                }
                try
                {
                    if (!_stopUpdateCoord) 
                        ComModule.Send(cmd, 0, cmd.Length);
                }
                catch (Exception)
                {
                }
                Thread.Sleep(300);
            }
            
        }

        public override bool BindComModule(IXCom comModule)
        {
            bool isOK;
            isOK = base.BindComModule(comModule);
            if (isOK)
            {
                ComModule.OnConnect += ComModule_OnConnect;
                ComModule.OnDisconnect += ComModule_OnDisconnect;
                ComModule.OnReceive += ComModule_OnReceive;
                ComModule.OnSend += ComModule_OnSend;
                try
                {
                    isOK = comModule.Connect();
                }
                catch(Exception)
                {
                    return false;
                }              
                if (!isOK)
                    return false;
                if (_updateCoord == null)
                {
                    _updateCoord = new Thread(UpdateCoordFun);
                    _updateCoord.Start();
                }
                else
                {
                    _updateCoord.Abort();
                    _updateCoord = new Thread(UpdateCoordFun);
                    _updateCoord.Start();
                }
                if (_receiveParse == null)
                {
                    
                    _receiveParse = new Thread(ParseResult);
                    _receiveParse.Start();
                }
                else
                {
                    _receiveParse.Abort();
                    _receiveParse = new Thread(ParseResult);
                    _receiveParse.Start();
                }
                return true;
            }
                
            else
            {
                return false;
            }
        }

        
        private void ComModule_OnSend(object sender, EventArgs e)
        {
            
        }

        private void ComModule_OnReceive(object sender, EventArgs e)
        {
           
            int rByte = 0;
            XComEventArgs args = e as XComEventArgs;
            if(args!=null)
            {
                rByte = args.ByteToRead;
            }
           
            byte[] readBuff = new byte[rByte];
            Array.Clear(readBuff, 0, readBuff.Length);
            if(ComModule!=null)
            {
                try
                {
                    ComModule.Receive(ref readBuff, 0, rByte);
                    lock(buffer)
                    {
                        buffer = Gadget.ArrayAppend<byte>(buffer, readBuff);
                       // buffer.Concat(readBuff);
                    }
                    
                }
                catch(Exception)
                {

                }
            }
        }

        private void ParseResult()
        {
            List<string> Position;
            while (true)
            {
                lock (buffer)
                {
                    string sub2 = ASCIIEncoding.Default.GetString(buffer).Trim();
                    Position = ParsePosition(buffer, out buffer);
                    if (Position != null)
                    {
                        try
                        {
                            SetRobotPosition(Position);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
                Thread.Sleep(50);
            }

        }

        private void SetRobotPosition(List<string> position)
        {
            if (position == null)
                return;
            Coordinate robotPosition = new Coordinate();
            try
            {
                robotPosition.X = double.Parse(position[1]);
                robotPosition.Y = double.Parse(position[2]);
                robotPosition.Z = double.Parse(position[3]);
                robotPosition.R = double.Parse(position[4]);
            }
            catch(Exception e)
            {
                throw new APXExeception(e.Message);
            }
            Position = robotPosition;

        }

        private List<string> ParsePosition(byte[] source, out byte[] newDst)
        {
            int position = 0;
            List<int> indexs = new List<int>();
           
            foreach (byte template in source)
            {
                if (template == ']')
                {
                    indexs.Add(position);
                }
                position++;
            }
            if (indexs.Count < 2)
            {
                newDst = source;
                return null;
            }
            byte[] dst = new byte[indexs[indexs.Count - 1] - indexs[indexs.Count - 2] +1];
            for (int i = 0; i < indexs[indexs.Count - 1] - indexs[indexs.Count - 2]; i++)
            {
                dst[i] = source[i + indexs[indexs.Count - 2]-4];
            }
            string sub = ASCIIEncoding.Default.GetString(dst).Trim();
            string[] strTemp = { "" };
            strTemp = sub.Split(new char[] { ' ', '|' });
            List<string> trimed = new List<string>(strTemp);
            trimed.RemoveAll(new Predicate<string>(compare));
            if (trimed[0] == "[POS]")
            {
                //               source = source.Remove(indexs[0], indexs[1]-1);
                //string sub1 = ASCIIEncoding.Default.GetString(source).Trim();
                byte[] rest = new byte[source.Length - indexs[indexs.Count - 1]+4];
                for (int i = 0; i < source.Length - indexs[indexs.Count - 1]+4; i++)
                {
                    rest[i] = source[i + indexs[indexs.Count - 1] -4];
                }
                newDst = rest;
                
                return trimed;
            }
            newDst = source;
            return null;
        }

        private bool compare(string obj)
        {
            if (obj == "")
                return true;
            else
                return false;
        }

        private string BuildWriteCmd(string port, Coordinate coord)
        {
            string cmd = "";
            cmd = _cmdWrite + " " + port + "\r\n"
                + port + "=" + coord.X.ToString() + " "
                + coord.Y.ToString() + " "
                + coord.Z.ToString() + " "
                + coord.R.ToString() + " "
                + "0 0" + "\r\n";
            return cmd;
        }

        public bool WritePositionOffset(Coordinate offset)
        {
            if (ComModule == null)
                return false;
            try
            {
                string writeCmd = BuildWriteCmd("P8", offset);
                byte[] cmd = new byte[1024];
                Array.Copy(Encoding.ASCII.GetBytes(writeCmd), cmd, writeCmd.Length);
                _stopUpdateCoord = true;
                buffer = new byte[0];
                ComModule.Send(cmd,0, writeCmd.Length);
                Thread.Sleep(300);
                string result = ASCIIEncoding.Default.GetString(buffer).Trim();
                if (result.Contains("OK"))
                    return true;
                else
                    return false;
            }
            catch(Exception)
            {
                return false;
            }
            finally
            {
                _stopUpdateCoord = false;
            }
        }

        public bool UnBindComModule(IXCom comModule)
        {
            bool isOK;
            if (comModule == null)
                return true;
            ComModule.OnConnect -= ComModule_OnConnect;
            ComModule.OnDisconnect -= ComModule_OnDisconnect;
            ComModule.OnReceive -= ComModule_OnReceive;
            ComModule.OnSend -= ComModule_OnSend;
            try
            {
                isOK = comModule.Disconnect();
            }
            catch (Exception)
            {
                return false;
            }
            if (!isOK)
                return false;
            if (_updateCoord == null)
            {
                _updateCoord.Abort();
            }
          
            if (_receiveParse == null)
            {
                _receiveParse.Abort();
            }
            return true;
        }

        private void ComModule_OnDisconnect(object sender, EventArgs e)
        {
           
        }

        private void ComModule_OnConnect(object sender, EventArgs e)
        {
            
        }

        public override bool Close()
        {
            try
            {
                if (_updateCoord != null)
                {
                    _updateCoord.Abort();
                }

                if (_receiveParse != null)
                {
                    _receiveParse.Abort();
                }
                return true;
            }
            catch(Exception)
            {
                return false;
            }
            
        }

        public override bool Open()
        {
            throw new NotImplementedException();
        }

        public override bool Start()
        {
            throw new NotImplementedException();
        }

        public override bool Stop()
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            
        }
    }
}
