namespace Easy14_SE
{
    partial class SE_Window
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SE_Window));
            this.CodeEditorRTB = new System.Windows.Forms.RichTextBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.OpenBTN = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // CodeEditorRTB
            // 
            this.CodeEditorRTB.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.CodeEditorRTB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CodeEditorRTB.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.857143F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CodeEditorRTB.ForeColor = System.Drawing.Color.White;
            this.CodeEditorRTB.Location = new System.Drawing.Point(0, 0);
            this.CodeEditorRTB.Name = "CodeEditorRTB";
            this.CodeEditorRTB.ReadOnly = true;
            this.CodeEditorRTB.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CodeEditorRTB.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.CodeEditorRTB.Size = new System.Drawing.Size(1501, 894);
            this.CodeEditorRTB.TabIndex = 8;
            this.CodeEditorRTB.Text = "(Waiting for Easy14 Interpertor)";
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(28, 28);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenBTN,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1501, 38);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // OpenBTN
            // 
            this.OpenBTN.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.OpenBTN.Image = ((System.Drawing.Image)(resources.GetObject("OpenBTN.Image")));
            this.OpenBTN.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.OpenBTN.Name = "OpenBTN";
            this.OpenBTN.Size = new System.Drawing.Size(40, 32);
            this.OpenBTN.Text = "toolStripButton1";
            this.OpenBTN.Click += new System.EventHandler(this.OpenBTN_Click);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(40, 32);
            this.toolStripButton2.Text = "toolStripButton2";
            // 
            // SE_Window
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1501, 894);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.CodeEditorRTB);
            this.Name = "SE_Window";
            this.Text = "SE_Window";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox CodeEditorRTB;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton OpenBTN;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
    }
}