using System;
using System.Drawing;
using System.Windows.Forms;


namespace GameOnHome_WINFORM
{
    public partial class settings_of_game : Form
    {

        public bool butt_switch = false;
        public string name_of_game;
        public Form form;
        bool mouse_out_form = false;

        public settings_of_game(string name_of_game, Form form)
        {
            InitializeComponent();
            this.name_of_game = name_of_game;                            //Инициализируем переменную 
            this.form = form;
            this.MouseClick += new MouseEventHandler(mouse_click);
        }

        private void Settings_of_game_MouseClick(object sender, MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        //Обработка события вхождения на "Поехали!"
        private void But_Go_MouseEnter(object sender, EventArgs e)
        {
            But_Go.Font = new Font("Segoe UI", 30f, FontStyle.Bold);     //Изменение стиля названия
            But_Go.Location = new Point(185, 221);
        }

        //Обработка события покидание из "Поехали!"
        private void But_Go_MouseLeave(object sender, EventArgs e)
        {
            But_Go.Font = new Font("Segoe UI", 28f);                     //Изменение стиля названия
            But_Go.Location = new Point(200, 221);
        }

        //Обработка события клик на "Поехали!"
        private void But_Go_Click(object sender, EventArgs e)
        {
            ListGames main = this.Owner as ListGames;                    //Поределение предка формы
            if (main != null)                                            //Условие, если список игр не закрыт, выбора игры
            {
                if (name_of_game == "tic_tac")
                {
                    if (butt_switch == false)
                    {
                        main.tic_tac_plqy(false);
                    }
                    else
                    {
                        main.tic_tac_plqy(true);
                    }
                }
                if (name_of_game == "shaski")
                {
                    if (butt_switch == false)
                    {
                        main.shaski_play(false);
                    }
                    else
                    {
                        main.shaski_play(true);
                    }
                }
                if (name_of_game == "chess")
                {
                    if (butt_switch == false)
                    {
                        main.chess_play(false);
                    }
                    else
                    {
                        main.chess_play(true);
                    }
                }
            }
            form.Close();
            this.Close();
        }

        //Обработка события клик на элемент Switch
        private void switch_button1_Click(object sender, EventArgs e)
        {
            if (butt_switch == false)
            {
                butt_switch = true;
            }
            else
            {
                butt_switch = false;
            }
        }

        private void settings_of_game_Load(object sender, EventArgs e)
        {

        }

        private void settings_of_game_MouseLeave(object sender, EventArgs e)
        {

        }

        private void mouse_click(object sender, MouseEventArgs e)
        {
            //var location = e.Location;

            //PointF dpi = PointF.Empty;
            //using (Graphics g = this.CreateGraphics())
            //{
            //    dpi.X = g.DpiX;
            //    dpi.Y = g.DpiY;
            //}

            //float centrscreenX = dpi.X;
            //float centrscreenY = dpi.Y;

            //if (e.X < centrscreenX - 287.5 && e.Y < centrscreenY - 144 && e.X > centrscreenX + 287.5 && e.Y > centrscreenY + 144) {
            //    this.Close();
            //}
        }
    }
}
