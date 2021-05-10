using GameOnHome_WINFORM.Games;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOnHome_WINFORM.end_of_game
{
    public partial class end_of_game : Form
    {
        bool isStatus;
        public Form selectForm;
        // False - проиграл, True - выиграл
        public end_of_game(bool _isStatus, Form form)
        {
            InitializeComponent();
            buttonRestart.Parent = pictureBox_eog_back;
            buttonExit.Parent = pictureBox_eog_back;
            isStatus = _isStatus;
            selectForm = form;
        }

        private void end_of_game_Load(object sender, EventArgs e)
        {
            if(isStatus) { pictureBox_eog_back.Image = Properties.Resources.win; }
            else { pictureBox_eog_back.Image = Properties.Resources.fail; }
        }

        public void buttonRestart_Click(object sender, EventArgs e)
        {
            string name = this.Owner.Name;

            switch (name)
            {
                case "Krestiki_Noliki":
                    Krestiki_Noliki tic_tac = this.Owner as Krestiki_Noliki;
                    if (tic_tac != null)
                    {
                        tic_tac.Close();
                        selectForm = new Krestiki_Noliki(tic_tac.GetStatus);
                    }
                    break;
                case "Shashki":
                    Shashki shashki = this.Owner as Shashki;
                    if (shashki != null)
                    {
                        shashki.Close();
                        selectForm = new Shashki(shashki.GetStatus);
                    }
                    break;
                case "Chess":
                    Chess chess = this.Owner as Chess;
                    if (chess != null)
                    {
                        chess.Close();
                        selectForm = new Chess(chess.GetStatus);
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
