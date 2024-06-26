﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Matix.Controls.Progressbar
{
    public partial class ProgressBar: UserControl
    {

        public ProgressBar()
        {
            InitializeComponent();
        }

        int min = 0;// Minimum value for progress range
        int max = 100;// Maximum value for progress range
        int val = 0;// Current progress

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            SolidBrush brush = new SolidBrush(ForeColor);
            float percent = (float)(val - min) / (float)(max - min);
            Rectangle rect = this.ClientRectangle;

            // Calculate area for drawing the progress.
            //rect.Width = (int)((float)rect.Width * percent);
            rect =  SplitRectangle(rect, this.FillDirection, percent*100);
            // Draw the progress meter.
            g.FillRectangle(brush, rect);

            // Draw a three-dimensional border around the control.
            //Draw3DBorder(g);

            // Clean up.
            brush.Dispose();
            g.Dispose();
        }

        public int Minimum
        {
            get
            {
                return min;
            }

            set
            {
                // Prevent a negative value.
                if (value < 0)
                {
                    min = 0;
                }

                // Make sure that the minimum value is never set higher than the maximum value.
                if (value > max)
                {
                    min = value;
                    min = value;
                }

                // Ensure value is still in range
                if (val < min)
                {
                    val = min;
                }

                // Invalidate the control to get a repaint.
                this.Invalidate();
            }
        }

        public int Maximum
        {
            get
            {
                return max;
            }

            set
            {
                // Make sure that the maximum value is never set lower than the minimum value.
                if (value < min)
                {
                    min = value;
                }

                max = value;

                // Make sure that value is still in range.
                if (val > max)
                {
                    val = max;
                }

                // Invalidate the control to get a repaint.
                this.Invalidate();
            }
        }

        public int Value
        {
            get
            {
                return val;
            }

            set
            {
                int oldValue = val;

                // Make sure that the value does not stray outside the valid range.
                if (value < min)
                {
                    val = min;
                }
                else if (value > max)
                {
                    val = max;
                }
                else
                {
                    val = value;
                }

                // Invalidate only the changed area.
                float percent;

                Rectangle newValueRect = this.ClientRectangle;
                Rectangle oldValueRect = this.ClientRectangle;

                // Use a new value to calculate the rectangle for progress.
                percent = (float)(val - min) / (float)(max - min);
                newValueRect.Width = (int)((float)newValueRect.Width * percent);

                // Use an old value to calculate the rectangle for progress.
                percent = (float)(oldValue - min) / (float)(max - min);
                oldValueRect.Width = (int)((float)oldValueRect.Width * percent);

                Rectangle updateRect = new Rectangle();

                // Find only the part of the screen that must be updated.
                if (newValueRect.Width > oldValueRect.Width)
                {
                    updateRect.X = oldValueRect.Size.Width;
                    updateRect.Width = newValueRect.Width - oldValueRect.Width;
                }
                else
                {
                    updateRect.X = newValueRect.Size.Width;
                    updateRect.Width = oldValueRect.Width - newValueRect.Width;
                }

                updateRect.Height = this.Height;

                // Invalidate the intersection region only.
                this.Invalidate(updateRect);
            }
        }

        /*
        private void Draw3DBorder(Graphics g)
        {
            int PenWidth = (int)Pens.White.Width;

            g.DrawLine(Pens.DarkGray,
            new Point(this.ClientRectangle.Left, this.ClientRectangle.Top),
            new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Top));
            g.DrawLine(Pens.DarkGray,
            new Point(this.ClientRectangle.Left, this.ClientRectangle.Top),
            new Point(this.ClientRectangle.Left, this.ClientRectangle.Height - PenWidth));
            g.DrawLine(Pens.White,
            new Point(this.ClientRectangle.Left, this.ClientRectangle.Height - PenWidth),
            new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Height - PenWidth));
            g.DrawLine(Pens.White,
            new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Top),
            new Point(this.ClientRectangle.Width - PenWidth, this.ClientRectangle.Height - PenWidth));
        }*/

        /// <summary>
        /// Filling direction
        /// </summary>
        public DockStyle FillDirection { get; set; } = DockStyle.Left;
        /// <summary>
        /// Split the rectangle with percertage and a direction 
        /// </summary>
        /// <param name="rectangle"></param>
        /// <param name="dockStyle"></param>
        /// <param name="percertage"></param>
        /// <returns></returns>
        private Rectangle SplitRectangle(Rectangle rectangle, DockStyle dockStyle, float percertage)
        {
            //check

            switch (dockStyle)
            {
                case DockStyle.None:

                    rectangle.X = 0;
                    rectangle.Y = 0;
                    rectangle.Width = 0;
                    rectangle.Height = 0;

                    break;

                case DockStyle.Top:

                    rectangle.Height =(int)( rectangle.Height * percertage / 100);

                    break;

                case DockStyle.Bottom:

                    rectangle.Y =(int)( rectangle.Height * (100 - percertage) / 100);

                    rectangle.Height = (int)(rectangle.Height * percertage / 100);
                    break;

                case DockStyle.Left:

                    rectangle.Width = (int)((float)rectangle.Width * percertage / 100);
                    break;

                case DockStyle.Right:

                    rectangle.X = (int)(rectangle.Width * (100 - percertage) / 100);
                    rectangle.Width = (int)(rectangle.Width * percertage / 100);

                    break;

                case DockStyle.Fill:

                    break;

                default:
                    break;
            }

            return rectangle;
        }
    }
}
