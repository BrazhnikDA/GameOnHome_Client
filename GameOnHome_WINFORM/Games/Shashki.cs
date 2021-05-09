using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GameOnHome_WINFORM.Games
{
    public partial class Shashki : Form
    {
        private string ID;                          // ID присвоенное сервером
        private bool IsStatus = false;              // True - онлайн, False - оффлайн

        private const string host = "127.0.0.1";    // IP
        private const int port = 7770;              // Порт
        TcpClient client;                           // Клиент
        NetworkStream stream;                       // Поток от клиента до сервера

        private const int mapSize = 8;       // Размер карты 8*8
        private const int cellSize = 100;    // Размер кнопки 100*100
        private int currentPlayer = 0;       // Текущий игрок   1 - белые, 2 - чёрные   

        private List<Button> simpleSteps = new List<Button>();  // Список кнопок во время хода куда можно сходить

        private bool isContinue;

        private int countEatSteps = 0;
        private Button prevButton;          // Предыдущая нажатая кнопка
        private Button pressedButton;       // Нажатая кнпока

        private bool isMoving;              // Находится ли шашка в процессе ходьбы

        private int[,] map = new int[mapSize, mapSize];             // Карта которая харнит 0, 1 и 2 
        private Button[,] buttons = new Button[mapSize, mapSize];   // Массив всех кнопок

        private Image whiteFigure;          // Изображение белой фигуры
        private Image blackFigure;          // Изображение чёрной фигуры

        List<int[]> listIJ = new List<int[]>();          // Список ходов которые бот может сделать пешкой 
        List<int[]> listIJDamka = new List<int[]>();     // Список ходов которые бот может сделать дамкой

        public Shashki(bool IsStatus_)
        {
            InitializeComponent();

            IsStatus = IsStatus_;

            whiteFigure = new Bitmap(Properties.Resources.white, new Size(cellSize - 10, cellSize - 10));   // Привязываем белую фигуру 
            blackFigure = new Bitmap(Properties.Resources.black, new Size(cellSize - 10, cellSize - 10));   // Привязываем чёрную фигуру

            Text = "Шашки";     // Название окна

            if (IsStatus)
            {
                Server_Connect();       // Функция для подключеняи к серверу
                Thread.Sleep(100);      // Ожидаем подключения
            }

            InitBoard();        // "Создаём" доску
        }
        private void InitBoard()
        {
            // Инициализируем переменные
            isContinue = false;
            currentPlayer = 0;  // Выбранный игрок пока что 0
            isMoving = false;
            prevButton = null;

            // 0 - пусто, 1 - белые, 2 - чёрные
            map = new int[mapSize, mapSize] {
                { 0,1,0,1,0,1,0,1 },
                { 1,0,1,0,1,0,1,0 },
                { 0,1,0,1,0,1,0,1 },
                { 0,0,0,0,0,0,0,0 },
                { 0,0,0,0,0,0,0,0 },
                { 2,0,2,0,2,0,2,0 },
                { 0,2,0,2,0,2,0,2 },
                { 2,0,2,0,2,0,2,0 }
            };
            CreateMap();    // Рисуем кнопки (карту)
        }
        private void CreateMap()
        {
            // Указываем размер окна относительно размера кнопок
            this.Width = (mapSize + 1) * cellSize + 50;
            this.Height = (mapSize + 1) * cellSize + 50;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    // Создаём кнопку и указвыаем нужные нам параметры
                    Button button = new Button();
                    button.Location = new Point(j * cellSize, i * cellSize);    // Местоположение
                    button.Size = new Size(cellSize, cellSize);
                    if (IsStatus)
                        button.Click += new EventHandler(OnFigurePressOnline);  // Привязываем функцию обработчика нажатий
                    else button.Click += new EventHandler(OnFigurePressOffline);
                    button.BackColor = GetPrevButtonColor(button);
                    button.ForeColor = Color.Red;
                    button.Name = "button" + i.ToString() + "_" + j.ToString(); // Даём ей имя, что бы можно это использовать где потребуется (псевдо ID)

                    if (map[i, j] == 1)
                        button.Image = whiteFigure;                             // Устанавливаем белую фигуру
                    else if (map[i, j] == 2) button.Image = blackFigure;        // Устанавливаем чёрную фигуру

                    buttons[i, j] = button;                                     // Заносим в наш массив кнопок

                    this.Controls.Add(button);                                  // Привязываем кнопки к нашему окну
                }
            }
        }

        // Проверка на победу
        private void CheckWin()
        {
            bool white = false;
            bool black = false;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] == 1)         // Если остались белые, значит чёрные не выиграли
                        white = true;
                    if (map[i, j] == 2)         //Если остались чёрные, значит белые не выиграли
                        black = true;
                }
            }
            if (white == false)
            {
                MessageBox.Show("Чёрные выиграли!");
            }
            if (black == false)
            {
                MessageBox.Show("Белые выиграли!");
            }
        }

        // Выставляем цвет для кнопки
        private Color GetPrevButtonColor(Button prevButton)
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

        // Обработка нажатия на фигуру при офлайн режиме
        private void OnFigurePressOffline(object sender, EventArgs e)
        {
            currentPlayer = 1;      // Пользователь всегда играет за белых
            pressedButton = sender as Button;

            // Проверка, выбранная кнопка не пустая и принадлежит нашему игроку (белая)
            if (CheckMap(ConvertNameI(pressedButton), ConvertNameY(pressedButton)) != 0 &&
                CheckMap(ConvertNameI(pressedButton), ConvertNameY(pressedButton)) == currentPlayer)
            {
                CloseSteps();                               // Закрываем шаги
                pressedButton.BackColor = Color.Red;        // Выделяем нажатую кнопку красным
                DeactivateAllButtons();                     // Деактивируем все кнопки
                pressedButton.Enabled = true;               // Нажатую клавишу делаем активной
                countEatSteps = 0;
                if (pressedButton.Text != "D")              // Проверка, это дамка или нет
                    ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, true);
                else ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, false);

                if (isMoving)               // Есть куда ходить
                {
                    CloseSteps();           // Закрываем все шаги 
                    pressedButton.BackColor = GetPrevButtonColor(pressedButton);
                    ShowPossibleSteps();    // Показываем куда можем сходить
                    isMoving = false;
                }
                else
                    isMoving = true;        // Активируем что бы проверить на возможные ходы 
            }
            else
            {
                if (isMoving)
                {
                    isContinue = false;
                    if (Math.Abs(pressedButton.Location.X / cellSize - prevButton.Location.X / cellSize) > 1)
                    {
                        isContinue = true;
                        DeleteEaten(pressedButton, prevButton);
                    }
                    int temp = map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize];
                    map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] = map[prevButton.Location.Y / cellSize, prevButton.Location.X / cellSize];
                    map[prevButton.Location.Y / cellSize, prevButton.Location.X / cellSize] = temp;
                    pressedButton.Image = prevButton.Image;
                    prevButton.Image = null;
                    pressedButton.Text = prevButton.Text;
                    prevButton.Text = "";
                    SwitchButtonToDamka(pressedButton);
                    countEatSteps = 0;
                    isMoving = false;
                    CloseSteps();
                    DeactivateAllButtons();
                    if (pressedButton.Text != "D")
                        ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, true);
                    else ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, false);
                    if (countEatSteps == 0 || !isContinue)
                    {
                        CloseSteps();
                        ShowPossibleSteps();
                        isContinue = false;
                    }
                    else if (isContinue)
                    {
                        pressedButton.BackColor = Color.Red;
                        pressedButton.Enabled = true;
                        isMoving = true;
                    }

                    bool IsEnd = true;
                    for (int i = 0; i < mapSize; i++)
                    {
                        for (int j = 0; j < mapSize; j++)
                        {
                            if (buttons[i, j].BackColor == Color.Yellow)
                            {
                                IsEnd = false;
                            }
                        }
                    }
                    if (IsEnd)
                    {
                        CheckWin();                                 // Нет ли победителя? 
                        //Thread brainBot = new Thread(new ThreadStart(BotBrainEasy));
                        //brainBot.Start();
                        BotBrainEasy();
                        CheckWin();                                 // Нет ли победителя после хода бота?
                    }
                }
            }
            prevButton = pressedButton;
        }

        // Интеллект бота сложность: легко
        private void BotBrainEasy()
        {
            //Thread.Sleep(150);
            bool IsEat = false;         // Есть ли съедобный ход
            for (int i = 0; i < mapSize; i++)
            {
                for(int j = 0; j < mapSize; j++)
                {
                    if(map[i,j] == 2)   // Нашли чёрную фигуру
                    {
                        // Сразу проверяем ходы по 4 диагоналям, можно ли кого нибудь съесть 
                        // Дамка
                        if (buttons[i, j].Text == "D")
                        {
                            IsEat = BotCheckEatDamka(i, j);
                            if (IsEat)
                                return;
                        }
                        else
                        {
                            //Thread.Sleep(50);
                            // Обычная шашка
                            // Для 2-3 хода

                            int I = i; int J = j;

                            int ii = I - 1;
                            int iii = I - 2;

                            int Ljj = J - 1; int Ljjj = J - 2;
                            int Rjj = J + 1; int Rjjj = J + 2;

                            while (true)
                            {
                                IsEat = BotCheckEat(I, J, ii, Ljj, iii, Ljjj);      // Проверить диагональ слева вверх
                                if (IsEat)
                                {
                                    I = iii; J = Ljjj;
                                    ii = I - 1; iii = I - 2;
                                    Ljj = J - 1; Ljjj = J - 2;
                                    MessageBox.Show("Left");
                                    continue;
                                }
                                IsEat = BotCheckEat(I, J, ii, Rjj, iii, Rjjj);      // Проверить диагональ справа вверх
                                if (IsEat)
                                {
                                    I = iii; J = Rjjj;
                                    ii = I - 1; iii = I - 2;
                                    Rjj = J + 1; Rjjj = J + 2;
                                    MessageBox.Show("Right");
                                    continue;
                                }
                                if (!IsEat) { break; }                               // Нет хода, выходим из цикла

                            }
                            if (IsEat)
                            {
                                SwitchButtonToDamka(buttons[i, j]);
                                return;
                            }
                        }
                    }

                }
            }
                if (!BotCheckStep() && !IsEat)     // Нет съедобных ходов, делаем обычный ход
                {
                    // Если единственный доступный ход для ПРОСТОЙ пешки, сьесть назад, съедаем назад
                    for (int i = 0; i < mapSize; i++)
                    {
                        for (int j = 0; j < mapSize; j++)
                        {
                            if (map[i, j] == 2)   // Мы нашли чёрную фигуру
                            {
                                IsEat = BotCheckEat(i, j, i + 1, j - 1, i + 2, j - 2);      // Проверить диагональ слева вниз
                                if (IsEat)
                                    return;
                                IsEat = BotCheckEat(i, j, i + 1, j + 1, i + 2, j + 2);      // Проверить диагональ справа вниз
                                if (IsEat)
                                    return;
                            }
                        }
                    }
                }
                listIJ.Clear();
                listIJDamka.Clear();
        }

        private bool BotCheckEatDamka(int i, int j)
        {
            bool IsEat = false;
            int ii = i - 1;
            int jj = j - 1;
            while (IsInsideBorders(ii, jj))     // Вверх-влево
            {
                if (map[ii, jj] == 1)
                {
                    if (IsInsideBorders(ii - 1, jj - 1))
                    {
                        if (map[ii - 1, jj - 1] == 0)
                        {
                            IsEat = BotCheckEat(i, j, ii, jj, ii - 1, jj - 1);
                            buttons[ii - 1, jj - 1].Text = "D";
                            if (IsEat)
                                return true;
                        }
                    }
                }
                ii--; jj--;
            }
            ii = i - 1;
            jj = j + 1;
            while (IsInsideBorders(ii, jj))     // Вверх-вправо 
            {
                if (map[ii, jj] == 1)
                {
                    if (IsInsideBorders(ii - 1, jj + 1))
                    {
                        if (map[ii - 1, jj + 1] == 0)
                        {
                            IsEat = BotCheckEat(i, j, ii, jj, ii - 1, jj + 1);
                            buttons[ii - 1, jj + 1].Text = "D";
                            if (IsEat)
                                return true;
                        }
                    }
                }
                ii--; jj++;
            }
            ii = i + 1;
            jj = j - 1;
            while (IsInsideBorders(ii, jj))     // Вниз-влево 
            {
                if (map[ii, jj] == 1)
                {
                    if (IsInsideBorders(ii + 1, jj - 1))
                    {
                        if (map[ii + 1, jj - 1] == 0)
                        {
                            IsEat = BotCheckEat(i, j, ii, jj, ii + 1, jj - 1);
                            buttons[ii + 1, jj - 1].Text = "D";
                            if (IsEat)
                                return true;
                        }
                    }
                }
                ii++; jj--;
            }
            ii = i + 1;
            jj = j + 1;
            while (IsInsideBorders(ii, jj))     // Вниз-вправо
            {
                if (map[ii, jj] == 1)
                {
                    if (IsInsideBorders(ii + 1, jj + 1))
                    {
                        if (map[ii + 1, jj + 1] == 0)
                        {
                            IsEat = BotCheckEat(i, j, ii, jj, ii + 1, jj + 1);
                            buttons[ii + 1, jj + 1].Text = "D";
                            if (IsEat)
                                return true;
                        }
                    }
                }
                ii++; jj++;
            }
            return false;
        }

        // Проверить есть ли съедобный ход по дагонали, если есть - съедаем
        private bool BotCheckEat(int i, int j, int ii, int jj, int iii, int jjj)
        {
            if (IsInsideBorders(ii, jj))
            {
                if (map[ii, jj] == 1)
                {
                    if (IsInsideBorders(iii, jjj))
                    {
                        if (map[iii, jjj] == 0)
                        {
                            buttons[i, j].Image = null;                 // Поле где стояла фигура изначально
                            buttons[i, j].Text = "";
                            buttons[i, j].Enabled = false;
                            map[i, j] = 0;

                            if (buttons[ii, jj].Enabled == true)
                            {
                                ActivateAllButtons();                   // Костыль, если мы сходили и у нас 1 активная фигура, то съев её все поля станут disable
                            }

                            buttons[ii, jj].Image = null;               // Поле где находилась съеденная фигура
                            buttons[ii, jj].Enabled = false;
                            buttons[ii, jj].Text = "";
                            map[ii, jj] = 0;

                            // Ставим нашу фигуру
                            if (buttons[i, j].Text == "D")
                            {
                                buttons[iii, jjj].Image = blackFigure;
                                buttons[iii, jjj].Text = "D";   // Если дамка приписываем букву "D"
                                map[iii, jjj] = 2;
                            }
                            else
                            {
                                buttons[iii, jjj].Image = blackFigure;
                                map[iii, jjj] = 2;
                            }

                            SwitchButtonToDamka(buttons[iii, jjj]);     // Проверяем не является ли она дамкой
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        // Просто ход бота, без съедания
        private bool BotCheckStep()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] == 2)                      // Найдена чёрная фигура
                    {
                        // Если встреченная кнопка дамка
                        if (buttons[i, j].Text == "D")
                        {
                            int[] xodDamka = new int[4] { 0, 0, 0, 0 }; // Запоминаем координаты хода где стояли/куда придём
                            // Начинаем смотреть по 4 диагоналям
                            int ii = i - 1;
                            int jj = j - 1;
                            while (xodDamka[0] != -1)   // Пока не дойдём до конца доски (опонента невозможно встретить, т.к. предыдущая функция съела противника если возможность была
                            {
                                xodDamka = bot_check_move(i, j, ii, jj);    
                                if (xodDamka[0] != -1)
                                {
                                    listIJDamka.Add(xodDamka);
                                }
                                ii--; jj--;
                            }
                            ii = i - 1;
                            jj = j + 1;
                            xodDamka[0] = 0;
                            while (xodDamka[0] != -1)
                            {
                                xodDamka = bot_check_move(i, j, ii, jj);
                                if (xodDamka[0] != -1)
                                {
                                    listIJDamka.Add(xodDamka);
                                }
                                ii--; jj++;
                            }
                            ii = i + 1;
                            jj = j - 1;
                            xodDamka[0] = 0;
                            while (xodDamka[0] != -1)
                            {
                                xodDamka = bot_check_move(i, j, ii, jj);
                                if (xodDamka[0] != -1)
                                {
                                    listIJDamka.Add(xodDamka);
                                }
                                ii++; jj--;
                            }
                            ii = i + 1;
                            jj = j + 1;
                            xodDamka[0] = 0;
                            while (xodDamka[0] != -1)
                            {
                                xodDamka = bot_check_move(i, j, ii, jj);
                                if (xodDamka[0] != -1)
                                {
                                    listIJDamka.Add(xodDamka);
                                }
                                ii++; jj++;
                            }
                        }

                        int[] check = new int[4];       // Переменная для хранения координат где стоит фигура и куда она может сходить (i, j, ii, jj) 

                        check = bot_check_move(i, j, i - 1, j - 1);
                        if(check[0] != -1 && check[1] != -1)
                        {
                            listIJ.Add(check);
                        }
                        check = bot_check_move(i, j, i - 1, j + 1);
                        if (check[0] != -1 && check[1] != -1)
                        {
                            listIJ.Add(check);
                        }
                    }
                }
            }

            // Есть дамка, ходим дамкой
            if (listIJDamka.Count > 0)
            {
                Random rnd = new Random();
                byte xod = Convert.ToByte(rnd.Next(0, listIJDamka.Count));   // Выбираем случайный ход из возможных

                map[listIJDamka[xod][0], listIJDamka[xod][1]] = 0;    // Где стояли теперь пустота
                map[listIJDamka[xod][2], listIJDamka[xod][3]] = 2;    // Куда сходили теперь наша фигура

                buttons[listIJDamka[xod][0], listIJDamka[xod][1]].Image = null;
                buttons[listIJDamka[xod][0], listIJDamka[xod][1]].Text = "";

                buttons[listIJDamka[xod][2], listIJDamka[xod][3]].Image = blackFigure;
                buttons[listIJDamka[xod][2], listIJDamka[xod][3]].Text = "D";

                return true;
            }

            // Есть хоть 1 возможный ход
            if (listIJ.Count > 0)
            {
                Random rnd = new Random();
                byte xod = Convert.ToByte(rnd.Next(0, listIJ.Count));   // Выбираем случайный ход из возможных

                map[listIJ[xod][0], listIJ[xod][1]] = 0;    // Где стояли теперь пустота
                map[listIJ[xod][2], listIJ[xod][3]] = 2;    // Куда сходили теперь наша фигура

                buttons[listIJ[xod][0], listIJ[xod][1]].Image = null;
                buttons[listIJ[xod][2], listIJ[xod][3]].Image = blackFigure;

                SwitchButtonToDamka(buttons[listIJ[xod][0], listIJ[xod][1]]);   // Проверяем не стали ли мы дамкой
                return true;
            }
            { return false; }
        }

        // Проверка хода, если сходить нельзя возвращем массив из -1...
        int[] bot_check_move(int i, int j, int ii, int jj)
        {
            int[] ij = new int[4];
            if (IsInsideBorders(ii, jj))
            {
                if (map[ii, jj] == 0)
                {
                    ij[0] = i; ij[1] = j;
                    ij[2] = ii; ij[3] = jj;
                    return ij;
                }
            }
            return new int[4] { -1, -1, -1, -1 };
        }


        // Обработка нажатия на фигуру при онлайн режиме
        private void OnFigurePressOnline(object sender, EventArgs e)
        {
            if (client != null)
            {
                if (!client.Connected)
                {
                    MessageBox.Show("Нет подключения к серверу!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show("Подключитесь к серверу!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (currentPlayer == 0)
            {
                currentPlayer = 1;  // Кто первый сходил,тот играет за белых 
            }
            pressedButton = sender as Button;

            if (CheckMap(ConvertNameI(pressedButton), ConvertNameY(pressedButton)) != 0 &&
                CheckMap(ConvertNameI(pressedButton), ConvertNameY(pressedButton)) == currentPlayer)
            {
                CloseSteps();                               // Закрываем шаги
                pressedButton.BackColor = Color.Red;        // Выделяем нажатую кнопку красным
                DeactivateAllButtons();                     // Деактивируем все кнопки
                pressedButton.Enabled = true;               // Нажатую клавишу делаем активной
                countEatSteps = 0;                          // Количество съедобных шагов
                if (pressedButton.Text != "D")              // Проверка, это дамка или нет
                    ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, true);
                else ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, false);

                if (isMoving)               // Есть куда ходить
                {
                    CloseSteps();           // Закрываем все шаги 
                    pressedButton.BackColor = GetPrevButtonColor(pressedButton);
                    ShowPossibleSteps();    // Показываем куда можем сходить
                    isMoving = false;
                }
                else
                    isMoving = true;        // Активируем что бы проверить на возможные ходы 
            }
            else
            {
                if (isMoving)
                {
                    isContinue = false;
                    if (Math.Abs(pressedButton.Location.X / cellSize - prevButton.Location.X / cellSize) > 1)
                    {
                        isContinue = true;
                        DeleteEaten(pressedButton, prevButton);
                    }
                    int temp = map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize];
                    map[pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize] = map[prevButton.Location.Y / cellSize, prevButton.Location.X / cellSize];
                    map[prevButton.Location.Y / cellSize, prevButton.Location.X / cellSize] = temp;
                    pressedButton.Image = prevButton.Image;
                    prevButton.Image = null;
                    pressedButton.Text = prevButton.Text;
                    prevButton.Text = "";
                    SwitchButtonToDamka(pressedButton);
                    countEatSteps = 0;
                    isMoving = false;
                    CloseSteps();
                    DeactivateAllButtons();
                    if (pressedButton.Text != "D")
                        ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, true);
                    else ShowSteps(pressedButton.Location.Y / cellSize, pressedButton.Location.X / cellSize, false);
                    if (countEatSteps == 0 || !isContinue)
                    {
                        CloseSteps();
                        ShowPossibleSteps();
                        isContinue = false;
                    }
                    else if (isContinue)
                    {
                        pressedButton.BackColor = Color.Red;
                        pressedButton.Enabled = true;
                        isMoving = true;
                    }
                    SendMessage(GetMap() + currentPlayer);      // Отправляем нашу карту на сервер 
                    CheckWin();                                 // Нет ли победителя? 
                    WaitXod();                                  // Закрываем ходы игроку
                }
            }
            prevButton = pressedButton;
        }
    

        private void WaitXod()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if(buttons[i, j].Enabled == false)
                    {
                        return; // Если есть куда сходить не ставим на паузу
                    }
                }
            }
            DeactivateAllButtons();
        }

        // Возвращает значение ячеек на карте через запятую
        private string GetMap()
        {
            string res = "";
            for(int i = 0; i < mapSize; i++)
            {
                for(int j = 0; j < mapSize; j++)
                {
                    res += map[i, j].ToString() + ",";
                }
            }
            return res;
        }

        // Удаляем съеденную фигуру
        private void DeleteEaten(Button endButton, Button startButton)
        {
            int count = Math.Abs(endButton.Location.Y / cellSize - startButton.Location.Y / cellSize);
            int startIndexX = endButton.Location.Y / cellSize - startButton.Location.Y / cellSize;
            int startIndexY = endButton.Location.X / cellSize - startButton.Location.X / cellSize;
            startIndexX = startIndexX < 0 ? -1 : 1;
            startIndexY = startIndexY < 0 ? -1 : 1;
            int currCount = 0;
            int i = startButton.Location.Y / cellSize + startIndexX;
            int j = startButton.Location.X / cellSize + startIndexY;
            while (currCount < count - 1)
            {
                map[i, j] = 0;
                buttons[i, j].Image = null;
                buttons[i, j].Text = "";
                i += startIndexX;
                j += startIndexY;
                currCount++;
            }

        }

        // Проверка может ли шашка стать дамкой
        private void SwitchButtonToDamka(Button button)
        {
            // Проверка для белых
            if (map[button.Location.Y / cellSize, button.Location.X / cellSize] == 1 && button.Location.Y / cellSize == mapSize - 1)
            {
                button.Text = "D";
            }
            // Проверка для чёрных
            if (map[button.Location.Y / cellSize, button.Location.X / cellSize] == 2 && button.Location.Y / cellSize == 0)
            {
                button.Text = "D";
            }
        }

        // Показывает возможные ходы для выбранной фигуры
        private void ShowPossibleSteps()
        {
            bool isOneStep = true;
            bool isEatStep = false;
            DeactivateAllButtons();
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    if (map[i, j] == currentPlayer)
                    {
                        if (buttons[i, j].Text == "D")
                            isOneStep = false;
                        else isOneStep = true;
                        if (IsButtonHasEatStep(i, j, isOneStep, new int[2] { 0, 0 }))
                        {
                            isEatStep = true;
                            buttons[i, j].Enabled = true;
                        }
                    }
                }
            }
            if (!isEatStep)
                ActivateAllButtons();   // Нет съедобных ходов активируем все кнпоки 
        }
        
        // Проверка можем ли мы сходить по 4 диагоналям
        private bool IsButtonHasEatStep(int IcurrFigure, int JcurrFigure, bool isOneStep, int[] dir)
        {
            bool eatStep = false;
            int j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (dir[0] == 1 && dir[1] == -1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j + 1))
                            eatStep = false;
                        else if (map[i - 1, j + 1] != 0)
                            eatStep = false;
                        else return eatStep;
                    }
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (dir[0] == 1 && dir[1] == 1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i - 1, j - 1))
                            eatStep = false;
                        else if (map[i - 1, j - 1] != 0)
                            eatStep = false;
                        else return eatStep;
                    }
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (dir[0] == -1 && dir[1] == 1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i + 1, j - 1))
                            eatStep = false;
                        else if (map[i + 1, j - 1] != 0)
                            eatStep = false;
                        else return eatStep;
                    }
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (dir[0] == -1 && dir[1] == -1 && !isOneStep) break;
                if (IsInsideBorders(i, j))
                {
                    if (map[i, j] != 0 && map[i, j] != currentPlayer)
                    {
                        eatStep = true;
                        if (!IsInsideBorders(i + 1, j + 1))
                            eatStep = false;
                        else if (map[i + 1, j + 1] != 0)
                            eatStep = false;
                        else return eatStep;
                    }
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
            return eatStep;
        }

        // Показать возможные шаги для фигуры
        private void ShowSteps(int iCurrFigure, int jCurrFigure, bool isOnestep = true)
        {
            simpleSteps.Clear();
            ShowDiagonal(iCurrFigure, jCurrFigure, isOnestep);
            if (countEatSteps > 0)
                CloseSimpleSteps(simpleSteps);
        }

        // Деактивируем шаги на которые не можем сходить во время хода
        private void CloseSimpleSteps(List<Button> simpleSteps)
        {
            if (simpleSteps.Count > 0)
            {
                for (int i = 0; i < simpleSteps.Count; i++)
                {
                    simpleSteps[i].BackColor = GetPrevButtonColor(simpleSteps[i]);
                    simpleSteps[i].Enabled = false;
                }
            }
        }

        private void ShowDiagonal(int IcurrFigure, int JcurrFigure, bool isOneStep)
        {
            int j = JcurrFigure + 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure - 1; i >= 0; i--)
            {
                if (currentPlayer == 1 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure - 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j > 0)
                    j--;
                else break;

                if (isOneStep)
                    break;
            }

            j = JcurrFigure + 1;
            for (int i = IcurrFigure + 1; i < 8; i++)
            {
                if (currentPlayer == 2 && isOneStep && !isContinue) break;
                if (IsInsideBorders(i, j))
                {
                    if (!DeterminePath(i, j))
                        break;
                }
                if (j < 7)
                    j++;
                else break;

                if (isOneStep)
                    break;
            }
        }

        private bool DeterminePath(int i, int j)
        {

            if (map[i, j] == 0 && !isContinue)
            {
                buttons[i, j].BackColor = Color.Yellow;           // Красим кнопки на которые може сходить в жёлтый
                buttons[i, j].Enabled = true;                     // Активируем их
                simpleSteps.Add(buttons[i, j]);                   // Добавляем в массив кнопок
            }
            else
            {
                if (map[i, j] != currentPlayer)                   // Если кнопка не пустая, проверяем можно ли её съесть 
                {
                    if (pressedButton.Text == "D")
                        ShowEat(i, j, false);       // Дамка
                    else ShowEat(i, j, true);       // Не дамка
                }

                return false;
            }
            return true;
        }

        // Показать что можно сьесть, передаются координаты и ход в единицу?
        private void ShowEat(int i, int j, bool isOneStep)
        {
            int dirX = i - pressedButton.Location.Y / cellSize;
            int dirY = j - pressedButton.Location.X / cellSize;
            dirX = dirX < 0 ? -1 : 1;
            dirY = dirY < 0 ? -1 : 1;
            int il = i;
            int jl = j;
            bool isEmpty = true;
            while (IsInsideBorders(il, jl))
            {
                if (map[il, jl] != 0 && map[il, jl] != currentPlayer)   // Клетка не пустая и не равна нашей фигуре
                {
                    isEmpty = false;
                    break;
                }
                il += dirX;
                jl += dirY;

                if (isOneStep)
                    break;
            }
            if (isEmpty)
                return;
            List<Button> toClose = new List<Button>();
            bool closeSimple = false;
            int ik = il + dirX;
            int jk = jl + dirY;
            while (IsInsideBorders(ik, jk))
            {
                if (map[ik, jk] == 0)
                {
                    //   
                    if (IsButtonHasEatStep(ik, jk, isOneStep, new int[2] { dirX, dirY }))
                    {
                        closeSimple = true;
                    }
                    else
                    {
                        toClose.Add(buttons[ik, jk]);
                    }
                    
                    buttons[ik, jk].BackColor = Color.Yellow;
                    buttons[ik, jk].Enabled = true;
                    countEatSteps++;
                }
                else break;
                if (isOneStep)
                    break;
                jk += dirY;
                ik += dirX;
            }
            if (closeSimple && toClose.Count > 0)
            {
                CloseSimpleSteps(toClose);
            }

        }

        // Вернуть всем кнопкам предыдущий цвет
        private void CloseSteps()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                }
            }
        }

        // Получить позицию кнопки по I
        private int ConvertNameI(Button b)
        {
            string sym = b.Name;
            return Convert.ToInt32(sym[6]) - 48;
        }

        // Получить позицию кнопки по Y
        private int ConvertNameY(Button b)
        {
            string sym = b.Name;
            return Convert.ToInt32(sym[8]) - 48;
        }

        // Возвращает фигуру находящуюся по переданным координатам
        private int CheckMap(int i, int j)
        {
            if (i < mapSize && j < mapSize)
            {
                return map[i, j];
            }else { return 0; }
        }

        // Находится ли переданный индекс за границей доски
        private bool IsInsideBorders(int i, int j)
        {
            if (i >= mapSize || j >= mapSize || i < 0 || j < 0)
            {
                return false;   // Находится
            }
            return true; // Не находится
        }

        // Пройтись по всем кнопкам и сделать их активными
        private void ActivateAllButtons()
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
        private void DeactivateAllButtons()
        {
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].Enabled = false;
                }
            }
        }
       
        // Сделать ход отправленный вторым игроком, после каждого хода карта перерисовывается
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

                        // Встретили белую фигуру
                        if (num == 1)
                        {
                            buttons[i, j].Image = whiteFigure;
                            buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                            SwitchButtonToDamka(buttons[i, j]);
                        }
                        else
                        {
                            // Встретили чёрнуб фигуру
                            if(num == 2)
                            {
                                buttons[i, j].Image = blackFigure;
                                buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                                SwitchButtonToDamka(buttons[i, j]);
                            }
                            else
                            {
                                // Встрели пустую ячейку
                                if(num == 0)
                                {
                                    buttons[i, j].Image = null;
                                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                                    buttons[i, j].Text = "";
                                }
                            }
                        }
                        k++;
                    }
                }
            });
            
            //  Присваиваем игроку его фигуру
            if(currentPlayer == 0)
            {
                if(Convert.ToInt32(razborMessage[64]) == 1)
                { currentPlayer = 2; }
                else 
                { 
                    if(Convert.ToInt32(razborMessage[64]) == 2)
                    {
                        currentPlayer = 1;
                    }
                }
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
        private void SendMessage(string message)
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
        private void CheckConnection()
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
