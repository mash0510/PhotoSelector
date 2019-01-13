namespace PhotoSelector
{
    partial class MainWindow
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.ファイルFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Open = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_Save = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_ExecSorting = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.menu_Quit = new System.Windows.Forms.ToolStripMenuItem();
            this.rb_AllPictures = new System.Windows.Forms.RadioButton();
            this.rb_OK = new System.Windows.Forms.RadioButton();
            this.rb_NG = new System.Windows.Forms.RadioButton();
            this.lbl_KeepPhotos = new System.Windows.Forms.Label();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.photoGrid = new PhotoSelector.Controls.PhotoGrid();
            this.keepPhotoGrid = new PhotoSelector.Controls.PhotoGrid();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ファイルFToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1593, 33);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // ファイルFToolStripMenuItem
            // 
            this.ファイルFToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_Open,
            this.menu_Save,
            this.toolStripSeparator1,
            this.menu_ExecSorting,
            this.toolStripSeparator2,
            this.menu_Quit});
            this.ファイルFToolStripMenuItem.Name = "ファイルFToolStripMenuItem";
            this.ファイルFToolStripMenuItem.Size = new System.Drawing.Size(94, 29);
            this.ファイルFToolStripMenuItem.Text = "ファイル(&F)";
            // 
            // menu_Open
            // 
            this.menu_Open.Name = "menu_Open";
            this.menu_Open.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.menu_Open.Size = new System.Drawing.Size(222, 30);
            this.menu_Open.Text = "開く...";
            // 
            // menu_Save
            // 
            this.menu_Save.Name = "menu_Save";
            this.menu_Save.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.menu_Save.Size = new System.Drawing.Size(222, 30);
            this.menu_Save.Text = "保存する";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(219, 6);
            // 
            // menu_ExecSorting
            // 
            this.menu_ExecSorting.Name = "menu_ExecSorting";
            this.menu_ExecSorting.Size = new System.Drawing.Size(222, 30);
            this.menu_ExecSorting.Text = "振り分け実行";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(219, 6);
            // 
            // menu_Quit
            // 
            this.menu_Quit.Name = "menu_Quit";
            this.menu_Quit.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Q)));
            this.menu_Quit.Size = new System.Drawing.Size(222, 30);
            this.menu_Quit.Text = "終了(&X)";
            // 
            // rb_AllPictures
            // 
            this.rb_AllPictures.AutoSize = true;
            this.rb_AllPictures.Checked = true;
            this.rb_AllPictures.Location = new System.Drawing.Point(12, 16);
            this.rb_AllPictures.Name = "rb_AllPictures";
            this.rb_AllPictures.Size = new System.Drawing.Size(87, 22);
            this.rb_AllPictures.TabIndex = 2;
            this.rb_AllPictures.TabStop = true;
            this.rb_AllPictures.Text = "全表示";
            this.rb_AllPictures.UseVisualStyleBackColor = true;
            // 
            // rb_OK
            // 
            this.rb_OK.AutoSize = true;
            this.rb_OK.Location = new System.Drawing.Point(122, 16);
            this.rb_OK.Name = "rb_OK";
            this.rb_OK.Size = new System.Drawing.Size(160, 22);
            this.rb_OK.TabIndex = 3;
            this.rb_OK.TabStop = true;
            this.rb_OK.Text = "OK画像のみ表示";
            this.rb_OK.UseVisualStyleBackColor = true;
            // 
            // rb_NG
            // 
            this.rb_NG.AutoSize = true;
            this.rb_NG.Location = new System.Drawing.Point(305, 16);
            this.rb_NG.Name = "rb_NG";
            this.rb_NG.Size = new System.Drawing.Size(160, 22);
            this.rb_NG.TabIndex = 4;
            this.rb_NG.TabStop = true;
            this.rb_NG.Text = "NG画像のみ表示";
            this.rb_NG.UseVisualStyleBackColor = true;
            // 
            // lbl_KeepPhotos
            // 
            this.lbl_KeepPhotos.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl_KeepPhotos.AutoSize = true;
            this.lbl_KeepPhotos.Location = new System.Drawing.Point(12, 20);
            this.lbl_KeepPhotos.Name = "lbl_KeepPhotos";
            this.lbl_KeepPhotos.Size = new System.Drawing.Size(44, 18);
            this.lbl_KeepPhotos.TabIndex = 6;
            this.lbl_KeepPhotos.Text = "保留";
            // 
            // splitContainer
            // 
            this.splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer.Location = new System.Drawing.Point(13, 47);
            this.splitContainer.Name = "splitContainer";
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.photoGrid);
            this.splitContainer.Panel1.Controls.Add(this.rb_NG);
            this.splitContainer.Panel1.Controls.Add(this.rb_OK);
            this.splitContainer.Panel1.Controls.Add(this.rb_AllPictures);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.keepPhotoGrid);
            this.splitContainer.Panel2.Controls.Add(this.lbl_KeepPhotos);
            this.splitContainer.Size = new System.Drawing.Size(1568, 909);
            this.splitContainer.SplitterDistance = 1051;
            this.splitContainer.TabIndex = 7;
            // 
            // photoGrid
            // 
            this.photoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.photoGrid.AutoScroll = true;
            this.photoGrid.BackColor = System.Drawing.SystemColors.Window;
            this.photoGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.photoGrid.CellMargin = 4;
            this.photoGrid.Location = new System.Drawing.Point(3, 65);
            this.photoGrid.Name = "photoGrid";
            this.photoGrid.PhotoList = null;
            this.photoGrid.Size = new System.Drawing.Size(1045, 841);
            this.photoGrid.TabIndex = 1;
            // 
            // keepPhotoGrid
            // 
            this.keepPhotoGrid.AllowDrop = true;
            this.keepPhotoGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keepPhotoGrid.AutoScroll = true;
            this.keepPhotoGrid.BackColor = System.Drawing.SystemColors.Window;
            this.keepPhotoGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.keepPhotoGrid.CellMargin = 4;
            this.keepPhotoGrid.Location = new System.Drawing.Point(3, 65);
            this.keepPhotoGrid.Name = "keepPhotoGrid";
            this.keepPhotoGrid.PhotoList = null;
            this.keepPhotoGrid.Size = new System.Drawing.Size(507, 841);
            this.keepPhotoGrid.TabIndex = 5;
            // 
            // MainWindow
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1593, 968);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainWindow";
            this.Text = "PhotoSelector";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            this.splitContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem ファイルFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menu_Open;
        private System.Windows.Forms.ToolStripMenuItem menu_Save;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem menu_ExecSorting;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem menu_Quit;
        private Controls.PhotoGrid photoGrid;
        private System.Windows.Forms.RadioButton rb_AllPictures;
        private System.Windows.Forms.RadioButton rb_OK;
        private System.Windows.Forms.RadioButton rb_NG;
        private Controls.PhotoGrid keepPhotoGrid;
        private System.Windows.Forms.Label lbl_KeepPhotos;
        private System.Windows.Forms.SplitContainer splitContainer;
    }
}