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
    public partial class ListGames : Form
    {
        public ListGames()
        {
            InitializeComponent();
        }

        private void pictureBoxKrestiki_Noliki_Click(object sender, EventArgs e)
        {
            Online.Online_Krestiki_Noliki krestiki_Noliki = new Online.Online_Krestiki_Noliki();
            krestiki_Noliki.Show();
            this.Hide();
        }
    }
}
