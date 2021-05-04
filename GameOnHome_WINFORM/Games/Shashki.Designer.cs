
namespace GameOnHome_WINFORM.Online
{
    partial class Shashki
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(805, 12);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(150, 34);
            this.buttonConnect.TabIndex = 0;
            this.buttonConnect.Text = "Подключиться";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // Shashki
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 871);
            this.Controls.Add(this.buttonConnect);
            this.Name = "Shashki";
            this.Text = "Shashki";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
    }
}