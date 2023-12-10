using System.Diagnostics;
using System.Runtime.InteropServices;

namespace SequenceMemory
{
    public partial class Form1 : Form
    {
        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);
        public static void SetCursorPosition(int x, int y)
        {
            SetCursorPos(x, y);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x02;
        private const uint MOUSEEVENTF_LEFTUP = 0x04;

        public static void LeftClick()
        {
            mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);
        }

        private Color GetColor(Point point)
        {
            Point cursorPos = point;
            using (Bitmap screenshot = new Bitmap(1, 1, System.Drawing.Imaging.PixelFormat.Format32bppArgb))
            {
                using (Graphics g = Graphics.FromImage(screenshot))
                {
                    g.CopyFromScreen(cursorPos, Point.Empty, new Size(1, 1));
                }
                return screenshot.GetPixel(0, 0);
            }
        }

        public Form1()
        {
            InitializeComponent();
        }
        List<Point> Pos = new List<Point>();
        List<Point> Order = new List<Point>();
        bool SavePhase = false;
        bool Started = false;
        StreamWriter f = new StreamWriter("log.txt");
        int lastUpadte = 0;
        int CurrnetLvl = 1;
        private void Form1_Load(object sender, EventArgs e)
        {
            Pos = new List<Point>{
                new Point(830,280), new Point(970, 280), new Point(1100, 280),
                new Point(830, 430), new Point (970, 430), new Point (1100, 430),
                new Point (830, 550), new Point (970, 550), new Point (1100, 550)
            };

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (lastUpadte > 4)
                SavePhase = false;
            if (SavePhase)
            {
                SaveOrder();
            }
            else
            {
                Solve();
            }
            label1.Text = "" + lastUpadte;
            if (Started)
            {
                lastUpadte++;
            }
        }
        private void SaveOrder()
        {
            for (int i = 0; i < Pos.Count; i++)
            {
                if (GetColor(Pos[i]).GetBrightness() > 0.98)
                {
                    f.WriteLine("-Save X:" + Pos[i].X + "  Y:" + Pos[i].Y + "-" + i + 1);
                    Order.Add(Pos[i]);
                    lastUpadte = 0;
                    Started = true;
                }
            }
        }
        private void Solve()
        {
            f.WriteLine("Solve Start " + CurrnetLvl);
            CurrnetLvl++;
            Order = RemoveConsecutiveDuplicates(Order);
            for (int i = 0; i < Order.Count; i++)
            {

                f.WriteLine($"solve X:{Order[i].X}, Y:{Order[i].Y}-{GetButtonIndex(Order[i])}");
                SetCursorPos(Order[i].X, Order[i].Y);
                LeftClick();
            }
            f.WriteLine("solve End");
            SavePhase = true;
            Order.Clear();
            lastUpadte = 0;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            SavePhase = true;
            lastUpadte = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            f.Close();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            Order.RemoveAt(0);
        }

        public static List<T> RemoveConsecutiveDuplicates<T>(List<T> list)
        {
            if (list == null || list.Count < 2)
            {
                return list;
            }

            List<T> result = new List<T> { list[0] };

            for (int i = 1; i < list.Count; i++)
            {
                if (!list[i].Equals(list[i - 1]))
                {
                    result.Add(list[i]);
                }
            }

            return result;
        }
        private int GetButtonIndex(Point point)
        {
            for (int i = 0; i < Pos.Count; i++)
            {
                if (point.X == Pos[i].X && point.Y == Pos[i].Y)
                    return i + 1;
            }
            return -1;
        }

    }
}