using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace WindowsFormsApplication2
{
    class Logic
    {
        public Logic()
        {
            crossCountWin = 0;
            circleCountWin = 0;
            noneWin = 0;
        }

        int crossCountWin = 0;
        int circleCountWin = 0;
        int noneWin = 0;

        const int PLAYER_CLICKED = 1;
        const int BOT_CLICKED = -1;
        const int EMPTY = 0;
        const int FAIL = -1;

        enum gameStatus
        {
            WIN = 1,
            FAIL = -1,
            PROCESS = 2,
            NULL = 0
        }

        public int GetCrossWinCount()
        {
            return crossCountWin;
        }

        public int GetCircleWinCount()
        {
            return circleCountWin;
        }

        public int GetNoneWinCount()
        {
            return noneWin;
        }

        private int[,] Map = new int[3, 3];

        public void ClearMap()
        {
            for (int i = 0; i < Map.GetLength(0); i++)
            {
                for (int j = 0; j < Map.GetLength(1); j++)
                {
                    Map[i, j] = 0;
                }
            }
        }

        public void SetToMap(int i, int j, int  value)
        {
            Map[i, j] = value;
        }

        public bool CheckGame(int moveCount)
        {
            gameStatus status = isWin(moveCount);
            
            for (int i = 0; i < 3; i++)
            { 
                for ( int j = 0; j < 3; j++)
                {
                    Console.Write(Map[i, j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine();
               
            if (status != gameStatus.PROCESS)
            {
                //string messageEnd = "";
                if (status == gameStatus.WIN)
                {
                    //messageEnd = "Ты Победил!";
                    crossCountWin += 1;
                }
                else if (status == gameStatus.FAIL)
                {
                    //messageEnd = "Ты Проиграл!";
                    circleCountWin += 1;
                }
                else
                {
                    //messageEnd = "Ничья! :)";
                    noneWin += 1;
                }
                return false;
            }
            return true;
        }

        private Pair<Point, Point> CheckWinPoints(int i, int somebodyClicked)
        {
            if (Map[i, 1] == somebodyClicked)
            {
                if (Map[i, 0] == somebodyClicked && Map[i, 2] == somebodyClicked)
                {
                    return new Pair<Point, Point>(new Point(i, 0), new Point(i, 2));
                }
            }
            if (Map[1, i] == somebodyClicked)
            {
                if (Map[0, i] == somebodyClicked && Map[2, i] == somebodyClicked)
                {
                    return new Pair<Point, Point>(new Point(0, i), new Point(2, i));
                }
            }

            return new Pair<Point, Point>(new Point(FAIL, FAIL), new Point(FAIL, FAIL));
        }

        private Pair<Point, Point> CheckWinPoints(int somebodyClicked)
        {
            if (Map[1, 1] == somebodyClicked)
            {
                if (Map[0, 0] == somebodyClicked && Map[2, 2] == somebodyClicked)
                {
                    return new Pair<Point, Point>(new Point(0, 0), new Point(2, 2));
                }
                if (Map[0, 2] == somebodyClicked && Map[2, 0] == somebodyClicked)
                {
                    return new Pair<Point, Point>(new Point(0, 2), new Point(2, 0));
                }
            }
            return new Pair<Point, Point>(new Point(FAIL, FAIL), new Point(FAIL, FAIL));
        }

        public Pair<Point, Point> getWinPoints()
        {
            Pair<Point, Point> point;
            for (int i = 0; i < 3; ++i)
            {
                point = CheckWinPoints(i, PLAYER_CLICKED);
                if (point.First.X != FAIL)
                {
                    return point;
                }

                point = CheckWinPoints(i, BOT_CLICKED);
                if (point.First.X != FAIL)
                {
                    return point;
                }
            }

            point = CheckWinPoints(PLAYER_CLICKED);
            if (point.First.X != FAIL)
            {
                return point;
            }

            point = CheckWinPoints(BOT_CLICKED);
            if (point.First.X != FAIL)
            {
                return point;
            }
            return new Pair<Point, Point>(new Point(FAIL, FAIL), new Point(FAIL, FAIL));
        }

        public Point GetPointCross(Point point)
        {
            if (point.X == FAIL || Map[point.X, point.Y] != EMPTY)
            {
                throw new Exception("Incorrect point!");
            }
            return point;

        }

        public Point GetPointCircle(int countMoves)
        {
            if (countMoves == 1)
            {
                return GetFirstPoint();
            }

            Point point = GetWinPoint();
            if (point.X != FAIL)
            {
                return point;
            }

            point = GetNotWinPlayerPoint();
            if (point.X != FAIL)
            {
                return point;
            }

            return GetRandomPoint();
        }

        public void MoveCircle(int countMoves)
        {
            Point point = GetPointCircle(countMoves);
            SetToMap(point.X, point.Y, BOT_CLICKED);
        }

        private bool CheckClicked(int i, int somebodyClicked)
        {
            if (Map[i, 1] == somebodyClicked)
            {
                if (Map[i, 0] == somebodyClicked && Map[i, 2] == somebodyClicked)
                {
                    return true;
                }
            }
            if (Map[1, i] == somebodyClicked)
            {
                if (Map[0, i] == somebodyClicked && Map[2, i] == somebodyClicked)
                {
                    return true;
                }
            }


            return false;
        }

        private bool CheckClicked(int somebodyClicked)
        {
            if (Map[1, 1] == somebodyClicked)
            {
                if ((Map[0, 0] == somebodyClicked && Map[2, 2] == somebodyClicked) ||
                    (Map[0, 2] == somebodyClicked && Map[2, 0] == somebodyClicked))
                {
                    return true;
                }
            }
            return false;
        }

        private gameStatus isWin(int moveCount)
        {
            for (int i = 0; i < 3; ++i)
            {
                if (CheckClicked(i, PLAYER_CLICKED))
                {
                    return gameStatus.WIN;
                }
                if (CheckClicked(i, BOT_CLICKED))
                {
                    return gameStatus.FAIL;
                }
            }

            if (CheckClicked(PLAYER_CLICKED))
            {
                return gameStatus.WIN;
            }
            if (CheckClicked(BOT_CLICKED))
            {
                return gameStatus.FAIL;
            }

            if (moveCount == 9)
            {
                return gameStatus.NULL;
            }

            return gameStatus.PROCESS;
        }

        private Point GetWinPoint()
        {
            Dictionary<Point, Pair<int, int>[]> winPoints = new Dictionary<Point, Pair<int, int>[]>();
            winPoints.Add(
                new Point(2, 2),
                new Pair<int, int>[] {
                        new Pair<int, int>(Map[0, 0], Map[1, 1]),
                        new Pair<int, int>(Map[2, 0], Map[2, 1]),
                        new Pair<int, int>(Map[0, 2], Map[1, 2])
                }
            );
            winPoints.Add(
                new Point(0, 0),
                new Pair<int, int>[] {
                        new Pair<int, int>(Map[2, 2], Map[1, 1]),
                        new Pair<int, int>(Map[0, 2], Map[0, 1]),
                        new Pair<int, int>(Map[2, 0], Map[1, 0])
                }
            );
            winPoints.Add(
                new Point(0, 2),
                new Pair<int, int>[] {
                        new Pair<int, int>(Map[1, 1], Map[2, 0]),
                        new Pair<int, int>(Map[0, 0], Map[0, 1]),
                        new Pair<int, int>(Map[2, 2], Map[1, 2])
                }
            );
            winPoints.Add(
                new Point(2, 0),
                new Pair<int, int>[] {
                        new Pair<int, int>(Map[1, 1], Map[0, 2]),
                        new Pair<int, int>(Map[0, 0], Map[1, 0]),
                        new Pair<int, int>(Map[2, 2], Map[2, 1])
                }
            );
            winPoints.Add(
                new Point(0, 1),
                new Pair<int, int>[] {
                        new Pair<int, int>(Map[2, 1], Map[1, 1]),
                        new Pair<int, int>(Map[0, 0], Map[0, 2])

                }
            );
            winPoints.Add(
                new Point(1, 0),
                new Pair<int, int>[] {
                        new Pair<int, int>(Map[1, 1], Map[1, 2]),
                        new Pair<int, int>(Map[0, 0], Map[2, 0])

                }
            );
            winPoints.Add(
                new Point(1, 2),
                new Pair<int, int>[] {
                        new Pair<int, int>(Map[1, 1], Map[1, 0]),
                        new Pair<int, int>(Map[2, 2], Map[0, 2])

                }
            );
            winPoints.Add(
                new Point(2, 1),
                new Pair<int, int>[] {
                        new Pair<int, int>(Map[0, 1], Map[1, 1]),
                        new Pair<int, int>(Map[2, 2], Map[2, 0])

                }
            );

            foreach (KeyValuePair<Point, Pair<int, int>[]> element in winPoints)
            {
                Point point = element.Key;
                foreach (Pair<int, int> data in element.Value)
                {
                    if (data.First == BOT_CLICKED && data.Second == BOT_CLICKED && Map[point.X, point.Y] == EMPTY)
                    {
                        return point;
                    }
                }
            }
            return new Point(FAIL, FAIL);
        }

        private Point GetNotWinPlayerPoint()
        {
            Dictionary<Point, Pair<int, int>[]> unWinPlayerPoints = new Dictionary<Point, Pair<int, int>[]>();
            unWinPlayerPoints.Add(
                new Point(0, 0),
                new Pair<int, int>[] {
                    new Pair<int, int>(Map[0, 2], Map[0, 1]),
                    new Pair<int, int>(Map[0, 2], Map[1, 0]),

                    new Pair<int, int>(Map[2, 0], Map[1, 0]),
                    new Pair<int, int>(Map[2, 0], Map[0, 1]),

                    new Pair<int, int>(Map[0, 1], Map[1, 0])
                }
            );
            unWinPlayerPoints.Add(
                new Point(0, 2),
                new Pair<int, int>[] {
                    new Pair<int, int>(Map[1, 1], Map[2, 0]),
                    new Pair<int, int>(Map[0, 0], Map[0, 1]),

                    new Pair<int, int>(Map[2, 2], Map[1, 2]),
                    new Pair<int, int>(Map[0, 1], Map[1, 2]),

                    new Pair<int, int>(Map[0, 0], Map[1, 2]),
                    new Pair<int, int>(Map[0, 1], Map[2, 2])
                }
            );
            unWinPlayerPoints.Add(
                new Point(2, 0),
                new Pair<int, int>[] {
                    new Pair<int, int>(Map[1, 1], Map[0, 2]),
                    new Pair<int, int>(Map[0, 0], Map[1, 0]),

                    new Pair<int, int>(Map[2, 2], Map[2, 1]),
                    new Pair<int, int>(Map[2, 1], Map[1, 0]),

                    new Pair<int, int>(Map[1, 0], Map[2, 2]),
                    new Pair<int, int>(Map[2, 1], Map[0, 0])
                }
            );
            unWinPlayerPoints.Add(
                new Point(2, 2),
                new Pair<int, int>[] {
                    new Pair<int, int>(Map[2, 0], Map[2, 1]),
                    new Pair<int, int>(Map[0, 2], Map[1, 2]),

                    new Pair<int, int>(Map[2, 1], Map[1, 2]),
                    new Pair<int, int>(Map[1, 2], Map[2, 0]),

                    new Pair<int, int>(Map[2, 1], Map[0, 2])
                }
            );
            unWinPlayerPoints.Add(
                new Point(0, 1),
                new Pair<int, int>[] {
                    new Pair<int, int>(Map[2, 1], Map[1, 1]),
                    new Pair<int, int>(Map[0, 0], Map[0, 2]),
                }
            );
            unWinPlayerPoints.Add(
                new Point(1, 0),
                new Pair<int, int>[] {
                    new Pair<int, int>(Map[1, 1], Map[1, 2]),
                    new Pair<int, int>(Map[0, 0], Map[1, 0]),
                    new Pair<int, int>(Map[0, 0], Map[2, 0])
                }
            );
            unWinPlayerPoints.Add(
                new Point(1, 2),
                new Pair<int, int>[] {
                    new Pair<int, int>(Map[1, 1], Map[1, 0]),
                    new Pair<int, int>(Map[2, 2], Map[0, 2])
                }
            );
            unWinPlayerPoints.Add(
                new Point(2, 1),
                new Pair<int, int>[] {
                    new Pair<int, int>(Map[0, 1], Map[1, 1]),
                    new Pair<int, int>(Map[2, 2], Map[2, 0])
                }
            );

            foreach (KeyValuePair<Point, Pair<int, int>[]> element in unWinPlayerPoints)
            {
                Point point = element.Key;
                foreach (Pair<int, int> data in element.Value)
                {
                    if (data.First == PLAYER_CLICKED && data.Second == PLAYER_CLICKED && Map[point.X, point.Y] == EMPTY)
                    {
                        return point;
                    }
                }
            }
            return new Point(FAIL, FAIL);
        }

        private Point GetRandomPoint()
        {
            Random g = new Random();
            Point point = new Point();
            do
            {
                point.X = g.Next(0, 3);
                point.Y = g.Next(0, 3);
            } while (Map[point.Y, point.X] != EMPTY);

            return point;
        }

        private Point GetFirstPoint()
        {
            if (Map[1, 1] == EMPTY)
            {
                return new Point(1, 1);
            }

            return GetRandomPoint();
        }
    }
}
