using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOnHome_WINFORM.Games
{
    public partial class Chess : Form
    {

        public const int mapSize = 8;


        public int[,] map = new int[mapSize, mapSize];   // Карта

        public Button[,] buttons = new Button[mapSize, mapSize];  // Все кнопки

        public int cellSize = 150;

        public int currentPlayer = 0;                  // Текущий игрок

        public Button pressedButton;                // Нажатая кнопка
        public Button prevButton;                   // Предыдущая нажатая кнопка

        public bool isMoving = false;               // Есть ли куда сходить

        public Image whiteKing;                      
        public Image whiteQueen;                      
        public Image whiteElephant;                      
        public Image whiteHorse;                      
        public Image whiteTower;                      
        public Image whitePawn;

        public Image blackKing;
        public Image blackQueen;
        public Image blackElephant;
        public Image blackHorse;
        public Image blackTower;
        public Image blackPawn;

        public Chess()
        {
            InitializeComponent();

            Initialization();
            InitImage();
            CreateMap();
        }

        public void Initialization()
        {
            // Белые:  0 - пустота, 11 - король, 12 - королева, 13 - слон, 14 - конь, 15 - ладья, 16 - пешка
            // Чёрные: 0 - пустота, 21 - король, 22 - королева, 23 - слон, 24 - конь, 25 - ладья, 26 - пешка 
            map = new int[mapSize, mapSize]
            {
            {15,14,13,12,11,13,14,15 },
            {16,16,16,16,16,16,16,16 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {0,0,0,0,0,0,0,0 },
            {26,26,26,26,26,26,26,26 },
            {25,24,23,22,21,23,24,25 },
            };
        }

        public void InitImage()
        {
          whiteKing = Properties.Resources.whiteKing;
          whiteQueen = Properties.Resources.whiteQuen;
          whiteElephant = Properties.Resources.whiteElephant;
          whiteHorse = Properties.Resources.whiteHorse;
          whiteTower = Properties.Resources.whiteTower;
          whitePawn = Properties.Resources.whitePawn;

          blackKing = Properties.Resources.blackKing;
          blackQueen = Properties.Resources.blackQuen;
          blackElephant = Properties.Resources.blackElephant;
          blackHorse = Properties.Resources.blackHorse;
          blackTower = Properties.Resources.blackTower;
          blackPawn = Properties.Resources.blackPawn;
    }

        public void CreateMap()
        {
            this.Width = (mapSize + 1) * cellSize - 120;
            this.Height = (mapSize + 1) * cellSize - 88;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    Button butt = new Button();
                    butt.Size = new Size(cellSize, cellSize);
                    butt.Location = new Point(j * cellSize, i * cellSize);
                    butt.BackColor = Color.White;
                    butt.Click += new EventHandler(FigureClick);
                    butt.Name = "button" + i.ToString() + "_" + j.ToString();
                    butt.Image = GetImage(map[i, j]);

                    this.Controls.Add(butt);

                    buttons[i, j] = butt;
                }
            }
        }

        public void FigureClick(object sender, EventArgs e)
        {
            if(currentPlayer == 0) { currentPlayer = 1; }

            pressedButton = sender as Button;

            if (GetColorFigure(CheckMap(ConvertNameI(pressedButton), ConvertNameY(pressedButton))) != 0 &&
                GetColorFigure(CheckMap(ConvertNameI(pressedButton), ConvertNameY(pressedButton))) == currentPlayer)
            {
                CloseSteps(pressedButton);
                pressedButton.BackColor = Color.Red;        // Выделяем нажатую кнопку красным
                ShowSteps();
            }
        }

        public void ShowSteps()
        {

        }

        // Скрываем шаги на котрые не можем сходить
        public void CloseSteps(Button button)
        {
            int ii = ConvertNameI(button);
            int jj = ConvertNameY(button);
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (i == ii && j == jj)
                    {
                        continue;
                    }
                    buttons[i, j].Enabled = false;
                }
            }
        }

        public Image GetImage(int type) // Белые:  0 - пустота, 11 - король, 12 - королева, 13 - слон, 14 - конь, 15 - ладья, 16 - пешка
        {
            switch(type)
            {
                // White
                case 11:
                    return whiteKing;
                case 12:
                    return whiteQueen;
                case 13:
                    return whiteElephant;
                case 14:
                    return whiteHorse;
                case 15:
                    return whiteTower;
                case 16:
                    return whitePawn;

                // Black
                case 21:
                    return blackKing;
                case 22:
                    return blackQueen;
                case 23:
                    return blackElephant;
                case 24:
                    return blackHorse;
                case 25:
                    return blackTower;
                case 26:
                    return blackPawn;
                default:
                    return null;
            }
        }

        // Цвет выбранной фигуры
        public int GetColorFigure(int val)
        {
            return val / 10;
        }

        // Тип выбранной фигуры
        public int GetTypeFigure(int val)
        {
            return val % 10;
        }

        // Возвращает фигуру находящуюся по переданным координатам
        public int CheckMap(int i, int j)
        {
            if (i < mapSize && j < mapSize)
            {
                return map[i, j];
            }
            else { return 0; }
        }

        // Получить позицию кнопки по I
        public int ConvertNameI(Button b)
        {
            string sym = b.Name;
            return Convert.ToInt32(sym[6]) - 48;
        }

        // Получить позицию кнопки по Y
        public int ConvertNameY(Button b)
        {
            string sym = b.Name;
            return Convert.ToInt32(sym[8]) - 48;
        }
        public bool IsInsideBorders(int i, int j)
        {
            if (i >= mapSize || j >= mapSize || i < 0 || j < 0)
            {
                return false;   // Находится
            }
            return true; // Не находится
        }

        // Пройтись по всем кнопкам и сделать их активными
        public void ActivateAllButtons()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].Enabled = true;
                }
            }
        }

        // Пройтись по всем кнопкам и сделать их не активными
        public void DeactivateAllButtons()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].Enabled = false;
                }
            }
        }

    }
}
