namespace SharpColossus
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.label1 = new System.Windows.Forms.Label();
            this.sotc_path = new System.Windows.Forms.ComboBox();
            this.path_state = new System.Windows.Forms.Label();
            this.open_obj = new System.Windows.Forms.Button();
            this.label_disc_type = new System.Windows.Forms.Label();
            this.fyle_system_group = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.autoLoadBtn = new System.Windows.Forms.Button();
            this.colossusout = new System.Windows.Forms.Button();
            this.miscout = new System.Windows.Forms.Button();
            this.advancedout = new System.Windows.Forms.Button();
            this.creditsout = new System.Windows.Forms.Button();
            this.fyle_system_group.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(401, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 15);
            this.label1.TabIndex = 11;
            this.label1.Text = "Path to NICO.dat";
            // 
            // sotc_path
            // 
            this.sotc_path.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.sotc_path.FormattingEnabled = true;
            this.sotc_path.Items.AddRange(new object[] {
            "Browse"});
            this.sotc_path.Location = new System.Drawing.Point(148, 23);
            this.sotc_path.Name = "sotc_path";
            this.sotc_path.Size = new System.Drawing.Size(350, 21);
            this.sotc_path.TabIndex = 12;
            this.sotc_path.SelectedIndexChanged += new System.EventHandler(this.sotc_SelectedIndexChanged);
            // 
            // path_state
            // 
            this.path_state.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.path_state.AutoSize = true;
            this.path_state.BackColor = System.Drawing.SystemColors.Control;
            this.path_state.ForeColor = System.Drawing.Color.DarkRed;
            this.path_state.Location = new System.Drawing.Point(425, 47);
            this.path_state.Name = "path_state";
            this.path_state.Size = new System.Drawing.Size(73, 13);
            this.path_state.TabIndex = 13;
            this.path_state.Text = "CHANGING...";
            this.path_state.Visible = false;
            // 
            // open_obj
            // 
            this.open_obj.Location = new System.Drawing.Point(372, 150);
            this.open_obj.Name = "open_obj";
            this.open_obj.Size = new System.Drawing.Size(126, 23);
            this.open_obj.TabIndex = 15;
            this.open_obj.Text = "View exported 3D file";
            this.open_obj.UseVisualStyleBackColor = true;
            this.open_obj.Click += new System.EventHandler(this.button1_Click);
            // 
            // label_disc_type
            // 
            this.label_disc_type.AutoSize = true;
            this.label_disc_type.BackColor = System.Drawing.Color.Transparent;
            this.label_disc_type.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label_disc_type.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.label_disc_type.Location = new System.Drawing.Point(10, 16);
            this.label_disc_type.Name = "label_disc_type";
            this.label_disc_type.Size = new System.Drawing.Size(117, 20);
            this.label_disc_type.TabIndex = 16;
            this.label_disc_type.Text = "No data loaded";
            // 
            // fyle_system_group
            // 
            this.fyle_system_group.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.fyle_system_group.AutoSize = true;
            this.fyle_system_group.Controls.Add(this.label_disc_type);
            this.fyle_system_group.Location = new System.Drawing.Point(9, 5);
            this.fyle_system_group.Name = "fyle_system_group";
            this.fyle_system_group.Size = new System.Drawing.Size(133, 52);
            this.fyle_system_group.TabIndex = 17;
            this.fyle_system_group.TabStop = false;
            this.fyle_system_group.Text = "Game version";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(372, 121);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(126, 23);
            this.button1.TabIndex = 18;
            this.button1.Text = "Game version info";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // autoLoadBtn
            // 
            this.autoLoadBtn.Location = new System.Drawing.Point(424, 63);
            this.autoLoadBtn.Name = "autoLoadBtn";
            this.autoLoadBtn.Size = new System.Drawing.Size(74, 23);
            this.autoLoadBtn.TabIndex = 22;
            this.autoLoadBtn.Text = "&AutoLoad";
            this.autoLoadBtn.UseVisualStyleBackColor = true;
            this.autoLoadBtn.Click += new System.EventHandler(this.autoLoadBtn_Click);
            // 
            // colossusout
            // 
            this.colossusout.Location = new System.Drawing.Point(9, 63);
            this.colossusout.Name = "colossusout";
            this.colossusout.Size = new System.Drawing.Size(75, 23);
            this.colossusout.TabIndex = 23;
            this.colossusout.Text = "Colossus";
            this.colossusout.UseVisualStyleBackColor = true;
            this.colossusout.Click += new System.EventHandler(this.colossusout_Click);
            // 
            // miscout
            // 
            this.miscout.Location = new System.Drawing.Point(9, 92);
            this.miscout.Name = "miscout";
            this.miscout.Size = new System.Drawing.Size(75, 23);
            this.miscout.TabIndex = 24;
            this.miscout.Text = "Misc.";
            this.miscout.UseVisualStyleBackColor = true;
            this.miscout.Click += new System.EventHandler(this.miscout_Click);
            // 
            // advancedout
            // 
            this.advancedout.Location = new System.Drawing.Point(9, 121);
            this.advancedout.Name = "advancedout";
            this.advancedout.Size = new System.Drawing.Size(75, 23);
            this.advancedout.TabIndex = 25;
            this.advancedout.Text = "Advanced";
            this.advancedout.UseVisualStyleBackColor = true;
            this.advancedout.Click += new System.EventHandler(this.advancedout_Click);
            // 
            // creditsout
            // 
            this.creditsout.Location = new System.Drawing.Point(9, 150);
            this.creditsout.Name = "creditsout";
            this.creditsout.Size = new System.Drawing.Size(75, 23);
            this.creditsout.TabIndex = 26;
            this.creditsout.Text = "Credits";
            this.creditsout.UseVisualStyleBackColor = true;
            this.creditsout.Click += new System.EventHandler(this.creditsout_Click);
            // 
            // Main
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(510, 185);
            this.Controls.Add(this.creditsout);
            this.Controls.Add(this.advancedout);
            this.Controls.Add(this.miscout);
            this.Controls.Add(this.colossusout);
            this.Controls.Add(this.autoLoadBtn);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.fyle_system_group);
            this.Controls.Add(this.open_obj);
            this.Controls.Add(this.path_state);
            this.Controls.Add(this.sotc_path);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Main";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SotC Viewer";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.fyle_system_group.ResumeLayout(false);
            this.fyle_system_group.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button open_obj;
        private System.Windows.Forms.GroupBox fyle_system_group;
        private System.Windows.Forms.Label label_disc_type;
        protected System.Windows.Forms.Label label1;
        protected System.Windows.Forms.ComboBox sotc_path;
        protected System.Windows.Forms.Label path_state;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button autoLoadBtn;
        private System.Windows.Forms.Button colossusout;
        private System.Windows.Forms.Button miscout;
        private System.Windows.Forms.Button advancedout;
        private System.Windows.Forms.Button creditsout;
    }
}

