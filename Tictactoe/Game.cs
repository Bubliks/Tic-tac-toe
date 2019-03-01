using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace TicTacToe
{
    class Game
    {
        Logic gameLogic;
        Map map;

        const int PLAYER_CLICKED = 1;
        const int BOT_CLICKED = -1;
        const int EMPTY = 0;
        const int FAIL = -1;

        bool isGame;
        int moveCount;

        public Game(PictureBox _map) {  
            gameLogic = new Logic();
            map = new Map(_map);

            isGame = true;
            moveCount = 0;
        }

        public void ClearMap()
        {
            gameLogic.ClearMap();
            map.ClearMap();
        }

        public void StartGame()
        {
            ClearMap();
            map.DrawMap();
        }

        public Bitmap UpdateMap()
        {
            return map.Update();
        }

        public bool Work(MouseEventArgs e)
        {
            if (!isGame)
            {
                return false;
            }

            try
            {
                MoveCross(e);
            }
            catch
            {
                return true;
            }
            if (CheckGame(++moveCount) == false)
            {
                return false;
            }

            MoveCircle(moveCount);
            isGame = CheckGame(++moveCount);
            return true;
        }

        private bool CheckGame(int moveCount)
        {
            if (gameLogic.CheckGame(moveCount) == true)
            {
                return true;
            }

            map.DrawWinLine(gameLogic.getWinPoints());
            return false;
        }

        public int GetCrossWinCount() {
            return gameLogic.GetCrossWinCount();
        }

        public int GetCircleWinCount()
        {
            return gameLogic.GetCircleWinCount();
        }

        public int GetNoneWinCount()
        {
            return gameLogic.GetNoneWinCount();
        }
     
        private void MoveCross(MouseEventArgs e)
        {
            Point point = gameLogic.GetPointCross(map.GetPosition(e.Y, e.X));
            gameLogic.SetToMap(point.X, point.Y, PLAYER_CLICKED);
            map.DrawCross(new Point(point.Y, point.X));
        }

        private void MoveCircle(int countMoves)
        {
            Point point = gameLogic.GetPointCircle(countMoves);
            gameLogic.SetToMap(point.X, point.Y, BOT_CLICKED);
            map.DrawCircle(new Point(point.Y, point.X));
        }
    }
}
