namespace ModuReceiver
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartClose = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txbPort = new System.Windows.Forms.TextBox();
            this.pnlInfo = new System.Windows.Forms.Panel();
            this.txbDebug = new System.Windows.Forms.TextBox();
            this.lblConnectionCount = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnStartClose
            // 
            this.btnStartClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnStartClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnStartClose.Location = new System.Drawing.Point(65, 44);
            this.btnStartClose.Name = "btnStartClose";
            this.btnStartClose.Size = new System.Drawing.Size(75, 23);
            this.btnStartClose.TabIndex = 0;
            this.btnStartClose.Text = "Start";
            this.btnStartClose.UseVisualStyleBackColor = true;
            this.btnStartClose.Click += new System.EventHandler(this.btnStartClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(26, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Port";
            // 
            // txbPort
            // 
            this.txbPort.Location = new System.Drawing.Point(81, 12);
            this.txbPort.Name = "txbPort";
            this.txbPort.Size = new System.Drawing.Size(116, 20);
            this.txbPort.TabIndex = 5;
            this.txbPort.Text = "18296";
            // 
            // pnlInfo
            // 
            this.pnlInfo.Controls.Add(this.txbDebug);
            this.pnlInfo.Controls.Add(this.lblConnectionCount);
            this.pnlInfo.Controls.Add(this.label1);
            this.pnlInfo.Location = new System.Drawing.Point(12, 38);
            this.pnlInfo.Name = "pnlInfo";
            this.pnlInfo.Size = new System.Drawing.Size(185, 203);
            this.pnlInfo.TabIndex = 6;
            this.pnlInfo.Visible = false;
            // 
            // txbDebug
            // 
            this.txbDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbDebug.Location = new System.Drawing.Point(3, 26);
            this.txbDebug.Multiline = true;
            this.txbDebug.Name = "txbDebug";
            this.txbDebug.ReadOnly = true;
            this.txbDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbDebug.Size = new System.Drawing.Size(179, 174);
            this.txbDebug.TabIndex = 6;
            // 
            // lblConnectionCount
            // 
            this.lblConnectionCount.AutoSize = true;
            this.lblConnectionCount.Location = new System.Drawing.Point(69, 0);
            this.lblConnectionCount.Name = "lblConnectionCount";
            this.lblConnectionCount.Size = new System.Drawing.Size(13, 13);
            this.lblConnectionCount.TabIndex = 5;
            this.lblConnectionCount.Text = "0";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Connections";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnStartClose;
            this.ClientSize = new System.Drawing.Size(209, 79);
            this.Controls.Add(this.btnStartClose);
            this.Controls.Add(this.pnlInfo);
            this.Controls.Add(this.txbPort);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Modu Receiver";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.pnlInfo.ResumeLayout(false);
            this.pnlInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txbPort;
        private System.Windows.Forms.Panel pnlInfo;
        private System.Windows.Forms.TextBox txbDebug;
        private System.Windows.Forms.Label lblConnectionCount;
        private System.Windows.Forms.Label label1;
    }
}

