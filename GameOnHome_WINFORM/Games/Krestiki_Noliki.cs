using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GameOnHome_WINFORM.Games
{
    public partial class Krestiki_Noliki : Form
    {
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
        public Krestiki_Noliki(bool IsStatus_)
        {
            InitializeComponent();

            cellSize = (Width / 5) + (Height / 9);      

            IsStatus = IsStatus_;

            Properties.Resources.krest.SetResolution(cellSize, cellSize);
            Properties.Resources.nol.SetResolution(cellSize, cellSize);

            tacFigure = Properties.Resources.krest;
            toeFigure = Properties.Resources.nol;

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
           
            this.Width = (mapSize) * cellSize + 35;
            this.Height = (mapSize) * cellSize + 65;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    // Создаём кнопку и указвыаем нужные нам параметры
                    Button button = new Button();
                    button.Location = new Point(j * cellSize, i * cellSize);    // Местоположение
                    button.BackColor = Color.Gray;
                    button.Size = new Size(cellSize, cellSize);
                    if(IsStatus)
                        button.Click += new EventHandler(Button_click_online);  // Привязываем функцию обработчика нажатий
                    else button.Click += new EventHandler(Button_click_ofline);
                    button.Name = "button" + i.ToString() + "_" + j.ToString(); // Даём ей имя, что бы можно это использовать где потребуется (псевдо ID)

                    buttons[i, j] = button;                                     // Заносим в наш массив кнопок

                    this.Controls.Add(button);                                  // Привязываем кнопки к нашему окну
                }
            }
        }

        // Функция для изменения размера кнопок при изменении размера окна
        private void Krestiki_Noliki_SizeChanged(object sender, EventArgs e)
        {
            if(buttons[0,0] == null) { return; }
            cellSize = (Width / 5) + (Height / 9);

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    buttons[i,j].Location = new Point(j * cellSize, i * cellSize);
                    buttons[i,j].Size = new Size(cellSize, cellSize);
                }
            }
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

            Bot_xod_easy();      // Ход бота
        }

        private void Bot_xod_easy()
        {
            if (count != 9)
            {
                Random rnd = new Random();
                // Получить случайное число (в диапазоне от 0 до 9)
                int xodI = rnd.Next(0, 3);
                int xodJ = rnd.Next(0, 3);

                while (map[xodI, xodJ] != 0)
                {
                    xodI = rnd.Next(0, 3);
                    xodJ = rnd.Next(0, 3);
                }
                map[xodI, xodJ] = 2;

                buttons[xodI, xodJ].Image = toeFigure;
                buttons[xodI, xodJ].BackColor = Color.White;

                count++;
                TableForWinner();
            }else { TableForWinner(); }
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

        private void TableForWinner()
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
                        if(currentPlayer == "X")
                        {
                            pictureWinGame.Visible = true;
                            pictureWait.Visible = false;

                        }
                        DeactivateAllButtons();
                        MessageBox.Show("Player X win");
                        break;

                    case 2:
                        // Выиграл O
                        DeactivateAllButtons();
                        MessageBox.Show("Player O win");
                        break;

                    case 3:
                        // Ничья
                        DeactivateAllButtons();
                        MessageBox.Show("Nichya win");
                        break;

                }            
            }
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
                return 1;
            else if (map[1, 0] == 2 && map[1, 1] == 2 && map[1, 2] == 2)
                return 1;

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
    }
}
