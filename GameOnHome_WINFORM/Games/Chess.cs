using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

// ДОБАВТЬТТ ИЗМЕНЕНИЕ КАРТЫ ПРИ СЪЕДЕНИИ ФИГУРЫ!

namespace GameOnHome_WINFORM.Games
{
    public partial class Chess : Form
    {
        private string ID;                          // ID присвоенное сервером
        private bool IsStatus = false;              // True - онлайн, False - оффлайн

        private const string host = "127.0.0.1";    // IP
        private const int port = 7770;              // Порт
        TcpClient client;                           // Клиент
        NetworkStream stream;                       // Поток от клиента до сервера

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

        public Chess(bool IsStatus_)
        {
            InitializeComponent();

            IsStatus = IsStatus_;

            Initialization();
            InitImage();
            CreateMap();

            if (IsStatus)
            {
                Server_Connect();       // Функция для подключеняи к серверу
                Thread.Sleep(100);      // Ожидаем подключения
            }
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
          whiteQueen = Properties.Resources.whiteQueen;
          whiteElephant = Properties.Resources.whiteElephant;
          whiteHorse = Properties.Resources.whiteHorse;
          whiteTower = Properties.Resources.whiteTower;
          whitePawn = Properties.Resources.whitePawn;

          blackKing = Properties.Resources.blackKing;
          blackQueen = Properties.Resources.blackQueen;
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
                    butt.Name = "button" + i.ToString() + "_" + j.ToString();
                    butt.Size = new Size(cellSize, cellSize);
                    butt.Location = new Point(j * cellSize, i * cellSize);
                    butt.BackColor = GetPrevButtonColor(butt);
                    butt.Click += new EventHandler(FigureClick);
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
                CloseSteps();
                pressedButton.BackColor = Color.Red;        // Выделяем нажатую кнопку красным
                pressedButton.Enabled = true;
                ShowSteps(ConvertNameI(pressedButton), ConvertNameY(pressedButton));

                if (isMoving)
                {
                    CloseSteps();           // Закрываем все шаги 
                    pressedButton.BackColor = GetPrevButtonColor(pressedButton);
                    ShowSteps(ConvertNameI(pressedButton), ConvertNameY(pressedButton));    // Показываем куда можем сходить
                    isMoving = false;
                }
                else isMoving = true;
            }else
            {
                if(isMoving)
                {
                    if(pressedButton.BackColor == Color.Red)
                    {
                        pressedButton.BackColor = GetPrevButtonColor(pressedButton);
                        CloseSteps();
                    }
                    if(pressedButton.BackColor == Color.Yellow)
                    {
                        map[ConvertNameI(pressedButton), ConvertNameY(pressedButton)] = map[ConvertNameI(prevButton), ConvertNameY(prevButton)];
                        map[ConvertNameI(prevButton), ConvertNameY(prevButton)] = 0;
                        pressedButton.Image = prevButton.Image;
                        prevButton.Image = null;
                        prevButton.BackColor = Color.White;

                        isMoving = false;
                        CloseSteps();
                        DeactivateAllButtons();
                        SendMessage(GetMap() + currentPlayer);
                        CheckWin();
                    }
                }
            }
            prevButton = pressedButton;
        }

        public void ShowSteps(int i, int j)
        {
            // false - black , true - white
            switch (map[i, j])
            {
                case 11:
                    ShowStepsKing(i, j, 2);
                    break;
                case 12:
                    ShowStepsQueen(i, j, 2);
                    break;
                case 13:
                    ShowStepsElephant(i, j, 2);
                    break;
                case 14:
                    ShowStepsHorse(i, j, 2);
                    break;
                case 15:
                    ShowStepsTower(i, j, 2);
                    break;
                case 16:
                    ShowEatStepsPawn(i, j, true);
                    ShowStepsPawn(i, j, true);
                    break;

                case 21:
                    ShowStepsKing(i, j, 1);
                    break;
                case 22:
                    ShowStepsQueen(i, j, 1);
                    break;
                case 23:
                    ShowStepsElephant(i, j, 1);
                    break;
                case 24:
                    ShowStepsHorse(i, j, 1);
                    break;
                case 25:
                    ShowStepsTower(i, j, 1);
                    break;
                case 26:
                    ShowEatStepsPawn(i, j, false);
                    ShowStepsPawn(i, j, false);
                    break;
            }
        }

