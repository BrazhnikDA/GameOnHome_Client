
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
            this.But_Go = new System.Windows.Forms.Button();
            this.switch_button1 = new GameOnHome_WINFORM.setting_of_games.switch_button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // But_Go
            // 
            this.But_Go.BackColor = System.Drawing.Color.Transparent;
            this.But_Go.FlatAppearance.BorderSize = 0;
            this.But_Go.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.But_Go.Font = new System.Drawing.Font("Segoe UI", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.But_Go.Location = new System.Drawing.Point(240, 280);
            this.But_Go.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.But_Go.Name = "But_Go";
            this.But_Go.Size = new System.Drawing.Size(344, 87);
            this.But_Go.TabIndex = 0;
            this.But_Go.TabStop = false;
            this.But_Go.Text = "Поехали!";
            this.But_Go.UseVisualStyleBackColor = false;
            this.But_Go.Click += new System.EventHandler(this.But_Go_Click);
            this.But_Go.MouseEnter += new System.EventHandler(this.But_Go_MouseEnter);
            this.But_Go.MouseLeave += new System.EventHandler(this.But_Go_MouseLeave);
            // 
            // switch_button1
            // 
            this.switch_button1.BackColor = System.Drawing.Color.White;
            this.switch_button1.Checked = false;
            this.switch_button1.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.switch_button1.Location = new System.Drawing.Point(240, 67);
            this.switch_button1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.switch_button1.Name = "switch_button1";
            this.switch_button1.Size = new System.Drawing.Size(344, 157);
            this.switch_button1.TabIndex = 1;
            this.switch_button1.Text = "switch_button1";
            this.switch_button1.Click += new System.EventHandler(this.switch_button1_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::GameOnHome_WINFORM.Properties.Resources.single_mod;
            this.pictureBox1.Location = new System.Drawing.Point(17, 20);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(214, 250);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::GameOnHome_WINFORM.Properties.Resources.online_mod;
            this.pictureBox2.InitialImage = null;
            this.pictureBox2.Location = new System.Drawing.Point(593, 20);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(214, 250);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 3;
            this.pictureBox2.TabStop = false;
            // 
            // settings_of_game
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(821, 387);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.switch_button1);
            this.Controls.Add(this.But_Go);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "settings_of_game";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Режим игры";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button But_Go;
        private setting_of_games.switch_button switch_button1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
    }
}