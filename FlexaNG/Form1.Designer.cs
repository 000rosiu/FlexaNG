namespace FlexaNG
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lb_settings = new System.Windows.Forms.Label();
            this.check_browsers = new System.Windows.Forms.CheckBox();
            this.check_makezip = new System.Windows.Forms.CheckBox();
            this.btn_proceed = new System.Windows.Forms.Button();
            this.lb_version = new System.Windows.Forms.Label();
            this.lbl_copyright = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.check_tree = new System.Windows.Forms.CheckBox();
            this.btn_proceed.Click += new System.EventHandler(this.btn_proceed_Click);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::FlexaNG.Properties.Resources.flexa_non;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(359, 71);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lb_settings
            // 
            this.lb_settings.AutoSize = true;
            this.lb_settings.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_settings.Location = new System.Drawing.Point(12, 74);
            this.lb_settings.Name = "lb_settings";
            this.lb_settings.Size = new System.Drawing.Size(67, 16);
            this.lb_settings.TabIndex = 1;
            this.lb_settings.Text = "Settings:";
            // 
            // check_browsers
            // 
            this.check_browsers.AutoSize = true;
            this.check_browsers.Location = new System.Drawing.Point(12, 116);
            this.check_browsers.Name = "check_browsers";
            this.check_browsers.Size = new System.Drawing.Size(340, 17);
            this.check_browsers.TabIndex = 2;
            this.check_browsers.Text = "Allow to read web browsers data (including passwords, history etc.)";
            this.check_browsers.UseVisualStyleBackColor = true;
            // 
            // check_makezip
            // 
            this.check_makezip.AutoSize = true;
            this.check_makezip.Location = new System.Drawing.Point(12, 93);
            this.check_makezip.Name = "check_makezip";
            this.check_makezip.Size = new System.Drawing.Size(201, 17);
            this.check_makezip.TabIndex = 3;
            this.check_makezip.Text = "Compress the log folder into a ZIP file";
            this.check_makezip.UseVisualStyleBackColor = true;
            // 
            // btn_proceed
            // 
            this.btn_proceed.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btn_proceed.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn_proceed.Location = new System.Drawing.Point(0, 212);
            this.btn_proceed.Name = "btn_proceed";
            this.btn_proceed.Size = new System.Drawing.Size(359, 55);
            this.btn_proceed.TabIndex = 4;
            this.btn_proceed.Text = "Proceed";
            this.btn_proceed.UseVisualStyleBackColor = true;
            // 
            // lb_version
            // 
            this.lb_version.AutoSize = true;
            this.lb_version.Location = new System.Drawing.Point(272, 191);
            this.lb_version.Name = "lb_version";
            this.lb_version.Size = new System.Drawing.Size(75, 13);
            this.lb_version.TabIndex = 5;
            this.lb_version.Text = "FlexaNG v.0.1";
            // 
            // lbl_copyright
            // 
            this.lbl_copyright.AutoSize = true;
            this.lbl_copyright.Location = new System.Drawing.Point(12, 191);
            this.lbl_copyright.Name = "lbl_copyright";
            this.lbl_copyright.Size = new System.Drawing.Size(127, 13);
            this.lbl_copyright.TabIndex = 6;
            this.lbl_copyright.Text = "©Verti-IML (IML Systems)";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 162);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(335, 23);
            this.progressBar1.TabIndex = 7;
            // 
            // check_tree
            // 
            this.check_tree.AutoSize = true;
            this.check_tree.Location = new System.Drawing.Point(12, 139);
            this.check_tree.Name = "check_tree";
            this.check_tree.Size = new System.Drawing.Size(165, 17);
            this.check_tree.TabIndex = 8;
            this.check_tree.Text = "Make TREE of entire C: drive";
            this.check_tree.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 267);
            this.Controls.Add(this.check_tree);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.lbl_copyright);
            this.Controls.Add(this.lb_version);
            this.Controls.Add(this.btn_proceed);
            this.Controls.Add(this.check_makezip);
            this.Controls.Add(this.check_browsers);
            this.Controls.Add(this.lb_settings);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "FlexaNG v0.1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lb_settings;
        private System.Windows.Forms.CheckBox check_browsers;
        private System.Windows.Forms.CheckBox check_makezip;
        private System.Windows.Forms.Button btn_proceed;
        private System.Windows.Forms.Label lb_version;
        private System.Windows.Forms.Label lbl_copyright;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox check_tree;
    }
}

