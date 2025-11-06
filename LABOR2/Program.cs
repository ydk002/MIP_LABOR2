using System;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace WinFormsApp
{
    public partial class Form1 : Form
    {
        private MyThread myThread;
        private Point textPosition = new Point(50, 50);
        private string textToDraw = "Moving Text!";
        private Color textColor = Color.Black;
        private readonly object textColorLock = new object();
        private Font textFont;
        private IContainer components;

        public Form1()
        {
            // Initialize the form and its controls manually
            InitializeComponent();
            textFont = new Font("Arial", 16);
            myThread = new MyThread(UpdateTextPosition, this.ClientSize);
            this.DoubleBuffered = true;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Text = "Moving Text Demo";
            // Add buttons for Start, Stop, BackColor, and TextColor
            var btnStart = new Button { Name = "btnStart", Text = "Start", Location = new Point(10, 10), Size = new Size(75, 23) };
            var btnStop = new Button { Name = "btnStop", Text = "Stop", Location = new Point(90, 10), Size = new Size(75, 23) };
            var btnBackColor = new Button { Name = "btnBackColor", Text = "Back Color", Location = new Point(170, 10), Size = new Size(90, 23) };
            var btnTextColor = new Button { Name = "btnTextColor", Text = "Text Color", Location = new Point(265, 10), Size = new Size(90, 23) };
            btnStart.Click += btnStart_Click;
            btnStop.Click += btnStop_Click;
            btnBackColor.Click += btnBackColor_Click;
            btnTextColor.Click += btnTextColor_Click;
            this.Controls.Add(btnStart);
            this.Controls.Add(btnStop);
            this.Controls.Add(btnBackColor);
            this.Controls.Add(btnTextColor);
            this.ResumeLayout(false);
        }

        public void UpdateTextPosition(int x, int y)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<int, int>(InvalidateDrawing), x, y);
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

        private void btnStart_Click(object sender, EventArgs e)
        {
            myThread?.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            myThread?.Stop();
        }

        private void btnBackColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    this.BackColor = colorDialog.Color;
                }
            }
        }

        private void btnTextColor_Click(object sender, EventArgs e)
        {
            using (ColorDialog colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    lock (textColorLock)
                    {
                        textColor = colorDialog.Color;
                    }
                    this.Invalidate();
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
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
                components?.Dispose();
                textFont?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}