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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button3 = new System.Windows.Forms.Button();
            this.btn_help_upload = new System.Windows.Forms.Button();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button5 = new System.Windows.Forms.Button();
            this.btn_help_visualizza = new System.Windows.Forms.Button();
            this.btn_download = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.btn_percorso = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.button10 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.btn_help_server = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(15, 45);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(109, 20);
            this.textBox1.TabIndex = 0;
            this.textBox1.Text = "127.0.0.1";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(127, 45);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(36, 20);
            this.textBox2.TabIndex = 1;
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(169, 42);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Start";
            this.button1.UseVisualStyleBackColor = true;
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
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(12, 127);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(151, 20);
            this.textBox3.TabIndex = 7;
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
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 430);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(347, 23);
            this.progressBar1.TabIndex = 10;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(250, 124);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 11;
            this.button3.Text = "Upload";
            this.button3.UseVisualStyleBackColor = true;
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
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 212);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(347, 160);
            this.listBox1.TabIndex = 13;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(12, 174);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(313, 23);
            this.button5.TabIndex = 14;
            this.button5.Text = "View available file";
            this.button5.UseVisualStyleBackColor = true;
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
            this.label6.Location = new System.Drawing.Point(124, 14);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(235, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "______________________________________";
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
            this.btn_percorso.Location = new System.Drawing.Point(169, 124);
            this.btn_percorso.Name = "btn_percorso";
            this.btn_percorso.Size = new System.Drawing.Size(75, 23);
            this.btn_percorso.TabIndex = 19;
            this.btn_percorso.Text = "Browse";
            this.btn_percorso.UseVisualStyleBackColor = true;
            this.btn_percorso.Click += new System.EventHandler(this.but_percorso_Click);
            // 
            // button9
            // 
            this.button9.Location = new System.Drawing.Point(250, 43);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(75, 23);
            this.button9.TabIndex = 20;
            this.button9.Text = "Stop";
            this.button9.UseVisualStyleBackColor = true;
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
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 493);
            this.Controls.Add(this.btn_help_server);
            this.Controls.Add(this.button10);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.btn_percorso);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btn_download);
            this.Controls.Add(this.btn_help_visualizza);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.btn_help_upload);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.Text = "Client";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button btn_help_upload;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button btn_help_visualizza;
        private System.Windows.Forms.Button btn_download;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btn_percorso;
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button button10;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button btn_help_server;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
    }
}

