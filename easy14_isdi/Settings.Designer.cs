namespace Easy14_SE
{
    partial class Settings
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
            this.ColorSelector = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.testText1 = new System.Windows.Forms.Label();
            this.EditorFont_label = new System.Windows.Forms.Label();
            this.changeThemeBTN = new System.Windows.Forms.Button();
            this.ThemeItemSelector = new System.Windows.Forms.ComboBox();
            this.fontTest_label = new System.Windows.Forms.Label();
            this.changeFontBTN = new System.Windows.Forms.Button();
            this.ReloadSettingsBTN = new System.Windows.Forms.Button();
            this.MessageLabel = new System.Windows.Forms.Label();
            this.Theme_label = new System.Windows.Forms.Label();
            this.title_label = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ColorSelector
            // 
            this.ColorSelector.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ColorSelector.FormattingEnabled = true;
            this.ColorSelector.Items.AddRange(new object[] {
            "Red",
            "Blue",
            "Green",
            "White",
            "Black",
            "DarkGray"});
            this.ColorSelector.Location = new System.Drawing.Point(567, 75);
            this.ColorSelector.Margin = new System.Windows.Forms.Padding(4);
            this.ColorSelector.Name = "ColorSelector";
            this.ColorSelector.Size = new System.Drawing.Size(168, 37);
            this.ColorSelector.TabIndex = 24;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.testText1);
            this.panel1.Location = new System.Drawing.Point(366, 132);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(681, 317);
            this.panel1.TabIndex = 23;
            // 
            // testText1
            // 
            this.testText1.AutoSize = true;
            this.testText1.Location = new System.Drawing.Point(40, 24);
            this.testText1.Name = "testText1";
            this.testText1.Size = new System.Drawing.Size(158, 25);
            this.testText1.TabIndex = 0;
            this.testText1.Text = "This is some text";
            // 
            // EditorFont_label
            // 
            this.EditorFont_label.AutoSize = true;
            this.EditorFont_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.EditorFont_label.Location = new System.Drawing.Point(89, 112);
            this.EditorFont_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.EditorFont_label.Name = "EditorFont_label";
            this.EditorFont_label.Size = new System.Drawing.Size(173, 32);
            this.EditorFont_label.TabIndex = 22;
            this.EditorFont_label.Text = "Editor Font:";
            // 
            // changeThemeBTN
            // 
            this.changeThemeBTN.Location = new System.Drawing.Point(689, 3);
            this.changeThemeBTN.Margin = new System.Windows.Forms.Padding(4);
            this.changeThemeBTN.Name = "changeThemeBTN";
            this.changeThemeBTN.Size = new System.Drawing.Size(165, 57);
            this.changeThemeBTN.TabIndex = 14;
            this.changeThemeBTN.Text = "Apply Theme";
            this.changeThemeBTN.UseVisualStyleBackColor = true;
            // 
            // ThemeItemSelector
            // 
            this.ThemeItemSelector.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ThemeItemSelector.FormattingEnabled = true;
            this.ThemeItemSelector.Items.AddRange(new object[] {
            "BackGround",
            "ForeGround"});
            this.ThemeItemSelector.Location = new System.Drawing.Point(756, 75);
            this.ThemeItemSelector.Margin = new System.Windows.Forms.Padding(4);
            this.ThemeItemSelector.Name = "ThemeItemSelector";
            this.ThemeItemSelector.Size = new System.Drawing.Size(290, 37);
            this.ThemeItemSelector.TabIndex = 17;
            // 
            // fontTest_label
            // 
            this.fontTest_label.AutoSize = true;
            this.fontTest_label.Location = new System.Drawing.Point(53, 370);
            this.fontTest_label.Name = "fontTest_label";
            this.fontTest_label.Size = new System.Drawing.Size(198, 50);
            this.fontTest_label.TabIndex = 21;
            this.fontTest_label.Text = "The brown fox jumps \r\nover the lazy dog";
            // 
            // changeFontBTN
            // 
            this.changeFontBTN.Location = new System.Drawing.Point(95, 174);
            this.changeFontBTN.Name = "changeFontBTN";
            this.changeFontBTN.Size = new System.Drawing.Size(156, 48);
            this.changeFontBTN.TabIndex = 20;
            this.changeFontBTN.Text = "Change";
            this.changeFontBTN.UseVisualStyleBackColor = true;
            // 
            // ReloadSettingsBTN
            // 
            this.ReloadSettingsBTN.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.ReloadSettingsBTN.Location = new System.Drawing.Point(874, 3);
            this.ReloadSettingsBTN.Margin = new System.Windows.Forms.Padding(4);
            this.ReloadSettingsBTN.Name = "ReloadSettingsBTN";
            this.ReloadSettingsBTN.Size = new System.Drawing.Size(172, 64);
            this.ReloadSettingsBTN.TabIndex = 19;
            this.ReloadSettingsBTN.Text = "Reload 🔄️";
            this.ReloadSettingsBTN.UseVisualStyleBackColor = true;
            // 
            // MessageLabel
            // 
            this.MessageLabel.AutoSize = true;
            this.MessageLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.MessageLabel.Location = new System.Drawing.Point(11, 420);
            this.MessageLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.MessageLabel.Name = "MessageLabel";
            this.MessageLabel.Size = new System.Drawing.Size(41, 32);
            this.MessageLabel.TabIndex = 18;
            this.MessageLabel.Text = "...";
            // 
            // Theme_label
            // 
            this.Theme_label.AutoSize = true;
            this.Theme_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold);
            this.Theme_label.Location = new System.Drawing.Point(574, 18);
            this.Theme_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Theme_label.Name = "Theme_label";
            this.Theme_label.Size = new System.Drawing.Size(107, 32);
            this.Theme_label.TabIndex = 16;
            this.Theme_label.Text = "Theme";
            // 
            // title_label
            // 
            this.title_label.AutoSize = true;
            this.title_label.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title_label.Location = new System.Drawing.Point(56, -3);
            this.title_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.title_label.Name = "title_label";
            this.title_label.Size = new System.Drawing.Size(278, 70);
            this.title_label.TabIndex = 15;
            this.title_label.Text = "Settings ⚙️";
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1194, 581);
            this.Controls.Add(this.ColorSelector);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.EditorFont_label);
            this.Controls.Add(this.changeThemeBTN);
            this.Controls.Add(this.ThemeItemSelector);
            this.Controls.Add(this.fontTest_label);
            this.Controls.Add(this.changeFontBTN);
            this.Controls.Add(this.ReloadSettingsBTN);
            this.Controls.Add(this.MessageLabel);
            this.Controls.Add(this.Theme_label);
            this.Controls.Add(this.title_label);
            this.Name = "Settings";
            this.Text = "Settings";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ColorSelector;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label testText1;
        private System.Windows.Forms.Label EditorFont_label;
        private System.Windows.Forms.Button changeThemeBTN;
        private System.Windows.Forms.ComboBox ThemeItemSelector;
        private System.Windows.Forms.Label fontTest_label;
        private System.Windows.Forms.Button changeFontBTN;
        private System.Windows.Forms.Button ReloadSettingsBTN;
        private System.Windows.Forms.Label MessageLabel;
        private System.Windows.Forms.Label Theme_label;
        private System.Windows.Forms.Label title_label;
    }
}