using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace DoomCopy
{
    static class Extensions
    {
        public static float VectorToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }
        public static Vector2 FromPolar(float angle, float magnitude)
        {
            return magnitude * new Vector2((float)Math.Cos(angle), (float)Math.Sin(angle));
        }
        public static float ToAngle(this Vector2 vector)
        {
            return (float)Math.Atan2(vector.Y, vector.X);
        }
        public static Vector2 ScaleTo(this Vector2 vector, float length)
        {
            return vector * (length / vector.Length());
        }
        public static float NextFloat(this Random rand, float minValue, float maxValue)
        {
            return (float)rand.NextDouble() * (maxValue - minValue) + minValue;
        }

        public static void DelayAction(float millisecond, Action action)
        {
            var timer = new DispatcherTimer();
            timer.Tick += delegate
            {
                action.Invoke();
                timer.Stop();
            };

            timer.Interval = TimeSpan.FromMilliseconds(millisecond);
            timer.Start();
        }

        public static void RepeatAction(int repeatCount, float interval, Action action)
        {
            for (int i = 0; i < repeatCount; i++)
            {
                DelayAction(interval, delegate () {
                    action();
                });
            }

            /*
            using (var timer = new System.Timers.Timer(interval))
            {
                var task = new Task(() => { });
                int remaining = repeatCount;

                timer.Elapsed += async (sender, args) =>
                {
                    try
                    {
                        await action();
                    }
                    finally
                    {
                        // Complete task.
                        remaining -= 1;

                        if (remaining == 0)
                        {
                            // No more items to process. Complete task.
                            task.Start();
                        }
                    }
                };

                timer.Start();
            }
            */
        }

        static public class HueUtility
        {
            public static Color HSVToColor(float h, float s, float v)
            {
                if (h == 0 && s == 0)
                    return new Color(v, v, v);

                float c = s * v;
                float x = c * (1 - Math.Abs(h % 2 - 1));
                float m = v - c;

                if (h < 1) return new Color(c + m, x + m, m);
                else if (h < 2) return new Color(x + m, c + m, m);
                else if (h < 3) return new Color(m, c + m, x + m);
                else if (h < 4) return new Color(m, x + m, c + m);
                else if (h < 5) return new Color(x + m, m, c + m);
                else return new Color(c + m, m, x + m);
            }
        }
    }
}
