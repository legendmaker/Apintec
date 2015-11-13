namespace Apintec.views.PlcBox
{
    partial class PlcPanel
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBoxPlcControl = new System.Windows.Forms.GroupBox();
            this.labelLength = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.textBoxAddressWord = new System.Windows.Forms.TextBox();
            this.comboBoxDataType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxPlcList = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxField = new System.Windows.Forms.ComboBox();
            this.textBoxLength = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radioButtonRead = new System.Windows.Forms.RadioButton();
            this.radioButtonWrite = new System.Windows.Forms.RadioButton();
            this.buttonOperate = new System.Windows.Forms.Button();
            this.richTextBoxInAndOut = new System.Windows.Forms.RichTextBox();
            this.textBoxAddressBit = new System.Windows.Forms.TextBox();
            this.groupBoxPlcControl.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxPlcControl
            // 
            this.groupBoxPlcControl.Controls.Add(this.labelLength);
            this.groupBoxPlcControl.Controls.Add(this.panel2);
            this.groupBoxPlcControl.Controls.Add(this.textBoxLength);
            this.groupBoxPlcControl.Controls.Add(this.panel1);
            this.groupBoxPlcControl.Controls.Add(this.buttonOperate);
            this.groupBoxPlcControl.Controls.Add(this.richTextBoxInAndOut);
            this.groupBoxPlcControl.Location = new System.Drawing.Point(3, 3);
            this.groupBoxPlcControl.Name = "groupBoxPlcControl";
            this.groupBoxPlcControl.Size = new System.Drawing.Size(364, 188);
            this.groupBoxPlcControl.TabIndex = 0;
            this.groupBoxPlcControl.TabStop = false;
            this.groupBoxPlcControl.Text = "PLC";
            // 
            // labelLength
            // 
            this.labelLength.AutoSize = true;
            this.labelLength.Location = new System.Drawing.Point(242, 84);
            this.labelLength.Name = "labelLength";
            this.labelLength.Size = new System.Drawing.Size(47, 12);
            this.labelLength.TabIndex = 20;
            this.labelLength.Text = "Length:";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.textBoxAddressBit);
            this.panel2.Controls.Add(this.textBoxAddressWord);
            this.panel2.Controls.Add(this.comboBoxDataType);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.comboBoxPlcList);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.comboBoxField);
            this.panel2.Location = new System.Drawing.Point(6, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(350, 59);
            this.panel2.TabIndex = 13;
            // 
            // textBoxAddressWord
            // 
            this.textBoxAddressWord.Location = new System.Drawing.Point(289, 28);
            this.textBoxAddressWord.Name = "textBoxAddressWord";
            this.textBoxAddressWord.Size = new System.Drawing.Size(33, 21);
            this.textBoxAddressWord.TabIndex = 21;
            this.textBoxAddressWord.Text = "0";
            // 
            // comboBoxDataType
            // 
            this.comboBoxDataType.FormattingEnabled = true;
            this.comboBoxDataType.Location = new System.Drawing.Point(185, 28);
            this.comboBoxDataType.Name = "comboBoxDataType";
            this.comboBoxDataType.Size = new System.Drawing.Size(98, 20);
            this.comboBoxDataType.TabIndex = 20;
            this.comboBoxDataType.SelectedIndexChanged += new System.EventHandler(this.comboBoxDataType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(207, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "DataType";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "PLC List";
            // 
            // comboBoxPlcList
            // 
            this.comboBoxPlcList.FormattingEnabled = true;
            this.comboBoxPlcList.Location = new System.Drawing.Point(3, 29);
            this.comboBoxPlcList.Name = "comboBoxPlcList";
            this.comboBoxPlcList.Size = new System.Drawing.Size(89, 20);
            this.comboBoxPlcList.TabIndex = 1;
            this.comboBoxPlcList.SelectedIndexChanged += new System.EventHandler(this.comboBoxPlcList_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(297, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 17;
            this.label3.Text = "Address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "Field";
            // 
            // comboBoxField
            // 
            this.comboBoxField.FormattingEnabled = true;
            this.comboBoxField.Location = new System.Drawing.Point(98, 29);
            this.comboBoxField.Name = "comboBoxField";
            this.comboBoxField.Size = new System.Drawing.Size(81, 20);
            this.comboBoxField.TabIndex = 13;
            this.comboBoxField.SelectedIndexChanged += new System.EventHandler(this.comboBoxField_SelectedIndexChanged);
            // 
            // textBoxLength
            // 
            this.textBoxLength.Location = new System.Drawing.Point(288, 81);
            this.textBoxLength.Name = "textBoxLength";
            this.textBoxLength.Size = new System.Drawing.Size(62, 21);
            this.textBoxLength.TabIndex = 19;
            this.textBoxLength.Text = "0";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.radioButtonRead);
            this.panel1.Controls.Add(this.radioButtonWrite);
            this.panel1.Location = new System.Drawing.Point(6, 77);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(153, 32);
            this.panel1.TabIndex = 1;
            // 
            // radioButtonRead
            // 
            this.radioButtonRead.AutoSize = true;
            this.radioButtonRead.Location = new System.Drawing.Point(3, 7);
            this.radioButtonRead.Name = "radioButtonRead";
            this.radioButtonRead.Size = new System.Drawing.Size(47, 16);
            this.radioButtonRead.TabIndex = 7;
            this.radioButtonRead.TabStop = true;
            this.radioButtonRead.Text = "Read";
            this.radioButtonRead.UseVisualStyleBackColor = true;
            // 
            // radioButtonWrite
            // 
            this.radioButtonWrite.AutoSize = true;
            this.radioButtonWrite.Location = new System.Drawing.Point(97, 7);
            this.radioButtonWrite.Name = "radioButtonWrite";
            this.radioButtonWrite.Size = new System.Drawing.Size(53, 16);
            this.radioButtonWrite.TabIndex = 8;
            this.radioButtonWrite.TabStop = true;
            this.radioButtonWrite.Text = "Write";
            this.radioButtonWrite.UseVisualStyleBackColor = true;
            this.radioButtonWrite.CheckedChanged += new System.EventHandler(this.radioButtonWrite_CheckedChanged);
            // 
            // buttonOperate
            // 
            this.buttonOperate.Location = new System.Drawing.Point(295, 118);
            this.buttonOperate.Name = "buttonOperate";
            this.buttonOperate.Size = new System.Drawing.Size(61, 59);
            this.buttonOperate.TabIndex = 6;
            this.buttonOperate.Text = "Operate";
            this.buttonOperate.UseVisualStyleBackColor = true;
            this.buttonOperate.Click += new System.EventHandler(this.buttonOperate_Click);
            // 
            // richTextBoxInAndOut
            // 
            this.richTextBoxInAndOut.Location = new System.Drawing.Point(6, 118);
            this.richTextBoxInAndOut.Name = "richTextBoxInAndOut";
            this.richTextBoxInAndOut.ReadOnly = true;
            this.richTextBoxInAndOut.Size = new System.Drawing.Size(270, 59);
            this.richTextBoxInAndOut.TabIndex = 3;
            this.richTextBoxInAndOut.Text = "";
            this.richTextBoxInAndOut.TextChanged += new System.EventHandler(this.richTextBoxInAndOut_TextChanged);
            // 
            // textBoxAddressBit
            // 
            this.textBoxAddressBit.Location = new System.Drawing.Point(328, 27);
            this.textBoxAddressBit.Name = "textBoxAddressBit";
            this.textBoxAddressBit.Size = new System.Drawing.Size(16, 21);
            this.textBoxAddressBit.TabIndex = 22;
            this.textBoxAddressBit.Text = "0";
            // 
            // PlcPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxPlcControl);
            this.Name = "PlcPanel";
            this.Size = new System.Drawing.Size(375, 201);
            this.groupBoxPlcControl.ResumeLayout(false);
            this.groupBoxPlcControl.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxPlcControl;
        private System.Windows.Forms.RadioButton radioButtonWrite;
        private System.Windows.Forms.RadioButton radioButtonRead;
        private System.Windows.Forms.Button buttonOperate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox richTextBoxInAndOut;
        private System.Windows.Forms.ComboBox comboBoxPlcList;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxField;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelLength;
        private System.Windows.Forms.TextBox textBoxLength;
        private System.Windows.Forms.ComboBox comboBoxDataType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxAddressWord;
        private System.Windows.Forms.TextBox textBoxAddressBit;
    }
}
