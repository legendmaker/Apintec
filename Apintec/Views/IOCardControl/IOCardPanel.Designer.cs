namespace Apintec.views.IOCardControl
{
    partial class IOCardPanel
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
            this.components = new System.ComponentModel.Container();
            this.groupBoxDI = new System.Windows.Forms.GroupBox();
            this.groupBoxDO = new System.Windows.Forms.GroupBox();
            this.timerDIRead = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // groupBoxDI
            // 
            this.groupBoxDI.Location = new System.Drawing.Point(3, 3);
            this.groupBoxDI.Name = "groupBoxDI";
            this.groupBoxDI.Size = new System.Drawing.Size(158, 310);
            this.groupBoxDI.TabIndex = 0;
            this.groupBoxDI.TabStop = false;
            this.groupBoxDI.Text = "DI";
            // 
            // groupBoxDO
            // 
            this.groupBoxDO.Location = new System.Drawing.Point(167, 3);
            this.groupBoxDO.Name = "groupBoxDO";
            this.groupBoxDO.Size = new System.Drawing.Size(158, 310);
            this.groupBoxDO.TabIndex = 1;
            this.groupBoxDO.TabStop = false;
            this.groupBoxDO.Text = "DO";
            // 
            // timerDIRead
            // 
            this.timerDIRead.Tick += new System.EventHandler(this.timerDIRead_Tick);
            // 
            // IOCardPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.groupBoxDO);
            this.Controls.Add(this.groupBoxDI);
            this.Name = "IOCardPanel";
            this.Size = new System.Drawing.Size(331, 320);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBoxDI;
        private System.Windows.Forms.GroupBox groupBoxDO;
        private System.Windows.Forms.Timer timerDIRead;
    }
}
