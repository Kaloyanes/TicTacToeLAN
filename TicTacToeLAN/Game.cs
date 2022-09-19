using System.ComponentModel;
using System.Net;
using System.Net.Sockets;

namespace TicTacToeLAN
{
    public partial class Game : Form
    {
        private int port = 5732;
        private char PlayerChar;
        private char OpponentChar;
        private Socket Socket;
        private BackgroundWorker MessageReciever = new BackgroundWorker();
        private TcpListener Server = null;
        private TcpClient Client;

        private Dictionary<int, Button> AvailableOptions = new Dictionary<int, Button>();

        public Game(bool isHost, string ip = null)
        {
            InitializeComponent();

            MessageReciever.DoWork += MessageReciever_DoWork;
            CheckForIllegalCrossThreadCalls = false;


            if (isHost)
            {
                PlayerChar = 'X';
                OpponentChar = 'O';

                Server = new TcpListener(IPAddress.Any, port);
                Server.Start();
                Socket = Server.AcceptSocket();
            }
            else
            {
                PlayerChar = 'O';
                OpponentChar = 'X';

                try
                {
                    Client = new TcpClient(ip, port);
                    Socket = Client.Client;

                    MessageReciever.RunWorkerAsync();
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message);
                    Close();
                }
            }

            Text = $"You are {PlayerChar}";
            AvailableOptions.Add(1, button1);
            AvailableOptions.Add(2, button2);
            AvailableOptions.Add(3, button3);
            AvailableOptions.Add(4, button4);
            AvailableOptions.Add(5, button5);
            AvailableOptions.Add(6, button6);
            AvailableOptions.Add(7, button7);
            AvailableOptions.Add(8, button8);
            AvailableOptions.Add(9, button9);

            foreach (var availableOption in AvailableOptions)
            {
                availableOption.Value.Text = "";
            }
        }

        private void Game_FormClosing(object sender, FormClosingEventArgs e)
        {
            MessageReciever.WorkerSupportsCancellation = true;
            MessageReciever.CancelAsync();

            if (Server != null)
            {
                Server.Stop();
            }
            else
            {
                Client.Close();
            }
        }

        private void MessageReciever_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (CheckForWinner())
                return;

            Freeze();
            turnLbl.Text = $"{OpponentChar}'s Turn";
            ReceiveMove();
            turnLbl.Text = "Your Turn";

            if (!CheckForWinner())
                UnFreeze();
        }

        private void Show(string condition)
        {
            MessageBox.Show(condition);
            turnLbl.Text = condition;
        }

        private bool CheckForWinner()
        {
            string condition = "";

            #region Horizontal

            bool horizontal = button1.Text == button2.Text && button2.Text != "" && button2.Text == button3.Text;
            if (horizontal)
            {
                condition = $"{button1.Text} Wins!";
                Show(condition);
                return true;
            }

            horizontal = button4.Text == button5.Text && button5.Text != "" && button5.Text == button6.Text;
            if (horizontal)
            {
                condition = $"{button4.Text} Wins!";
                Show(condition);

                return true;
            }

            horizontal = button7.Text == button8.Text && button8.Text != "" && button8.Text == button9.Text;
            if (horizontal)
            {
                condition = $"{button7.Text} Wins!";
                Show(condition);
                return true;
            }

            #endregion

            #region Vertical

            bool vertical = button1.Text == button4.Text && button4.Text != "" && button4.Text == button7.Text;
            if (vertical)
            {
                condition = $"{button1.Text} Wins!";
                Show(condition);
                return true;
            }

            vertical = button2.Text == button5.Text && button5.Text != "" && button5.Text == button8.Text;
            if (vertical)
            {
                condition = $"{button2.Text} Wins!";
                Show(condition);
                return true;
            }

            vertical = button3.Text == button6.Text && button6.Text != "" && button6.Text == button9.Text;
            if (vertical)
            {
                condition = $"{button3.Text} Wins!";
                Show(condition);
                return true;
            }

            #endregion


            #region Diagonal

            // Diagonal
            bool diagonal = button1.Text == button5.Text && button5.Text != "" && button5.Text == button9.Text;
            if (diagonal)
            {
                condition = $"{button1.Text} Wins!";
                Show(condition);
                return true;
            }

            diagonal = button3.Text == button5.Text && button5.Text != "" && button5.Text == button7.Text;
            if (diagonal)
            {
                condition = $"{button3.Text} Wins!";
                Show(condition);
                return true;
            }

            #endregion

            // Draw
            if (AvailableOptions.Count == 0)
            {
                condition = "Draw!";
                Show(condition);
                return true;
            }


            return false;
        }

        private void Freeze()
        {
            foreach (var availableOption in AvailableOptions)
            {
                availableOption.Value.Enabled = false;
            }
        }

        public void UnFreeze()
        {
            foreach (var availableOption in AvailableOptions)
            {
                availableOption.Value.Enabled = true;
            }
        }

        public void ReceiveMove()
        {
            byte[] buffer = new byte[1];
            Socket.Receive(buffer);

            var btn = AvailableOptions[buffer[0]];

            btn.Text = OpponentChar.ToString();
            AvailableOptions.Remove(buffer[0]);
        }

        private void buttonClick(Button btn, int num)
        {
            byte[] nums = {(byte) num};
            Socket.Send(nums);

            btn.Text = PlayerChar.ToString();

            MessageReciever.RunWorkerAsync();
            AvailableOptions.Remove(num);
            btn.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            buttonClick(sender as Button, 1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            buttonClick(sender as Button, 2);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            buttonClick(sender as Button, 3);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            buttonClick(sender as Button, 4);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            buttonClick(sender as Button, 5);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            buttonClick(sender as Button, 6);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            buttonClick(sender as Button, 7);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            buttonClick(sender as Button, 8);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            buttonClick(sender as Button, 9);
        }
    }
}