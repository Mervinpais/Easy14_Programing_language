namespace easy14_isde
{
    partial class Main_Editor
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
            this.code_text_area_rtb = new System.Windows.Forms.RichTextBox();
            this.run_code_btn = new System.Windows.Forms.Button();
            this.open_file_btn = new System.Windows.Forms.Button();
            this.settings_btn = new System.Windows.Forms.Button();
            this.save_file_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // code_text_area_rtb
            // 
            this.code_text_area_rtb.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.code_text_area_rtb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.code_text_area_rtb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.code_text_area_rtb.Font = new System.Drawing.Font("Lucida Console", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.code_text_area_rtb.ForeColor = System.Drawing.SystemColors.Window;
            this.code_text_area_rtb.Location = new System.Drawing.Point(15, 144);
            this.code_text_area_rtb.Margin = new System.Windows.Forms.Padding(4);
            this.code_text_area_rtb.Name = "code_text_area_rtb";
            this.code_text_area_rtb.Size = new System.Drawing.Size(1556, 890);
            this.code_text_area_rtb.TabIndex = 0;
            this.code_text_area_rtb.Text = " ";
            this.code_text_area_rtb.TextChanged += new System.EventHandler(this.code_text_area_rtb_TextChanged);
            // 
            // run_code_btn
            // 
            this.run_code_btn.BackColor = System.Drawing.Color.DimGray;
            this.run_code_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.run_code_btn.Cursor = System.Windows.Forms.Cursors.Default;
            this.run_code_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.run_code_btn.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.run_code_btn.ForeColor = System.Drawing.Color.Lime;
            this.run_code_btn.Location = new System.Drawing.Point(25, 58);
            this.run_code_btn.Margin = new System.Windows.Forms.Padding(4);
            this.run_code_btn.Name = "run_code_btn";
            this.run_code_btn.Size = new System.Drawing.Size(77, 69);
            this.run_code_btn.TabIndex = 1;
            this.run_code_btn.Text = "▶️";
            this.run_code_btn.UseVisualStyleBackColor = false;
            this.run_code_btn.Click += new System.EventHandler(this.run_code_btn_Click);
            // 
            // open_file_btn
            // 
            this.open_file_btn.BackColor = System.Drawing.Color.DimGray;
            this.open_file_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.open_file_btn.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_file_btn.ForeColor = System.Drawing.Color.Gold;
            this.open_file_btn.Location = new System.Drawing.Point(110, 57);
            this.open_file_btn.Margin = new System.Windows.Forms.Padding(4);
            this.open_file_btn.Name = "open_file_btn";
            this.open_file_btn.Size = new System.Drawing.Size(78, 69);
            this.open_file_btn.TabIndex = 2;
            this.open_file_btn.Text = "📂";
            this.open_file_btn.UseVisualStyleBackColor = false;
            this.open_file_btn.Click += new System.EventHandler(this.open_file_btn_Click);
            // 
            // settings_btn
            // 
            this.settings_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settings_btn.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settings_btn.Location = new System.Drawing.Point(1373, 58);
            this.settings_btn.Margin = new System.Windows.Forms.Padding(4);
            this.settings_btn.Name = "settings_btn";
            this.settings_btn.Size = new System.Drawing.Size(189, 69);
            this.settings_btn.TabIndex = 4;
            this.settings_btn.Text = "Settings";
            this.settings_btn.UseVisualStyleBackColor = true;
            this.settings_btn.Click += new System.EventHandler(this.settings_btn_Click);
            // 
            // save_file_btn
            // 
            this.save_file_btn.BackColor = System.Drawing.Color.DimGray;
            this.save_file_btn.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.save_file_btn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.save_file_btn.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save_file_btn.ForeColor = System.Drawing.Color.DarkTurquoise;
            this.save_file_btn.Location = new System.Drawing.Point(205, 58);
            this.save_file_btn.Margin = new System.Windows.Forms.Padding(4);
            this.save_file_btn.Name = "save_file_btn";
            this.save_file_btn.Size = new System.Drawing.Size(75, 69);
            this.save_file_btn.TabIndex = 3;
            this.save_file_btn.Text = "💾";
            this.save_file_btn.UseVisualStyleBackColor = false;
            this.save_file_btn.Click += new System.EventHandler(this.save_file_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(786, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(580, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Themes have been disabled for being unfinished until next update\r\n";
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1585, 38);
            this.menuStrip1.TabIndex = 6;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.recentToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(62, 34);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(315, 40);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(315, 40);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // recentToolStripMenuItem
            // 
            this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
            this.recentToolStripMenuItem.Size = new System.Drawing.Size(315, 40);
            this.recentToolStripMenuItem.Text = "Recent";
            // 
            // Main_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1585, 1049);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.save_file_btn);
            this.Controls.Add(this.settings_btn);
            this.Controls.Add(this.open_file_btn);
            this.Controls.Add(this.run_code_btn);
            this.Controls.Add(this.code_text_area_rtb);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main_Editor";
            this.Text = "Easy14 Scripter";
            this.Load += new System.EventHandler(this.Main_Editor_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Main_Editor_Paint);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox code_text_area_rtb;
        private System.Windows.Forms.Button run_code_btn;
        private System.Windows.Forms.Button open_file_btn;
        private System.Windows.Forms.Button settings_btn;
        private System.Windows.Forms.Button save_file_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
    }
}

