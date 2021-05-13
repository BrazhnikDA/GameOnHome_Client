using GameOnHome_WINFORM.Games;
using System;
using System.Windows.Forms;

namespace GameOnHome_WINFORM.end_of_game
{
    public partial class end_of_game : Form
    {
        string isStatus;
        Form selectForm;

        // False - проиграл, True - выиграл
        public end_of_game(string _isStatus, Form form)
        {
            InitializeComponent();
            buttonRestart.Parent = pictureBox_eog_back;
            buttonExit.Parent = pictureBox_eog_back;
            label1.Parent = pictureBox_eog_back;
            isStatus = _isStatus;
            selectForm = form;
            //Form SF = this.Owner as Form;
            //selectForm = SF;

        }

        private void end_of_game_Load(object sender, EventArgs e)
        {
            switch (isStatus)
            {
                case "Win":
                    pictureBox_eog_back.Image = Properties.Resources.win;
                    break;
                case "Fail":
                    pictureBox_eog_back.Image = Properties.Resources.fail;
                    break;
                case "Draw":
                    pictureBox_eog_back.Image = Properties.Resources.draw;
                    break;
                case "Score":
                    //int score = selectForm.score;
                    //label1.Text = "Счёт: " + selectForm.score;
                    label1.Text = "Счёт: ";
                    pictureBox_eog_back.Image = Properties.Resources.scoreTime_back;
                    break;
                case "Time":
                    //int score = selectForm.score;
                    //label1.Text = "Счёт: " + selectForm.score;
                    label1.Text = "Время: ";
                    pictureBox_eog_back.Image = Properties.Resources.scoreTime_back;
                    break;
            }
        }

        public void buttonRestart_Click(object sender, EventArgs e)
        {
            string name = this.Owner.Name;

            switch (name)
            {
                case "Krestiki_Noliki":
                    Krestiki_Noliki tic_tac = this.Owner as Krestiki_Noliki;
                    tic_tac.Restart();
                    break;
                case "Shashki":
                    Shashki shashki = this.Owner as Shashki;
                    shashki.Restart();
                    break;
                case "Chess":
                    Chess chess = this.Owner as Chess;
                    chess.Restart();
                    break;
                case "Miner":
                    Miner miner = this.Owner as Miner;
                    if (miner != null)
                    {
                        miner.Close();
                        selectForm = new Miner();
                    }
                    break;
                case "ThreeRow":
                    ThreeRow threeRow = this.Owner as ThreeRow;
                    if (threeRow != null)
                    {
                        threeRow.Close();
                        selectForm = new ThreeRow();
                    }
                    break;
            }
            selectForm.Show();
            this.Close();
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {
            selectForm.Close();
            this.Close();
            ListGames LG = new ListGames();
            LG.Show();
        }
    }
}