        public void ShowStepsKing(int i, int j, int Figure)
        {
            if (IsInsideBorders(i + 1, j))
            {
                if (map[i + 1, j] == 0 || GetColorFigure(map[i + 1, j]) == Figure)
                {
                    buttons[i + 1, j].Enabled = true;
                    buttons[i + 1, j].BackColor = Color.Yellow;
                }
            }

            if (IsInsideBorders(i - 1, j))
            {
                if (map[i - 1, j] == 0 || GetColorFigure(map[i - 1, j]) == Figure)
                {
                    buttons[i - 1, j].Enabled = true;
                    buttons[i - 1, j].BackColor = Color.Yellow;
                }
            }

            if (IsInsideBorders(i, j + 1))
            {
                if (map[i, j + 1] == 0 || GetColorFigure(map[i, j + 1]) == Figure)
                {
                    buttons[i, j + 1].Enabled = true;
                    buttons[i, j + 1].BackColor = Color.Yellow;
                }
            }

            if (IsInsideBorders(i, j - 1))
            {
                if (map[i, j - 1] == 0 || GetColorFigure(map[i, j - 1]) == Figure)
                {
                    buttons[i, j - 1].Enabled = true;
                    buttons[i, j - 1].BackColor = Color.Yellow;
                }
            }

            if (IsInsideBorders(i + 1, j + 1))
            {
                if (map[i + 1, j + 1] == 0 || GetColorFigure(map[i + 1, j + 1]) == Figure)
                {
                    buttons[i + 1, j + 1].Enabled = true;
                    buttons[i + 1, j + 1].BackColor = Color.Yellow;
                }
            }

            if (IsInsideBorders(i - 1, j + 1))
            {
                if (map[i - 1, j + 1] == 0 || GetColorFigure(map[i - 1, j + 1]) == Figure)
                {
                    buttons[i - 1, j + 1].Enabled = true;
                    buttons[i - 1, j + 1].BackColor = Color.Yellow;
                }
            }

            if (IsInsideBorders(i + 1, j - 1))
            {
                if (map[i + 1, j - 1] == 0 || GetColorFigure(map[i + 1, j - 1]) == Figure)
                {
                    buttons[i + 1, j - 1].Enabled = true;
                    buttons[i + 1, j - 1].BackColor = Color.Yellow;
                }
            }

            if (IsInsideBorders(i - 1, j - 1))
            {
                if (map[i - 1, j - 1] == 0 || GetColorFigure(map[i - 1, j - 1]) == Figure)
                {
                    buttons[i - 1, j - 1].Enabled = true;
                    buttons[i - 1, j - 1].BackColor = Color.Yellow;
                }
            }

            
        }

        public void ShowStepsQueen(int i, int j, int Figure)
        {
            ShowStepsElephant(i, j, Figure);
            ShowStepsTower(i, j, Figure);
        }

