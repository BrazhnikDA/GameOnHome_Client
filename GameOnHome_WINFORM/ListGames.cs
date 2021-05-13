using GameOnHome_WINFORM.Games;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOnHome_WINFORM
{
    public partial class ListGames : Form
    {

        Graphics g;

        public ListGames()
        {
            InitializeComponent();
            pictureBox_tic_tac.Parent = pictureBox_back;
            pictureBox_shaski.Parent = pictureBox_back;
            pictureBox_Three_Row.Parent = pictureBox_back;
            pictureBox_sapper.Parent = pictureBox_back;
            pictureBox_chess.Parent = pictureBox_back;
            label_tic_tac.Parent = pictureBox_back;
            label_shaski.Parent = pictureBox_back;
            label_3ryd.Parent = pictureBox_back;
            label_saper.Parent = pictureBox_back;
            label_chess.Parent = pictureBox_back;
        }

        private void ListGames_Load(object sender, EventArgs e)
        {

        }

        //Обработка события клик по "Крестики_Нолики"
        private void pictureBox_tic_tac_Click(object sender, EventArgs e)
        {
            string name_of_game = "tic_tac";                                                             //Задаём имя игры
            settings_of_game sof = new settings_of_game(name_of_game, this);                             //Вызываем окно настроек с параметром названия игры
            sof.Owner = this;                                                                            //Указываем, что настройки игры - это дочерняя форма от листа с играми
            sof.ShowDialog();                                                                            //Показываем форму
        }

        //Метод для вызова определённого режима игры "Крестики-Нолики"
        public void tic_tac_plqy(bool result)
        {
            Krestiki_Noliki krestiki_Noliki = new Krestiki_Noliki(result);
            krestiki_Noliki.Show();
        }

        //Обработка события наведение на "Крестики-нолики"
        private void pictureBox_tic_tac_MouseEnter(object sender, EventArgs e)
        {
            g = pictureBox_back.CreateGraphics();                                                        //Инициализация графики для заднего PictureBox          
            g.DrawRectangle(new Pen(Color.FromArgb(100, 255, 165, 0), 3), 37, 37, 133, 133);             //Создание пера и отрисовка прямоугольника
            label_tic_tac.Location = new Point(36, 173);                                                 //Корректируем положение названия
            label_tic_tac.Font = new Font("Stencil", 10f, FontStyle.Bold);                               //Изменяем стиль названия
            label_tic_tac.ForeColor = Color.Gold;
        }

        //Обработка события покидание из "Крестики-Нолики"
        private void pictureBox_tic_tac_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_back.Image = null;                                                                //Удаления изображения
            label_tic_tac.Location = new Point(44, 173);                                                 //Корректируем положение названия
            label_tic_tac.Font = new Font("Stencil", 9.75f);                                             //Изменяем стиль названия
            label_tic_tac.ForeColor = Color.White;                                                       //Изменение цвета текста
        }

        //Обработка события клик по "Шашки"
        private void pictureBox_shaski_Click(object sender, EventArgs e)
        {
            string name_of_game = "shaski";                                                              //Задаём имя игры
            settings_of_game sof = new settings_of_game(name_of_game, this);                             //Вызываем окно настроек с параметром названия игры
            sof.Owner = this;                                                                            //Указываем, что настройки игры - это дочерняя форма от листа с играми
            sof.ShowDialog();                                                                            //Показываем форму
        }

        //Метод для вызова определённого режима игры "Шашки"
        public void shaski_play(bool result)
        {
            Shashki shashki = new Shashki(result);
            shashki.Show();
        }

        //Обработка события наведение на "Шашки"
        private void pictureBox_shaski_MouseEnter(object sender, EventArgs e)
        {
            g = pictureBox_back.CreateGraphics();                                                        //Инициализация графики для заднего PictureBox
            g.DrawRectangle(new Pen(Color.FromArgb(100, 255, 165, 0), 3), 205, 37, 134, 134);            //Создание пера и отрисовка прямоугольника
            label_shaski.Location = new Point(208, 173);                                                 //Корректируем положение названия
            label_shaski.Font = new Font("Stencil", 10f, FontStyle.Bold);                                //Изменяем стиль названия
            label_shaski.ForeColor = Color.Gold;                                                         //Изменение цвета текста
        }

        //Обработка события покидания из "Шашки"
        private void pictureBox_shaski_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_back.Image = null;                                                                //Удаления изображения
            label_shaski.Location = new Point(208, 173);                                                 //Корректируем положение названия
            label_shaski.Font = new Font("Stencil", 9.75f);                                              //Изменяем стиль названия
            label_shaski.ForeColor = Color.White;                                                        //Изменение цвета текста
        }

        //Обработка события клик на "3 в ряд"
        private void pictureBox_Three_Row_Click(object sender, EventArgs e)
        {
            ThreeRow three = new ThreeRow();
            this.Close();
            three.ShowDialog();
        }

        //Обработка события наведения на "3 в ряд"
        private void pictureBox_Three_Row_MouseEnter(object sender, EventArgs e)
        {
            g = pictureBox_back.CreateGraphics();                                                        //Инициализация графики для заднего PictureBox
            g.DrawRectangle(new Pen(Color.FromArgb(100, 255, 165, 0), 3), 373, 37, 134, 134);            //Создание пера и отрисовка прямоугольника
            label_3ryd.Location = new Point(376, 173);                                                   //Корректируем положение названия
            label_3ryd.Font = new Font("Stencil", 10f, FontStyle.Bold);                                  //Изменяем стиль названия
            label_3ryd.ForeColor = Color.Gold;                                                           //Изменение цвета текста
        }

        //Обработка события покидание из "3 в ряд"
        private void pictureBox_Three_Row_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_back.Image = null;                                                                //Удаления изображения
            label_3ryd.Location = new Point(376, 173);                                                   //Корректируем положение названия
            label_3ryd.Font = new Font("Stencil", 9.75f);                                                //Изменяем стиль названия
            label_3ryd.ForeColor = Color.White;                                                          //Изменение цвета текста
        }

        //Обработка события клик по "Сапёр"
        private void pictureBox_saper_Click(object sender, EventArgs e)
        {
            Miner miner = new Miner();
            this.Close();
            miner.ShowDialog();
        }

        //Обработка события наведения на "Сапёр"
        private void pictureBox_saper_MouseEnter(object sender, EventArgs e)
        {
            g = pictureBox_back.CreateGraphics();                                                        //Инициализация графики для заднего PictureBox
            g.DrawRectangle(new Pen(Color.FromArgb(100, 255, 165, 0), 3), 541, 37, 134, 134);            //Создание пера и отрисовка прямоугольника
            label_saper.Location = new Point(544, 173);                                                  //Корректируем положение названия
            label_saper.Font = new Font("Stencil", 10f, FontStyle.Bold);                                 //Изменяем стиль названия
            label_saper.ForeColor = Color.Gold;                                                          //Изменение цвета текста
        }

        //Обработка события покидание из "Сапёр"
        private void pictureBox_saper_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_back.Image = null;                                                                //Удаления изображения
            label_saper.Location = new Point(544, 173);                                                  //Корректируем положение названия
            label_saper.Font = new Font("Stencil", 9.75f);                                               //Изменяем стиль названия
            label_saper.ForeColor = Color.White;                                                         //Изменение цвета текста
        }

        //Обработка события клик по "Шахматы"
        private void pictureBox_chess_Click(object sender, EventArgs e)
        {
            string name_of_game = "chess";                                                               //Задаём имя игры
            settings_of_game sof = new settings_of_game(name_of_game, this);                             //Вызываем окно настроек с параметром названия игры
            sof.Owner = this;                                                                            //Указываем, что настройки игры - это дочерняя форма от листа с играми
            sof.ShowDialog();                                                                            //Показываем форму
        }

        //Метод для вызова определённого режима игры "Шахматы"
        public void chess_play(bool result)
        {
            Chess chess = new Chess(result);                                                           //Заготовка кнопки для "Шахмат", тут она уже готовая. 
            chess.Show();                                                                              //Остаётся только конструктор поменять 
        }

        //Обработка события наведения на "Шахматы"
        private void pictureBox_chess_MouseEnter(object sender, EventArgs e)
        {
            g = pictureBox_back.CreateGraphics();                                                        //Инициализация графики для заднего PictureBox
            g.DrawRectangle(new Pen(Color.FromArgb(100, 255, 165, 0), 3), 709, 37, 134, 134);            //Создание пера и отрисовка прямоугольника
            label_chess.Location = new Point(712, 173);                                                  //Корректируем положение названия
            label_chess.Font = new Font("Stencil", 10f, FontStyle.Bold);                                 //Изменяем стиль названия
            label_chess.ForeColor = Color.Gold;                                                          //Изменение цвета текста
        }

        //Обработка события покидание из "Шахматы"
        private void pictureBox_chess_MouseLeave(object sender, EventArgs e)
        {
            pictureBox_back.Image = null;                                                                //Удаления изображения
            label_chess.Location = new Point(712, 173);                                                  //Корректируем положение названия
            label_chess.Font = new Font("Stencil", 9.75f);                                               //Изменяем стиль названия
            label_chess.ForeColor = Color.White;                                                         //Изменение цвета текста
        }



        private void ListGames_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}

