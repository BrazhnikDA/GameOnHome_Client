using GameOnHome_WINFORM.Online;
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

        private void ListGames_Load(object sender, EventArgs e)
        {
            g = CreateGraphics();
            g.Clear(Color.White);
            label_tic_tac.Location = new Point(44, 171);
            label_tic_tac.Font = new Font("Stencil", 9.75f);
        }

        private void pictureBox1_Click(object sender, EventArgs e) {
            Online.Krestiki_Noliki krestiki_Noliki = new Krestiki_Noliki(false);
            krestiki_Noliki.Show();
        }

        Graphics g;


        private void pictureBox1_MouseEnter(object sender, EventArgs e) {
            g = CreateGraphics();
            Pen backPen = new Pen(Color.OrangeRed, 3);
            g.DrawRectangle(backPen, 39, 39, 129, 129);
            label_tic_tac.Location = new Point(36, 171);
            label_tic_tac.Font = new Font("Stencil", 10f, FontStyle.Bold);
        }

        private void pictureBox1_MouseLeave(object sender, EventArgs e) {
            g = CreateGraphics();
            g.Clear(Color.White);
            label_tic_tac.Location = new Point(44, 171);
            label_tic_tac.Font = new Font("Stencil", 9.75f);
        }
    }
}
