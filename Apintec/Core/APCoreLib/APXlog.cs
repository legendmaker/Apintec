using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apintec.Core.APCoreLib
{
    public class APXlog
    {
        private static object _safeLock = new object();
        public static void Write(string msg)
        {
            lock (_safeLock)
            {
                try
                {
                    UnicodeEncoding uniencoding = new UnicodeEncoding();
                    string filename = @"log.txt";

                    byte[] result = Encoding.UTF8.GetBytes(DateTime.Now.ToString() + " " + msg + "\r\n");

                    using (FileStream SourceStream = File.Open(filename, FileMode.OpenOrCreate))
                    {
                        SourceStream.Seek(0, SeekOrigin.End);
                        SourceStream.Write(result, 0, result.Length);
                        SourceStream.Close();
                    }
                }
                catch (Exception)
                {

                }
            }
        }

        public static int GetLineNum()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(2).GetFileLineNumber();
        }

        public static string GetCurSourceFileName()
        {
            System.Diagnostics.StackTrace st = new System.Diagnostics.StackTrace(1, true);
            return st.GetFrame(2).GetFileName();
        }

        public static string BuildLogMsg(string msg)
        {
            return String.Format(GetCurSourceFileName() + ":" +
                GetLineNum().ToString() + "  --->  " + msg);
        }
    }
}
