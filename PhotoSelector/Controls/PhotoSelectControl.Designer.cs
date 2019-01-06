namespace PhotoSelector.Controls
{
    partial class PhotoSelectControl
    {
        /// <summary> 
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region コンポーネント デザイナーで生成されたコード

        /// <summary> 
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を 
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.rb_OK = new System.Windows.Forms.RadioButton();
            this.rb_NG = new System.Windows.Forms.RadioButton();
            this.lbl_FileName = new System.Windows.Forms.Label();
            this.pb_Thumbnail = new PhotoSelector.Controls.PictureBoxZoom();
            ((System.ComponentModel.ISupportInitialize)(this.pb_Thumbnail)).BeginInit();
            this.SuspendLayout();
            // 
            // rb_OK
            // 
            this.rb_OK.AutoSize = true;
            this.rb_OK.Location = new System.Drawing.Point(176, 278);
            this.rb_OK.Name = "rb_OK";
            this.rb_OK.Size = new System.Drawing.Size(57, 22);
            this.rb_OK.TabIndex = 1;
            this.rb_OK.TabStop = true;
            this.rb_OK.Text = "OK";
            this.rb_OK.UseVisualStyleBackColor = true;
            // 
            // rb_NG
            // 
            this.rb_NG.AutoSize = true;
            this.rb_NG.Location = new System.Drawing.Point(248, 278);
            this.rb_NG.Name = "rb_NG";
            this.rb_NG.Size = new System.Drawing.Size(57, 22);
            this.rb_NG.TabIndex = 2;
            this.rb_NG.TabStop = true;
            this.rb_NG.Text = "NG";
            this.rb_NG.UseVisualStyleBackColor = true;
            // 
            // lbl_FileName
            // 
            this.lbl_FileName.AutoSize = true;
            this.lbl_FileName.Location = new System.Drawing.Point(21, 279);
            this.lbl_FileName.Name = "lbl_FileName";
            this.lbl_FileName.Size = new System.Drawing.Size(73, 18);
            this.lbl_FileName.TabIndex = 4;
            this.lbl_FileName.Text = "fileName";
            // 
            // pb_Thumbnail
            // 
            this.pb_Thumbnail.FileFullPath = null;
            this.pb_Thumbnail.Location = new System.Drawing.Point(3, 3);
            this.pb_Thumbnail.Name = "pb_Thumbnail";
            this.pb_Thumbnail.Size = new System.Drawing.Size(326, 265);
            this.pb_Thumbnail.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pb_Thumbnail.TabIndex = 3;
            this.pb_Thumbnail.TabStop = false;
            // 
            // PhotoSelectControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl_FileName);
            this.Controls.Add(this.pb_Thumbnail);
            this.Controls.Add(this.rb_NG);
            this.Controls.Add(this.rb_OK);
            this.Name = "PhotoSelectControl";
            this.Size = new System.Drawing.Size(332, 308);
            ((System.ComponentModel.ISupportInitialize)(this.pb_Thumbnail)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private PictureBoxZoom pb_Thumbnail;
        public System.Windows.Forms.RadioButton rb_OK;
        public System.Windows.Forms.RadioButton rb_NG;
        public System.Windows.Forms.Label lbl_FileName;
    }
}
