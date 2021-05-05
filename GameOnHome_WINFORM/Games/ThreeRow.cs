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
    public partial class ThreeRow : Form
    {
        public ThreeRow()
        {
            InitializeComponent();
        }

        private void GameScreen_Load(object sender, EventArgs e)
        {
            Paint += delegate {
                gameField.Refresh();
            };

            m_timerCount = 60;
            m_timer = new Timer();
            m_timer.Interval = 1000;
            timerLabel.Text = (m_timerCount / 60).ToString("00") + ":" + (m_timerCount - 60 * (m_timerCount / 60)).ToString("00");
            m_timer.Tick += delegate
            {
                if (m_timerCount > 0)
                {
                    m_timerCount--;
                    timerLabel.Text = (m_timerCount / 60).ToString("00") + ":" + (m_timerCount - 60 * (m_timerCount / 60)).ToString("00");
                }
                //игра может завершиться, только когда активна
                else if (m_active)
                    Finish();
            };

            m_game = new LogicThreeRow(fieldSize, fieldSize, Enum.GetValues(typeof(ElementType)).Length);
            m_game.ElementRemoved += Game_ElementRemoved;
            m_game.MatchesRemoved += Game_MatchesRemoved;
            m_game.ElementsFalled += Game_ElementsFalled;
            m_game.FillMatrix();

            gameField.Paint += GameField_Paint;

            m_elementSize = Math.Min(gameField.Width, gameField.Height) / fieldSize;
            m_bitmaps = new Bitmap[5];
            m_elements = new VisualElement[fieldSize, fieldSize];

            UpdateBitmaps();
            InitElements();
            UpdateElements();
            scoreLabel.Text = "Score: " + m_game.GetScore().ToString();

            //FormBorderStyle = FormBorderStyle.None;
            //WindowState = FormWindowState.Maximized;

            Active = true;

            m_timer.Start();
        }

        private void GameField_Paint(object sender, PaintEventArgs e)
        {
            for (int y = 0; y < fieldSize; ++y)
                for (int x = 0; x < fieldSize; ++x)
                {
                    VisualElement el = m_elements[y, x];
                    if (el.BackColor != Color.Transparent)
                    {
                        SolidBrush b = new SolidBrush(el.BackColor);
                        e.Graphics.FillRectangle(b, el.Rectangle);
                    }
                    if (el.Image != null)
                        e.Graphics.DrawImage(el.Image, el.Rectangle);
                }
        }

        private void Game_ElementRemoved(int x, int y)
        {
            m_elements[y, x].Image = null;
        }

        private void Game_MatchesRemoved()
        {
            scoreLabel.Text = "Score: " + m_game.GetScore().ToString();
            //UpdateElements();
            m_game.Fall();
                //Active = true;
        }

        private void Game_ElementsFalled(List<Index> indices)
        {
            FallAnimation anim = new FallAnimation(this);
            List<VisualElement> elements = new List<VisualElement>();
            foreach (Index index in indices)
                elements.Add(m_elements[index.Y, index.X]);
            anim.AnimationEnd += delegate
            {
                if (m_game.Fall())
                {

                    //UpdateElements();
                }
                else if (m_game.RemoveMatches() == false)
                    Active = true;
            };
            UpdateElements();
            anim.Start(elements);
        }

        public void Finish()
        {
            m_active = false;
            MessageBox.Show("Время вышло. Очки: " + m_game.GetScore().ToString(), "Конец игры", MessageBoxButtons.OK);
        }

        public void UpdateBitmaps()
        {
            if (m_elementSize == 0)
                return;
            Size s = new Size(m_elementSize, m_elementSize);
            m_bitmaps[0] = new Bitmap(Properties.Resources.pink_icing_white_drizzle, s);
            m_bitmaps[1] = new Bitmap(Properties.Resources.blue_icing, s);
            m_bitmaps[2] = new Bitmap(Properties.Resources.green_icing_green_sprinkles, s);
            m_bitmaps[3] = new Bitmap(Properties.Resources.white_icing, s);
            m_bitmaps[4] = new Bitmap(Properties.Resources.chocolate_icing, s);
        }

        public void UpdateElements()
        {
            Point start = new Point((gameField.Width - m_elementSize * fieldSize) / 2,
                                    (gameField.Height - m_elementSize * fieldSize) / 2);
            for (int y = 0; y < fieldSize; ++y)
                for (int x = 0; x < fieldSize; ++x)
                {
                    m_elements[y, x].Location = new Point(start.X + x * m_elementSize, start.Y + y * m_elementSize);
                    m_elements[y, x].Size = new Size(m_elementSize, m_elementSize);
                    sbyte value = m_game.GetValue(x, y);
                    if (value >= 0)
                        m_elements[y, x].Image = m_bitmaps[m_game.GetValue(x, y)];
                    else
                        m_elements[y, x].Image = null;
                }
            //gameField.Refresh();
        }

        public void InitElements()
        {
            for (int y = 0; y < fieldSize; ++y)
                for (int x = 0; x < fieldSize; ++x)
                {
                    m_elements[y, x] = new VisualElement(this);
                    m_elements[y, x].Index = new Index(x, y);
                }
        }

        private void GameScreen_Resize(object sender, EventArgs e)
        {
            m_elementSize = Math.Min(gameField.Width, gameField.Height) / fieldSize;
            UpdateBitmaps();
            UpdateElements();
        }

        private void swapElements(Index a, Index b)
        {
            VisualElement tmpEl = m_elements[a.Y, a.X];
            m_elements[a.Y, a.X] = m_elements[b.Y, b.X];
            m_elements[b.Y, b.X] = tmpEl;

            m_elements[a.Y, a.X].Index = new Index(a.X, a.Y);
            m_elements[b.Y, b.X].Index = new Index(b.X, b.Y);
        }

        public void MoveElements(VisualElement a, VisualElement b)
        {
            Index aInd = new Index(a.Index);
            Index bInd = new Index(b.Index);
            m_game.Swap(aInd, bInd);
            swapElements(aInd, bInd);

            bool result = m_game.RemoveMatches();
            if (result == false)
            {
                SwapAnimation swap = new SwapAnimation(this);
                swap.AnimationEnd += delegate
                {
                    //возврат                
                    aInd = new Index(a.Index);
                    bInd = new Index(b.Index);
                    m_game.Swap(aInd, bInd);
                    swapElements(aInd, bInd);
                    Active = true;
                };
                swap.Start(b, a);
            }
        }

        private void GameScreen_KeyDown(object sender, KeyEventArgs e)
        {
            //выход на ESC
            if (e.KeyCode == Keys.Escape && m_active)
            {
                m_timer.Dispose();
                Close();
            }
        }

        private void gameField_MouseDown(object sender, MouseEventArgs e)
        {
            //определение элемента, на который нажали
            Point start = new Point((gameField.Width - m_elementSize * fieldSize) / 2,
                                    (gameField.Height - m_elementSize * fieldSize) / 2);
            Point pos = new Point(e.Location.X - start.X, e.Location.Y - start.Y);
            int col = pos.X / m_elementSize;
            int row = pos.Y / m_elementSize;
            if (col < fieldSize && row < fieldSize)
                m_elements[row, col].Click();
        }

        //размер можно поставить и больше
        const int fieldSize = 8;

        Bitmap[] m_bitmaps;
        VisualElement[,] m_elements;
        LogicThreeRow m_game;
        Timer m_timer;
        int m_elementSize;
        int m_timerCount;
        bool m_active;
        public bool Active
        {
            get { return m_active; }
            set
            {
                //игра может завершиться, только когда активна
                if (m_active == false && value == true && m_timerCount <= 0)
                    Finish();
                else
                    m_active = value;
            }
        }
    }

    public enum ElementType
    {
        Red = 0,
        Yellow,
        Green,
        Blue,
        White
    }

    public class VisualElement
    {
        public VisualElement(ThreeRow game) : base()
        {
            m_game = game;
        }

        public void Click()
        {
            if (m_game.Active == false)
                return;
            if (checkedElement != null)
            {
                if (checkedElement == this)
                {
                    //отмена выбора
                    m_backColor = Color.Transparent;
                    checkedElement = null;
                }
                else
                {
                    bool near = false;

                    if ((checkedElement.m_index.X == m_index.X - 1 && checkedElement.m_index.Y == m_index.Y) ||
                        (checkedElement.m_index.X == m_index.X + 1 && checkedElement.m_index.Y == m_index.Y) ||
                        (checkedElement.m_index.X == m_index.X && checkedElement.m_index.Y == m_index.Y - 1) ||
                        (checkedElement.m_index.X == m_index.X && checkedElement.m_index.Y == m_index.Y + 1))
                        near = true;

                    if (!near)
                    {
                        //Выбор другого элемента
                        checkedElement.m_backColor = Color.Transparent;
                        m_backColor = Color.DarkSlateBlue;
                        checkedElement = this;
                    }
                    else
                    {
                        //проверка двух соседних элементов
                        checkedElement.m_backColor = Color.Transparent;
                        m_game.Active = false;
                        VisualElement el = checkedElement;
                        checkedElement = null;
                        SwapAnimation swap = new SwapAnimation(m_game);
                        swap.AnimationEnd += delegate
                        {
                            m_game.MoveElements(el, this);
                        };
                        swap.Start(el, this);
                    }
                }
            }
            else
            {
                //выбор элемента
                m_backColor = Color.DarkSlateBlue;
                checkedElement = this;
            }
            m_game.Refresh();
        }

        public Index Index { get { return m_index; } set { m_index = new Index(value); } }
        public ElementType Type { get { return m_type; } set { m_type = value; } }
        public Point Location { get { return m_rect.Location; } set { m_rect = new Rectangle(value, m_rect.Size); } }
        public Size Size { get { return m_rect.Size; } set { m_rect = new Rectangle(m_rect.Location, value); } }
        public Rectangle Rectangle { get { return m_rect; } set { m_rect = value; } }
        public Color BackColor { get { return m_backColor; } set { m_backColor = value; } }
        public Image Image { get { return m_image; } set { m_image = value; } }

        Index m_index;
        ElementType m_type;
        ThreeRow m_game;
        Rectangle m_rect;
        Color m_backColor;
        Image m_image;

        static VisualElement checkedElement = null;
    };
}
