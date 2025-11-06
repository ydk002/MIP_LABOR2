using System;
using System.Windows.Forms;
using System.Drawing;

namespace LABOR2
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnBackColor;
        private System.Windows.Forms.Button btnTextColor;

        private void InitializeComponent()
        {
            this.btnStart = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnBackColor = new System.Windows.Forms.Button();
            this.btnTextColor = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnStart
            // 
            this.btnStart.Location = new System.Drawing.Point(12, 12);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(75, 23);
            this.btnStart.TabIndex = 0;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(93, 12);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 1;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnBackColor
            // 
            this.btnBackColor.Location = new System.Drawing.Point(174, 12);
            this.btnBackColor.Name = "btnBackColor";
            this.btnBackColor.Size = new System.Drawing.Size(120, 23);
            this.btnBackColor.TabIndex = 2;
            this.btnBackColor.Text = "Background Color";
            this.btnBackColor.UseVisualStyleBackColor = true;
            this.btnBackColor.Click += new System.EventHandler(this.btnBackColor_Click);
            // 
            // btnTextColor
            // 
            this.btnTextColor.Location = new System.Drawing.Point(300, 12);
            this.btnTextColor.Name = "btnTextColor";
            this.btnTextColor.Size = new System.Drawing.Size(100, 23);
            this.btnTextColor.TabIndex = 3;
            this.btnTextColor.Text = "Text Color";
            this.btnTextColor.UseVisualStyleBackColor = true;
            this.btnTextColor.Click += new System.EventHandler(this.btnTextColor_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(420, 300);
            this.Controls.Add(this.btnTextColor);
            this.Controls.Add(this.btnBackColor);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnStart);
            this.Name = "Form1";
            this.Text = "MIP Laborator 2";
            this.ResumeLayout(false);
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
    }
}
