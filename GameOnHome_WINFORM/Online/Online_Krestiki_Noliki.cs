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
        string userName;                            // Фигура игркока X/O                 
        private const string host = "127.0.0.1";    // IP
        private const int port = 7770;              // Порт
        TcpClient client;                           // Клиент
        NetworkStream stream;                       // Поток от клиента до сервера

        int count = 0;                              // Количество сделанных ходов
        public Online_Krestiki_Noliki()
        {
            InitializeComponent();

            foreach (Control b in panel1.Controls)
            {
                if (b.Name != "pictureWait")
                {
                    b.Click += Button_click;
                }
            }
        }

        public void Button_click(object sender, EventArgs e)
        {
            if (count == 0)
            {
                if(userName == null) { userName = "X"; }
                else { userName = "O"; }
            }

            ((Button)sender).Text = userName.ToString();
            ((Button)sender).Enabled = false;
            count++;


            this.Invoke((MethodInvoker)delegate ()                  // Выполнения действие с формой в потоке
            {
                //pictureWait.Visible = true;
                pictureWait.BackColor = Color.Transparent;
            });

            if (client != null)
            {
                SendMessage((sender as Button).Name + "_" + userName);
                TableForWinner();
            }
        }

        public void TableForWinner()
        {
            if(count > 4)
            {

                switch (IsWin())
                {
                    case 1:
                        if(userName == "X")
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
                        /*
                        grid_end_game.Visibility = Visibility.Visible;
                        text_end_game.Content = "Игрок O выиграл!";
                        Wait.Visibility = Visibility.Hidden;
                        */
                        MessageBox.Show("Player O win");
                        break;

                    case 3:
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
            string[,] values = new string[3, 3];    // Масив для хранения значений ячеек

            // Смотрим все кнопки

            values[0, 0] = button1.Text;
            values[0, 1] = button2.Text;
            values[0, 2] = button3.Text;
            values[1, 0] = button4.Text;
            values[1, 1] = button5.Text;
            values[1, 2] = button6.Text;
            values[2, 0] = button7.Text;
            values[2, 1] = button8.Text;
            values[2, 2] = button9.Text;

            if (values[0, 0] == "X" && values[0, 1] == "X" && values[0, 2] == "X")
                return 1;
            else if (values[0, 0] == "X" && values[1, 0] == "X" && values[2, 0] == "X")
                return 1;
            else if (values[2, 0] == "X" && values[2, 1] == "X" && values[2, 2] == "X")
                return 1;
            else if (values[0, 2] == "X" && values[1, 2] == "X" && values[2, 2] == "X")
                return 1;
            else if (values[0, 0] == "X" && values[1, 1] == "X" && values[2, 2] == "X")
                return 1;
            else if (values[0, 2] == "X" && values[1, 1] == "X" && values[2, 0] == "X")
                return 1;
            else if (values[0, 1] == "X" && values[1, 1] == "X" && values[2, 1] == "X")
                return 1;
            else if (values[1, 0] == "X" && values[1, 1] == "X" && values[1, 2] == "X")
                return 1;

            if (values[0, 0] == "O" && values[0, 1] == "O" && values[0, 2] == "O")
                return 2;
            else if (values[0, 0] == "O" && values[1, 0] == "O" && values[2, 0] == "O")
                return 2;
            else if (values[2, 0] == "O" && values[2, 1] == "O" && values[2, 2] == "O")
                return 2;
            else if (values[0, 2] == "O" && values[1, 2] == "O" && values[2, 2] == "O")
                return 1;
            else if (values[0, 0] == "O" && values[1, 1] == "O" && values[2, 2] == "O")
                return 2;
            else if (values[0, 2] == "O" && values[1, 1] == "O" && values[2, 0] == "O")
                return 2;
            else if (values[0, 1] == "O" && values[1, 1] == "O" && values[2, 1] == "O")
                return 1;
            else if (values[1, 0] == "O" && values[1, 1] == "O" && values[1, 2] == "O")
                return 1;

            else if (count == 9)
                return 3;

            else return 0;
        }

        private void ChangeAfterListen(string msg)
        {
            string nameButton = msg.Split('_')[0];
            string val = msg[8].ToString();
            if (msg != "")
            {
                this.Invoke((MethodInvoker)delegate ()
                {
                    Control control = panel1.Controls.Owner;
                    foreach (Control b in panel1.Controls)
                    {
                        if (b.Name == nameButton)
                        {
                            if(count == 0)
                            {
                                userName = "O";
                            }
                            b.Text = val;
                            b.Enabled = false;
                            //userName = "O";
                            break;
                        }
                    }

                    count++;
                    pictureWait.Visible = false;
                    TableForWinner();
                });
            }
        }
        
        private void buttonConnect_Click(object sender, EventArgs e)
        {
            client = new TcpClient();
            try
            {
                client.Connect(host, port); //подключение клиента
                stream = client.GetStream(); // получаем поток

                // запускаем новый поток для получения данных
                Thread receiveThread = new Thread(new ThreadStart(ReceiveMessage));
                receiveThread.Start(); //старт потока

                Status_connect.Text = "Статус: Подключён";
                pictureConnect.Image = Properties.Resources.green_krug;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void SendMessage(string message)
        {
            if ((message != ""))
            {
                byte[] buffer = new byte[1024];
                buffer = Encoding.UTF8.GetBytes(message);   // Делаем байт код и отправляем его на сервер
                stream.Write(buffer, 0, buffer.Length);
            }
        }

        // получение сообщений
        void ReceiveMessage()
        {
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

                    ChangeAfterListen(builder.ToString());

                    Console.WriteLine(builder.ToString());//вывод сообщения
                }
                catch
                {
                    Console.WriteLine("Подключение прервано!"); //соединение было прервано
                    Console.ReadLine();
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
