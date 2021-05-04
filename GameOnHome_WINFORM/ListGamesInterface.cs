using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace GameOnHome_WINFORM
{
    class ListGamesInterface {

        ListGames parent;
        PictureBox pb;

        public ListGamesInterface(ListGames forms) {
            parent = forms;

            pb = new PictureBox();
            pb.Size = new Size(800, 800);
            pb.Location = new Point(0, 0);
            pb.Image = Image.FromFile(@"../../Resources/dr");

            parent.Controls.Add(pb);
        }
    }
}
