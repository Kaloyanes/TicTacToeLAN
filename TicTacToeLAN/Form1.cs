using System.Net.Sockets;

namespace TicTacToeLAN
{
    public partial class Form1 : Form
    {
        private Socket socket;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        private void HostBtn_Click(object sender, EventArgs e)
        {
            JoinCreateGame(true);

            //var newGame = new Game(true);

            //Visible = false;

            //if (!newGame.IsDisposed)
            //    newGame.ShowDialog();

            //Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            JoinCreateGame(false, textBox1.Text);
            //var newGame = new Game(false, textBox1.Text);

            //Visible = false;

            //if (!newGame.IsDisposed)
            //    newGame.ShowDialog();

            //Visible = true;
        }

        private void JoinCreateGame(bool isHost, string ip = null)
        {
            var newGame = new Game(isHost, ip);

            Visible = false;

            if (!newGame.IsDisposed)
                newGame.ShowDialog();

            Visible = true;
        }
    }
}