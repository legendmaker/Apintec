namespace APINTEC.views
{
    partial class NetClient
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
            this.groupBoxMain = new System.Windows.Forms.GroupBox();
            this.textBoxSend = new System.Windows.Forms.TextBox();
            this.comboBoxLocalHost = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonSend = new System.Windows.Forms.Button();
            this.labelPort = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.labelIP = new System.Windows.Forms.Label();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.richTextBoxMsg = new System.Windows.Forms.RichTextBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonDisConnect = new System.Windows.Forms.Button();
            this.groupBoxMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxMain
            // 
            this.groupBoxMain.Controls.Add(this.textBoxSend);
            this.groupBoxMain.Controls.Add(this.comboBoxLocalHost);
            this.groupBoxMain.Controls.Add(this.label1);
            this.groupBoxMain.Controls.Add(this.buttonSend);
            this.groupBoxMain.Controls.Add(this.labelPort);
            this.groupBoxMain.Controls.Add(this.textBoxPort);
            this.groupBoxMain.Controls.Add(this.labelIP);
            this.groupBoxMain.Controls.Add(this.textBoxIP);
            this.groupBoxMain.Controls.Add(this.richTextBoxMsg);
            this.groupBoxMain.Controls.Add(this.buttonConnect);
            this.groupBoxMain.Controls.Add(this.buttonDisConnect);
            this.groupBoxMain.Location = new System.Drawing.Point(3, 3);
            this.groupBoxMain.Name = "groupBoxMain";
            this.groupBoxMain.Size = new System.Drawing.Size(492, 320);
            this.groupBoxMain.TabIndex = 0;
            this.groupBoxMain.TabStop = false;
            this.groupBoxMain.Text = "NetClient";
            // 
            // textBoxSend
            // 
            this.textBoxSend.Location = new System.Drawing.Point(6, 62);
            this.textBoxSend.Multiline = true;
            this.textBoxSend.Name = "textBoxSend";
            this.textBoxSend.Size = new System.Drawing.Size(395, 60);
            this.textBoxSend.TabIndex = 22;
            // 
            // comboBoxLocalHost
            // 
            this.comboBoxLocalHost.FormattingEnabled = true;
            this.comboBoxLocalHost.Location = new System.Drawing.Point(75, 14);
            this.comboBoxLocalHost.Name = "comboBoxLocalHost";
            this.comboBoxLocalHost.Size = new System.Drawing.Size(100, 20);
            this.comboBoxLocalHost.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 19;
            this.label1.Text = "Localhost:";
            // 
            // buttonSend
            // 
            this.buttonSend.Enabled = false;
            this.buttonSend.Location = new System.Drawing.Point(414, 62);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(75, 60);
            this.buttonSend.TabIndex = 18;
            this.buttonSend.Text = "Send";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // labelPort
            // 
            this.labelPort.AutoSize = true;
            this.labelPort.Location = new System.Drawing.Point(338, 21);
            this.labelPort.Name = "labelPort";
            this.labelPort.Size = new System.Drawing.Size(11, 12);
            this.labelPort.TabIndex = 16;
            this.labelPort.Text = ":";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(355, 16);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(46, 21);
            this.textBoxPort.TabIndex = 15;
            this.textBoxPort.Text = "1111";
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(189, 19);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(47, 12);
            this.labelIP.TabIndex = 14;
            this.labelIP.Text = "Server:";
            // 
            // textBoxIP
            // 
            this.textBoxIP.Location = new System.Drawing.Point(242, 16);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(90, 21);
            this.textBoxIP.TabIndex = 13;
            this.textBoxIP.Text = "192.168.0.2";
            // 
            // richTextBoxMsg
            // 
            this.richTextBoxMsg.Location = new System.Drawing.Point(6, 144);
            this.richTextBoxMsg.Name = "richTextBoxMsg";
            this.richTextBoxMsg.ReadOnly = true;
            this.richTextBoxMsg.Size = new System.Drawing.Size(483, 168);
            this.richTextBoxMsg.TabIndex = 12;
            this.richTextBoxMsg.Text = "";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(414, 12);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonConnect.TabIndex = 17;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonDisConnect
            // 
            this.buttonDisConnect.Enabled = false;
            this.buttonDisConnect.Location = new System.Drawing.Point(414, 12);
            this.buttonDisConnect.Name = "buttonDisConnect";
            this.buttonDisConnect.Size = new System.Drawing.Size(75, 23);
            this.buttonDisConnect.TabIndex = 21;
            this.buttonDisConnect.Text = "Disconnect";
            this.buttonDisConnect.UseVisualStyleBackColor = true;
            this.buttonDisConnect.Click += new System.EventHandler(this.buttonDisConnect_Click);
            // 
            // NetClient
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxMain);
            this.Name = "NetClient";
            this.Size = new System.Drawing.Size(498, 327);
            this.groupBoxMain.ResumeLayout(false);
            this.groupBoxMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxMain;
        private System.Windows.Forms.TextBox textBoxSend;
        private System.Windows.Forms.ComboBox comboBoxLocalHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.Label labelPort;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.RichTextBox richTextBoxMsg;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonDisConnect;
    }
}
