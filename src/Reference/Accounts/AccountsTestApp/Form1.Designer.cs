namespace AccountsTestApp
{
    partial class Form1
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
            this.label1 = new System.Windows.Forms.Label();
            this.BaseUrl = new System.Windows.Forms.TextBox();
            this.ClientId = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.ClientSecret = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.IdentifierUri = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Tenant = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.StartStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Base Url";
            // 
            // BaseUrl
            // 
            this.BaseUrl.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BaseUrl.Location = new System.Drawing.Point(85, 12);
            this.BaseUrl.Name = "BaseUrl";
            this.BaseUrl.Size = new System.Drawing.Size(447, 20);
            this.BaseUrl.TabIndex = 1;
            // 
            // ClientId
            // 
            this.ClientId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ClientId.Location = new System.Drawing.Point(85, 38);
            this.ClientId.Name = "ClientId";
            this.ClientId.Size = new System.Drawing.Size(447, 20);
            this.ClientId.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(45, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Client Id";
            // 
            // ClientSecret
            // 
            this.ClientSecret.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ClientSecret.Location = new System.Drawing.Point(85, 64);
            this.ClientSecret.Name = "ClientSecret";
            this.ClientSecret.Size = new System.Drawing.Size(447, 20);
            this.ClientSecret.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Client Secret";
            // 
            // IdentifierUri
            // 
            this.IdentifierUri.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.IdentifierUri.Location = new System.Drawing.Point(85, 90);
            this.IdentifierUri.Name = "IdentifierUri";
            this.IdentifierUri.Size = new System.Drawing.Size(447, 20);
            this.IdentifierUri.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 93);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Identifier Uri";
            // 
            // Tenant
            // 
            this.Tenant.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Tenant.Location = new System.Drawing.Point(85, 116);
            this.Tenant.Name = "Tenant";
            this.Tenant.Size = new System.Drawing.Size(447, 20);
            this.Tenant.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 119);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Tenant";
            // 
            // StartStop
            // 
            this.StartStop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.StartStop.Location = new System.Drawing.Point(15, 142);
            this.StartStop.Name = "StartStop";
            this.StartStop.Size = new System.Drawing.Size(517, 23);
            this.StartStop.TabIndex = 10;
            this.StartStop.Text = "Start";
            this.StartStop.UseVisualStyleBackColor = true;
            this.StartStop.Click += new System.EventHandler(this.DownloadButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(550, 182);
            this.Controls.Add(this.StartStop);
            this.Controls.Add(this.Tenant);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.IdentifierUri);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.ClientSecret);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ClientId);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BaseUrl);
            this.Controls.Add(this.label1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox BaseUrl;
        private System.Windows.Forms.TextBox ClientId;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox ClientSecret;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox IdentifierUri;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox Tenant;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button StartStop;
    }
}

