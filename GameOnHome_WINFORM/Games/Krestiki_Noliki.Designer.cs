
namespace GameOnHome_WINFORM.Games
{
    partial class Krestiki_Noliki
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
            this.pictureWait = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureWait)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureWait
            // 
            this.pictureWait.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureWait.Image = global::GameOnHome_WINFORM.Properties.Resources.Wait;
            this.pictureWait.Location = new System.Drawing.Point(0, 0);
            this.pictureWait.Name = "pictureWait";
            this.pictureWait.Size = new System.Drawing.Size(645, 594);
            this.pictureWait.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureWait.TabIndex = 4;
            this.pictureWait.TabStop = false;
            this.pictureWait.UseWaitCursor = true;
            this.pictureWait.Visible = false;
            // 
            // Krestiki_Noliki
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(510, 416);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Krestiki_Noliki";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.Krestiki_Noliki_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureWait)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureWait;
    }
}