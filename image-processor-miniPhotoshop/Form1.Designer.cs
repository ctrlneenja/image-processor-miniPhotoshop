namespace image_processor_miniPhotoshop
{
    partial class Form1
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnUploadFile = new System.Windows.Forms.Button();
            this.btnWebcam = new System.Windows.Forms.Button();
            this.lblFooter = new System.Windows.Forms.Label();
            this.btnGreenScreen = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.ForeColor = System.Drawing.Color.Cornsilk;
            this.lblTitle.Location = new System.Drawing.Point(160, 50);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(307, 51);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Mini Photoshop";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnUploadFile
            // 
            this.btnUploadFile.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnUploadFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnUploadFile.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUploadFile.Location = new System.Drawing.Point(213, 138);
            this.btnUploadFile.Name = "btnUploadFile";
            this.btnUploadFile.Size = new System.Drawing.Size(180, 50);
            this.btnUploadFile.TabIndex = 1;
            this.btnUploadFile.Text = "📂 Upload a File";
            this.btnUploadFile.UseVisualStyleBackColor = false;
            this.btnUploadFile.Click += new System.EventHandler(this.button1_Click);
            // 
            // btnWebcam
            // 
            this.btnWebcam.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnWebcam.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnWebcam.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnWebcam.Location = new System.Drawing.Point(213, 274);
            this.btnWebcam.Name = "btnWebcam";
            this.btnWebcam.Size = new System.Drawing.Size(180, 50);
            this.btnWebcam.TabIndex = 2;
            this.btnWebcam.Text = "🎥 Turn On Webcam";
            this.btnWebcam.UseVisualStyleBackColor = false;
            this.btnWebcam.Click += new System.EventHandler(this.button2_Click);
            // 
            // lblFooter
            // 
            this.lblFooter.AutoSize = true;
            this.lblFooter.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFooter.ForeColor = System.Drawing.Color.LightGray;
            this.lblFooter.Location = new System.Drawing.Point(220, 360);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(139, 15);
            this.lblFooter.TabIndex = 3;
            this.lblFooter.Text = "Janeen Gabrielle Lim - F2";
            this.lblFooter.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnGreenScreen
            // 
            this.btnGreenScreen.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnGreenScreen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGreenScreen.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGreenScreen.Location = new System.Drawing.Point(213, 206);
            this.btnGreenScreen.Name = "btnGreenScreen";
            this.btnGreenScreen.Size = new System.Drawing.Size(180, 50);
            this.btnGreenScreen.TabIndex = 4;
            this.btnGreenScreen.Text = "📷 Green Screen";
            this.btnGreenScreen.UseVisualStyleBackColor = false;
            this.btnGreenScreen.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(29)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(608, 391);
            this.Controls.Add(this.btnGreenScreen);
            this.Controls.Add(this.lblFooter);
            this.Controls.Add(this.btnWebcam);
            this.Controls.Add(this.btnUploadFile);
            this.Controls.Add(this.lblTitle);
            this.Name = "Form1";
            this.Text = "Mini Photoshop - Image Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnUploadFile;
        private System.Windows.Forms.Button btnWebcam;
        private System.Windows.Forms.Label lblFooter;
        private System.Windows.Forms.Button btnGreenScreen;
    }
}