        public void ShowStepsElephant(int i, int j, int Figure)
        {
            int ii = i - 1;
            int jj = j - 1;
            while (IsInsideBorders(ii, jj))          // Вверх-влево
            {
                if (map[ii, jj] == 0 || GetColorFigure(map[ii, jj]) == Figure)
                {
                    buttons[ii, jj].Enabled = true;
                    buttons[ii, jj].BackColor = Color.Yellow;
                    if(GetColorFigure(map[ii, jj]) == Figure) { break; }
                }
                else { break; }
                ii--; jj--;
            }

            ii = i + 1;
            jj = j - 1;
            while (IsInsideBorders(ii, jj))          // Вниз-влево
            {
                if (map[ii, jj] == 0 || GetColorFigure(map[ii, jj]) == Figure)
                {
                    buttons[ii, jj].Enabled = true;
                    buttons[ii, jj].BackColor = Color.Yellow;
                    if (GetColorFigure(map[ii, jj]) == Figure) { break; }
                }
                else { break; }
                ii++; jj--;
            }

            ii = i - 1;
            jj = j + 1;
            while (IsInsideBorders(ii, jj))          // Вверх-вправо
            {
                if (map[ii, jj] == 0 || GetColorFigure(map[ii, jj]) == Figure)
                {
                    buttons[ii, jj].Enabled = true;
                    buttons[ii, jj].BackColor = Color.Yellow;
                    if (GetColorFigure(map[ii, jj]) == Figure) { break; }
                }
                else { break; }
                ii--; jj++;
            }

            ii = i + 1;
            jj = j + 1;
            while (IsInsideBorders(ii, jj))          // Вниз-вправо
            {
                if (map[ii, jj] == 0 || GetColorFigure(map[ii, jj]) == Figure)
                {
                    buttons[ii, jj].Enabled = true;
                    buttons[ii, jj].BackColor = Color.Yellow;
                    if (GetColorFigure(map[ii, jj]) == Figure) { break; }
                }
                else { break; }
                ii++; jj++;
            }
        }

        public void ShowStepsHorse(int i, int j, int Figure)
        {
            if (IsInsideBorders(i - 2, j + 1))
            {
                if (map[i - 2, j + 1] == 0 || GetColorFigure(map[i - 2, j + 1]) == Figure)
                {
                    buttons[i - 2, j + 1].Enabled = true;
                    buttons[i - 2, j + 1].BackColor = Color.Yellow;
                }
            }
            if (IsInsideBorders(i - 2, j - 1))
            {
                if (map[i - 2, j - 1] == 0 || GetColorFigure(map[i - 2, j - 1]) == Figure)
                {
                    buttons[i - 2, j - 1].Enabled = true;
                    buttons[i - 2, j - 1].BackColor = Color.Yellow;
                }
            }
            if (IsInsideBorders(i + 2, j + 1))
            {
                if (map[i + 2, j + 1] == 0 || GetColorFigure(map[i + 2, j + 1]) == Figure)
                {
                    buttons[i + 2, j + 1].Enabled = true;
                    buttons[i + 2, j + 1].BackColor = Color.Yellow;
                }
            }
            if (IsInsideBorders(i + 2, j - 1))
            {
                if (map[i + 2, j - 1] == 0 || GetColorFigure(map[i + 2, j - 1]) == Figure)
                {
                    buttons[i + 2, j - 1].Enabled = true;
                    buttons[i + 2, j - 1].BackColor = Color.Yellow;
                }
            }
            if (IsInsideBorders(i - 1, j + 2))
            {
                if (map[i - 1, j + 2] == 0 || GetColorFigure(map[i - 1, j + 2]) == Figure)
                {
                    buttons[i - 1, j + 2].Enabled = true;
                    buttons[i - 1, j + 2].BackColor = Color.Yellow;
                }
            }
            if (IsInsideBorders(i + 1, j + 2))
            {
                if (map[i + 1, j + 2] == 0 || GetColorFigure(map[i + 1, j + 2]) == Figure)
                {
                    buttons[i + 1, j + 2].Enabled = true;
                    buttons[i + 1, j + 2].BackColor = Color.Yellow;
                }
            }
            if (IsInsideBorders(i - 1, j - 2))
            {
                if (map[i - 1, j - 2] == 0 || GetColorFigure(map[i - 1, j - 2]) == Figure)
                {
                    buttons[i - 1, j - 2].Enabled = true;
                    buttons[i - 1, j - 2].BackColor = Color.Yellow;
                }
            }
            if (IsInsideBorders(i + 1, j - 2))
            {
                if (map[i + 1, j - 2] == 0 || GetColorFigure(map[i + 1, j - 2]) == Figure)
                {
                    buttons[i + 1, j - 2].Enabled = true;
                    buttons[i + 1, j - 2].BackColor = Color.Yellow;
                }
            }
        }

