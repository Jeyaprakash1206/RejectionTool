namespace ImageProcessing
{
    partial class ImageDialog
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
            this.imageViewerFull = new ImageProcessing.ImageViewer();
            this.SuspendLayout();
            // 
            // imageViewerFull
            // 
            this.imageViewerFull.AutoValidate = System.Windows.Forms.AutoValidate.EnableAllowFocusChange;
            this.imageViewerFull.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.imageViewerFull.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewerFull.Image = null;
            this.imageViewerFull.ImageLocation = null;
            this.imageViewerFull.IsActive = false;
            this.imageViewerFull.IsThumbnail = false;
            this.imageViewerFull.IsDialogue = true;
            this.imageViewerFull.Location = new System.Drawing.Point(0, 0);
            this.imageViewerFull.Margin = new System.Windows.Forms.Padding(4);
            this.imageViewerFull.Name = "imageViewerFull";
            this.imageViewerFull.Size = new System.Drawing.Size(730, 1040);
            this.imageViewerFull.TabIndex = 0;
            this.imageViewerFull.Load += new System.EventHandler(this.imageViewerFull_Load);
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Location = new System.Drawing.Point(dialogloc("X"), dialogloc("Y"));
            //this.imageViewerFull.MouseDown += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseDown);
            //this.imageViewerFull.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseMove);
            //this.imageViewerFull.MouseUp += new System.Windows.Forms.MouseEventHandler(this.PictureBox1_MouseUp);
            // 
            // ImageDialog
            // 
            //this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 1040);
            this.Controls.Add(this.imageViewerFull);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ImageDialog";
            this.ShowIcon = false;
            this.Text = "Image Viewer";
            this.TopMost = true;
            this.Resize += new System.EventHandler(this.ImageDialog_Resize);
            this.ResumeLayout(false);

        }

        #endregion

        private ImageViewer imageViewerFull;
    }
}