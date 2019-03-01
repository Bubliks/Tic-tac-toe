using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        const int PLAYER_CLICKED = 1;
        const int BOT_CLICKED = -1;
        const int EMPTY = 0;
        const int FAIL = -1;

        Logic gameLogic;
        Map map;

        int WIDTH_MAP;
        int HEIGHT_MAP;

        const int COUNT_PART = 3;
        int WIDTH_PART_MAP;
        int HEIGHT_PART_MAP;

        enum gameStatus
        {
            WIN = 1,
            FAIL = -1,
            PROCESS = 2,
            NULL = 0 
        }

        public Form1()
        {
            InitializeComponent();

            gameLogic = new Logic();
            map = new Map(pictureBox1);

            WIDTH_MAP = pictureBox1.Width;
            HEIGHT_MAP = pictureBox1.Height;

            WIDTH_PART_MAP = WIDTH_MAP / COUNT_PART;
            HEIGHT_PART_MAP = HEIGHT_MAP / COUNT_PART;
            clearMap();   
        }


        private void clearMap()
        {
            gameLogic.ClearMap();
            map.ClearMap();   
        }


        int moveCount = 0;
        bool gamego = true;

        private void button1_Click(object sender, EventArgs e)
        {
            clearMap();

            pictureBox1.Enabled = true;
            gamego = true;
            moveCount = 0;
        }

        private Point GetPosition(int x_click, int y_click)
        {
            int i = x_click / WIDTH_PART_MAP;
            int j = y_click / HEIGHT_PART_MAP; 
            if ((i >= 0 && i < WIDTH_MAP) && (j >= 0 && j < HEIGHT_MAP))
            {
                return new Point(i, j); 
            }
            return new Point(FAIL, FAIL);
        }

        private void MoveCross(MouseEventArgs e)
        {
            Point point = gameLogic.GetPointCross(GetPosition(e.Y, e.X));
            gameLogic.SetToMap(point.X, point.Y, PLAYER_CLICKED);
            map.DrawCross(new Point(point.Y, point.X));
        }

        private void MoveCircle(int countMoves)
        {
            Point point = gameLogic.GetPointCircle(countMoves);
            gameLogic.SetToMap(point.X, point.Y, BOT_CLICKED);
            map.DrawCircle(new Point(point.Y, point.X));
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (gamego)
            {
                try { 
                    MoveCross(e);
                }
                catch
                {
                    return;
                }
                if (CheckGame(++moveCount) == false)
                {
                    return;
                }

                MoveCircle(moveCount);
                gamego = CheckGame(++moveCount);
            }
        }

        private bool CheckGame(int moveCount)
        {
            pictureBox1.Image = map.Update();
            if (gameLogic.CheckGame(moveCount) == true)
            {
                return true;
            }

            map.DrawWinLine(gameLogic.getWinPoints());

            pictureBox1.Enabled = false;
            label3.Text = gameLogic.GetCrossWinCount().ToString();
            label4.Text = gameLogic.GetCircleWinCount().ToString();
            label6.Text = gameLogic.GetNoneWinCount().ToString();

            return false;     
        }
    }
}