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

        MainButtons MB;

        public MainMenu() {
            InitializeComponent();
            MB = new MainButtons(this);
        }

        private void MainMenu_Load(object sender, EventArgs e)
        {

        }
    }
}
