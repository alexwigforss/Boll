using static System.Console;

namespace Boll
{
    public struct V2
    {
        public int x;
        public int y;
        public V2(int x, int y) { this.x = x; this.y = y; }
        public V2(V2 xy) { this.x = xy.x; this.y = xy.y; }
    }

    public class Boll
    {
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
            if (xy.x + speed.x >= 40 || xy.x + speed.x < 0)
                speed.x *= -1;
            if (xy.y + speed.y >= 20 || xy.y + speed.y < 0)
                speed.y *= -1;
        }
        //public bool CheckPos(V2 pos)
        //{
        //    if (pos.x >= 40 || pos.x < 0)
        //        speed.x *= -1;
        //    if (pos.y >= 20 || pos.y < 0)
        //        speed.y *= -1;
        //}
        public void Move()
        {
            CheckWalls();
            xyPrev = xy;
            if (Math.Abs(speed.x) == Math.Abs(speed.y))
            {
                xy.x += speed.x;
                xy.y += speed.y;
            }
            else if (horizontal == true)
            {
                xy.x += speed.x / speed.x;
                if (diffCount == 0)
                {
                    if (xy.y + speed.y >= 20 || xy.y + speed.y < 0)
                        xy.y += speed.y;
                }
                diffCount = (diffCount > 0) ? diffCount - 1 : xyDiff;
            }
            else if (horizontal == false)
            {
                xy.y += speed.y / speed.y;
                if (diffCount == 0)
                {
                    if (xy.x + speed.x >= 40 || xy.x + speed.x < 0)
                        xy.x += speed.x;
                }
                diffCount = (diffCount > 0) ? diffCount - 1 : xyDiff;
            }

        }
        public void PrintSelf()
        {
            SetCursorPosition(xy.x, xy.y);
            //Write("O");
            Write(diffCount);
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

        int width = 40;
        int height = 20;

        public static System.Timers.Timer aTimer;
        public static System.Timers.Timer timestep;


        static void Main(string[] args)
        {
            Boll bollen, boll2;
            bollen = new Boll(new V2(5, 5), new V2(1, 1));
            boll2 = new Boll(new V2(10, 3), new V2(1, 2));
            aTimer = new System.Timers.Timer();
            aTimer.Interval = 250;
            aTimer.Elapsed += TimerEvent;
            aTimer.Start();

            timestep = new System.Timers.Timer();
            timestep.Interval = 250;
            timestep.Elapsed += TimerEventStep;
            timestep.Start();

            SetWindowSize(1, 1);
            SetBufferSize(40, 20);
            SetWindowSize(40, 20);
            CursorVisible = false;

            int sec = sec_scince_start;
            bollen.PrintSelf();
            // boll2.PrintSelf();
            while (true)
            {
                SetCursorPosition(35, 1);
                Write(sec_scince_start);
                // SetCursorPosition(35, 3);
                // Write(steps_scince_start);
                // Write(bollen.XyToString());
                // Write(steps_scince_start);

                if (sec_scince_start > sec)
                {
                    bollen.Move();
                    // boll2.Move();
                    sec = sec_scince_start;
                    bollen.PrintSelfClearTrail();
                    // boll2.PrintSelfClearTrail();
                }
            }
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