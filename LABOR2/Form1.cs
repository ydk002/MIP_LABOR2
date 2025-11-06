using System;
using System.Drawing;
using System.Windows.Forms;

namespace LABOR2
{
    public partial class Form1 : Form
    {
        private MyThread myThread;
        private Point textPosition = new Point(50, 50);
        private readonly string textToDraw = "Moving Text!";
        private Color textColor = Color.Black;
        private readonly object textColorLock = new object();
        private Font textFont;

        public Form1()
        {
            InitializeComponent();
            textFont = new Font("Arial", 16, FontStyle.Bold);
            this.DoubleBuffered = true;
            this.BackColor = Color.White;
            
            // Initialize thread with actual form size
            myThread = new MyThread(UpdateTextPosition, this.ClientSize);
            
            // Handle form resize
            this.Resize += Form1_Resize;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            // Restart thread with new dimensions if it's running
            if (myThread != null)
            {
                bool wasRunning = false;
                // Check if we need to restart
                myThread.Stop();
                myThread = new MyThread(UpdateTextPosition, this.ClientSize);
            }
        }

        public void UpdateTextPosition(int x, int y)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new Action<int, int>(InvalidateDrawing), x, y);
            }
            else
            {
                InvalidateDrawing(x, y);
            }
        }

        private void InvalidateDrawing(int x, int y)
        {
            textPosition = new Point(x, y);
            this.Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            
            Color currentColor;
            lock (textColorLock)
            {
                currentColor = textColor;
            }
            
            using (Brush textBrush = new SolidBrush(currentColor))
            {
                e.Graphics.DrawString(textToDraw, textFont, textBrush, textPosition);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                myThread?.Stop();
                textFont?.Dispose();
                components?.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
