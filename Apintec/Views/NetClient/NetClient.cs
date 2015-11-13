using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using APINTEC.Net.Tcp;

namespace APINTEC.views
{
    public partial class NetClient : UserControl
    {
        private TcpClientX _client;
        public NetClient()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _client = new TcpClientX();
            _client._tcpRecvHandler += _client__tcpRecvHandler; ;
            NetPoint.LocalHostPoint[0].IpAddress.ToString();
            foreach (NetPoint localHostPoint in NetPoint.LocalHostPoint)
            {
                comboBoxLocalHost.Items.Add(localHostPoint.IpAddress.ToString());
            }
            comboBoxLocalHost.SelectedIndex = 0;
        }
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            bool result;
            if (textBoxIP.Text.Trim() != "" && textBoxPort.Text.Trim() != "")
            {
                result = _client.Connnect(comboBoxLocalHost.SelectedItem.ToString(), textBoxIP.Text, Convert.ToInt32(textBoxPort.Text));
                if (!result)
                {
                    richTextBoxMsg.AppendText(DateTime.Now.ToString() + " " + _client.LastErrorCode + "\r\n");
                }
                else
                {
                    buttonSend.Enabled = true;
                    buttonConnect.Enabled = false;
                    buttonDisConnect.Enabled = true;
                    buttonConnect.Hide();
                    buttonDisConnect.Show();
                }
            }
            else
            {
                MessageBox.Show("IP addresss or Net port is NULL.");
            }
        }

        public delegate void Mydelegate(string msg);
        private void _client__tcpRecvHandler(string pkg)
        {
            richTextBoxMsg.BeginInvoke(new Mydelegate(ShowMsg), pkg);
        }

        private void ShowMsg(string msg)
        {
            richTextBoxMsg.AppendText(DateTime.Now.ToString() + " " + msg + "\r\n");
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            _client.Send(textBoxSend.Text);
        }

        private void buttonDisConnect_Click(object sender, EventArgs e)
        {
            _client.Disconnect();
            buttonConnect.Enabled = true;
            buttonDisConnect.Enabled = false;
            buttonDisConnect.Hide();
            buttonConnect.Show();
            buttonSend.Enabled = false;
        }

        public void Release()
        {
            _client.Dispose();
        }
    }
}
