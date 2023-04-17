using static System.Console;

namespace Boll
{
    // Integral vektor för position och hastighet i x & y-led
    public struct V2
    {
        public int x;
        public int y;
        public V2(int x, int y) { this.x = x; this.y = y; }
        public V2(V2 xy) { this.x = xy.x; this.y = xy.y; }
    }

    public class Boll
    {
        static int width = 80;
        static int height = 40;
        V2 xy;
        V2 xyPrev;
        V2 speed;
        bool? horizontal = null;
        int xyDiff, diffCount;
        public V2 Xy { get => xy; set => xy = value; }
        /// <summary>
        /// Boll Constructor
        /// </summary>
        /// <param name="xy">position cordinates</param>
        /// <param name="speed">movement force</param>
        public Boll(V2 XY, V2 SPEED)
        {
            xy = XY;
            xyPrev = new V2(-1, -1); // börjar på minus så den inte suddar säg själv i första bildrutan.
            speed = SPEED;
            if (speed.x != speed.y)
            {
                if (speed.x > speed.y)
                {
                    xyDiff = Math.Abs(speed.x) - Math.Abs(speed.y);
                    diffCount = xyDiff;
                    horizontal = true;
                }
                else if (speed.x < speed.y)
                {
                    xyDiff = Math.Abs(speed.y) - Math.Abs(speed.x);
                    diffCount = xyDiff;
                    horizontal = false;
                }
            }
        }
        public string XyToString()
        {
            return xy.x.ToString() + xy.y.ToString();
        }
        public void CheckWalls()
        {
            if (xy.x + speed.x >= width || xy.x + speed.x < 0)
                speed.x *= -1;
            if (xy.y + speed.y >= height || xy.y + speed.y < 0)
                speed.y *= -1;
        }
        public void Move()
        {
            CheckWalls();
            xyPrev = xy;
            xy.x += speed.x;
            xy.y += speed.y;
        }

        public void PrintSelf()
        {
            SetCursorPosition(xy.x, xy.y);
            Write("O");
            // Write(diffCount);
        }
        public void PrintSelfClearTrail()
        {
            PrintSelf();
            ClearTrail();
        }
        public void ClearTrail()
        {
            SetCursorPosition(xyPrev.x, xyPrev.y);
            Write(" ");
        }
    }

    public class Program
    {
        static int sec_scince_start;
        static int steps_scince_start;

        static int width = 80;
        static int height = 40;

        public static System.Timers.Timer aTimer;
        public static System.Timers.Timer timestep;

        static void Main(string[] args)
        {
            Boll bollen, boll2;
            bollen = new Boll(new V2(5, 5), new V2(1, 1));
            boll2 = new Boll(new V2(10, 3), new V2(2, 1));
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 250;
            aTimer.Elapsed += TimerEvent;
            aTimer.Start();

            timestep = new System.Timers.Timer();
            timestep.Interval = 250;
            timestep.Elapsed += TimerEventStep;
            timestep.Start();

            SetWindowSize(1, 1);
            SetBufferSize(width, height);
            SetWindowSize(width, height);
            CursorVisible = false;

            int sec = sec_scince_start;
            bollen.PrintSelf();
            boll2.PrintSelf();


            while (true)
            {

                    SetCursorPosition(75, 1);
                    Write(sec_scince_start);

                    if (sec_scince_start > sec)
                    {
                        bollen.Move();
                        boll2.Move();
                        sec = sec_scince_start;
                        bollen.PrintSelfClearTrail();
                        boll2.PrintSelfClearTrail();
                    }
                

            }


        }
        private static void FlushKeyboard()
        {
            while (Console.In.Peek() != -1)
                Console.In.Read();
        }
        public static void TimerEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            sec_scince_start++;
        }

        public static void TimerEventStep(Object source, System.Timers.ElapsedEventArgs e)
        {
            steps_scince_start++;
        }
    }
}