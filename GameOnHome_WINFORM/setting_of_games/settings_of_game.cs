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
    public partial class settings_of_game : Form 
    {

        public bool butt_switch = false;
        public string name_of_game;

        public settings_of_game(string name_of_game) {
            InitializeComponent();
            this.name_of_game = name_of_game;                            //Инициализируем переменную 
        }

        //Обработка события вхождения на "Поехали!"
        private void But_Go_MouseEnter(object sender, EventArgs e) {
            But_Go.Font = new Font("Segoe UI", 30f, FontStyle.Bold);     //Изменение стиля названия
        }

        //Обработка события покидание из "Поехали!"
        private void But_Go_MouseLeave(object sender, EventArgs e) {
            But_Go.Font = new Font("Segoe UI", 28f, FontStyle.Bold);     //Изменение стиля названия
        }

        //Обработка события клик на "Поехали!"
        private void But_Go_Click(object sender, EventArgs e) {
            ListGames main = this.Owner as ListGames;                    //Поределение предка формы
            if (main != null)                                            //Условие, если список игр не закрыт, выбора игры
            {
                if (name_of_game == "tic_tac") {                         
                    if (butt_switch == false) {
                        main.tic_tac_plqy(false);
                    }
                    else {
                        main.tic_tac_plqy(true);
                    }
                }
                if (name_of_game == "shaski") {
                    if (butt_switch == false) {
                        main.shaski_play(false);
                    }
                    else {
                        main.shaski_play(true);
                    }
                }
            }
        }

        //Обработка события клик на элемент Switch
        private void switch_button1_Click(object sender, EventArgs e) {
            if (butt_switch == false) {
                butt_switch = true;
            }
            else {
                butt_switch = false;
            }
        }
    }
}
