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
            this.label1 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(599, 187);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(135, 47);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // title_label
            // 
            this.title_label.AutoSize = true;
            this.title_label.Font = new System.Drawing.Font("Monoid", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.title_label.Location = new System.Drawing.Point(47, 29);
            this.title_label.Name = "title_label";
            this.title_label.Size = new System.Drawing.Size(283, 64);
            this.title_label.TabIndex = 1;
            this.title_label.Text = "Settings";
            this.title_label.Click += new System.EventHandler(this.title_label_Click);
            // 
            // Theme_label
            // 
            this.Theme_label.AutoSize = true;
            this.Theme_label.Font = new System.Drawing.Font("Monoid", 12F, System.Drawing.FontStyle.Bold);
            this.Theme_label.Location = new System.Drawing.Point(56, 190);
            this.Theme_label.Name = "Theme_label";
            this.Theme_label.Size = new System.Drawing.Size(94, 32);
            this.Theme_label.TabIndex = 2;
            this.Theme_label.Text = "Theme";
            // 
            // comboBox1
            // 
            this.comboBox1.Font = new System.Drawing.Font("Franklin Gothic Medium Cond", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Light (Not Avaliable)",
            "Dark",
            "Communist"});
            this.comboBox1.Location = new System.Drawing.Point(156, 194);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(313, 33);
            this.comboBox1.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Monoid", 12F, System.Drawing.FontStyle.Bold);
            this.label1.ForeColor = System.Drawing.Color.DarkKhaki;
            this.label1.Location = new System.Drawing.Point(124, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(110, 32);
            this.label1.TabIndex = 4;
            this.label1.Text = "(Beta)";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::easy14_isde.Properties.Resources.keep_calm_its_not_finished_yet;
            this.pictureBox1.Location = new System.Drawing.Point(237, 262);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(371, 320);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // settings_form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(809, 606);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.Theme_label);
            this.Controls.Add(this.title_label);
            this.Controls.Add(this.button1);
            this.Name = "settings_form";
            this.Text = "settings_form";
            this.Load += new System.EventHandler(this.settings_form_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label title_label;
        private System.Windows.Forms.Label Theme_label;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}