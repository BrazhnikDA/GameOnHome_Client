using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GameOnHome_WINFORM.Games
{
    public partial class Krestiki_Noliki : Form
    {
        private end_of_game.end_of_game EndGame;

        SoundPlayer sound;

        private string ID;                          // ID присвоенное сервером
        private bool IsStatus = false;              // True - онлайн, False - оффлайн

        private const string host = "127.0.0.1";    // IP
        private const int port = 7770;              // Порт
        TcpClient client;                           // Клиент
        NetworkStream stream;                       // Поток от клиента до сервера

        private const int mapSize = 3;          // Размер карты 3*3
        private int cellSize;                   // Размер ячейки
        private string currentPlayer = "";      // X или O

        private int[,] map = new int[mapSize, mapSize];             // Карта игры { 0 - Пусто, 1 - Крестик, 2 - Нолик } 
        private Button[,] buttons = new Button[mapSize, mapSize];   // Массив кнопок

        private Image tacFigure;            // Изображение крестика
        private Image toeFigure;            // Изображение нолика

        private int count = 0;              // Количество сделанных ходов

        public bool GetStatus
        {
            get { return IsStatus; }
        }

        public Krestiki_Noliki(bool IsStatus_)
        {
            InitializeComponent();

            cellSize = (Width / 5) + (Height / 9);      

            IsStatus = IsStatus_;

            Properties.Resources.krest.SetResolution(cellSize, cellSize);
            Properties.Resources.nol.SetResolution(cellSize, cellSize);

            tacFigure = Properties.Resources.krest;
            toeFigure = Properties.Resources.nol;

            sound = new SoundPlayer(Properties.Resources.fonMusic);
            sound.Play();

            Text = "Крестики-нолики";

            if (IsStatus)
            {
                Server_Connect();       // Функция для подключеняи к серверу
                Thread.Sleep(100);      // Ожидаем подключения
            }
            CreatePlayBoard();
        }

        private void CreatePlayBoard()
        {
            map = new int[mapSize, mapSize] {
                { 0,0,0 },
                { 0,0,0 },
                { 0,0,0 },
            };
           
            this.Width = (mapSize) * cellSize + 226;
            this.Height = (mapSize) * cellSize + 99;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    //Разметка кнопок
                    int otstopX = 0, otstopY = 0;
                    if (j > 0)
                    {
                        otstopX = 5;
                    }
                    if (j > 1)
                    {
                        otstopX = 10;
                    }
                    if (i > 0)
                    {
                        otstopY = 5;
                    }
                    if (i > 1)
                    {
                        otstopY = 10;
                    }

                    //Параметры кнопок
                    Button button = new Button();
                    button.Size = new Size(cellSize, cellSize);                 //Размер
                    button.Location = new Point(j * cellSize + otstopX + 100, i * cellSize + otstopY + 25);    //Местоположение
                    button.TabStop = false;                                     //Выделение
                    button.FlatStyle = FlatStyle.Flat;                          //Стиль рамок
                    button.FlatAppearance.BorderSize = 0;                       //Ширина рамок
                    button.BackColor = Color.Gray;
                    if (IsStatus)
                        button.Click += new EventHandler(Button_click_online);  // Привязываем функцию обработчика нажатий
                    else button.Click += new EventHandler(Button_click_ofline);
                    button.Name = "button" + i.ToString() + "_" + j.ToString(); // Даём ей имя, что бы можно это использовать где потребуется (псевдо ID)

                    buttons[i, j] = button;                                     // Заносим в наш массив кнопок

                    this.Controls.Add(button);                                  // Привязываем
                }
            }
        }

        public void Restart()
        {
            for(int i = 0; i < mapSize; i++)
            {
                for(int j = 0; j < mapSize; j++)
                {
                    map[i, j] = 0;
                    buttons[i, j].Image = null;
                    buttons[i, j].BackColor = Color.Gray; ;
                }
            }
            count = 0;
            //CreatePlayBoard();
            Krestiki_Noliki_Load(null, null);
            ActivateAllButtons();
            currentPlayer = "";
        }

        private void Button_click_ofline(object sender, EventArgs e)
        {
            currentPlayer = "X";
            Button currentButton = sender as Button;

            // Пользователь нажал на занятую ячейку, выходим из функции
            if (map[ConvertNameI(currentButton), ConvertNameY(currentButton)] != 0)
            {
                return;
            }

            currentButton.Image = tacFigure;
            currentButton.BackColor = Color.White;
            map[ConvertNameI(currentButton), ConvertNameY(currentButton)] = 1;
            count++;
            //DeactivateAllButtons();     // Ждём ответ бота
            // Если нет победителя играем дальше
            DeactivateAllButtons();
            if (IsWin() == 0)
            {
                Thread brainBot = new Thread(new ThreadStart(BotXodHard));      // Ход бота
                brainBot.Start();
            }
            else
            {
                TableForWinner();
            }
        }

        private void BotXodHard()
        {
            Thread.Sleep(250);
            // Проверяем можно сходить в центр
            if (map[1, 1] == 0)
            {
                SetXodBot(1, 1);
                return;
            }

            // Проверяем можем ли мы перекрыть игроку выгрышный ход по ГОРИЗОНТАЛИ и ВЕРТИКАЛЕ
            for (int i = 0; i < mapSize; i++)
            {
                if (map[i, 0] == 1 && map[i, 1] == 1)
                {
                    if (map[i, 2] == 0)
                    {
                        SetXodBot(i, 2);
                        return;
                    }
                }
                if (map[i, 1] == 1 && map[i, 2] == 1)
                {
                    if (map[i, 0] == 0)
                    {
                        SetXodBot(i, 0);
                        return;
                    }
                }
                if (map[i, 0] == 1 && map[i, 2] == 1)
                {
                    if (map[i, 1] == 0)
                    {
                        SetXodBot(i, 1);
                        return;
                    }
                }
                if (map[0, i] == 1 && map[1, i] == 1)
                {
                    if (map[2, i] == 0)
                    {
                        SetXodBot(2, i);
                        return;
                    }
                }
                if (map[1, i] == 1 && map[2, i] == 1)
                {
                    if (map[0, i] == 0)
                    {
                        SetXodBot(0, i);
                        return;
                    }
                }
                if (map[0, i] == 1 && map[2, i] == 1)
                {
                    if (map[1, i] == 0)
                    {
                        SetXodBot(1, i);
                        return;
                    }
                }
            }

            // Проверяем дигонали
            if (map[1, 1] == 1 && map[2, 2] == 1)
            {
                if (map[0, 0] == 0)
                {
                    SetXodBot(0, 0);
                    return;
                }
            }
            if (map[0, 0] == 1 && map[1, 1] == 1)
            {
                if (map[2, 2] == 0)
                {
                    SetXodBot(2, 2);
                    return;
                }
            }
            if (map[0, 0] == 1 && map[2, 2] == 0)
            {
                if (map[1, 1] == 0)
                {
                    SetXodBot(1, 1);
                    return;
                }
            }
            if (map[1, 1] == 1 && map[2, 0] == 1)
            {
                if (map[0, 2] == 0)
                {
                    SetXodBot(0, 2);
                    return;
                }
            }
            if (map[0, 2] == 1 && map[1, 1] == 1)
            {
                if (map[2, 0] == 0)
                {
                    SetXodBot(2, 0);
                    return;
                }
            }
            if (map[0, 2] == 1 && map[2, 0] == 1)
            {
                if (map[1, 1] == 0)
                {
                    SetXodBot(1, 1);
                    return;
                }
            }

            // Проверяем можно ли сходить по диагонали
            List<int[]> corners = new List<int[]>(4);
            if (map[0, 0] == 0)
                corners.Add(new int[2] { 0, 0 });
            if (map[0, 2] == 0)
                corners.Add(new int[2] { 0, 2 });
            if (map[2, 0] == 0)
                corners.Add(new int[2] { 2, 0 });
            if (map[2, 2] == 0)
                corners.Add(new int[2] { 2, 2 });

            if (corners.Count > 0)
            {
                Random rnd = new Random();
                int xod = rnd.Next(0, corners.Count);

                SetXodBot(corners[xod][0], corners[xod][1]);
            }
            else
            {
                BotXodEasy();       // Выбираем ход рандомно
            }
            this.Invoke((MethodInvoker)delegate ()
            {
                ActivateAllButtons();
                TableForWinner();
            });
        }

        private void SetXodBot(int i, int j)
        {
            map[i, j] = 2;

            buttons[i, j].Image = toeFigure;
            buttons[i, j].BackColor = Color.White;

            count++;

            this.Invoke((MethodInvoker)delegate ()
            {
                ActivateAllButtons();
                TableForWinner();
            });
        }

        private void BotXodEasy()
        {
            if (count != 9)
            {
                Random rnd = new Random();
                // Получить случайное число (в диапазоне от 0 до 9)
                int xodI = rnd.Next(0, mapSize);
                int xodJ = rnd.Next(0, mapSize);

                while (map[xodI, xodJ] != 0)
                {
                    xodI = rnd.Next(0, mapSize);
                    xodJ = rnd.Next(0, mapSize);
                }
                SetXodBot(xodI, xodJ);
            }
        }
        private void Button_click_online(object sender, EventArgs e)
        {
            Button currentButton = sender as Button;

            // Пользователь нажал на занятую ячейку, выходим из функции
            if(map[ConvertNameI(currentButton), ConvertNameY(currentButton)] != 0)
            {
                return;
            }

            // Если клиент не иниициализирован выдать ошибку 
            if (client != null)
            {
                if (!client.Connected)  // Клиент инициализирован нет соеденения с сервером
                {
                    MessageBox.Show("Нет подключения к серверу!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }else 
            {
                MessageBox.Show("Подключитесь к серверу!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Если мы делаем первый ход Мы - X
            if (count == 0)
            {
                if(currentPlayer == "") { currentPlayer = "X"; }
                else { currentPlayer = "O"; }
            }

            // Изменить кнопку на которую нажал пользователь (Текст, сделать неактивной)
            if (currentPlayer == "X")
            {
                currentButton.Image = tacFigure;
                currentButton.BackColor = Color.White;
                map[ConvertNameI(currentButton), ConvertNameY(currentButton)] = 1;
            }
            else
            {
                currentButton.Image = toeFigure;
                currentButton.BackColor = Color.White; 
                map[ConvertNameI(currentButton), ConvertNameY(currentButton)] = 2;
            }
            count++;

            // Если клиент к чему-то подключился, отправить сообщение и вызвать функцию на првоерку победителя
            if (client != null)
            {
                SendMessage(GetMap() + currentPlayer);  // Отправляем карту и фигуру которую мы поставили
                DeactivateAllButtons();                 // Деактивируем все кнопки
                TableForWinner();                       // Проверяем нет ли победителя
            }
        }

        // Возвращает карту в формате string для отправки на сервер
        private string GetMap()
        {
            string res = "";
            for(int i = 0; i < mapSize; i++)
            {
                for(int j = 0; j < mapSize; j++)
                {
                    res += Convert.ToInt32(map[i, j]) + ",";
                }
            }
            return res;
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

        private bool TableForWinner()
        {
            // Минимальное кол-во ходов для победы 5, начинаем проверять только с этого момента
            if(count > 4)
            {
                // Вызываем функцию на проверку победы и смотрим что вернула
                switch (IsWin())
                {
                    // Желательно добавить красивые таблички о победе с 2 кнопками, переиграть, главное меню
                    case 1:
                        // Выиграл X
                        if (currentPlayer == "X")
                        {
                            EndGame = new end_of_game.end_of_game("Win", this);
                            EndGame.Owner = this;
                            EndGame.Show();
                        }
                        else
                        {
                            EndGame = new end_of_game.end_of_game("Fail", this);
                            EndGame.Owner = this;
                            EndGame.Show();
                        }
                        DeactivateAllButtons();
                        return true;

                    case 2:
                        // Выиграл O
                        if (currentPlayer == "O")
                        {
                            EndGame = new end_of_game.end_of_game("Win", this);
                            EndGame.Owner = this;
                            EndGame.Show();
                        }
                        else
                        {
                            EndGame = new end_of_game.end_of_game("Fail", this);
                            EndGame.Owner = this;

                            EndGame.Show();
                        }
                        DeactivateAllButtons();
                        return true;

                    case 3:
                        // Ничья
                        EndGame = new end_of_game.end_of_game("Draw", this);
                        EndGame.Owner = this;
                        EndGame.Show();
                        DeactivateAllButtons();
                        return true;
                }
            }
            return false;
        }

        private int IsWin()
        {
            // Проверяем миллион условий на победу 
            if (map[0, 0] == 1 && map[0, 1] == 1 && map[0, 2] == 1)
                return 1;
            else if (map[0, 0] == 1 && map[1, 0] == 1 && map[2, 0] == 1)
                return 1;
            else if (map[2, 0] == 1 && map[2, 1] == 1 && map[2, 2] == 1)
                return 1;
            else if (map[0, 2] == 1 && map[1, 2] == 1 && map[2, 2] == 1)
                return 1;
            else if (map[0, 0] == 1 && map[1, 1] == 1 && map[2, 2] == 1)
                return 1;
            else if (map[0, 2] == 1 && map[1, 1] == 1 && map[2, 0] == 1)
                return 1;
            else if (map[0, 1] == 1 && map[1, 1] == 1 && map[2, 1] == 1)
                return 1;
            else if (map[1, 0] == 1 && map[1, 1] == 1 && map[1, 2] == 1)
                return 1;

            if (map[0, 0] == 2 && map[0, 1] == 2 && map[0, 2] == 2)
                return 2;
            else if (map[0, 0] == 2 && map[1, 0] == 2 && map[2, 0] == 2)
                return 2;
            else if (map[2, 0] == 2 && map[2, 1] == 2 && map[2, 2] == 2)
                return 2;
            else if (map[0, 2] == 2 && map[1, 2] == 2 && map[2, 2] == 2)
                return 1;
            else if (map[0, 0] == 2 && map[1, 1] == 2 && map[2, 2] == 2)
                return 2;
            else if (map[0, 2] == 2 && map[1, 1] == 2 && map[2, 0] == 2)
                return 2;
            else if (map[0, 1] == 2 && map[1, 1] == 2 && map[2, 1] == 2)
                return 2;
            else if (map[1, 0] == 2 && map[1, 1] == 2 && map[1, 2] == 2)
                return 2;

            else if (count == 9) // Ничья
                return 3;

            else return 0;       // Нет победителя
        }

        // Зафиксировать изменения после хода соперника
        private void ChangeAfterListen(string msg)
        {
            string[] razborMessage = new string[10];
            razborMessage = msg.Split(',');  
            string val = razborMessage[9].ToString();         // Фигура которой он играет
            if (msg != "")
            {
                // Выполняем в отдельном потоке
                this.Invoke((MethodInvoker)delegate ()
                {
                    if(currentPlayer == "")
                    {
                        if(val == "X") { currentPlayer = "O"; }
                        else { currentPlayer = "X"; }
                    }

                    int counter = 0;
                    for(int i = 0; i < mapSize; i++)
                    {
                        for(int j = 0; j < mapSize; j++)
                        {
                            int input = Convert.ToInt32(razborMessage[counter]);
                            if (map[i, j] != input)
                            {
                                if (input == 1)
                                {
                                    buttons[i, j].Image = tacFigure;
                                    buttons[i, j].BackColor = Color.White;
                                }
                                else 
                                { 
                                    buttons[i, j].Image = toeFigure;
                                    buttons[i, j].BackColor = Color.White;
                                }
                            }
                            
                            map[i, j] = input;
                            counter++;
                        }
                    }
                    count++;    // Увеличиваем ход на 1
                                            
                    pictureWait.Visible = false;    // Делаем табличку с ожиданием неактивной, так как мы дождались ответа соперника
                    TableForWinner();               // Проверяем на победу
                });
            }
        }

        // Активировать все кнопки
        private void ActivateAllButtons()
        {
            for(int i = 0; i < mapSize; i++)
            {
                for(int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].Enabled = true;
                }
            }
        }

        // Деактивировать все кнопки
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

        // Подключение к серверу
        private void Server_Connect()
        {
            // Создаём клиент
            client = new TcpClient();
            try
            {
                client.Connect(host, port);     // Подключение клиента
                stream = client.GetStream();    // Получаем поток

                // Получаем ID
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

                // Запускаем новый поток для получения данных от сервера
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

                    this.Invoke((MethodInvoker)delegate ()
                    {
                        ChangeAfterListen(builder.ToString());  // Вызываем функцию разбора пришедшего сообщения
                        ActivateAllButtons();                   // Активируем все кнопки 
                        TableForWinner();                       // Проверяем нет ли победителя
                    });
                    Console.WriteLine(builder.ToString());      // Вывод полученного сообщения
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!"); // Соединение было прервано
                    MessageBox.Show("Подключение разрвано, приложение закроется через 5 секунд", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Thread.Sleep(5000);
                    Disconnect();
                }
            }
        }

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

        void Disconnect()
        {
            if (stream != null)
                stream.Close();     // Отключение потока
            if (client != null)
                client.Close();     // Отключение клиента
            Environment.Exit(0);    // Закртиые приложения
        }

        private void Krestiki_Noliki_Load(object sender, EventArgs e)
        {
            PictureBox PB = new PictureBox();
            PB.Dock = DockStyle.Fill;
            PB.Location = new Point(0, 0);
            PB.Image = Properties.Resources.tic_tac_back;


            Graphics g = Graphics.FromImage(PB.Image);
            Pen PenGird = new Pen(Color.Black, 5);

            //Отрисовка линий по вертикали
            for (int i = 0; i < 4; i++)
            {
                int tmp = 0;
                if (i == 1) { tmp = 5; }
                if (i == 2) { tmp = 10; }
                if (i == 3) { tmp = 15; }

                g.DrawLine(PenGird, new Point(97 + cellSize * i + tmp, 20), new Point(97 + cellSize * i + tmp, Height - 59));
            }

            //Отрисовка линий по горизонтали
            for (int i = 0; i < 4; i++)
            {
                int tmp = 0;
                if (i == 1) { tmp = 5; }
                if (i == 2) { tmp = 10; }
                if (i == 3) { tmp = 15; }

                g.DrawLine(PenGird, new Point(95, 22 + cellSize * i + tmp), new Point(Width - 115, 22 + cellSize * i + tmp));
            }

            this.Controls.Add(PB);
        }

        private void Krestiki_Noliki_FormClosing(object sender, FormClosingEventArgs e)
        {
            ListGames lg = new ListGames();

            if (count > 0)
            {
                DialogResult dialog = MessageBox.Show("Игра только началась. Закрыть окно?", "Предупреждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialog == DialogResult.Yes)
                {
                    sound.Stop();
                    lg.Show();
                }
            }
            else
            {
                sound.Stop();
                lg.Show();
            }
        }
    }
}
