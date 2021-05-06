using GameOnHome_WINFORM.Online;
using System;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;

namespace GameOnHome_WINFORM
{
    public partial class ListGames : Form
    {
        private string ID;                          // ID присвоенное сервером
        private bool IsStatus = false;              // True - онлайн, False - оффлайн

        private const string host = "127.0.0.1";    // IP
        private const int port = 7770;              // Порт
        TcpClient client;                           // Клиент
        NetworkStream stream;                       // Поток от клиента до сервера

        Graphics g;

        public ListGames() {
            InitializeComponent();
        }

        private void ListGames_Load(object sender, EventArgs e)
        {

        }

        //Обработка события клик по "Крестики_Нолики"
        private void pictureBox_tic_tac_Click(object sender, EventArgs e) {
            string name_of_game = "tic_tac";                                          //Задаём имя игры
            settings_of_game sof = new settings_of_game(name_of_game);                //Вызываем окно настроек с параметром названия игры
            sof.Owner = this;                                                         //Указываем, что настройки игры - это дочерняя форма от листа с играми
            sof.ShowDialog();                                                         //Показываем форму
        }

        //Метод для вызова определённого режима игры "Крестики-Нолики"
        public void tic_tac_plqy(bool result) {
            Krestiki_Noliki krestiki_Noliki = new Krestiki_Noliki(result);
            krestiki_Noliki.Show();
        }

        //Обработка события наведение на "Крестики-нолики"
        private void pictureBox_tic_tac_MouseEnter(object sender, EventArgs e) {
            g = CreateGraphics();                                                     //Инициализируем графику
            Pen backPen = new Pen(Color.OrangeRed, 3);                                //Создаём кисть
            g.DrawRectangle(backPen, 37, 37, 134, 134);                               //Рисуем прямоугольник
            label_tic_tac.Location = new Point(36, 175);                              //Корректируем положение названия
            label_tic_tac.Font = new Font("Stencil", 10f, FontStyle.Bold);            //Изменяем стиль названия
        }

        //Обработка события покидание из "Крестики-Нолики"
        private void pictureBox_tic_tac_MouseLeave(object sender, EventArgs e) {
            g = CreateGraphics();                                                     //Инициализируем графику
            g.Clear(Color.White);                                                     //Стираем прямоугольник
            label_tic_tac.Location = new Point(44, 175);                              //Корректируем положение названия
            label_tic_tac.Font = new Font("Stencil", 9.75f);                          //Изменяем стиль названия
        }

        //Обработка события клик по "Шашки"
        private void pictureBox_shaski_Click(object sender, EventArgs e) {
            string name_of_game = "shaski";                                           //Задаём имя игры
            settings_of_game sof = new settings_of_game(name_of_game);                //Вызываем окно настроек с параметром названия игры
            sof.Owner = this;                                                         //Указываем, что настройки игры - это дочерняя форма от листа с играми
            sof.ShowDialog();                                                         //Показываем форму
        }

        //Метод для вызова определённого режима игры "Шашки"
        public void shaski_play(bool result) {
            Shashki shashki = new Shashki(result);       //Когда режимы шашек будут доделаны, нужно будет поставить "result" в конструктор создания Shashki()
            shashki.Show();
        }

        //Обработка события наведение на "Шашки"
        private void pictureBox_shaski_MouseEnter(object sender, EventArgs e) {
            g = CreateGraphics();                                                      //Инициализируем графику
            Pen backPen = new Pen(Color.OrangeRed, 3);                                 //Создаём кисть
            g.DrawRectangle(backPen, 205, 37, 134, 134);                               //Рисуем прямоугольник
            label_shaski.Location = new Point(245, 175);                               //Корректируем положение названия
            label_shaski.Font = new Font("Stencil", 10f, FontStyle.Bold);              //Изменяем стиль названия
        }

        //Обработка события покидания из "Шашки"
        private void pictureBox_shaski_MouseLeave(object sender, EventArgs e) {
            g = CreateGraphics();                                                       //Инициализируем графику
            g.Clear(Color.White);                                                       //Стираем прямоугольник
            label_shaski.Location = new Point(248, 175);                                //Корректируем положение названия
            label_shaski.Font = new Font("Stencil", 9.75f);                             //Изменяем стиль названия
        }

        //Обработка события клик на "3 в ряд"
        private void pictureBox_Three_Row_Click(object sender, EventArgs e) {
            ThreeRow three = new ThreeRow();
            three.Show();
        }

        //Обработка события наведения на "3 в ряд"
        private void pictureBox_Three_Row_MouseEnter(object sender, EventArgs e) {
            g = CreateGraphics();                                                        //Инициализируем графику
            Pen backPen = new Pen(Color.OrangeRed, 3);                                   //Создаём кисть
            g.DrawRectangle(backPen, 373, 37, 134, 134);                                 //Рисуем прямоугольник
            label_3ryd.Location = new Point(401, 175);                                   //Корректируем положение названия
            label_3ryd.Font = new Font("Stencil", 10f, FontStyle.Bold);                  //Изменяем стиль названия
        }

        //Обработка события покидание из "3 в ряд"
        private void pictureBox_Three_Row_MouseLeave(object sender, EventArgs e) {
            g = CreateGraphics();                                                         //Инициализируем графику
            g.Clear(Color.White);                                                         //Стираем прямоугольник
            label_3ryd.Location = new Point(406, 175);                                    //Корректируем положение названия
            label_3ryd.Font = new Font("Stencil", 9.75f);                                 //Изменяем стиль названия
        }

        //Обработка события клик по "Сапёр"
        private void pictureBox_saper_Click(object sender, EventArgs e)
        {
            Games.Miner miner = new Games.Miner();
            miner.Show();
        }
    }
}
