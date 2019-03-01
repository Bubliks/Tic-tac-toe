using System;
using System.Drawing;
using System.Windows.Forms;

namespace TicTacToe
{
    public partial class  MainForm: Form
    {
        Game game;
        
        public MainForm()
        {
            InitializeComponent();
            game = new Game(pictureBox1);
            StartGame();
        }

        private void StartGame()
        {
            game.StartGame();
            pictureBox1.Enabled = true;
            pictureBox1.Image = game.UpdateMap();
        }

        private void StopGame()
        {
            pictureBox1.Image = game.UpdateMap();   
            pictureBox1.Enabled = false;
            label3.Text = game.GetCrossWinCount().ToString();
            label4.Text = game.GetCircleWinCount().ToString();
            label6.Text = game.GetNoneWinCount().ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            StartGame();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {   
            if (game.Work(e) == false)
            {
                StopGame();
            }
            pictureBox1.Image = game.UpdateMap();
        }
    }
}