using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GameOnHome_WINFORM.setting_of_games
{
    public class switch_button : Control {

        Rectangle rect;
        int SwitchOn_X;
        int SwitchOff_X;

        //Метод переключения
        public bool Checked { get; set; } = false;

        public switch_button() {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            DoubleBuffered = true;                                              //Команды для оптимизации

            Size = new Size(40,15);                                             //размер элемента

            Font = new Font("Verdana", 9F, FontStyle.Regular);                  //Стиль элемента
            BackColor = Color.White;                                            //Задний фон

            rect = new Rectangle(1, 1, Width - 3, Height - 3);                  //Отрисовка элемента
            SwitchOff_X = rect.X;                                               //Координаты элемента при Off
            SwitchOn_X = rect.Width - rect.Height;                              //Координаты элемента при On
        }

        //Метод перерисовки при изменения размера
        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);                                              //Метод изменения размера               

            rect = new Rectangle(1, 1, Width - 3, Height - 3);                  //Перерисовка элемента
            SwitchOff_X = rect.X;                                               //Координаты элемента при Off
            SwitchOn_X = rect.Width - rect.Height;                              //Координаты элемента при On
        }

        //Отрисовка элемента
        protected override void OnPaint(PaintEventArgs e) {
            base.OnPaint(e);                                                    //Метод отрисовки

            Graphics graph = e.Graphics;                                        //Инициализация графики
            graph.SmoothingMode = SmoothingMode.HighQuality;                    //Установление качества
            graph.Clear(Parent.BackColor);                                      //Стерание

            Pen Tpen = new Pen(Color.Gray, 3);                                  //Создание кисти прямоугольника
            Pen STpen = new Pen(Color.Gray, 3);                                 //Создание кисти элипса

            GraphicsPath rectGP = RounderRectangle(rect, rect.Height);          //Создание согнутого прямоугольника
            Rectangle rectToggle = new Rectangle(rect.X, rect.Y, rect.Height, rect.Height);     //Создание элипса

            graph.DrawPath(Tpen, rectGP);                                       //Отрисовка согнутого прямоугольника

            if (Checked == true) {                                              //Отрисовка положение элипса при Onn
                rectToggle.Location = new Point(SwitchOn_X, rect.Y);
                graph.FillPath(new SolidBrush(Color.Green), rectGP);            //Заполнение пути при On
            }
            else {                                                              //Отрисовка положение элир=пса при Off
                rectToggle.Location = new Point(SwitchOff_X, rect.Y);
                graph.FillPath(new SolidBrush(Color.Red), rectGP);              //Заполнение пути при Off
            }

            graph.DrawEllipse(STpen, rectToggle);                               //Отрисовка элипса
            graph.FillEllipse(new SolidBrush(Color.White), rectToggle);         //Заполнение элипса
        }

        //Создание закруглённого прямоугольника
        private GraphicsPath RounderRectangle(Rectangle rect, int RoundSize) {
            GraphicsPath gp = new GraphicsPath();                                                                      //Инициализация графического пути

            gp.AddArc(rect.X, rect.Y, RoundSize, RoundSize, 180, 90);                                                  //Отрисовка верхнего-левого закруглёного угла
            gp.AddArc(rect.X + rect.Width - RoundSize, rect.Y, RoundSize, RoundSize, 270, 90);                         //Отрисовка верхнего-правого закруглёного угла
            gp.AddArc(rect.X + rect.Width - RoundSize, rect.Y + rect.Height - RoundSize, RoundSize, RoundSize, 0, 90); //Отрисовка нижнего-правого закруглённого угла
            gp.AddArc(rect.X, rect.Y + rect.Height - RoundSize, RoundSize, RoundSize, 90, 90);                         //Отрисовка нижнего-левого закруглённого угла

            gp.CloseFigure();                                                    //Закрываем фигуру

            return gp;
        }

        //Обработка события нажатия на элемент
        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);

            Checked = !Checked;
            Invalidate();
        }
    }
}
