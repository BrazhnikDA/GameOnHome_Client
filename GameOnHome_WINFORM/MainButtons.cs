using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Media;

namespace GameOnHome_WINFORM
{
    class MainButtons
    {

        MainMenu parent;
        Button lab1, lab2, lab3;

        public MainButtons(MainMenu form)
        {
            parent = form;

            //Лейбл "Играть"
            lab1 = new Button();
            lab1.Location = new Point(450, 220);
            lab1.Size = new Size(250, 70);
            lab1.Font = new Font("Comic Sans MS", 25f);
            lab1.FlatStyle = FlatStyle.Flat;
            lab1.FlatAppearance.BorderSize = 5;
            lab1.ForeColor = Color.Black;
            lab1.BackColor = Color.White;
            lab1.Text = "Играть";
            lab1.TabStop = false;
            lab1.TextAlign = ContentAlignment.MiddleCenter;
            lab1.Click += new EventHandler(lab1_click);
            lab1.MouseEnter += new EventHandler(lab1_enter);
            lab1.MouseLeave += new EventHandler(lab1_leave);

            parent.Controls.Add(lab1);

            //Лейбл "Настройки"
            lab2 = new Button();
            lab2.Location = new Point(450, 300);
            lab2.Size = new Size(250, 70);
            lab2.Font = new Font("Comic Sans MS", 25f);
            lab2.FlatStyle = FlatStyle.Flat;
            lab2.FlatAppearance.BorderSize = 5;
            lab2.ForeColor = Color.Black;
            lab2.BackColor = Color.White;
            lab2.Text = "Настройки";
            lab2.TabStop = false;
            lab2.TextAlign = ContentAlignment.MiddleCenter;
            lab2.Click += new EventHandler(lab2_click);
            lab2.MouseEnter += new EventHandler(lab2_enter);
            lab2.MouseLeave += new EventHandler(lab2_leave);

            parent.Controls.Add(lab2);

            //Лейбл "Выход"
            lab3 = new Button();
            lab3.Location = new Point(450, 380);
            lab3.Size = new Size(250, 70);
            lab3.Font = new Font("Comic Sans MS", 25f);
            lab3.FlatStyle = FlatStyle.Flat;
            lab3.FlatAppearance.BorderSize = 5;
            lab3.ForeColor = Color.Black;
            lab3.BackColor = Color.White;
            lab3.Text = "Выход";
            lab3.TabStop = false;
            lab3.TextAlign = ContentAlignment.MiddleCenter;
            lab3.Click += new EventHandler(lab3_click);
            lab3.MouseEnter += new EventHandler(lab3_enter);
            lab3.MouseLeave += new EventHandler(lab3_leave);

            parent.Controls.Add(lab3);

        }

        private void PlaySound()
        {
            SoundPlayer sound = new SoundPlayer(Properties.Resources.button_20);
            sound.Play();
        }

        private void lab1_click(object sender, EventArgs e)
        {
            PlaySound();
            ListGames lg = new ListGames();                                              //Инициализация новой формы
            lg.Show();                                                                   //Показ новой формы
            MainMenu mm = parent;
            mm.Hide();
        }

        private void lab1_enter(object sender, EventArgs e)
        {
            lab1.Font = new Font("Comic Sans MS", 28f, FontStyle.Bold);                  //Создание нового стиля
        }

        private void lab1_leave(object sender, EventArgs e)
        {
            lab1.Font = new Font("Comic Sans MS", 25f);                                  //Создание нового стиля
        }

        private void lab2_click(object sender, EventArgs e)
        {
            PlaySound();
            Settings settings = new Settings();                                          //Инициализация новой формы
            settings.Show();
        }

        private void lab2_enter(object sender, EventArgs e)
        {
            lab2.Font = new Font("Comic Sans MS", 28f, FontStyle.Bold);                  //Создание нового стиля
        }

        private void lab2_leave(object sender, EventArgs e)
        {
            lab2.Font = new Font("Comic Sans MS", 25f);                                  //Создание нового стиля
        }

        private void lab3_click(object sender, EventArgs e)
        {
            Application.Exit();                                                          //Конец события
        }

        private void lab3_enter(object sender, EventArgs e)
        {
            lab3.Font = new Font("Comic Sans MS", 28f, FontStyle.Bold);                  //Создание нового стиля
        }

        private void lab3_leave(object sender, EventArgs e)
        {
            lab3.Font = new Font("Comic Sans MS", 25f);                                  //Создание нового стиля
        }
    }
}