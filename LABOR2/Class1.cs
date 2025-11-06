using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;

namespace LABOR2
{
    public class MyThread
    {
        public delegate void DrawDelegate(int x, int y);
        private Thread thread;
        private volatile bool isRunning = false;
        private readonly DrawDelegate drawMethod;
        private readonly Size formSize;
        private static readonly BooleanSwitch loggingSwitch = new BooleanSwitch("loggingSwitch", "Activates logging");

        public MyThread(DrawDelegate drawDelegate, Size formSize)
        {
            this.drawMethod = drawDelegate ?? throw new ArgumentNullException(nameof(drawDelegate));
            this.formSize = formSize;
        }

        public void Start()
        {
            if (!isRunning)
            {
                isRunning = true;
                thread = new Thread(Run) { IsBackground = true };
                thread.Start();
            }
        }

        public void Stop()
        {
            isRunning = false;
            if (thread != null && thread.IsAlive)
            {
                thread.Join(1000); // Add timeout to prevent hanging
            }
        }

        private void Run()
        {
            int x = 50, y = 50, dx = 2, dy = 2;
            while (isRunning)
            {
                x += dx;
                y += dy;
                
                // Use actual form dimensions with proper boundary checking
                if (x < 0 || x > formSize.Width - 150) // Account for text width
                {
                    dx = -dx;
                    x = Math.Max(0, Math.Min(x, formSize.Width - 150));
                }
                if (y < 0 || y > formSize.Height - 50) // Account for text height
                {
                    dy = -dy;
                    y = Math.Max(0, Math.Min(y, formSize.Height - 50));
                }
                
                drawMethod(x, y);
                
                if (loggingSwitch.Enabled)
                {
                    Trace.WriteLine($"Text moved to ({x}, {y}) at: {DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}");
                }
                
                Thread.Sleep(16); // ~60 FPS for smoother animation
            }
        }
    }
}