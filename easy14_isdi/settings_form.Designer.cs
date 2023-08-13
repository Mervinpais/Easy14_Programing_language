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
            this.button1 = new System.Windows.Forms.Button();
            this.title_label = new System.Windows.Forms.Label();
            this.Theme_label = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.ReloadSettingsBTN = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Enabled = false;
            this.button1.Location = new System.Drawing.Point(654, 223);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(165, 57);
            this.button1.TabIndex = 0;
            this.button1.Text = "Apply Theme";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // title_label
            // 
            this.title_label.AutoSize = true;
            this.title_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title_label.Location = new System.Drawing.Point(58, 35);
            this.title_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.title_label.Name = "title_label";
            this.title_label.Size = new System.Drawing.Size(238, 64);
            this.title_label.TabIndex = 1;
            this.title_label.Text = "Settings";
            // 
            // Theme_label
            // 
            this.Theme_label.AutoSize = true;
            this.Theme_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.Theme_label.Location = new System.Drawing.Point(76, 232);
            this.Theme_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Theme_label.Name = "Theme_label";
            this.Theme_label.Size = new System.Drawing.Size(107, 32);
            this.Theme_label.TabIndex = 2;
            this.Theme_label.Text = "Theme";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Light (Not Avaliable)",
            "Dark"});
            this.comboBox1.Location = new System.Drawing.Point(191, 232);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(455, 37);
            this.comboBox1.TabIndex = 3;
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.MessageLabel.Location = new System.Drawing.Point(402, 374);
            this.MessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(41, 32);
            this.MessageLabel.TabIndex = 6;
            this.MessageLabel.Text = "...";
            // 
            // ReloadSettingsBTN
            // 
            this.ReloadSettingsBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ReloadSettingsBTN.Location = new System.Drawing.Point(304, 35);
            this.ReloadSettingsBTN.Margin = new System.Windows.Forms.Padding(4);
            this.ReloadSettingsBTN.Name = "ReloadSettingsBTN";
            this.ReloadSettingsBTN.Size = new System.Drawing.Size(139, 64);
            this.ReloadSettingsBTN.TabIndex = 7;
            this.ReloadSettingsBTN.Text = "Reload 🔄️";
            this.ReloadSettingsBTN.UseVisualStyleBackColor = true;
            this.ReloadSettingsBTN.Click += new System.EventHandler(this.ReloadSettingsBTN_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Red;
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(181, 232);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(638, 37);
            this.panel1.TabIndex = 8;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(158, -1);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(308, 32);
            this.label1.TabIndex = 0;
            this.label1.Text = "Disabled (Unfinished)";
            // 
            // settings_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 443);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ReloadSettingsBTN);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Theme_label);
            this.Controls.Add(this.title_label);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "settings_form";
            this.Text = "settings_form";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.settings_form_Paint);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label title_label;
        private System.Windows.Forms.Label Theme_label;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.Button ReloadSettingsBTN;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
    }
}