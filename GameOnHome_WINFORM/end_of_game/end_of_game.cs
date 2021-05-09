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
        string nameGame;
        // False - проиграл, True - выиграл
        public end_of_game(bool _isStatus, string _nameGame)
        {
            InitializeComponent();
            isStatus = _isStatus;
            nameGame = _nameGame;
        }

        private void end_of_game_Load(object sender, EventArgs e)
        {
            if(isStatus) { this.BackgroundImage = Properties.Resources.Вы_выиграли; }
            else { this.BackgroundImage = Properties.Resources.Вы_проиграли; }
        }

        public void buttonRestart_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonExit_Click(object sender, EventArgs e)
        {

        }
    }
}
