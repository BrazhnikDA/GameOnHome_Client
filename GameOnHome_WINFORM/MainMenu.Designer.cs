
namespace GameOnHome_WINFORM
{
    partial class MainMenu
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.But_Play = new System.Windows.Forms.Button();
            this.But_Setting = new System.Windows.Forms.Button();
            this.But_Exit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // But_Play
            // 
            this.But_Play.BackgroundImage = global::GameOnHome_WINFORM.Properties.Resources.Play;
            this.But_Play.FlatAppearance.BorderSize = 0;
            this.But_Play.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.But_Play.Font = new System.Drawing.Font("Comic Sans MS", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.But_Play.Location = new System.Drawing.Point(171, 583);
            this.But_Play.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.But_Play.Name = "But_Play";
            this.But_Play.Size = new System.Drawing.Size(257, 100);
            this.But_Play.TabIndex = 0;
            this.But_Play.Text = "Играть";
            this.But_Play.UseVisualStyleBackColor = true;
            this.But_Play.Click += new System.EventHandler(this.But_Play_Click);
            this.But_Play.MouseEnter += new System.EventHandler(this.But_Play_MouseEnter);
            this.But_Play.MouseLeave += new System.EventHandler(this.But_Play_MouseLeave);
            // 
            // But_Setting
            // 
            this.But_Setting.BackgroundImage = global::GameOnHome_WINFORM.Properties.Resources.Settings;
            this.But_Setting.FlatAppearance.BorderSize = 0;
            this.But_Setting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.But_Setting.Font = new System.Drawing.Font("Comic Sans MS", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.But_Setting.Location = new System.Drawing.Point(171, 700);
            this.But_Setting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.But_Setting.Name = "But_Setting";
            this.But_Setting.Size = new System.Drawing.Size(257, 100);
            this.But_Setting.TabIndex = 1;
            this.But_Setting.Text = "Настройки";
            this.But_Setting.UseVisualStyleBackColor = true;
            this.But_Setting.Click += new System.EventHandler(this.But_Setting_Click);
            this.But_Setting.MouseEnter += new System.EventHandler(this.Bur_Setting_MouseEnter);
            this.But_Setting.MouseLeave += new System.EventHandler(this.Bur_Setting_MouseLeave);
            // 
            // But_Exit
            // 
            this.But_Exit.BackgroundImage = global::GameOnHome_WINFORM.Properties.Resources.Exit;
            this.But_Exit.FlatAppearance.BorderSize = 0;
            this.But_Exit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.But_Exit.Font = new System.Drawing.Font("Comic Sans MS", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.But_Exit.Location = new System.Drawing.Point(171, 817);
            this.But_Exit.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.But_Exit.Name = "But_Exit";
            this.But_Exit.Size = new System.Drawing.Size(257, 100);
            this.But_Exit.TabIndex = 2;
            this.But_Exit.Text = "Выход";
            this.But_Exit.UseVisualStyleBackColor = true;
            this.But_Exit.Click += new System.EventHandler(this.But_Exit_Click);
            this.But_Exit.MouseEnter += new System.EventHandler(this.But_Exit_MouseEnter);
            this.But_Exit.MouseLeave += new System.EventHandler(this.But_Exit_MouseLeave);
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::GameOnHome_WINFORM.Properties.Resources.Main_Menu;
            this.ClientSize = new System.Drawing.Size(1543, 1190);
            this.Controls.Add(this.But_Exit);
            this.Controls.Add(this.But_Setting);
            this.Controls.Add(this.But_Play);
            this.Name = "MainMenu";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Главное меню";
            this.Load += new System.EventHandler(this.MainMenu_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button But_Play;
        private System.Windows.Forms.Button But_Setting;
        private System.Windows.Forms.Button But_Exit;
    }
}