        public void ShowStepsTower(int i, int j, int Figure)
        {

            int jj = j + 1;
            while (IsInsideBorders(i, jj))          // вправо
            {
                if (map[i, jj] == 0 || GetColorFigure(map[i, jj]) == Figure)
                {
                    buttons[i, jj].Enabled = true;
                    buttons[i, jj].BackColor = Color.Yellow;
                    if (GetColorFigure(map[i, jj]) == Figure) { break; }
                }
                else { break; }
                jj++;
            }
            jj = j - 1;
            while (IsInsideBorders(i, jj))          // влево
            {
                if (map[i, jj] == 0 || GetColorFigure(map[i, jj]) == Figure)
                {
                    buttons[i, jj].Enabled = true;
                    buttons[i, jj].BackColor = Color.Yellow;
                    if (GetColorFigure(map[i, jj]) == Figure) { break; }
                }
                else { break; }
                jj--;
            }

            int ii = i + 1;
            while (IsInsideBorders(ii, j))          // вниз
            {
                if (map[ii, j] == 0 || GetColorFigure(map[ii, j]) == Figure)
                {
                    buttons[ii, j].Enabled = true;
                    buttons[ii, j].BackColor = Color.Yellow;
                    if (GetColorFigure(map[ii, j]) == Figure) { break; }
                }
                else { break; }
                ii++;
            }

            ii = i - 1;
            while (IsInsideBorders(ii, j))          // вверх
            {
                if (map[ii, j] == 0 || GetColorFigure(map[ii, j]) == Figure)
                {
                    buttons[ii, j].Enabled = true;
                    buttons[ii, j].BackColor = Color.Yellow;
                    if (GetColorFigure(map[ii, j]) == Figure) { break; }
                }
                else { break; }
                ii--;
            }
        }

        public void ShowEatStepsPawn(int i, int j, bool IsColor)
        {
            if(IsColor)
            {
                int ii = i + 1;
                int jj = j - 1;
                if (IsInsideBorders(ii, jj))
                {
                    if (GetColorFigure(map[ii, jj]) == 2)
                    {
                        buttons[ii, jj].Enabled = true;
                        buttons[ii, jj].BackColor = Color.Yellow;
                    }
                }

                ii = i + 1;
                jj = j + 1;
                if (IsInsideBorders(ii, jj))
                {
                    if (GetColorFigure(map[ii, jj]) == 2)
                    {
                        buttons[ii, jj].Enabled = true;
                        buttons[ii, jj].BackColor = Color.Yellow;
                    }
                }
            }
            else
            {
                int ii = i - 1;
                int jj = j - 1;
                if (IsInsideBorders(ii, jj))
                {
                    if (GetColorFigure(map[ii, jj]) == 2)
                    {
                        buttons[ii, jj].Enabled = true;
                        buttons[ii, jj].BackColor = Color.Yellow;
                    }
                }

                ii = i - 1;
                jj = j + 1;
                if (IsInsideBorders(ii, jj))
                {
                    if (GetColorFigure(map[ii, jj]) == 2)
                    {
                        buttons[ii, jj].Enabled = true;
                        buttons[ii, jj].BackColor = Color.Yellow;
                    }
                }
            }
        }

