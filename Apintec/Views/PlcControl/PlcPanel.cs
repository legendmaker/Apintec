using System;
using System.Collections.Generic;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Apintec.Modules.Plcs;
using Apintec.Modules.Plcs.Vendors;
using System.Reflection;
using Apintec.Core.APCoreLib;
using Apintec.Modules.Plcs.Protocols.Fins;

namespace Apintec.views.PlcBox
{
    public partial class PlcPanel : UserControl
    {
        private Plc _plc = null;
        public PlcPanel()
        {
            InitializeComponent();
        }

        private void InitControlsValues()
        {
            foreach (var item in PlcManager.PlcDict)
            {
                comboBoxPlcList.Items.Add(item.Value.Info.IPAddress);
            }
            comboBoxPlcList.SelectedIndex = 0;
            radioButtonRead.Checked = true;
        }

        private void InitDataFieldShow()
        {
            if (comboBoxPlcList.SelectedIndex < 0)
                return;
            PlcInstanceInfo plcInstanceInfo = PlcManager.PlcDict[comboBoxPlcList.SelectedIndex] as PlcInstanceInfo;
            if (plcInstanceInfo != null)
            {
                try
                {
                    if (_plc != null)
                        _plc.Stop();
                    _plc = plcInstanceInfo.Instance();
                    if (_plc != null)
                        _plc.Start();
                    if (_plc is Omron)
                    {
                        if((_plc as Omron).IsFinsProtocol)
                        {
                            comboBoxField.DataSource = Enum.GetNames(typeof(IOMemoryArea));
                        }
                        comboBoxField.SelectedIndex = 3;
                    }
                }
                catch(Exception e)
                {
                    richTextBoxInAndOut.BeginInvoke(new Action(() =>
                    {
                        richTextBoxInAndOut.Text = e.Message;
                    }));
                }
                
            }
        }

        public void Init()
        {
            InitControlsValues();
        }

        private void radioButtonWrite_CheckedChanged(object sender, EventArgs e)
        {
            if(radioButtonWrite.Checked)
            {
                richTextBoxInAndOut.ReadOnly = false;
                richTextBoxInAndOut.BackColor = Color.White;
                richTextBoxInAndOut.Text = "";
            }
            else
            {
                richTextBoxInAndOut.ReadOnly = true;
                richTextBoxInAndOut.BackColor = Control.DefaultBackColor;
                richTextBoxInAndOut.Text = "";
            }
        }

        public void CloseAll()
        {
            if(_plc!=null)
            {
                try
                {
                    _plc.Stop();
                }
                catch(Exception e)
                {
                    throw new APXExeception(e.Message);
                }
            }
        }

        private void comboBoxPlcList_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitDataFieldShow();
        }

        private void comboBoxField_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitDataTypeShow();
        }

        private void InitDataTypeShow()
        {
            if (_plc is Omron)
            {
                if ((_plc as Omron).IsFinsProtocol)
                {
                    comboBoxDataType.Items.Clear();
                    foreach (var item in IOMemoryAddress.MemoryAreaCodeDict[(IOMemoryArea)Enum.Parse
                        (typeof(IOMemoryArea), comboBoxField.SelectedItem.ToString())])
                    {
                        comboBoxDataType.Items.Add(item.Key.ToString());
                    }
                    comboBoxDataType.SelectedIndex = 0;
                }

            }
        }

        private void buttonOperate_Click(object sender, EventArgs e)
        {
            int length = 0;

            try
            {
                length = int.Parse(textBoxLength.Text);
                ushort addrWord = ushort.Parse(textBoxAddressWord.Text);
                byte addrBit = byte.Parse(textBoxAddressBit.Text);
                if (radioButtonRead.Checked)
                {
                    byte[] buff = new byte[0];
                    if ((_plc as Omron).IsFinsProtocol)
                    {
                        bool isOK = _plc.Read(ref buff, 0, length, new IOMemoryAddress((IOMemoryArea)Enum.Parse
                         (typeof(IOMemoryArea), comboBoxField.SelectedItem.ToString()),
                         (IOMemeoryDataType)Enum.Parse(typeof(IOMemeoryDataType), comboBoxDataType.SelectedItem.ToString()),
                         Omron.Mode.CJ, addrWord, addrBit));
                        if(isOK)
                        {
                            richTextBoxInAndOut.Text = "";
                            foreach (var item in buff)
                            {
                                richTextBoxInAndOut.BeginInvoke(new Action(() =>
                                {
                                    richTextBoxInAndOut.AppendText(item.ToString("X2")+" ");
                                }));
                            }
                        }
                        else
                        {
                            richTextBoxInAndOut.BeginInvoke(new Action(() =>
                            {
                                richTextBoxInAndOut.Text = "Plc Read Fail.";
                            }));
                        }
                    }
                }
                if(radioButtonWrite.Checked)
                {
                    string[] data = richTextBoxInAndOut.Text.Trim(' ').Split(' ');
                    List<string> dataList = new List<string>(data);
                    dataList.RemoveAll(new Predicate<string>(n => n == ""));
                    byte[] buff = dataList.ToArray().Select(n => Convert.ToByte(n, 16)).ToArray();
                    length = int.Parse(textBoxLength.Text);
                    if ((_plc as Omron).IsFinsProtocol)
                    {
                        IOMemoryArea memArea = (IOMemoryArea)Enum.Parse(typeof(IOMemoryArea), comboBoxField.SelectedItem.ToString());
                        IOMemeoryDataType dataType = (IOMemeoryDataType)Enum.Parse(typeof(IOMemeoryDataType), comboBoxDataType.SelectedItem.ToString());
                        bool isOK = _plc.Write(buff, 0, length, new IOMemoryAddress(memArea, dataType, Omron.Mode.CJ, addrWord, addrBit));
                        if (isOK)
                        {
                            foreach (var item in buff)
                            {
                                richTextBoxInAndOut.BeginInvoke(new Action(() =>
                                {
                                    richTextBoxInAndOut.Text="Plc write OK.";
                                }));
                            }
                        }
                        else
                        {
                            richTextBoxInAndOut.BeginInvoke(new Action(() =>
                            {
                                richTextBoxInAndOut.Text = "Plc write Fail.";
                            }));
                        }
                    }
                }
            }
            catch(Exception except)
            {
                richTextBoxInAndOut.BeginInvoke(new Action(() =>
                {
                    richTextBoxInAndOut.Text = except.Message;
                }));
            }
          
        }

        private void richTextBoxInAndOut_TextChanged(object sender, EventArgs e)
        {
            InitDataLength();
        }

        private void comboBoxDataType_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitDataLength();
        }

        private void InitDataLength()
        {
            if (!radioButtonWrite.Checked)
                return;
            IOMemeoryDataType dataType = (IOMemeoryDataType)Enum.Parse(typeof(IOMemeoryDataType), comboBoxDataType.SelectedItem.ToString());
            string[] data = richTextBoxInAndOut.Text.Trim(' ').Split(' ');
            List<string> dataList = new List<string>(data);
            dataList.RemoveAll(new Predicate<string>(n => n == ""));
            switch (dataType)
            {
                case IOMemeoryDataType.Bit:
                case IOMemeoryDataType.BitWithForcedStatus:
                    textBoxLength.Text = dataList.Count.ToString();
                    break;
                default:
                    textBoxLength.Text = (dataList.Count / 2).ToString();
                    break;
            }
        }
    }
}
