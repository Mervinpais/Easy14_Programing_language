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
            this.CodeEditorArea_rtb = new System.Windows.Forms.RichTextBox();
            this.run_code_btn = new System.Windows.Forms.Button();
            this.open_file_btn = new System.Windows.Forms.Button();
            this.settings_btn = new System.Windows.Forms.Button();
            this.save_file_btn = new System.Windows.Forms.Button();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wordWrapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OutputRTB = new System.Windows.Forms.RichTextBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.actionLB = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CodeEditorArea_rtb
            // 
            this.CodeEditorArea_rtb.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.CodeEditorArea_rtb.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.CodeEditorArea_rtb.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CodeEditorArea_rtb.Font = new System.Drawing.Font("Lucida Console", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CodeEditorArea_rtb.ForeColor = System.Drawing.SystemColors.Window;
            this.CodeEditorArea_rtb.Location = new System.Drawing.Point(0, 0);
            this.CodeEditorArea_rtb.Margin = new System.Windows.Forms.Padding(4);
            this.CodeEditorArea_rtb.Name = "CodeEditorArea_rtb";
            this.CodeEditorArea_rtb.Size = new System.Drawing.Size(782, 817);
            this.CodeEditorArea_rtb.TabIndex = 0;
            this.CodeEditorArea_rtb.Text = " ";
            this.CodeEditorArea_rtb.TextChanged += new System.EventHandler(this.code_text_area_rtb_TextChanged);
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
            // menuStrip1
            // 
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 8.142858F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.editToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1585, 36);
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
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(60, 32);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(187, 40);
            this.openToolStripMenuItem.Text = "Open";
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(187, 40);
            this.saveToolStripMenuItem.Text = "Save";
            // 
            // recentToolStripMenuItem
            // 
            this.recentToolStripMenuItem.Name = "recentToolStripMenuItem";
            this.recentToolStripMenuItem.Size = new System.Drawing.Size(187, 40);
            this.recentToolStripMenuItem.Text = "Recent";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wordWrapToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(64, 32);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // wordWrapToolStripMenuItem
            // 
            this.wordWrapToolStripMenuItem.Name = "wordWrapToolStripMenuItem";
            this.wordWrapToolStripMenuItem.Size = new System.Drawing.Size(315, 40);
            this.wordWrapToolStripMenuItem.Text = "Word Wrap";
            this.wordWrapToolStripMenuItem.Click += new System.EventHandler(this.wordWrapToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(71, 32);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(184, 40);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // OutputRTB
            // 
            this.OutputRTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.OutputRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OutputRTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OutputRTB.ForeColor = System.Drawing.Color.White;
            this.OutputRTB.Location = new System.Drawing.Point(0, 0);
            this.OutputRTB.Name = "OutputRTB";
            this.OutputRTB.ReadOnly = true;
            this.OutputRTB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.OutputRTB.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.OutputRTB.Size = new System.Drawing.Size(762, 817);
            this.OutputRTB.TabIndex = 7;
            this.OutputRTB.Text = "(Waiting for Easy14 Interpertor)";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(25, 134);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.CodeEditorArea_rtb);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.OutputRTB);
            this.splitContainer1.Size = new System.Drawing.Size(1548, 817);
            this.splitContainer1.SplitterDistance = 782;
            this.splitContainer1.TabIndex = 8;
            // 
            // actionLB
            // 
            this.actionLB.AutoSize = true;
            this.actionLB.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.actionLB.ForeColor = System.Drawing.Color.White;
            this.actionLB.Location = new System.Drawing.Point(20, 969);
            this.actionLB.Name = "actionLB";
            this.actionLB.Size = new System.Drawing.Size(84, 32);
            this.actionLB.TabIndex = 9;
            this.actionLB.Text = "(Idle)";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.142858F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(459, 57);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(644, 25);
            this.label1.TabIndex = 5;
            this.label1.Text = "Themes have been disabled for being unfinished until next update\r\n";
            // 
            // Main_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(1585, 1025);
            this.Controls.Add(this.actionLB);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.save_file_btn);
            this.Controls.Add(this.settings_btn);
            this.Controls.Add(this.open_file_btn);
            this.Controls.Add(this.run_code_btn);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Main_Editor";
            this.Text = "Easy14 Scripter";
            this.Load += new System.EventHandler(this.Main_Editor_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Main_Editor_Paint);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox CodeEditorArea_rtb;
        private System.Windows.Forms.Button run_code_btn;
        private System.Windows.Forms.Button open_file_btn;
        private System.Windows.Forms.Button settings_btn;
        private System.Windows.Forms.Button save_file_btn;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem recentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wordWrapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.RichTextBox OutputRTB;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Label actionLB;
        private System.Windows.Forms.Label label1;
    }
}

