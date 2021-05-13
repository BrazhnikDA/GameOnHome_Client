
namespace GameOnHome_WINFORM
{
    partial class settings_of_game
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
            this.pictureBox_single = new System.Windows.Forms.PictureBox();
            this.pictureBox_online = new System.Windows.Forms.PictureBox();
            this.switch_button1 = new GameOnHome_WINFORM.setting_of_games.switch_button();
            this.label1 = new System.Windows.Forms.Label();
            this.But_Go = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_single)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_online)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox_single
            // 
            this.pictureBox_single.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_single.Image = global::GameOnHome_WINFORM.Properties.Resources.single_mod;
            this.pictureBox_single.Location = new System.Drawing.Point(11, 62);
            this.pictureBox_single.Name = "pictureBox_single";
            this.pictureBox_single.Size = new System.Drawing.Size(150, 150);
            this.pictureBox_single.TabIndex = 5;
            this.pictureBox_single.TabStop = false;
            // 
            // pictureBox_online
            // 
            this.pictureBox_online.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox_online.Image = global::GameOnHome_WINFORM.Properties.Resources.online_mod;
            this.pictureBox_online.Location = new System.Drawing.Point(412, 62);
            this.pictureBox_online.Name = "pictureBox_online";
            this.pictureBox_online.Size = new System.Drawing.Size(150, 150);
            this.pictureBox_online.TabIndex = 6;
            this.pictureBox_online.TabStop = false;
            // 
            // switch_button1
            // 
            this.switch_button1.BackColor = System.Drawing.Color.Transparent;
            this.switch_button1.Checked = false;
            this.switch_button1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.switch_button1.Location = new System.Drawing.Point(200, 99);
            this.switch_button1.Name = "switch_button1";
            this.switch_button1.Size = new System.Drawing.Size(167, 75);
            this.switch_button1.TabIndex = 7;
            this.switch_button1.Text = "switch_button1";
            this.switch_button1.Click += new System.EventHandler(this.switch_button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(200, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(173, 25);
            this.label1.TabIndex = 9;
            this.label1.Text = "Выбор режима";
            // 
            // But_Go
            // 
            this.But_Go.AutoSize = true;
            this.But_Go.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.But_Go.Font = new System.Drawing.Font("Comic Sans MS", 24.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.But_Go.Location = new System.Drawing.Point(200, 221);
            this.But_Go.Name = "But_Go";
            this.But_Go.Size = new System.Drawing.Size(174, 46);
            this.But_Go.TabIndex = 10;
            this.But_Go.Text = "Поехали!";
            this.But_Go.Click += new System.EventHandler(this.But_Go_Click);
            this.But_Go.MouseEnter += new System.EventHandler(this.But_Go_MouseEnter);
            this.But_Go.MouseLeave += new System.EventHandler(this.But_Go_MouseLeave);
            // 
            // settings_of_game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.ClientSize = new System.Drawing.Size(575, 288);
            this.Controls.Add(this.But_Go);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.switch_button1);
            this.Controls.Add(this.pictureBox_online);
            this.Controls.Add(this.pictureBox_single);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "settings_of_game";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Режим игры";
            this.Load += new System.EventHandler(this.settings_of_game_Load);
            this.MouseLeave += new System.EventHandler(this.settings_of_game_MouseLeave);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_single)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_online)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox_single;
        private System.Windows.Forms.PictureBox pictureBox_online;
        private setting_of_games.switch_button switch_button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label But_Go;
    }
}