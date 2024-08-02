namespace WinDropV2
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
            this.fileCache = new System.Windows.Forms.ListView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.localDeviceCache = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.listView2 = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.connectionAddPanel = new System.Windows.Forms.Panel();
            this.addConnectionButton = new System.Windows.Forms.Button();
            this.nameTextBox = new System.Windows.Forms.TextBox();
            this.ipAddressTextBox = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.connectionAddPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // fileCache
            // 
            this.fileCache.AllowDrop = true;
            this.fileCache.HideSelection = false;
            this.fileCache.Location = new System.Drawing.Point(31, 157);
            this.fileCache.Name = "fileCache";
            this.fileCache.Size = new System.Drawing.Size(541, 258);
            this.fileCache.TabIndex = 0;
            this.fileCache.UseCompatibleStateImageBehavior = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.localDeviceCache);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.listView2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(585, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(203, 402);
            this.panel1.TabIndex = 1;
            // 
            // localDeviceCache
            // 
            this.localDeviceCache.HideSelection = false;
            this.localDeviceCache.Location = new System.Drawing.Point(2, 215);
            this.localDeviceCache.Name = "localDeviceCache";
            this.localDeviceCache.Size = new System.Drawing.Size(182, 184);
            this.localDeviceCache.TabIndex = 7;
            this.localDeviceCache.UseCompatibleStateImageBehavior = false;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(1, 198);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 23);
            this.label2.TabIndex = 6;
            this.label2.Text = "local Connections:\r\n";
            // 
            // listView2
            // 
            this.listView2.HideSelection = false;
            this.listView2.Location = new System.Drawing.Point(1, 18);
            this.listView2.Name = "listView2";
            this.listView2.Size = new System.Drawing.Size(182, 177);
            this.listView2.TabIndex = 3;
            this.listView2.UseCompatibleStateImageBehavior = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(148, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "online Connections:\r\n";
            // 
            // connectionAddPanel
            // 
            this.connectionAddPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.connectionAddPanel.Controls.Add(this.addConnectionButton);
            this.connectionAddPanel.Controls.Add(this.nameTextBox);
            this.connectionAddPanel.Controls.Add(this.ipAddressTextBox);
            this.connectionAddPanel.Location = new System.Drawing.Point(300, 17);
            this.connectionAddPanel.Name = "connectionAddPanel";
            this.connectionAddPanel.Size = new System.Drawing.Size(271, 127);
            this.connectionAddPanel.TabIndex = 2;
            // 
            // addConnectionButton
            // 
            this.addConnectionButton.Location = new System.Drawing.Point(177, 90);
            this.addConnectionButton.Name = "addConnectionButton";
            this.addConnectionButton.Size = new System.Drawing.Size(75, 23);
            this.addConnectionButton.TabIndex = 2;
            this.addConnectionButton.Text = "Save";
            this.addConnectionButton.UseVisualStyleBackColor = true;
            this.addConnectionButton.Click += new System.EventHandler(this.addConnectionButton_Click);
            // 
            // nameTextBox
            // 
            this.nameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nameTextBox.Location = new System.Drawing.Point(12, 12);
            this.nameTextBox.Name = "nameTextBox";
            this.nameTextBox.Size = new System.Drawing.Size(162, 20);
            this.nameTextBox.TabIndex = 1;
            this.nameTextBox.Text = "Connection name...";
            // 
            // ipAddressTextBox
            // 
            this.ipAddressTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ipAddressTextBox.Location = new System.Drawing.Point(12, 55);
            this.ipAddressTextBox.Name = "ipAddressTextBox";
            this.ipAddressTextBox.Size = new System.Drawing.Size(162, 20);
            this.ipAddressTextBox.TabIndex = 0;
            this.ipAddressTextBox.Text = "Address of system...";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.connectionAddPanel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.fileCache);
            this.Name = "Form1";
            this.Text = "Windrop 1.9";
            this.panel1.ResumeLayout(false);
            this.connectionAddPanel.ResumeLayout(false);
            this.connectionAddPanel.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Button addConnectionButton;

        private System.Windows.Forms.TextBox ipAddressTextBox;
        private System.Windows.Forms.TextBox nameTextBox;

        private System.Windows.Forms.Panel connectionAddPanel;

        private System.Windows.Forms.Label label2;

        private System.Windows.Forms.ListView localDeviceCache;
        private System.Windows.Forms.ListView listView2;
        private System.Windows.Forms.Label label1;

        private System.Windows.Forms.Panel panel1;

        private System.Windows.Forms.ListView fileCache;

        #endregion
    }
}