        public void ShowStepsPawn(int i, int j, bool IsColor)
        {
            if(IsColor)
            {
                // На своей половине
                if (i < 3)
                {
                    int ii = i + 1;
                    int iii = i + 2;
                    if (IsInsideBorders(ii, j))
                    {
                        if (map[ii, j] == 0)
                        {
                            buttons[ii, j].Enabled = true;
                            buttons[ii, j].BackColor = Color.Yellow;
                            if (IsInsideBorders(iii, j))
                            {
                                if (iii > 3) { return; }
                                if (map[iii, j] == 0)
                                {
                                    buttons[iii, j].Enabled = true;
                                    buttons[iii, j].BackColor = Color.Yellow;
                                }
                            }
                        }
                    }
                }
                else
                {
                    int ii = i + 1;
                    if (IsInsideBorders(ii, j))
                    {
                        if (map[ii, j] == 0)
                        {
                            buttons[ii, j].Enabled = true;
                            buttons[ii, j].BackColor = Color.Yellow;
                        }
                    }
                }
            }
            else
            {
                int ii = i - 1;
                int jj = j - 1;
                if (IsInsideBorders(ii, jj))
                {
                    if (GetColorFigure(map[ii, jj]) == 1)
                    {
                        buttons[ii, jj].Enabled = true;
                        buttons[ii, jj].BackColor = Color.Yellow;
                    }
                }

                ii = i - 1;
                jj = j + 1;
                if (IsInsideBorders(ii, jj))
                {
                    if (GetColorFigure(map[ii, jj]) == 1)
                    {
                        buttons[ii, jj].Enabled = true;
                        buttons[ii, jj].BackColor = Color.Yellow;
                    }
                }

                // Чёрные пешки
                if (i > 3)
                {
                    ii = i - 1;
                    int iii = i - 2;
                    if (IsInsideBorders(ii, j))
                    {
                        if (map[ii, j] == 0)
                        {
                            buttons[ii, j].Enabled = true;
                            buttons[ii, j].BackColor = Color.Yellow;
                            if (IsInsideBorders(iii, j))
                            {
                                if (iii < 3) { return; }
                                if (map[iii, j] == 0)
                                {
                                    buttons[iii, j].Enabled = true;
                                    buttons[iii, j].BackColor = Color.Yellow;
                                }
                            }
                        }
                    }
                }
                else
                {
                    ii = i - 1;
                    //jj;
                    if (IsInsideBorders(ii, j))
                    {
                        if (map[ii, j] == 0)
                        {
                            buttons[ii, j].Enabled = true;
                            buttons[ii, j].BackColor = Color.Yellow;
                        }
                    }
                }
            }
        }

        public void CheckWin()
        {
            bool IsLiveWhite = false;
            bool IsLiveBlack = false;

            for(int i = 0; i < mapSize; i++)
            {
                for(int j = 0; j < mapSize; j++)
                {
                    if(map[i,j] == 11)
                    {
                        IsLiveWhite = true;
                    }
                    if(map[i,j] == 21)
                    {
                        IsLiveBlack = true;
                    }
                }
            }
            if(IsLiveBlack && IsLiveWhite) { return; }
            else 
            {
                if(!IsLiveBlack) { MessageBox.Show("Белые выиграли!"); }
                if(!IsLiveWhite) { MessageBox.Show("Чёрные выиграли!"); }
            }
        }

        public void CloseSteps()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
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

        public Color GetPrevButtonColor(Button prevButton)
        {
            if ((prevButton.Location.Y / cellSize % 2) != 0)
            {
                if ((prevButton.Location.X / cellSize % 2) == 0)
                {
                    return Color.Gray;
                }
            }
            if ((prevButton.Location.Y / cellSize) % 2 == 0)
            {
                if ((prevButton.Location.X / cellSize) % 2 != 0)
                {
                    return Color.Gray;
                }
            }
            return Color.White;
        }

