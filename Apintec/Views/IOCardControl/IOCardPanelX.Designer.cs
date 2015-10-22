using System.Windows.Forms;

namespace Apintec.views.IOCardControl
{
    partial class IOCtrlElement
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
            this.labelBit = new System.Windows.Forms.Label();
            this.labelDIOValue = new System.Windows.Forms.Label();
            this.labelFunction = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelBit
            // 
            this.labelBit.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.labelBit.Location = new System.Drawing.Point(3, 0);
            this.labelBit.Name = "labelBit";
            this.labelBit.Size = new System.Drawing.Size(17, 12);
            this.labelBit.TabIndex = 56;
            this.labelBit.Text = "0";
            this.labelBit.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // labelDIOValue
            // 
            this.labelDIOValue.BackColor = System.Drawing.Color.LightGray;
            this.labelDIOValue.ForeColor = System.Drawing.SystemColors.ControlText;
            this.labelDIOValue.Location = new System.Drawing.Point(26, 0);
            this.labelDIOValue.Name = "labelDIOValue";
            this.labelDIOValue.Size = new System.Drawing.Size(17, 12);
            this.labelDIOValue.TabIndex = 57;
            this.labelDIOValue.Text = "0";
            this.labelDIOValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.labelDIOValue.Click += new System.EventHandler(this.labelDIOValue_Click);
            // 
            // labelFunction
            // 
            this.labelFunction.AutoSize = true;
            this.labelFunction.BackColor = System.Drawing.SystemColors.Control;
            this.labelFunction.Location = new System.Drawing.Point(51, 0);
            this.labelFunction.Name = "labelFunction";
            this.labelFunction.Size = new System.Drawing.Size(41, 12);
            this.labelFunction.TabIndex = 58;
            this.labelFunction.Text = "CPU OK";
            this.labelFunction.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // IOCtrlElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.labelBit);
            this.Controls.Add(this.labelDIOValue);
            this.Controls.Add(this.labelFunction);
            this.Name = "IOCtrlElement";
            this.Size = new System.Drawing.Size(150, 12);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelBit;
        private System.Windows.Forms.Label labelDIOValue;
        private System.Windows.Forms.Label labelFunction;

    }
}
