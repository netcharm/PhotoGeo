﻿namespace FolderFilter_Demo
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose( bool disposing )
        {
            if ( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.explorerBrowser = new Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser();
            this.SuspendLayout();
            // 
            // explorerBrowser
            // 
            this.explorerBrowser.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.explorerBrowser.Location = new System.Drawing.Point(12, 12);
            this.explorerBrowser.Name = "explorerBrowser";
            this.explorerBrowser.PropertyBagName = "Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser";
            this.explorerBrowser.Size = new System.Drawing.Size(737, 429);
            this.explorerBrowser.TabIndex = 0;
            this.explorerBrowser.SelectionChanged += new System.EventHandler(this.explorerBrowser_SelectionChanged);
            this.explorerBrowser.NavigationComplete += new System.EventHandler<Microsoft.WindowsAPICodePack.Controls.NavigationCompleteEventArgs>(this.explorerBrowser_NavigationComplete);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(761, 453);
            this.Controls.Add(this.explorerBrowser);
            this.Name = "MainForm";
            this.Text = "FolderFilter Demo";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.WindowsAPICodePack.Controls.WindowsForms.ExplorerBrowser explorerBrowser;
    }
}

