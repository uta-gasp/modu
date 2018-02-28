namespace Modu
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.label1 = new System.Windows.Forms.Label();
            this.txbIP = new System.Windows.Forms.TextBox();
            this.txbDebug = new System.Windows.Forms.TextBox();
            this.lblConnectionCount = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnToggleSender = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "IP";
            // 
            // txbIP
            // 
            this.txbIP.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbIP.Location = new System.Drawing.Point(35, 12);
            this.txbIP.Name = "txbIP";
            this.txbIP.Size = new System.Drawing.Size(179, 20);
            this.txbIP.TabIndex = 1;
            this.txbIP.TextChanged += new System.EventHandler(this.txbIP_TextChanged);
            // 
            // txbDebug
            // 
            this.txbDebug.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbDebug.Location = new System.Drawing.Point(12, 59);
            this.txbDebug.Multiline = true;
            this.txbDebug.Name = "txbDebug";
            this.txbDebug.ReadOnly = true;
            this.txbDebug.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txbDebug.Size = new System.Drawing.Size(202, 186);
            this.txbDebug.TabIndex = 2;
            this.txbDebug.TabStop = false;
            // 
            // lblConnectionCount
            // 
            this.lblConnectionCount.AutoSize = true;
            this.lblConnectionCount.Location = new System.Drawing.Point(78, 35);
            this.lblConnectionCount.Name = "lblConnectionCount";
            this.lblConnectionCount.Size = new System.Drawing.Size(13, 13);
            this.lblConnectionCount.TabIndex = 7;
            this.lblConnectionCount.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Connections";
            // 
            // btnToggleSender
            // 
            this.btnToggleSender.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnToggleSender.Location = new System.Drawing.Point(80, 251);
            this.btnToggleSender.Name = "btnToggleSender";
            this.btnToggleSender.Size = new System.Drawing.Size(75, 23);
            this.btnToggleSender.TabIndex = 8;
            this.btnToggleSender.Text = "Start";
            this.btnToggleSender.UseVisualStyleBackColor = true;
            this.btnToggleSender.Click += new System.EventHandler(this.btnToggleSender_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(226, 286);
            this.Controls.Add(this.btnToggleSender);
            this.Controls.Add(this.lblConnectionCount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txbDebug);
            this.Controls.Add(this.txbIP);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Modu";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txbIP;
        private System.Windows.Forms.TextBox txbDebug;
        private System.Windows.Forms.Label lblConnectionCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnToggleSender;
    }
}

