using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOnHome_WINFORM
{
    public partial class MainMenu : Form {

        public MainMenu() {
            InitializeComponent();
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }

        //Обработка события клик по "Играть"
        private void But_Play_Click(object sender, EventArgs e)                         
        {
            ListGames lg = new ListGames();                                              //Инициализация новой формы
            lg.Show();                                                                   //Показ новой формы
        }

        //Обработка события вхождение в "Играть"
        private void But_Play_MouseEnter(object sender, EventArgs e)                     
        { 
            But_Play.Font = new Font("Comic Sans MS", 23f, FontStyle.Bold);              //Создание нового cnbkz
        }

        //Обработка события покидания из "Играть"
        private void But_Play_MouseLeave(object sender, EventArgs e)                     
        {
            But_Play.Font = new Font("Comic Sans MS", 22f);                              //Создания новго стиля
        }

        //Обработка события клик по "Настройки"
        private void But_Setting_Click(object sender, EventArgs e)                       
        {
            Settings settings = new Settings();                                          //Инициализация новой формы
            settings.Show();                                                             //Показ новой формы
        }

        //Обработка события вхождения в "Настройки"
        private void Bur_Setting_MouseEnter(object sender, EventArgs e)            
        {
            But_Setting.Font = new Font("Comic Sans MS", 21f, FontStyle.Bold);           //Создание новго стиля
        }

        //Обработка события покидания из "Настройи"
        private void Bur_Setting_MouseLeave(object sender, EventArgs e)
        {
            But_Setting.Font = new Font("Comic Sans MS", 22f);                           //Создание новго стиля
        }

        //Обарботка события клик по "Выход"
        private void But_Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();                                                          //Конец события
        }

        //Обработка события вхождение в "Выход"
        private void But_Exit_MouseEnter(object sender, EventArgs e)
        {
            But_Exit.Font = new Font("Comic Sans MS", 23f, FontStyle.Bold);              //Создание новго стиля
        }

        //Обработка события покидания из "Выход"
        private void But_Exit_MouseLeave(object sender, EventArgs e)
        {
            But_Exit.Font = new Font("Comic Sans MS", 22f);                              //Создание новго стиля
        }
    }
}
