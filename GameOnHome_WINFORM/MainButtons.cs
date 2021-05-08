using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOnHome_WINFORM
{
    class MainButtons {
        MainMenu parent;
        Button But_Play, But_Setting, But_Exit;

        public MainButtons(MainMenu form)
        {
            parent = form;                                                                         //Инициализация контейнера

            //Кнопка "Играть"
            But_Play = new Button();                                                               //Инациализация новой кнопки
            But_Play.BackgroundImage = Properties.Resources.Play;                                  //Инициализация заднего фона
            But_Play.Font = new Font("Comic Sans MS", 22f);                                        //Характеристика текста
            But_Play.FlatAppearance.BorderSize = 0;                                                //Размер рамок по перимитру 
            But_Play.FlatStyle = FlatStyle.Flat;                                                   //Стиль рамки = плоская линия
            But_Play.Text = "Играть";                                                              //Текст
            But_Play.Size = new Size(180, 60);                                                     //Размер кнопки
            But_Play.TabStop = false;
            But_Play.Location = new Point(120, 350);                                               //Расположение кнопки
            But_Play.Click += new EventHandler(But_Play_Click);                                  //Событие - при нажатии
            But_Play.MouseEnter += new EventHandler(But_play_Mouse_Enter);                         //Событие - при наведении
            But_Play.MouseLeave += new EventHandler(But_play_Mouse_Leave);                         //Событие - при покидании

            parent.Controls.Add(But_Play);                                                         //Заполнение контейнера

            //Кнопка "Настройки"

            But_Setting = new Button();                                                             //Инациализация новой кнопки
            But_Setting.BackgroundImage = Properties.Resources.Settings;                            //Инициализация заднего фона
            But_Setting.Font = new Font("Comic Sans MS", 22f);                                      //Характеристика текста
            But_Setting.FlatAppearance.BorderSize = 0;                                              //Размер рамок по перимитру 
            But_Setting.FlatStyle = FlatStyle.Flat;                                                 //Стиль рамки = плоская линия
            But_Setting.Text = "Настройки";                                                         //Текст
            But_Setting.Size = new Size(180, 60);                                                   //Размер кнопки
            But_Setting.TabStop = false;
            But_Setting.Location = new Point(120, 420);                                             //Расположение кнопки
            But_Setting.Click += new EventHandler(But_Setting_Click);                               //Событие - при нажатии
            But_Setting.MouseEnter += new EventHandler(But_Setting_Mouse_Enter);                    //Событие - при наведении
            But_Setting.MouseLeave += new EventHandler(But_Setting_Mouse_Leave);                    //Событие - при покидании

            parent.Controls.Add(But_Setting);                                                       //Заполнение контейнера

            //Кнопка "Выход"

            But_Exit = new Button();                                                                 //Инациализация новой кнопки
            But_Exit.BackgroundImage = Properties.Resources.Exit;                                    //Инициализация заднего фона
            But_Exit.Font = new Font("Comic Sans MS", 22f);                                          //Характеристика текста
            But_Exit.FlatAppearance.BorderSize = 0;                                                  //Размер рамок по перимитру 
            But_Exit.FlatStyle = FlatStyle.Flat;                                                     //Стиль рамки = плоская линия
            But_Exit.Text = "Выход";                                                                 //Текст
            But_Exit.Size = new Size(180, 60);                                                       //Размер кнопки
            But_Exit.TabStop = false;
            But_Exit.Location = new Point(120, 490);                                                 //Расположение кнопки
            But_Exit.Click += new EventHandler(But_Exit_Click);                                      //Событие - при нажатии
            But_Exit.MouseEnter += new EventHandler(But_Exit_Mouse_Enter);                           //Событие - при наведении
            But_Exit.MouseLeave += new EventHandler(But_Exit_Mouse_Leave);                           //Событие - при покидании

            parent.Controls.Add(But_Exit);                                                           //Заполнение контейнера
        }

        //Описание события нажатия на "Играть"
        private void But_Play_Click(object sender, EventArgs e)
        {
            ListGames play = new ListGames();                                                                  //Инициализация новой формы
            play.Show();                                                                             //Показать новую форму
        }

        //Описание события наведения на "Играть"
        private void But_play_Mouse_Enter(object sender, EventArgs e)
        {
            But_Play.Font = new Font("Comic Sans MS", 23f, FontStyle.Bold);                          //Инициализация новго стиля текста
        }

        //Описание события покидания на "Играть"
        private void But_play_Mouse_Leave(object sender, EventArgs e)
        {
            But_Play.Font = new Font("Comic Sans MS", 22f);                                          //Инициализация новго стиля текста
        }

        //Описание события нажатия на "Настройки"
        private void But_Setting_Click(object sender, EventArgs e)
        {
            Settings settings = new Settings();                                                      //Инициализация новой формы
            settings.Show();                                                                         //Показать новую форму
        }

        //Описание события наведения на "Настройки"
        private void But_Setting_Mouse_Enter(object sender, EventArgs e)
        {
            But_Setting.Font = new Font("Comic Sans MS", 21f, FontStyle.Bold);                          //Инициализация новго стиля текста
        }

        //Описание события покидания на "Настройки"
        private void But_Setting_Mouse_Leave(object sender, EventArgs e)
        {
            But_Setting.Font = new Font("Comic Sans MS", 22f);                                          //Инициализация новго стиля текста
        }

        //Описание события нажатия на "Выход"
        private void But_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();                                                                      //Выход из приложения
        }

        //Описание события наведения на "Выход"
        private void But_Exit_Mouse_Enter(object sender, EventArgs e)
        {
            But_Exit.Font = new Font("Comic Sans MS", 23f, FontStyle.Bold);                          //Инициализация новго стиля текста
        }

        //Описание события покидания на "Выход"
        private void But_Exit_Mouse_Leave(object sender, EventArgs e)
        {
            But_Exit.Font = new Font("Comic Sans MS", 22f);                                           //Инициализация новго стиля текста
        }
    }
}
