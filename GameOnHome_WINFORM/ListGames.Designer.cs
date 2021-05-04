
namespace GameOnHome_WINFORM
{
    partial class ListGames
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
            this.pictureBox_tic_tac = new System.Windows.Forms.PictureBox();
            this.label_tic_tac = new System.Windows.Forms.Label();
            this.pictureBox_shashki = new System.Windows.Forms.PictureBox();
            this.label_shaski = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_tic_tac)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_shashki)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_tic_tac
            // 
            this.pictureBox_tic_tac.Image = global::GameOnHome_WINFORM.Properties.Resources.Tic_tac_icone;
            this.pictureBox_tic_tac.Location = new System.Drawing.Point(57, 67);
            this.pictureBox_tic_tac.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox_tic_tac.Name = "pictureBox_tic_tac";
            this.pictureBox_tic_tac.Size = new System.Drawing.Size(183, 213);
            this.pictureBox_tic_tac.TabIndex = 0;
            this.pictureBox_tic_tac.TabStop = false;
            this.pictureBox_tic_tac.Click += new System.EventHandler(this.pictureBox_tic_tac_Click);
            this.pictureBox_tic_tac.MouseEnter += new System.EventHandler(this.pictureBox_tic_tac_MouseEnter);
            this.pictureBox_tic_tac.MouseLeave += new System.EventHandler(this.pictureBox_tic_tac_MouseLeave);
            // 
            // label_tic_tac
            // 
            this.label_tic_tac.AutoSize = true;
            this.label_tic_tac.Font = new System.Drawing.Font("Stencil", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_tic_tac.Location = new System.Drawing.Point(73, 285);
            this.label_tic_tac.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_tic_tac.Name = "label_tic_tac";
            this.label_tic_tac.Size = new System.Drawing.Size(155, 24);
            this.label_tic_tac.TabIndex = 1;
            this.label_tic_tac.Text = "Крестики-Нолики";
            // 
            // pictureBox_shashki
            // 
            this.pictureBox_shashki.Image = global::GameOnHome_WINFORM.Properties.Resources.white;
            this.pictureBox_shashki.Location = new System.Drawing.Point(248, 67);
            this.pictureBox_shashki.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox_shashki.Name = "pictureBox_shashki";
            this.pictureBox_shashki.Size = new System.Drawing.Size(183, 213);
            this.pictureBox_shashki.TabIndex = 0;
            this.pictureBox_shashki.TabStop = false;
            this.pictureBox_shashki.Click += new System.EventHandler(this.pictureBox_shaski_Click);
            this.pictureBox_shashki.MouseEnter += new System.EventHandler(this.pictureBox_shaski_MouseEnter);
            this.pictureBox_shashki.MouseLeave += new System.EventHandler(this.pictureBox_shaski_MouseLeave);
            // 
            // label_shaski
            // 
            this.label_shaski.AutoSize = true;
            this.label_shaski.Font = new System.Drawing.Font("Stencil", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.label_shaski.Location = new System.Drawing.Point(257, 285);
            this.label_shaski.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label_shaski.Name = "label_shaski";
            this.label_shaski.Size = new System.Drawing.Size(68, 24);
            this.label_shaski.TabIndex = 1;
            this.label_shaski.Text = "Шашки";
            // 
            // ListGames
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1090, 1232);
            this.Controls.Add(this.label_shaski);
            this.Controls.Add(this.label_tic_tac);
            this.Controls.Add(this.pictureBox_shashki);
            this.Controls.Add(this.pictureBox_tic_tac);
            this.Name = "ListGames";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Список игр";
            this.Load += new System.EventHandler(this.ListGames_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_tic_tac)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_shashki)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox_tic_tac;
        private System.Windows.Forms.Label label_tic_tac;
        private System.Windows.Forms.PictureBox pictureBox_shashki;
        private System.Windows.Forms.Label label_shaski;
    }
}