        public string GetMap()
        {
            string res = "";
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    res += map[i, j].ToString() + ",";
                }
            }
            return res;
        }

        public bool IsInsideBorders(int i, int j)
        {
            if (i >= mapSize || j >= mapSize || i < 0 || j < 0)
            {
                return false;   // Не нахоидтся
            }
            return true; // Находится
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

        private void ChangeAfterListen(string msg)
        {
            string[] razborMessage = new string[65];
            razborMessage = msg.Split(',');

            // Выполняем в отдельном потоке, так как будем менять нашу форму
            this.Invoke((MethodInvoker)delegate
            {
                int k = 0;
                // Переписываем все пришедшие координаты
                for (int i = 0; i < mapSize; i++)
                {
                    for (int j = 0; j < mapSize; j++)
                    {
                        int num = Convert.ToInt32(razborMessage[k]);
                        map[i, j] = num;    // Переписываем пришедшую карту на нашу

                        SetInputFigure(i, j, num);
                        k++;
                    }
                }
            });

            //  Присваиваем игроку его фигуру
            if (currentPlayer == 0)
            {
                if (Convert.ToInt32(razborMessage[64]) == 1)
                { currentPlayer = 2; }
            }
            ActivateAllButtons();
        }

        // 0 - пустота, 11 - король, 12 - королева, 13 - слон, 14 - конь, 15 - ладья, 16 - пешка
        private void SetInputFigure(int i, int j, int num)
        {
            switch(num)
            {
                case 11:
                    buttons[i, j].Image = whiteKing;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;
                case 12:
                    buttons[i, j].Image = whiteQueen;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;
                case 13:
                    buttons[i, j].Image = whiteElephant;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;
                case 14:
                    buttons[i, j].Image = whiteHorse;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;
                case 15:
                    buttons[i, j].Image = whiteTower;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;
                case 16:
                    buttons[i, j].Image = whitePawn;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;

                // Black
                case 21:
                    buttons[i, j].Image = blackKing;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;
                case 22:
                    buttons[i, j].Image = blackQueen;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;
                case 23:
                    buttons[i, j].Image = blackElephant;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;
                case 24:
                    buttons[i, j].Image = blackHorse;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;
                case 25:
                    buttons[i, j].Image = blackTower;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;
                case 26:
                    buttons[i, j].Image = blackPawn;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;
                
                case 0:
                    buttons[i, j].Image = null;
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                    break;
            }    
        }

        // Подключение к серверу
        private void Server_Connect()
        {
            // Создаём клиент
            client = new TcpClient();
            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                while (true)
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;

                    bytes = stream.Read(data, 0, data.Length);
                    builder.Append(Encoding.UTF8.GetString(data, 0, bytes));

                    ID = builder.ToString();            // Присваиваем ID

                    break;
                }

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока

                // Каждые 10 секунд проверяем есть ли соединение с сервером!
                Thread listenConnection = new Thread(new ThreadStart(CheckConnection));
                listenConnection.Start();
            }
            catch (Exception ex)
            {
                // Вывод ошибки в консоль, скорее всего сервер не запущен, или случилась хрень
                Console.WriteLine(ex.Message);
                MessageBox.Show("Сервер не отвечает", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        // Отправить сообщение
        public void SendMessage(string message)
        {
            if ((message != ""))
            {
                byte[] buffer = new byte[1024];
                buffer = Encoding.UTF8.GetBytes(message);   // Делаем байт код в формате UTF-8(Это важно) и отправляем его на сервер
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        // Получение сообщений
        void ReceiveMessage()
        {
            // Бесконечно слушаем
            while (true)
            {
                try
                {
                    byte[] data = new byte[64]; // буфер для получаемых данных
                    StringBuilder builder = new StringBuilder();
                    int bytes = 0;
                    do
                    {
                        bytes = stream.Read(data, 0, data.Length);
                        builder.Append(Encoding.UTF8.GetString(data, 0, bytes));
                    }
                    while (stream.DataAvailable);

                    this.Invoke((MethodInvoker)delegate
                    {
                        ChangeAfterListen(builder.ToString());  // Вызываем функцию разбора пришедшего сообщения
                        ActivateAllButtons();                   // Активируем все кнопки 
                        CheckWin();                             // Проверяем нет ли победителя
                    });

                    Console.WriteLine(builder.ToString());      // Вывод полученного сообщения
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!"); // Соединение было прервано (Игра выключена, игра крашнулась)
                    MessageBox.Show("Подключение разрвано, приложение закроется через 5 секунд", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Thread.Sleep(5000);
                    Disconnect();
                }
            }
        }

        // Проверка есть ли соединение с сервером, каждые 10 секунд
        public void CheckConnection()
        {
            while (true)
            {
                Thread.Sleep(10000);
                if (!client.Connected)
                {
                    MessageBox.Show("Соединение с сервером разорвано!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Отключение, закрывает подключение с сервером и убивает ВСЁ приложение
        void Disconnect()
        {
            if (stream != null)
                stream.Close();//отключение потока
            if (client != null)
                client.Close();//отключение клиента
            Environment.Exit(0); //завершение процесса
        }
    }
}
