namespace CodeOverload.Windows.UI.Components
{
    partial class FileBrowserButton
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.FilePath = new System.Windows.Forms.TextBox();
            this.FileBrowse = new System.Windows.Forms.Button();
            this.Browser = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // FilePath
            // 
            this.FilePath.AcceptsReturn = true;
            this.FilePath.AcceptsTab = true;
            this.FilePath.Location = new System.Drawing.Point(6, 5);
            this.FilePath.Name = "FilePath";
            this.FilePath.Size = new System.Drawing.Size(162, 20);
            this.FilePath.TabIndex = 0;
            // 
            // FileBrowse
            // 
            this.FileBrowse.Location = new System.Drawing.Point(174, 3);
            this.FileBrowse.Name = "FileBrowse";
            this.FileBrowse.Size = new System.Drawing.Size(75, 23);
            this.FileBrowse.TabIndex = 1;
            this.FileBrowse.Text = "Browse";
            this.FileBrowse.UseVisualStyleBackColor = true;
            this.FileBrowse.Click += new System.EventHandler(this.FileBrowse_Click);
            // 
            // Browser
            // 
            this.Browser.FileName = "openFileDialog1";
            this.Browser.RestoreDirectory = true;
            this.Browser.SupportMultiDottedExtensions = true;
            // 
            // FileBrowserButton
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FileBrowse);
            this.Controls.Add(this.FilePath);
            this.Name = "FileBrowserButton";
            this.Size = new System.Drawing.Size(252, 30);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button FileBrowse;
        private System.Windows.Forms.OpenFileDialog Browser;
        public System.Windows.Forms.TextBox FilePath;
    }
}
