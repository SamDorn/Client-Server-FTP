namespace Client_FTP
{
    partial class Form1
    {
        /// <summary>
        /// Variabile di progettazione necessaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Pulire le risorse in uso.
        /// </summary>
        /// <param name="disposing">ha valore true se le risorse gestite devono essere eliminate, false in caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Codice generato da Progettazione Windows Form

        /// <summary>
        /// Metodo necessario per il supporto della finestra di progettazione. Non modificare
        /// il contenuto del metodo con l'editor di codice.
        /// </summary>
        private void InitializeComponent()
        {
            this.txt_ip = new System.Windows.Forms.TextBox();
            this.txt_port = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btm_start = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_box_path = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.prg_bar = new System.Windows.Forms.ProgressBar();
            this.btn_upload = new System.Windows.Forms.Button();
            this.btn_help_upload = new System.Windows.Forms.Button();
            this.list_box_files = new System.Windows.Forms.ListBox();
            this.btn_list = new System.Windows.Forms.Button();
            this.btn_help_visualizza = new System.Windows.Forms.Button();
            this.btn_download = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_percorso = new System.Windows.Forms.Button();
            this.btn_stop = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btn_help_server = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.label8 = new System.Windows.Forms.Label();
            this.btn_help_progress = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_ip
            // 
            this.txt_ip.Location = new System.Drawing.Point(15, 45);
            this.txt_ip.Name = "txt_ip";
            this.txt_ip.Size = new System.Drawing.Size(109, 20);
            this.txt_ip.TabIndex = 0;
            this.txt_ip.Text = "127.0.0.1";
            // 
            // txt_port
            // 
            this.txt_port.Location = new System.Drawing.Point(127, 45);
            this.txt_port.Name = "txt_port";
            this.txt_port.Size = new System.Drawing.Size(36, 20);
            this.txt_port.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(124, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(26, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Port";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "IP";
            // 
            // btm_start
            // 
            this.btm_start.Location = new System.Drawing.Point(169, 42);
            this.btm_start.Name = "btm_start";
            this.btm_start.Size = new System.Drawing.Size(75, 23);
            this.btm_start.TabIndex = 4;
            this.btm_start.Text = "Start";
            this.btm_start.UseVisualStyleBackColor = true;
            this.btm_start.Click += new System.EventHandler(this.btm_start_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(103, 20);
            this.label3.TabIndex = 5;
            this.label3.Text = "Connession";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(8, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Upload/Download";
            // 
            // txt_box_path
            // 
            this.txt_box_path.Enabled = false;
            this.txt_box_path.Location = new System.Drawing.Point(12, 127);
            this.txt_box_path.Name = "txt_box_path";
            this.txt_box_path.Size = new System.Drawing.Size(151, 20);
            this.txt_box_path.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(47, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "File path";
            // 
            // prg_bar
            // 
            this.prg_bar.Location = new System.Drawing.Point(12, 430);
            this.prg_bar.Name = "prg_bar";
            this.prg_bar.Size = new System.Drawing.Size(313, 23);
            this.prg_bar.TabIndex = 10;
            // 
            // btn_upload
            // 
            this.btn_upload.Enabled = false;
            this.btn_upload.Location = new System.Drawing.Point(250, 124);
            this.btn_upload.Name = "btn_upload";
            this.btn_upload.Size = new System.Drawing.Size(75, 23);
            this.btn_upload.TabIndex = 11;
            this.btn_upload.Text = "Upload";
            this.btn_upload.UseVisualStyleBackColor = true;
            this.btn_upload.Click += new System.EventHandler(this.btn_upload_Click);
            // 
            // btn_help_upload
            // 
            this.btn_help_upload.Location = new System.Drawing.Point(331, 124);
            this.btn_help_upload.Name = "btn_help_upload";
            this.btn_help_upload.Size = new System.Drawing.Size(28, 23);
            this.btn_help_upload.TabIndex = 12;
            this.btn_help_upload.Text = "?";
            this.btn_help_upload.UseVisualStyleBackColor = true;
            this.btn_help_upload.Click += new System.EventHandler(this.btn_help_upload_Click);
            // 
            // list_box_files
            // 
            this.list_box_files.FormattingEnabled = true;
            this.list_box_files.Location = new System.Drawing.Point(12, 212);
            this.list_box_files.Name = "list_box_files";
            this.list_box_files.Size = new System.Drawing.Size(347, 160);
            this.list_box_files.TabIndex = 13;
            // 
            // btn_list
            // 
            this.btn_list.Enabled = false;
            this.btn_list.Location = new System.Drawing.Point(12, 174);
            this.btn_list.Name = "btn_list";
            this.btn_list.Size = new System.Drawing.Size(313, 23);
            this.btn_list.TabIndex = 14;
            this.btn_list.Text = "View available file";
            this.btn_list.UseVisualStyleBackColor = true;
            // 
            // btn_help_visualizza
            // 
            this.btn_help_visualizza.Location = new System.Drawing.Point(331, 174);
            this.btn_help_visualizza.Name = "btn_help_visualizza";
            this.btn_help_visualizza.Size = new System.Drawing.Size(28, 23);
            this.btn_help_visualizza.TabIndex = 15;
            this.btn_help_visualizza.Text = "?";
            this.btn_help_visualizza.UseVisualStyleBackColor = true;
            this.btn_help_visualizza.Click += new System.EventHandler(this.but_help_visualizza_Click);
            // 
            // btn_download
            // 
            this.btn_download.Enabled = false;
            this.btn_download.Location = new System.Drawing.Point(12, 378);
            this.btn_download.Name = "btn_download";
            this.btn_download.Size = new System.Drawing.Size(313, 23);
            this.btn_download.TabIndex = 16;
            this.btn_download.Text = "Download";
            this.btn_download.UseVisualStyleBackColor = true;
            this.btn_download.Click += new System.EventHandler(this.btn_download_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(109, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(253, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "_________________________________________";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(154, 92);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(205, 13);
            this.label7.TabIndex = 18;
            this.label7.Text = "_________________________________";
            // 
            // btn_percorso
            // 
            this.btn_percorso.Enabled = false;
            this.btn_percorso.Location = new System.Drawing.Point(169, 124);
            this.btn_percorso.Name = "btn_percorso";
            this.btn_percorso.Size = new System.Drawing.Size(75, 23);
            this.btn_percorso.TabIndex = 19;
            this.btn_percorso.Text = "Browse";
            this.btn_percorso.UseVisualStyleBackColor = true;
            this.btn_percorso.Click += new System.EventHandler(this.but_percorso_Click);
            // 
            // btn_stop
            // 
            this.btn_stop.Enabled = false;
            this.btn_stop.Location = new System.Drawing.Point(250, 43);
            this.btn_stop.Name = "btn_stop";
            this.btn_stop.Size = new System.Drawing.Size(75, 23);
            this.btn_stop.TabIndex = 20;
            this.btn_stop.Text = "Stop";
            this.btn_stop.UseVisualStyleBackColor = true;
            // 
            // button10
            // 
            this.button10.Location = new System.Drawing.Point(331, 378);
            this.button10.Name = "button10";
            this.button10.Size = new System.Drawing.Size(28, 23);
            this.button10.TabIndex = 21;
            this.button10.Text = "?";
            this.button10.UseVisualStyleBackColor = true;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // btn_help_server
            // 
            this.btn_help_server.Location = new System.Drawing.Point(331, 43);
            this.btn_help_server.Name = "btn_help_server";
            this.btn_help_server.Size = new System.Drawing.Size(28, 23);
            this.btn_help_server.TabIndex = 22;
            this.btn_help_server.Text = "?";
            this.btn_help_server.UseVisualStyleBackColor = true;
            this.btn_help_server.Click += new System.EventHandler(this.btn_help_server_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(140, 414);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Progress...0%";
            // 
            // btn_help_progress
            // 
            this.btn_help_progress.Location = new System.Drawing.Point(331, 430);
            this.btn_help_progress.Name = "btn_help_progress";
            this.btn_help_progress.Size = new System.Drawing.Size(28, 23);
            this.btn_help_progress.TabIndex = 24;
            this.btn_help_progress.Text = "?";
            this.btn_help_progress.UseVisualStyleBackColor = true;
            this.btn_help_progress.Click += new System.EventHandler(this.btn_help_progress_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 493);
            this.Controls.Add(this.btn_help_progress);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btn_help_server);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.btn_stop);
            this.Controls.Add(this.btn_percorso);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_download);
            this.Controls.Add(this.btn_help_visualizza);
            this.Controls.Add(this.btn_list);
            this.Controls.Add(this.list_box_files);
            this.Controls.Add(this.btn_help_upload);
            this.Controls.Add(this.btn_upload);
            this.Controls.Add(this.prg_bar);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_box_path);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btm_start);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_port);
            this.Controls.Add(this.txt_ip);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_ip;
        private System.Windows.Forms.TextBox txt_port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btm_start;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_box_path;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar prg_bar;
        private System.Windows.Forms.Button btn_upload;
        private System.Windows.Forms.Button btn_help_upload;
        private System.Windows.Forms.ListBox list_box_files;
        private System.Windows.Forms.Button btn_list;
        private System.Windows.Forms.Button btn_help_visualizza;
        private System.Windows.Forms.Button btn_download;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_percorso;
        private System.Windows.Forms.Button btn_stop;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btn_help_server;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btn_help_progress;
    }
}

