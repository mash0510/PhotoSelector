namespace PhotoSelector.Dialogs
{
    partial class PhotoDialog
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
            this.btn_Back = new System.Windows.Forms.Button();
            this.btn_Forward = new System.Windows.Forms.Button();
            this.photoSelectControl = new PhotoSelector.Controls.PhotoSelectControl();
            this.SuspendLayout();
            // 
            // btn_Back
            // 
            this.btn_Back.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_Back.Location = new System.Drawing.Point(504, 773);
            this.btn_Back.Name = "btn_Back";
            this.btn_Back.Size = new System.Drawing.Size(75, 34);
            this.btn_Back.TabIndex = 1;
            this.btn_Back.Text = "戻る";
            this.btn_Back.UseVisualStyleBackColor = true;
            // 
            // btn_Forward
            // 
            this.btn_Forward.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btn_Forward.Location = new System.Drawing.Point(610, 773);
            this.btn_Forward.Name = "btn_Forward";
            this.btn_Forward.Size = new System.Drawing.Size(75, 34);
            this.btn_Forward.TabIndex = 2;
            this.btn_Forward.Text = "進む";
            this.btn_Forward.UseVisualStyleBackColor = true;
            // 
            // photoSelectControl
            // 
            this.photoSelectControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.photoSelectControl.FileFullPath = null;
            this.photoSelectControl.Location = new System.Drawing.Point(12, 12);
            this.photoSelectControl.Name = "photoSelectControl";
            this.photoSelectControl.PhotoSizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.photoSelectControl.Selected = false;
            this.photoSelectControl.Size = new System.Drawing.Size(1180, 741);
            this.photoSelectControl.TabIndex = 0;
            // 
            // PhotoDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1204, 819);
            this.Controls.Add(this.btn_Forward);
            this.Controls.Add(this.btn_Back);
            this.Controls.Add(this.photoSelectControl);
            this.Name = "PhotoDialog";
            this.Text = "PhotoDialog";
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.PhotoSelectControl photoSelectControl;
        private System.Windows.Forms.Button btn_Back;
        private System.Windows.Forms.Button btn_Forward;
    }
}