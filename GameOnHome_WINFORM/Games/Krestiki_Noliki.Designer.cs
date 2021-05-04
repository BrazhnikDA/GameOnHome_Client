
namespace GameOnHome_WINFORM.Online
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.Status_connect = new System.Windows.Forms.Label();
            this.pictureConnect = new System.Windows.Forms.PictureBox();
            this.pictureWinGame = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureWait)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureConnect)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureWinGame)).BeginInit();
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
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(694, 31);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(146, 34);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Подключиться";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // Status_connect
            // 
            this.Status_connect.AutoSize = true;
            this.Status_connect.Location = new System.Drawing.Point(879, 31);
            this.Status_connect.Name = "Status_connect";
            this.Status_connect.Size = new System.Drawing.Size(189, 25);
            this.Status_connect.TabIndex = 2;
            this.Status_connect.Text = "Статус: не подключён";
            // 
            // pictureConnect
            // 
            this.pictureConnect.BackgroundImage = global::GameOnHome_WINFORM.Properties.Resources.red_krug;
            this.pictureConnect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureConnect.Location = new System.Drawing.Point(1074, 31);
            this.pictureConnect.Name = "pictureConnect";
            this.pictureConnect.Size = new System.Drawing.Size(26, 31);
            this.pictureConnect.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureConnect.TabIndex = 3;
            this.pictureConnect.TabStop = false;
            // 
            // pictureWinGame
            // 
            this.pictureWinGame.Image = global::GameOnHome_WINFORM.Properties.Resources.Вы_выиграли;
            this.pictureWinGame.Location = new System.Drawing.Point(12, 292);
            this.pictureWinGame.Name = "pictureWinGame";
            this.pictureWinGame.Size = new System.Drawing.Size(1138, 352);
            this.pictureWinGame.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureWinGame.TabIndex = 4;
            this.pictureWinGame.TabStop = false;
            this.pictureWinGame.Visible = false;
            // 
            // Online_Krestiki_Noliki
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1163, 742);
            this.Controls.Add(this.pictureWinGame);
            this.Controls.Add(this.pictureConnect);
            this.Controls.Add(this.Status_connect);
            this.Controls.Add(this.buttonConnect);
            this.Name = "Krestiki_Noliki";
            ((System.ComponentModel.ISupportInitialize)(this.pictureWait)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureConnect)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureWinGame)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label Status_connect;
        private System.Windows.Forms.PictureBox pictureConnect;
        private System.Windows.Forms.PictureBox pictureWait;
        private System.Windows.Forms.PictureBox pictureWinGame;
    }
}