using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameOnHome_WINFORM.Online
{
    public partial class Shashki : Form
    {
        private const string host = "127.0.0.1";    // IP
        private const int port = 7770;              // Порт
        TcpClient client;                           // Клиент
        NetworkStream stream;                       // Поток от клиента до сервера

        const int mapSize = 8;       // Размер карты 8*8
        const int cellSize = 100;    // Размер кнопки 100*100
        int currentPlayer = 0;       // Текущий игрок   1 - белые, 2 - чёрные   

        List<Button> simpleSteps = new List<Button>();  // Список кнопок во время хода куда можно сходить

        bool isContinue;

        int countEatSteps = 0;
        Button prevButton;          // Предыдущая нажатая кнопка
        Button pressedButton;       // Нажатая кнпока

        bool isMoving;              // Находится ли шашка в процессе ходьбы

        int[,] map = new int[mapSize, mapSize];             // Карта которая харнит 0, 1 и 2 
        Button[,] buttons = new Button[mapSize, mapSize];   // Массив всех кнопок

        Image whiteFigure;          // Изображение белой фигуры
        Image blackFigure;          // Изображение чёрной фигуры

        public Shashki()
        {
            InitializeComponent();
            whiteFigure = new Bitmap(Properties.Resources.white, new Size(cellSize - 10, cellSize - 10));   // Привязываем белую фигуру 
            blackFigure = new Bitmap(Properties.Resources.black, new Size(cellSize - 10, cellSize - 10));   // Привязываем чёрную фигуру
            Text = "Шашки";     // Название окна
            InitBoard();        // "Создаём" доску
        }
        public void InitBoard()
        {
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
        public void CreateMap()
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
                    button.Click += new EventHandler(OnFigurePress);            // Привязываем функцию обработчика нажатий
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
        public void CheckWin()
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

        // Обработка нажатия на фигуру
        public void OnFigurePress(object sender, EventArgs e)
        {
            if (currentPlayer == 0)
            {
                currentPlayer = 1;  // Кто первый сходил,тот играет за белых 
            }
            pressedButton = sender as Button;
            
            if (CheckMap(ConvertNameI(pressedButton),ConvertNameY(pressedButton)) != 0 &&
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

                    SendMessage(GetMap() + currentPlayer);      // Отправляем нашу карту на сервер 
                    CheckWin();                                 // Нет ли победителя? 
                    WaitXod();                                  // Закрываем ходы игроку
                }
            }
            prevButton = pressedButton;
        }

        public void WaitXod()
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
        public string GetMap()
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
        public void DeleteEaten(Button endButton, Button startButton)
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

        // Превратить шашку в дамку
        public void SwitchButtonToDamka(Button button)
        {
            if (map[button.Location.Y / cellSize, button.Location.X / cellSize] == 1 && button.Location.Y / cellSize == mapSize - 1)
            {
                button.Text = "D";
            }
            if (map[button.Location.Y / cellSize, button.Location.X / cellSize] == 2 && button.Location.Y / cellSize == 0)
            {
                button.Text = "D";
            }
        }

        // Показывает возможные ходы для выбранной фигуры
        public void ShowPossibleSteps()
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
                ActivateAllButtons();
        }

        // Проверка ходов по 4 диагоналям
        public bool IsButtonHasEatStep(int IcurrFigure, int JcurrFigure, bool isOneStep, int[] dir)
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
        public void ShowSteps(int iCurrFigure, int jCurrFigure, bool isOnestep = true)
        {
            simpleSteps.Clear();
            ShowDiagonal(iCurrFigure, jCurrFigure, isOnestep);
            if (countEatSteps > 0)
                CloseSimpleSteps(simpleSteps);
        }

        // Деактивируем шаги на которые не можем сходить во время хода
        public void CloseSimpleSteps(List<Button> simpleSteps)
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

        public void ShowDiagonal(int IcurrFigure, int JcurrFigure, bool isOneStep)
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

        public bool DeterminePath(int i, int j)
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
                        ShowEat(i, j, false);
                    else ShowEat(i, j, true);
                }

                return false;
            }
            return true;
        }

        // Показать что можно сьесть, передаются координаты и ход в единицу?
        public void ShowEat(int i, int j, bool isOneStep)
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

        // Возвращает фигуру находящуюся по переданным координатам
        public int CheckMap(int i, int j)
        {
            if (i < mapSize && j < mapSize)
            {
                return map[i, j];
            }else { return 0; }
        }

        // Находится ли переданный индекс за границей доски
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
                        map[i, j] = num;

                        if (num == 1)
                        {
                            buttons[i, j].Image = whiteFigure;
                            buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                            SwitchButtonToDamka(buttons[i, j]);
                        }
                        else
                        {
                            if(num == 2)
                            {
                                buttons[i, j].Image = blackFigure;
                                buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                                SwitchButtonToDamka(buttons[i, j]);
                            }
                            else
                            {
                                if(num == 0)
                                {
                                    buttons[i, j].Image = null;
                                    buttons[i, j].BackColor = GetPrevButtonColor(buttons[i, j]);
                                }
                            }
                        }
                        k++;
                    }
                }
            });
            
            // Задаём фигуру игроку
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

        // Кнопка подключитсья
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            // Создаём клиент
            client = new TcpClient();
            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока

                buttonConnect.Enabled = false;      // Выключаем, больше она нам не понадобится

            }
            catch (Exception ex)
            {
                // Вывод ошибки в консоль, скорее всего сервер не запущен, или случилась хрень
                Console.WriteLine(ex.Message);
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
                    Disconnect();
                }
            }
        }

        // Отключение
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
