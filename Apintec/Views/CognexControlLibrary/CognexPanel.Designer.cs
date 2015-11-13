using System;

namespace Apintec.views.CognexControlLibrary
{
    partial class CognexPanel
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
            this.cvsInSightDisplay = new Cognex.InSight.Controls.Display.CvsInSightDisplay();
            this.SuspendLayout();
            // 
            // cvsInSightDisplay
            // 
            this.cvsInSightDisplay.DefaultTextScaleMode = Cognex.InSight.Controls.Display.CvsInSightDisplay.TextScaleModeType.Proportional;
            this.cvsInSightDisplay.DialogIcon = null;
            this.cvsInSightDisplay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cvsInSightDisplay.Location = new System.Drawing.Point(0, 0);
            this.cvsInSightDisplay.Name = "cvsInSightDisplay";
            this.cvsInSightDisplay.Size = new System.Drawing.Size(349, 284);
            this.cvsInSightDisplay.TabIndex = 1;
            // 
            // CognexPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cvsInSightDisplay);
            this.Name = "CognexPanel";
            this.Size = new System.Drawing.Size(349, 284);
            this.ResumeLayout(false);

        }
        #endregion
        private Cognex.InSight.Controls.Display.CvsInSightDisplay cvsInSightDisplay;

        
    }
}
