namespace RejectionTool5
{
    partial class Form3
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
            this.Srocode = new System.Windows.Forms.TextBox();
            this.Left = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Srocode
            // 
            this.Srocode.Location = new System.Drawing.Point(12, 12);
            this.Srocode.Name = "Srocode";
            this.Srocode.Size = new System.Drawing.Size(100, 22);
            this.Srocode.TabIndex = 0;
            // 
            // Left
            // 
            this.Left.Location = new System.Drawing.Point(139, 13);
            this.Left.Name = "Left";
            this.Left.Size = new System.Drawing.Size(75, 23);
            this.Left.TabIndex = 1;
            this.Left.Text = "Top";
            this.Left.UseVisualStyleBackColor = true;
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.Left);
            this.Controls.Add(this.Srocode);
            this.Name = "Form3";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Srocode;
        private System.Windows.Forms.Button Left;
    }
}