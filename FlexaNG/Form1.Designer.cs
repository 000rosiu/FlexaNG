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
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.check_tree = new System.Windows.Forms.CheckBox();
            this.lb_current = new System.Windows.Forms.Label();
            this.panel_logo = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel_logo.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_logo
            // 
            this.panel_logo.BackColor = System.Drawing.Color.White;
            this.panel_logo.Controls.Add(this.pictureBox1);
            this.panel_logo.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_logo.Location = new System.Drawing.Point(0, 0);
            this.panel_logo.Name = "panel_logo";
            this.panel_logo.Padding = new System.Windows.Forms.Padding(20, 10, 20, 10);
            this.panel_logo.Size = new System.Drawing.Size(500, 100);
            this.panel_logo.TabIndex = 10;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::FlexaNG.Properties.Resources.flexa_non;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Location = new System.Drawing.Point(20, 10);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(460, 80);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // lb_settings
            // 
            this.lb_settings.AutoSize = true;
            this.lb_settings.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_settings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(87)))), ((int)(((byte)(193)))));
            this.lb_settings.Location = new System.Drawing.Point(25, 120);
            this.lb_settings.Name = "lb_settings";
            this.lb_settings.Size = new System.Drawing.Size(83, 20);
            this.lb_settings.TabIndex = 1;
            this.lb_settings.Text = "⚙ Settings";
            // 
            // check_makezip
            // 
            this.check_makezip.AutoSize = true;
            this.check_makezip.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.check_makezip.ForeColor = System.Drawing.Color.White;
            this.check_makezip.Location = new System.Drawing.Point(29, 155);
            this.check_makezip.Name = "check_makezip";
            this.check_makezip.Size = new System.Drawing.Size(218, 19);
            this.check_makezip.TabIndex = 3;
            this.check_makezip.Text = "📦 Compress logs into a ZIP archive";
            this.check_makezip.UseVisualStyleBackColor = true;
            // 
            // check_browsers
            // 
            this.check_browsers.AutoSize = true;
            this.check_browsers.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.check_browsers.ForeColor = System.Drawing.Color.White;
            this.check_browsers.Location = new System.Drawing.Point(29, 180);
            this.check_browsers.Name = "check_browsers";
            this.check_browsers.Size = new System.Drawing.Size(348, 19);
            this.check_browsers.TabIndex = 2;
            this.check_browsers.Text = "🌐 Collect browser data (bookmarks, passwords, history)";
            this.check_browsers.UseVisualStyleBackColor = true;
            this.check_browsers.CheckedChanged += new System.EventHandler(this.check_browsers_CheckedChanged);
            // 
            // check_tree
            // 
            this.check_tree.AutoSize = true;
            this.check_tree.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.check_tree.ForeColor = System.Drawing.Color.White;
            this.check_tree.Location = new System.Drawing.Point(29, 205);
            this.check_tree.Name = "check_tree";
            this.check_tree.Size = new System.Drawing.Size(204, 19);
            this.check_tree.TabIndex = 8;
            this.check_tree.Text = "📁 Generate C:\\ directory tree";
            this.check_tree.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(29, 250);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(442, 28);
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressBar1.TabIndex = 7;
            // 
            // lb_current
            // 
            this.lb_current.AutoSize = true;
            this.lb_current.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_current.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.lb_current.Location = new System.Drawing.Point(29, 285);
            this.lb_current.Name = "lb_current";
            this.lb_current.Size = new System.Drawing.Size(67, 15);
            this.lb_current.TabIndex = 9;
            this.lb_current.Text = "⏳ Waiting...";
            // 
            // btn_proceed
            // 
            this.btn_proceed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(87)))), ((int)(((byte)(193)))));
            this.btn_proceed.FlatAppearance.BorderSize = 0;
            this.btn_proceed.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(67)))), ((int)(((byte)(173)))));
            this.btn_proceed.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(120)))), ((int)(((byte)(107)))), ((int)(((byte)(213)))));
            this.btn_proceed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_proceed.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.btn_proceed.ForeColor = System.Drawing.Color.White;
            this.btn_proceed.Location = new System.Drawing.Point(29, 320);
            this.btn_proceed.Name = "btn_proceed";
            this.btn_proceed.Size = new System.Drawing.Size(442, 50);
            this.btn_proceed.TabIndex = 4;
            this.btn_proceed.Text = "▶ Proceed";
            this.btn_proceed.UseVisualStyleBackColor = false;
            this.btn_proceed.Click += new System.EventHandler(this.btn_proceed_Click);
            // 
            // lb_version
            // 
            this.lb_version.AutoSize = true;
            this.lb_version.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.lb_version.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(100)))));
            this.lb_version.Location = new System.Drawing.Point(402, 380);
            this.lb_version.Name = "lb_version";
            this.lb_version.Size = new System.Drawing.Size(86, 13);
            this.lb_version.TabIndex = 5;
            this.lb_version.Text = "FlexaNG v0.4";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ClientSize = new System.Drawing.Size(500, 400);
            this.Controls.Add(this.lb_version);
            this.Controls.Add(this.btn_proceed);
            this.Controls.Add(this.lb_current);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.check_tree);
            this.Controls.Add(this.check_browsers);
            this.Controls.Add(this.check_makezip);
            this.Controls.Add(this.lb_settings);
            this.Controls.Add(this.panel_logo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FlexaNG v0.4 - Device information grabbing tool";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel_logo.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel_logo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lb_settings;
        private System.Windows.Forms.CheckBox check_browsers;
        private System.Windows.Forms.CheckBox check_makezip;
        private System.Windows.Forms.Button btn_proceed;
        private System.Windows.Forms.Label lb_version;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.CheckBox check_tree;
        private System.Windows.Forms.Label lb_current;
    }
}

