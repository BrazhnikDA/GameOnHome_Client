using System;
using System.Drawing;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GameOnHome_WINFORM.Online
{
    public partial class Online_Krestiki_Noliki : Form
    {          
        private const string host = "127.0.0.1";    // IP
        private const int port = 7770;              // Порт
        TcpClient client;                           // Клиент
        NetworkStream stream;                       // Поток от клиента до сервера

        const int mapSize = 3;          // Размер карты 3*3
        const int cellSize = 225;       // Размер ячейки
        string currentPlayer = "";      // X или O

        int[,] map = new int[mapSize, mapSize];             // Карта игры { 0 - Пусто, 1 - Крестик, 2 - Нолик } 
        Button[,] buttons = new Button[mapSize, mapSize];   // Массив кнопок

        Image tacFigure;            // Изображение крестика
        Image toeFigure;            // Изображение нолика

        int count = 0;                              // Количество сделанных ходов
        public Online_Krestiki_Noliki()
        {
            InitializeComponent();

            Properties.Resources.krest.SetResolution(225, 225);
            Properties.Resources.nol.SetResolution(225, 225);

            tacFigure = Properties.Resources.krest;
            toeFigure = Properties.Resources.nol;

            Text = "Крестики-нолики";
            CreatePlayBoard();
        }

        public void CreatePlayBoard()
        {
            map = new int[mapSize, mapSize] {
                { 0,0,0 },
                { 0,0,0 },
                { 0,0,0 },
            };

            this.Width = (mapSize + 1) * cellSize + 50;
            this.Height = (mapSize + 1) * cellSize + 50;

            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    // Создаём кнопку и указвыаем нужные нам параметры
                    Button button = new Button();
                    button.Location = new Point(j * cellSize, i * cellSize);    // Местоположение
                    button.BackColor = Color.Gray;
                    button.Size = new Size(cellSize, cellSize);
                    button.Click += new EventHandler(Button_click);             // Привязываем функцию обработчика нажатий
                    button.Name = "button" + i.ToString() + "_" + j.ToString(); // Даём ей имя, что бы можно это использовать где потребуется (псевдо ID)

                    buttons[i, j] = button;                                     // Заносим в наш массив кнопок

                    this.Controls.Add(button);                                  // Привязываем кнопки к нашему окну
                }
            }
        }

        public void Button_click(object sender, EventArgs e)
        {
            // Если мы делаем первый ход Мы - X
            if (count == 0)
            {
                if(currentPlayer == "") { currentPlayer = "X"; }
                else { currentPlayer = "O"; }
            }

            Button currentButton = sender as Button;

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
            //currentButton.Enabled = false;
            count++;

            this.Invoke((MethodInvoker)delegate ()                  // Выполнения действие с формой в потоке
            {
                // Тут надо расскоментить и сделать пикчерВэйт полупрозрачным
                //pictureWait.Visible = true;
                //pictureWait.BackColor = Color.Transparent;
            });

            // Если клиент к чему-то подключился, отправить сообщение и вызвать функцию на првоерку победителя
            if (client != null)
            {
                SendMessage(GetMap() + currentPlayer);
                DeactivateAllButtons();
                TableForWinner();
            }
        }

        public string GetMap()
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

        public void TableForWinner()
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
                        /*
                        grid_end_game.Visibility = Visibility.Visible;
                        text_end_game.Content = "Игрок X выиграл!";
                        Wait.Visibility = Visibility.Hidden;
                        */
                        MessageBox.Show("Player X win");
                        break;

                    case 2:
                        // Выиграл O
                        /*
                        grid_end_game.Visibility = Visibility.Visible;
                        text_end_game.Content = "Игрок O выиграл!";
                        Wait.Visibility = Visibility.Hidden;
                        */
                        MessageBox.Show("Player O win");
                        break;

                    case 3:
                        // Ничья
                        /*
                        grid_end_game.Visibility = Visibility.Visible;
                        text_end_game.Content = "Ничья";
                        Wait.Visibility = Visibility.Hidden;
                        */
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

            if (map[0, 0] == 0 && map[0, 1] == 0 && map[0, 2] == 0)
                return 2;
            else if (map[0, 0] == 0 && map[1, 0] == 0 && map[2, 0] == 0)
                return 2;
            else if (map[2, 0] == 0 && map[2, 1] == 0 && map[2, 2] == 0)
                return 2;
            else if (map[0, 2] == 0 && map[1, 2] == 0 && map[2, 2] == 0)
                return 1;
            else if (map[0, 0] == 0 && map[1, 1] == 0 && map[2, 2] == 0)
                return 2;
            else if (map[0, 2] == 0 && map[1, 1] == 0 && map[2, 0] == 0)
                return 2;
            else if (map[0, 1] == 0 && map[1, 1] == 0 && map[2, 1] == 0)
                return 1;
            else if (map[1, 0] == 0 && map[1, 1] == 0 && map[1, 2] == 0)
                return 1;

            else if (count == 9) // Ничья
                return 3;

            else return 0;       // Нет победителя
        }

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
                    count++;    // Увеличиваем ходы на 1
                                            
                    pictureWait.Visible = false;    // Делаем табличку с ожиданием неактивной, так как мы дождались ответа соперника
                    TableForWinner();               // Проверяем на победу
                });
            }
        }

        public void ActivateAllButtons()
        {
            for(int i = 0; i < mapSize; i++)
            {
                for(int j = 0; j < mapSize; j++)
                {
                    buttons[i, j].Enabled = true;
                }
            }
        }

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

                // На форме меняем статус подключения
                buttonConnect.Enabled = false;      // Выключаем, больше она нам не понадобится
                Status_connect.Text = "Статус: Подключён";
                pictureConnect.Image = Properties.Resources.green_krug;
            }
            catch (Exception ex)
            {
                // Вывод ошибки в консоль, скорее всего сервер не запущен, или случилась хрень
                Console.WriteLine(ex.Message);
            }
        }

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

                    this.Invoke((MethodInvoker)delegate ()
                    {
                        ChangeAfterListen(builder.ToString()); // Вызываем функцию разбора пришедшего сообщения
                        ActivateAllButtons();
                        TableForWinner();
                    });
                    Console.WriteLine(builder.ToString()); // Вывод полученного сообщения
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    Disconnect();
                }
            }
        }

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
