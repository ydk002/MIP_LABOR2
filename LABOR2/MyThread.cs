using System;
using System.Drawing;
using System.Threading;

namespace WinFormsApp
{
    public class MyThread
    {
        private readonly Action<int, int> updatePositionCallback;
        private readonly Size clientSize;
        private Thread thread;
        private bool running;
        private int x;
        private int y;
        private int dx;
        private int dy;

        public MyThread(Action<int, int> updatePositionCallback, Size clientSize)
        {
            this.updatePositionCallback = updatePositionCallback;
            this.clientSize = clientSize;
            this.x = 50;
            this.y = 50;
            this.dx = 2;
            this.dy = 2;
        }

        public void Start()
        {
            if (running)
                return;
            running = true;
            thread = new Thread(Run);
            thread.IsBackground = true;
            thread.Start();
        }

        public void Stop()
        {
            running = false;
            if (thread != null && thread.IsAlive)
            {
                thread.Join();
            }
        }

        private void Run()
        {
            while (running)
            {
                x += dx;
                y += dy;

                if (x < 0 || x > clientSize.Width - 100)
                    dx = -dx;
                if (y < 0 || y > clientSize.Height - 50)
                    dy = -dy;

                updatePositionCallback(x, y);
                Thread.Sleep(20);
            }
        }
    }
}