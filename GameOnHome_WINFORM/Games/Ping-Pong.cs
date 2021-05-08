using System;
using System.Drawing;
using System.Windows.Forms;

namespace GameOnHome_WINFORM.Games
{
    public partial class Ping_Pong : Form
    {
        Graphics graphics;              // Для рисования
        Timer timer = new Timer();      // Таймер

        private Point MP;               // Для мышки
        private Point p;                // Для мышки

        int FPS = 35;                   // Количество отрисовок в секунду
        int player1y;                   // Значение платформы

        int ballx;                      // Координаты мяча по X
        int bally;                      // Координаты мяча по Y        
        int ballspdx = 10;              // Скорость по X
        int ballspdy = 10;              // Скорость по Y

        public Ping_Pong()
        {
            InitializeComponent();

            timer.Enabled = true;
            timer.Interval = 1000 / FPS;
            timer.Tick += new EventHandler(TimerCallback);

            ballx = this.Width / 2 - 10;
            bally = this.Height / 2 - 10;
        }

        private void MainFormPaint(object sender, PaintEventArgs e)
        {
            graphics = CreateGraphics();
            DrawRectangle(0, player1y, 20, 130, new SolidBrush(Color.Black));

            DrawRectangle(ballx, bally, 30, 30, new SolidBrush(Color.Red));
        }

        //Method for drawing rectangles
        private void DrawRectangle(int x, int y, int w, int h, SolidBrush Color)
        {
            graphics.FillRectangle(Color, new Rectangle(x, y, w, h));
        }

        //Ball border check	
        private void UpdateBall()
        {
            ballx += ballspdx;
            bally += ballspdy;

            if (ballx + 50 > this.Width)
            {
                ballspdx = -ballspdx;
            }

            if (bally < 0 || bally + 80 > this.Height)
            {
                ballspdy = -ballspdy;
            }

            if (IsCollided())
            {
                ballspdx = -ballspdx;
            }

            if (ballx < 0)
            {
                label1.Visible = true;
                timer.Stop();
            }
        }

        //Player and Ball collision
        private bool IsCollided()
        {
            if (ballx < 20 && bally > player1y && bally < player1y + 130)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //Update graphics
        private void TimerCallback(object sender, EventArgs e)
        {

            //Draw
            DrawRectangle(0, player1y, 20, 130, new SolidBrush(Color.Black));
            DrawRectangle(ballx, bally, 20, 20, new SolidBrush(Color.Black));
            UpdateBall();

            this.Invalidate();
            return;
        }

        //Keydown event
        private void MainFormKeyDown(object sender, KeyEventArgs e)
        {
            int key = e.KeyValue;
            //38 - up arrow
            //40 - down arrow
            if (key == 38)
            {
                player1y -= 15; //15 - moving speed
            }

            if (key == 40)
            {
                player1y += 15; //15 - moving speed
            }
        }

        private void Ping_Pong_MouseDown(object sender, MouseEventArgs e)
        {
            MP = PointToClient(MousePosition);
            p = new Point(1, MP.Y - button1.Location.Y);
        }

        private void Ping_Pong_MouseMove(object sender, MouseEventArgs e)
        {
            MP = PointToClient(MousePosition);
            Point d = new Point(1, MP.Y - p.Y);
            button1.Location = d;

            player1y = MP.Y - p.Y; 
        }
    }
}
