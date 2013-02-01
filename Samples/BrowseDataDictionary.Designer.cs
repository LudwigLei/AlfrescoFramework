namespace Samples
{
    partial class BrowseDataDictionary
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BrowseDataDictionary));
            this.listBoxClasses = new System.Windows.Forms.ListBox();
            this.groupBoxDetails = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelSuperClass = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelDescription = new System.Windows.Forms.Label();
            this.groupBoxDetails.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBoxClasses
            // 
            this.listBoxClasses.FormattingEnabled = true;
            this.listBoxClasses.Location = new System.Drawing.Point(12, 12);
            this.listBoxClasses.Name = "listBoxClasses";
            this.listBoxClasses.Size = new System.Drawing.Size(427, 147);
            this.listBoxClasses.TabIndex = 0;
            this.listBoxClasses.SelectedIndexChanged += new System.EventHandler(this.listBoxClasses_SelectedIndexChanged);
            // 
            // groupBoxDetails
            // 
            this.groupBoxDetails.Controls.Add(this.labelDescription);
            this.groupBoxDetails.Controls.Add(this.label7);
            this.groupBoxDetails.Controls.Add(this.labelTitle);
            this.groupBoxDetails.Controls.Add(this.label5);
            this.groupBoxDetails.Controls.Add(this.labelName);
            this.groupBoxDetails.Controls.Add(this.label3);
            this.groupBoxDetails.Controls.Add(this.labelSuperClass);
            this.groupBoxDetails.Controls.Add(this.label1);
            this.groupBoxDetails.Location = new System.Drawing.Point(12, 165);
            this.groupBoxDetails.Name = "groupBoxDetails";
            this.groupBoxDetails.Size = new System.Drawing.Size(427, 90);
            this.groupBoxDetails.TabIndex = 1;
            this.groupBoxDetails.TabStop = false;
            this.groupBoxDetails.Text = "Class Details";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Super Class:";
            // 
            // labelSuperClass
            // 
            this.labelSuperClass.AutoSize = true;
            this.labelSuperClass.Location = new System.Drawing.Point(89, 16);
            this.labelSuperClass.Name = "labelSuperClass";
            this.labelSuperClass.Size = new System.Drawing.Size(0, 13);
            this.labelSuperClass.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Name:";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(89, 33);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(0, 13);
            this.labelName.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 50);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(30, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Title:";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Location = new System.Drawing.Point(89, 50);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(0, 13);
            this.labelTitle.TabIndex = 5;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "Description:";
            // 
            // labelDescription
            // 
            this.labelDescription.AutoSize = true;
            this.labelDescription.Location = new System.Drawing.Point(89, 67);
            this.labelDescription.Name = "labelDescription";
            this.labelDescription.Size = new System.Drawing.Size(0, 13);
            this.labelDescription.TabIndex = 7;
            // 
            // BrowseDataDictionary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 272);
            this.Controls.Add(this.groupBoxDetails);
            this.Controls.Add(this.listBoxClasses);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "BrowseDataDictionary";
            this.Text = "BrowseDataDictionary";
            this.Load += new System.EventHandler(this.BrowseDataDictionary_Load);
            this.groupBoxDetails.ResumeLayout(false);
            this.groupBoxDetails.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox listBoxClasses;
        private System.Windows.Forms.GroupBox groupBoxDetails;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label labelSuperClass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelDescription;
        private System.Windows.Forms.Label label7;
    }
}