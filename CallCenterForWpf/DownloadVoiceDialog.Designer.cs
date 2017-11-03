namespace CallCenterForWpf
{
    partial class DownloadVoiceDialog
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
            this.FlowComboBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DownloadInfoTextBox = new System.Windows.Forms.TextBox();
            this.DownloadControlButton = new System.Windows.Forms.Button();
            this.LabText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(43, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "流程选择：";
            // 
            // FlowComboBox
            // 
            this.FlowComboBox.BackColor = System.Drawing.Color.White;
            this.FlowComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FlowComboBox.FormattingEnabled = true;
            this.FlowComboBox.Location = new System.Drawing.Point(114, 22);
            this.FlowComboBox.Name = "FlowComboBox";
            this.FlowComboBox.Size = new System.Drawing.Size(209, 20);
            this.FlowComboBox.TabIndex = 1;
            this.FlowComboBox.SelectedIndexChanged += new System.EventHandler(this.FlowComboBox_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "下载进度：";
            // 
            // DownloadInfoTextBox
            // 
            this.DownloadInfoTextBox.BackColor = System.Drawing.Color.White;
            this.DownloadInfoTextBox.Location = new System.Drawing.Point(114, 60);
            this.DownloadInfoTextBox.Name = "DownloadInfoTextBox";
            this.DownloadInfoTextBox.ReadOnly = true;
            this.DownloadInfoTextBox.Size = new System.Drawing.Size(209, 21);
            this.DownloadInfoTextBox.TabIndex = 3;
            // 
            // DownloadControlButton
            // 
            this.DownloadControlButton.Location = new System.Drawing.Point(126, 105);
            this.DownloadControlButton.Name = "DownloadControlButton";
            this.DownloadControlButton.Size = new System.Drawing.Size(120, 34);
            this.DownloadControlButton.TabIndex = 4;
            this.DownloadControlButton.Text = "开始下载";
            this.DownloadControlButton.UseVisualStyleBackColor = true;
            this.DownloadControlButton.Click += new System.EventHandler(this.DownloadControlButton_Click);
            // 
            // LabText
            // 
            this.LabText.AutoSize = true;
            this.LabText.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.LabText.Location = new System.Drawing.Point(42, 151);
            this.LabText.Name = "LabText";
            this.LabText.Size = new System.Drawing.Size(287, 14);
            this.LabText.TabIndex = 5;
            this.LabText.Text = "说明：录音文件用于任务外呼时的机器人声音";
            // 
            // DownloadVoiceDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(376, 196);
            this.Controls.Add(this.LabText);
            this.Controls.Add(this.DownloadControlButton);
            this.Controls.Add(this.DownloadInfoTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.FlowComboBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DownloadVoiceDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "流程语音文件下载";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownloadVoiceDialog_FormClosing);
            this.Load += new System.EventHandler(this.DownloadVoiceDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox FlowComboBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox DownloadInfoTextBox;
        private System.Windows.Forms.Button DownloadControlButton;
        private System.Windows.Forms.Label LabText;
    }
}