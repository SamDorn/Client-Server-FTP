namespace Client
{
    partial class History
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
            this.txt_box_history = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txt_box_history
            // 
            this.txt_box_history.Location = new System.Drawing.Point(12, 12);
            this.txt_box_history.Multiline = true;
            this.txt_box_history.Name = "txt_box_history";
            this.txt_box_history.ReadOnly = true;
            this.txt_box_history.Size = new System.Drawing.Size(350, 375);
            this.txt_box_history.TabIndex = 0;
            // 
            // btn_ok
            // 
            this.btn_ok.Location = new System.Drawing.Point(274, 393);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(88, 45);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            this.btn_ok.Click += new System.EventHandler(this.btn_ok_Click);
            // 
            // History
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 450);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.txt_box_history);
            this.Name = "History";
            this.Text = "Hystory";
            this.Load += new System.EventHandler(this.History_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txt_box_history;
        private System.Windows.Forms.Button btn_ok;
    }
}