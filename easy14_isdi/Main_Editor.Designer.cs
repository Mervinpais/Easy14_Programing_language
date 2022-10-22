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
            this.code_text_area_rtb.Location = new System.Drawing.Point(12, 86);
            this.code_text_area_rtb.Name = "code_text_area_rtb";
            this.code_text_area_rtb.Size = new System.Drawing.Size(1238, 691);
            this.code_text_area_rtb.TabIndex = 0;
            this.code_text_area_rtb.Text = " ";
            this.code_text_area_rtb.TextChanged += new System.EventHandler(this.code_text_area_rtb_TextChanged);
            // 
            // run_code_btn
            // 
            this.run_code_btn.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.run_code_btn.Location = new System.Drawing.Point(12, 12);
            this.run_code_btn.Name = "run_code_btn";
            this.run_code_btn.Size = new System.Drawing.Size(125, 68);
            this.run_code_btn.TabIndex = 1;
            this.run_code_btn.Text = "Run";
            this.run_code_btn.UseVisualStyleBackColor = true;
            this.run_code_btn.Click += new System.EventHandler(this.run_code_btn_Click);
            // 
            // open_file_btn
            // 
            this.open_file_btn.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.open_file_btn.Location = new System.Drawing.Point(143, 12);
            this.open_file_btn.Name = "open_file_btn";
            this.open_file_btn.Size = new System.Drawing.Size(125, 68);
            this.open_file_btn.TabIndex = 2;
            this.open_file_btn.Text = "Open";
            this.open_file_btn.UseVisualStyleBackColor = true;
            this.open_file_btn.Click += new System.EventHandler(this.open_file_btn_Click);
            // 
            // settings_btn
            // 
            this.settings_btn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.settings_btn.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settings_btn.Location = new System.Drawing.Point(1086, 12);
            this.settings_btn.Name = "settings_btn";
            this.settings_btn.Size = new System.Drawing.Size(155, 68);
            this.settings_btn.TabIndex = 3;
            this.settings_btn.Text = "Settings";
            this.settings_btn.UseVisualStyleBackColor = true;
            this.settings_btn.Click += new System.EventHandler(this.settings_btn_Click);
            // 
            // save_file_btn
            // 
            this.save_file_btn.Font = new System.Drawing.Font("Microsoft YaHei UI", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.save_file_btn.Location = new System.Drawing.Point(274, 12);
            this.save_file_btn.Name = "save_file_btn";
            this.save_file_btn.Size = new System.Drawing.Size(125, 68);
            this.save_file_btn.TabIndex = 4;
            this.save_file_btn.Text = "Save";
            this.save_file_btn.UseVisualStyleBackColor = true;
            this.save_file_btn.Click += new System.EventHandler(this.save_file_btn_Click);
            // 
            // Main_Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.ClientSize = new System.Drawing.Size(1262, 789);
            this.Controls.Add(this.save_file_btn);
            this.Controls.Add(this.settings_btn);
            this.Controls.Add(this.open_file_btn);
            this.Controls.Add(this.run_code_btn);
            this.Controls.Add(this.code_text_area_rtb);
            this.Name = "Main_Editor";
            this.Text = "Easy14 Scripter";
            this.Load += new System.EventHandler(this.Main_Editor_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox code_text_area_rtb;
        private System.Windows.Forms.Button run_code_btn;
        private System.Windows.Forms.Button open_file_btn;
        private System.Windows.Forms.Button settings_btn;
        private System.Windows.Forms.Button save_file_btn;
    }
}

