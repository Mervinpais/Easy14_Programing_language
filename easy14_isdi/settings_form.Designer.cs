namespace easy14_isde
{
    partial class settings_form
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
            this.title_label = new System.Windows.Forms.Label();
            this.Theme_label = new System.Windows.Forms.Label();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.ReloadSettingsBTN = new System.Windows.Forms.Button();
            this.changeFontBTN = new System.Windows.Forms.Button();
            this.fontTest_label = new System.Windows.Forms.Label();
            this.EditorFont_label = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.changeThemeBTN = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // title_label
            // 
            this.title_label.AutoSize = true;
            this.title_label.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title_label.Location = new System.Drawing.Point(58, 35);
            this.title_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.title_label.Name = "title_label";
            this.title_label.Size = new System.Drawing.Size(278, 70);
            this.title_label.TabIndex = 1;
            this.title_label.Text = "Settings ⚙️";
            // 
            // Theme_label
            // 
            this.Theme_label.AutoSize = true;
            this.Theme_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.Theme_label.Location = new System.Drawing.Point(76, 145);
            this.Theme_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Theme_label.Name = "Theme_label";
            this.Theme_label.Size = new System.Drawing.Size(107, 32);
            this.Theme_label.TabIndex = 2;
            this.Theme_label.Text = "Theme";
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.MessageLabel.Location = new System.Drawing.Point(13, 458);
            this.MessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(41, 32);
            this.MessageLabel.TabIndex = 6;
            this.MessageLabel.Text = "...";
            // 
            // ReloadSettingsBTN
            // 
            this.ReloadSettingsBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ReloadSettingsBTN.Location = new System.Drawing.Point(866, 41);
            this.ReloadSettingsBTN.Margin = new System.Windows.Forms.Padding(4);
            this.ReloadSettingsBTN.Name = "ReloadSettingsBTN";
            this.ReloadSettingsBTN.Size = new System.Drawing.Size(172, 64);
            this.ReloadSettingsBTN.TabIndex = 7;
            this.ReloadSettingsBTN.Text = "Reload 🔄️";
            this.ReloadSettingsBTN.UseVisualStyleBackColor = true;
            this.ReloadSettingsBTN.Click += new System.EventHandler(this.ReloadSettingsBTN_Click);
            // 
            // changeFontBTN
            // 
            this.changeFontBTN.Location = new System.Drawing.Point(82, 379);
            this.changeFontBTN.Name = "changeFontBTN";
            this.changeFontBTN.Size = new System.Drawing.Size(156, 48);
            this.changeFontBTN.TabIndex = 9;
            this.changeFontBTN.Text = "Change";
            this.changeFontBTN.UseVisualStyleBackColor = true;
            this.changeFontBTN.Click += new System.EventHandler(this.changeFontBTN_Click);
            // 
            // fontTest_label
            // 
            this.fontTest_label.AutoSize = true;
            this.fontTest_label.Location = new System.Drawing.Point(287, 326);
            this.fontTest_label.Name = "fontTest_label";
            this.fontTest_label.Size = new System.Drawing.Size(198, 50);
            this.fontTest_label.TabIndex = 10;
            this.fontTest_label.Text = "The brown fox jumps \r\nover the lazy dog";
            // 
            // EditorFont_label
            // 
            this.EditorFont_label.AutoSize = true;
            this.EditorFont_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.EditorFont_label.Location = new System.Drawing.Point(76, 326);
            this.EditorFont_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.EditorFont_label.Name = "EditorFont_label";
            this.EditorFont_label.Size = new System.Drawing.Size(173, 32);
            this.EditorFont_label.TabIndex = 11;
            this.EditorFont_label.Text = "Editor Font:";
            // 
            // comboBox1
            // 
            this.comboBox1.Enabled = false;
            this.comboBox1.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Light (Not Avaliable)",
            "Dark"});
            this.comboBox1.Location = new System.Drawing.Point(208, 145);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(455, 37);
            this.comboBox1.TabIndex = 3;
            // 
            // changeThemeBTN
            // 
            this.changeThemeBTN.Enabled = false;
            this.changeThemeBTN.Location = new System.Drawing.Point(686, 135);
            this.changeThemeBTN.Margin = new System.Windows.Forms.Padding(4);
            this.changeThemeBTN.Name = "changeThemeBTN";
            this.changeThemeBTN.Size = new System.Drawing.Size(165, 57);
            this.changeThemeBTN.TabIndex = 0;
            this.changeThemeBTN.Text = "Apply Theme";
            this.changeThemeBTN.UseVisualStyleBackColor = true;
            this.changeThemeBTN.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Red;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(227, 144);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(404, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "      Disabled (Unfinished)      ";
            // 
            // settings_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(1072, 499);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.EditorFont_label);
            this.Controls.Add(this.changeThemeBTN);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.fontTest_label);
            this.Controls.Add(this.changeFontBTN);
            this.Controls.Add(this.ReloadSettingsBTN);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.Theme_label);
            this.Controls.Add(this.title_label);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "settings_form";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.settings_form_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.settings_form_Paint);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label title_label;
        private System.Windows.Forms.Label Theme_label;
        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.Button ReloadSettingsBTN;
        private System.Windows.Forms.Button changeFontBTN;
        private System.Windows.Forms.Label fontTest_label;
        private System.Windows.Forms.Label EditorFont_label;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button changeThemeBTN;
        private System.Windows.Forms.Label label1;
    }
}