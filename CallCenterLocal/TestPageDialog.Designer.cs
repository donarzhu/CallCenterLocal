using CallCenterLocal.Data;
using System;
using System.Collections.Generic;

namespace CallCenterLocal
{
    partial class TestPageDialog
    {
        private ResultWorkflows _data = null;
        private Dictionary<String, String> _dict = new Dictionary<string, string>();
        public Token Token { get; set; }
        public ResultWorkflows Data {
            set { _data = value; LoadData(); }
            get { return _data; }
        }
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

        private void LoadData()
        {
            if (this.Data == null || this.Data.successful == false)
                return;
            foreach(Workflow info in Data.data)
            {
                try
                {
                    _dict.Add(info.title, info.id);
                    this.mWorkflowComboBox.Items.Add(info.title);
                }
                catch(Exception e)
                {
                    continue;
                }
            }
        }
        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestPageDialog));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.mPhoneOneTextBox = new System.Windows.Forms.TextBox();
            this.mPhoneTowTextBox = new System.Windows.Forms.TextBox();
            this.TestButton = new System.Windows.Forms.Button();
            this.mWorkflowComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // mPhoneOneTextBox
            // 
            resources.ApplyResources(this.mPhoneOneTextBox, "mPhoneOneTextBox");
            this.mPhoneOneTextBox.Name = "mPhoneOneTextBox";
            this.mPhoneOneTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mPhoneOneTextBox_KeyPress);
            // 
            // mPhoneTowTextBox
            // 
            resources.ApplyResources(this.mPhoneTowTextBox, "mPhoneTowTextBox");
            this.mPhoneTowTextBox.Name = "mPhoneTowTextBox";
            this.mPhoneTowTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.mPhoneTowTextBox_KeyPress);
            // 
            // TestButton
            // 
            resources.ApplyResources(this.TestButton, "TestButton");
            this.TestButton.Name = "TestButton";
            this.TestButton.UseVisualStyleBackColor = true;
            this.TestButton.Click += new System.EventHandler(this.TestButton_Click);
            // 
            // mWorkflowComboBox
            // 
            this.mWorkflowComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.mWorkflowComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.mWorkflowComboBox, "mWorkflowComboBox");
            this.mWorkflowComboBox.Name = "mWorkflowComboBox";
            // 
            // TestPageDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mWorkflowComboBox);
            this.Controls.Add(this.TestButton);
            this.Controls.Add(this.mPhoneTowTextBox);
            this.Controls.Add(this.mPhoneOneTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TestPageDialog";
            this.ShowIcon = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox mPhoneOneTextBox;
        private System.Windows.Forms.TextBox mPhoneTowTextBox;
        private System.Windows.Forms.Button TestButton;
        private System.Windows.Forms.ComboBox mWorkflowComboBox;
    }
}