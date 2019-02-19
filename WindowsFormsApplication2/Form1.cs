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
    public class Pair<T, U>
    {
        public Pair()
        {
        }

        public Pair(T first, U second)
        {
            this.First = first;
            this.Second = second;
        }

        public T First { get; set; }
        public U Second { get; set; }
    };

    public partial class Form1 : Form
    {
        const int PLAYER_CLICKED = 1;
        const int BOT_CLICKED = -1;
        const int EMPTY = 0;
        const int FAIL = -1;

        Bitmap bm;
        Graphics gr;

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
            Bm = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            Gr = Graphics.FromImage(Bm);
            pictureBox1.Image = Bm;

            WIDTH_MAP = pictureBox1.Width;
            HEIGHT_MAP = pictureBox1.Height;

            WIDTH_PART_MAP = WIDTH_MAP / COUNT_PART;
            HEIGHT_PART_MAP = HEIGHT_MAP / COUNT_PART;
            //textBox5.Text = (1 / 2).ToString();
            clearMap();
        }


        private void clearMap()
        {
            for (int i = 0; i < M.GetLength(0); i++)
            {
                for (int j = 0; j < M.GetLength(1); j++)
                {
                    M[i, j] = 0;
                }
            }

            Gr.Clear(Color.White);
            for (int i = 1; i < 3; ++i)
            {
                Gr.DrawLine(Pens.Black, WIDTH_MAP * (i / 3f), 0, WIDTH_MAP * (i / 3f), HEIGHT_MAP);
                Gr.DrawLine(Pens.Black, 0, HEIGHT_MAP * (i / 3f), WIDTH_MAP, HEIGHT_MAP * (i / 3f));
            }

            pictureBox1.Image = Bm;
        }

        int[,] m = new int[3, 3];
        int n = 0;
        int p = 0;
        int c = 0;

        bool gamego = true;
        int ch = 0;
        bool hod = false;
        bool hod_krest = false;

        public Bitmap Bm { get => bm; set => bm = value; }
        public Graphics Gr { get => gr; set => gr = value; }
        public int[,] M { get => m; set => m = value; }
        public int N { get => n; set => n = value; }
        public int P { get => p; set => p = value; }
        public int C { get => c; set => c = value; }
        public bool Gamego { get => gamego; set => gamego = value; }
        public int Ch { get => ch; set => ch = value; }
        public bool Hod { get => hod; set => hod = value; }
        public bool Hod_krest { get => hod_krest; set => hod_krest = value; }

        private void button1_Click(object sender, EventArgs e)
        {
            clearMap();

            pictureBox1.Enabled = true;
            Gamego = true;
            Hod = false;
            Hod_krest = true;
            Ch = 0;
        }

        private bool CheckClicked(int i, int somebodyClicked)
        {
            if (M[i, 1] == somebodyClicked)
            {
                if ((M[i, 0] == somebodyClicked && M[i, 2] == somebodyClicked) ||
                    (M[0, i] == somebodyClicked && M[2, i] == somebodyClicked))
                {
                    return true;
                }
            }
            return false;
        }

        private bool CheckClicked(int somebodyClicked)
        {
            if (M[1, 1] == somebodyClicked)
            {
                if ((M[0, 0] == somebodyClicked && M[2, 2] == somebodyClicked) ||
                    (M[0, 2] == somebodyClicked && M[2, 0] == somebodyClicked))
                {
                    return true;
                }
            }
            return false;
        }

        private gameStatus isWin()
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

            return gameStatus.PROCESS;
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

        private void DrawCross(Point point)
        {
            int offset_width = (int) (WIDTH_PART_MAP * (0.5 / 3f));
            int offset_height = (int)(HEIGHT_PART_MAP * (0.5 / 3f));
            Gr.DrawLine(Pens.Red, point.X * WIDTH_PART_MAP + offset_width, point.Y * HEIGHT_PART_MAP + offset_height, 
                (point.X + 1) * WIDTH_PART_MAP - offset_width , (point.Y + 1) * HEIGHT_PART_MAP - offset_height);
            Gr.DrawLine(Pens.Red, (point.X + 1) * WIDTH_PART_MAP - offset_width, point.Y * HEIGHT_PART_MAP + offset_height,
                 point.X * WIDTH_PART_MAP + offset_width, (point.Y + 1) * HEIGHT_PART_MAP - offset_height);
        }

        private void DrawCircle(Point point)
        {  
            int offset_width = (int)(WIDTH_PART_MAP * (0.5 / 3f));
            int offset_height = (int)(HEIGHT_PART_MAP * (0.5 / 3f));
            Gr.DrawEllipse(Pens.Blue, point.X * WIDTH_PART_MAP + offset_width, point.Y * HEIGHT_PART_MAP + offset_height, 
                 WIDTH_PART_MAP - 2 * offset_width, HEIGHT_PART_MAP - 2 * offset_height);
        }



        private Point GetPointCross(MouseEventArgs e)
        {
            while (true)
            {
                Point point = GetPosition(e.X, e.Y);
                if (point.X == FAIL)
                {
                    continue;
                }
                if (M[point.Y, point.X] == EMPTY)
                {
                    M[point.Y, point.X] = PLAYER_CLICKED;
                    return point;
                }
            }
        }

        private Point GetPointCircle()
        {
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

        private void MoveCross(MouseEventArgs e)
        {
            Point point = GetPointCross(e);
            M[point.X, point.Y] = PLAYER_CLICKED;
            DrawCross(point);
        }

        private void MoveCircle()
        {
            Point point = GetPointCircle();
            M[point.X, point.Y] = BOT_CLICKED;
            DrawCircle(point);
        }

        //НУЖНО ЕЩЕ ПОМЕТИТЬ ТОЧКУ В МАТРИЦЕ КАК ЗАПОЛНЕННУЮ
        private Point GetWinPoint()
        {
            Dictionary<Point, Pair<int, int>[]> winPoints = new Dictionary<Point, Pair<int, int>[]>();
            winPoints.Add(
                new Point(2, 2),
                new Pair<int, int>[] {
                        new Pair<int, int>(M[0, 0], M[1, 1]),
                        new Pair<int, int>(M[2, 0], M[2, 1]),
                        new Pair<int, int>(M[0, 2], M[1, 2])
                }
            );
            winPoints.Add(
                new Point(0, 0),
                new Pair<int, int>[] {
                        new Pair<int, int>(M[2, 2], M[1, 1]),
                        new Pair<int, int>(M[0, 2], M[0, 1]),
                        new Pair<int, int>(M[2, 0], M[1, 0])
                }
            );
            winPoints.Add(
                new Point(0, 2),
                new Pair<int, int>[] {
                        new Pair<int, int>(M[1, 1], M[2, 0]),
                        new Pair<int, int>(M[0, 0], M[0, 1]),
                        new Pair<int, int>(M[2, 2], M[1, 2])
                }
            );
            winPoints.Add(
                new Point(2, 0),
                new Pair<int, int>[] {
                        new Pair<int, int>(M[1, 1], M[0, 2]),
                        new Pair<int, int>(M[0, 0], M[1, 0]),
                        new Pair<int, int>(M[2, 2], M[2, 1])
                }
            );
            winPoints.Add(
                new Point(0, 1),
                new Pair<int, int>[] {
                        new Pair<int, int>(M[2, 1], M[1, 1]),
                        new Pair<int, int>(M[0, 0], M[0, 2])

                }
            );
            winPoints.Add(
                new Point(1, 0),
                new Pair<int, int>[] {
                        new Pair<int, int>(M[1, 1], M[1, 2]),
                        new Pair<int, int>(M[0, 0], M[2, 0])

                }
            );
            winPoints.Add(
                new Point(1, 2),
                new Pair<int, int>[] {
                        new Pair<int, int>(M[1, 1], M[1, 0]),
                        new Pair<int, int>(M[2, 2], M[0, 2])

                }
            );
            winPoints.Add(
                new Point(2, 1),
                new Pair<int, int>[] {
                        new Pair<int, int>(M[0, 1], M[1, 1]),
                        new Pair<int, int>(M[2, 2], M[2, 0])

                }
            );

            foreach (KeyValuePair<Point, Pair<int, int>[]> element in winPoints)
            {
                Point point = element.Key;
                foreach (Pair<int, int> data in element.Value)
                {
                    if (data.First == BOT_CLICKED && data.Second == BOT_CLICKED && M[point.X, point.Y] == EMPTY)
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
                    new Pair<int, int>(M[0, 2], M[0, 1]),
                    new Pair<int, int>(M[0, 2], M[1, 0]),

                    new Pair<int, int>(M[2, 0], M[1, 0]),
                    new Pair<int, int>(M[2, 0], M[0, 1]),                        
                        
                    new Pair<int, int>(M[0, 1], M[1, 0])
                }
            );
            unWinPlayerPoints.Add(
                new Point(0, 2),
                new Pair<int, int>[] {
                    new Pair<int, int>(M[1, 1], M[2, 0]),
                    new Pair<int, int>(M[0, 0], M[0, 1]),

                    new Pair<int, int>(M[2, 2], M[1, 2]),
                    new Pair<int, int>(M[0, 1], M[1, 2]),

                    new Pair<int, int>(M[0, 0], M[1, 2]),
                    new Pair<int, int>(M[0, 1], M[2, 2])
                }
            );
            unWinPlayerPoints.Add(
                new Point(2, 0),
                new Pair<int, int>[] {
                    new Pair<int, int>(M[1, 1], M[0, 2]),
                    new Pair<int, int>(M[0, 0], M[1, 0]),

                    new Pair<int, int>(M[2, 2], M[2, 1]),
                    new Pair<int, int>(M[2, 1], M[1, 0]),

                    new Pair<int, int>(M[1, 0], M[2, 2]),
                    new Pair<int, int>(M[2, 1], M[0, 0])
                }
            );
            unWinPlayerPoints.Add(
                new Point(2, 2),
                new Pair<int, int>[] {
                    new Pair<int, int>(M[2, 0], M[2, 1]),
                    new Pair<int, int>(M[0, 2], M[1, 2]),

                    new Pair<int, int>(M[2, 1], M[1, 2]),
                    new Pair<int, int>(M[1, 2], M[2, 0]),

                    new Pair<int, int>(M[2, 1], M[0, 2])
                }
            );
            unWinPlayerPoints.Add(
                new Point(0, 1),
                new Pair<int, int>[] {
                    new Pair<int, int>(M[2, 1], M[1, 1]),
                    new Pair<int, int>(M[0, 0], M[0, 2]),
                }
            );
            unWinPlayerPoints.Add(
                new Point(1, 0),
                new Pair<int, int>[] {
                    new Pair<int, int>(M[1, 1], M[1, 2]),
                    new Pair<int, int>(M[0, 0], M[1, 0]),
                    new Pair<int, int>(M[0, 0], M[2, 0])
                }
            );
            unWinPlayerPoints.Add(
                new Point(1, 2),
                new Pair<int, int>[] {
                    new Pair<int, int>(M[1, 1], M[1, 0]),
                    new Pair<int, int>(M[2, 2], M[0, 2])
                }
            );
            unWinPlayerPoints.Add(
                new Point(2, 1),
                new Pair<int, int>[] {
                    new Pair<int, int>(M[0, 1], M[1, 1]),
                    new Pair<int, int>(M[2, 2], M[2, 0])
                }
            );

            foreach (KeyValuePair<Point, Pair<int, int>[]> element in unWinPlayerPoints)
            {
                Point point = element.Key;
                foreach (Pair<int, int> data in element.Value)
                {
                    if (data.First == PLAYER_CLICKED && data.Second == PLAYER_CLICKED && M[point.X, point.Y] == EMPTY)
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
            } while (M[point.X, point.Y] != 0);

            return point;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Hod = false;
            Ch++;

            MoveCross(e);

            gameStatus status = isWin();
            if (status == gameStatus.WIN)
            {
                //Gr.DrawLine(Pens.Green, 61, 13, 61, 440);
                MessageBox.Show("Ты Победил!");
                Gamego = false;
                pictureBox1.Enabled = false;
                P += 1;
                label3.Text = P.ToString();
            }

            if (status == gameStatus.FAIL)
            {
                // Gr.DrawLine(Pens.Green, 61, 13, 61, 440);
                MessageBox.Show("Ты Проиграл!");
                Gamego = false;
                pictureBox1.Enabled = false;
                N = +1;
                label3.Text = N.ToString();
            }

            //------------   
            if (Ch == 1)
            {
                if ((M[1, 1] == EMPTY) && (!Hod))
                {
                    M[1, 1] = BOT_CLICKED;
                    Hod = true;
                    DrawCircle(new Point(1, 1));
                }
                else
                {
                    if (M[1, 1] == PLAYER_CLICKED)
                    {
                        Random rand_click = new Random();
                        Point point = new Point(rand_click.Next(0, 4) / 2, rand_click.Next(0, 4) / 2);
                        
                        if ((M[point.Y * 2, point.X * 2] == EMPTY) && (!Hod))
                        {
                            M[point.Y * 2, point.X * 2] = BOT_CLICKED;
                            Hod = true;
                            DrawCircle(point);
                        }

                        for (int i = 0; i < COUNT_PART - 1; i += 2)
                        {
                            if ((M[0, i] == EMPTY) && (!Hod))
                            {
                                M[0, i] = BOT_CLICKED;
                                Hod = true;
                                DrawCircle(new Point(0, i));
                            }
                        }

                        if ((M[2, 0] == 0) && (!Hod))
                        {
                            M[2, 0] = -1;
                            Hod = true;
                            DrawCircle(new Point(2, 0));
                        }

                        if ((M[2, 2] == 0) && (!Hod))
                        {
                            M[2, 2] = -1;
                            Hod = true;
                            DrawCircle(new Point(2, 2));
                        }
                    }
                }
            }
            else
            {

              


            }


            MessageBox.Show("Ничья! :)");
            Gamego = false;
            C += 1;
            label6.Text = C.ToString();
            

          

            status = isWin();
            if (status != gameStatus.PROCESS)
            {
                string messageEnd = "";
                if (status == gameStatus.WIN)
                {
                    messageEnd = "Ты Победил!";
                    P += 1;
                }
                else
                {
                    messageEnd = "Ты Проиграл!";
                    N = +1;
                }

                MessageBox.Show(messageEnd);
                Gamego = false;
                pictureBox1.Enabled = false;
               
                label3.Text = P.ToString();
                label4.Text = N.ToString();
            }

            pictureBox1.Image = Bm;
        }
    }
}