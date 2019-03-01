using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    class Map
    {
        Bitmap bm;
        Graphics gr;
        PictureBox map;

        const int FAIL = -1;

        int WIDTH_MAP;
        int HEIGHT_MAP;

        int WIDTH_PART_MAP;
        int HEIGHT_PART_MAP;
        int COUNT_PART = 3;

        public Map(PictureBox _map)
        {
            map = _map;
            bm = new Bitmap(_map.Width, _map.Height);
            gr = Graphics.FromImage(bm);

            WIDTH_MAP = map.Width;
            HEIGHT_MAP = map.Height;

            WIDTH_PART_MAP = WIDTH_MAP / COUNT_PART;
            HEIGHT_PART_MAP = HEIGHT_MAP / COUNT_PART;
        }

        public void ClearMap()
        {
            gr.Clear(Color.White);
        }

        public void DrawMap()
        {
            int offset = 5;
            Pen pen = new Pen(Color.Gray, 2);
            for (int i = 1; i < 3; ++i)
            {
                gr.DrawLine(pen, WIDTH_MAP * (i / 3f), 0 + offset, WIDTH_MAP * (i / 3f), HEIGHT_MAP - offset);
                gr.DrawLine(pen, 0 + offset, HEIGHT_MAP * (i / 3f), WIDTH_MAP - offset, HEIGHT_MAP * (i / 3f));
            }
        }

        public void DrawWinLine(Pair<Point, Point> point)
        {
            if (point.First.X == FAIL)
            {
                return;
            }

            int offset_x = WIDTH_PART_MAP / 2;
            int offset_y = WIDTH_PART_MAP / 2;
            gr.DrawLine(
                new Pen(Color.Black, 4),
                WIDTH_PART_MAP * point.First.Y + offset_x,
                HEIGHT_PART_MAP * point.First.X + offset_y,
                WIDTH_PART_MAP * point.Second.Y + offset_x,
                HEIGHT_PART_MAP * point.Second.X + offset_y
            );
        }

        public void DrawCross(Point point)
        {
            int offset_width = (int)(WIDTH_PART_MAP * (0.5 / 3f));
            int offset_height = (int)(HEIGHT_PART_MAP * (0.5 / 3f));
            Pen pen = new Pen(Color.Red, 10);

            gr.DrawLine(pen, point.X * WIDTH_PART_MAP + offset_width, point.Y * HEIGHT_PART_MAP + offset_height,
                (point.X + 1) * WIDTH_PART_MAP - offset_width, (point.Y + 1) * HEIGHT_PART_MAP - offset_height);
            gr.DrawLine(pen, (point.X + 1) * WIDTH_PART_MAP - offset_width, point.Y * HEIGHT_PART_MAP + offset_height,
                 point.X * WIDTH_PART_MAP + offset_width, (point.Y + 1) * HEIGHT_PART_MAP - offset_height);
        }

        public void DrawCircle(Point point)
        {
            int offset_width = (int)(WIDTH_PART_MAP * (0.5 / 3f));
            int offset_height = (int)(HEIGHT_PART_MAP * (0.5 / 3f));

            gr.DrawEllipse(new Pen(Color.Blue, 10), point.X * WIDTH_PART_MAP + offset_width, point.Y * HEIGHT_PART_MAP + offset_height,
                 WIDTH_PART_MAP - 2 * offset_width, HEIGHT_PART_MAP - 2 * offset_height);
        }

        public Bitmap Update()
        {
            return bm;
        }
    }
}
