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
        Graphics g;

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

            label_shaski.Location = new Point(280, 171);
            label_shaski.Font = new Font("Stencil", 9.75f);
        }

        private void pictureBox_tic_tac_Click(object sender, EventArgs e) {
            Krestiki_Noliki krestiki_Noliki = new Krestiki_Noliki(false);
            krestiki_Noliki.Show();
        }

        private void pictureBox_tic_tac_MouseEnter(object sender, EventArgs e) {
            g = CreateGraphics();
            Pen backPen = new Pen(Color.OrangeRed, 3);
            g.DrawRectangle(backPen, 39, 39, 129, 129);
            label_tic_tac.Location = new Point(36, 171);
            label_tic_tac.Font = new Font("Stencil", 10f, FontStyle.Bold);
        }

        private void pictureBox_tic_tac_MouseLeave(object sender, EventArgs e) {
            g = CreateGraphics();
            g.Clear(Color.White);
            label_tic_tac.Location = new Point(44, 171);
            label_tic_tac.Font = new Font("Stencil", 9.75f);
        }

        private void pictureBox_shaski_Click(object sender, EventArgs e)
        {
            Shashki shashki = new Shashki();
            shashki.Show();
        }

        private void pictureBox_shaski_MouseEnter(object sender, EventArgs e)
        {
            g = CreateGraphics();
            Pen backPen = new Pen(Color.OrangeRed, 3);
            g.DrawRectangle(backPen, 215, 39, 129, 129);    // Подобрать координаты
            label_shaski.Location = new Point(280, 171);
            label_shaski.Font = new Font("Stencil", 10f, FontStyle.Bold);
        }

        private void pictureBox_shaski_MouseLeave(object sender, EventArgs e)
        {
            g = CreateGraphics();
            g.Clear(Color.White);
            label_shaski.Location = new Point(280, 171);
            label_shaski.Font = new Font("Stencil", 9.75f);
        }

        private void pictureBox_Three_Row_Click(object sender, EventArgs e)
        {
            ThreeRow three = new ThreeRow();
            three.Show();
        }
    }
}
