using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LAB_9
{

    public partial class Form1 : Form
    {
        int delay = 5;
        List<Thread> threads;
        List<Button> btns;
        public Form1()
        {
            InitializeComponent();
            threads = new List<Thread>();
            btns = new List<Button>();
            label1.Text = "Текущее кол-во потоков " + Process.GetCurrentProcess().Threads.Count.ToString();
            label2.Text = "Начальное кол-во потоков " + Process.GetCurrentProcess().Threads.Count.ToString();
            label3.Text = "Кол-во созданных кнопок " + 0;

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void button1_MouseMove(object sender, MouseEventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            threads.Add(addButton());
            threads.Last().Start();
            label1.Text = "Текущее кол-во потоков " + Process.GetCurrentProcess().Threads.Count.ToString();
            label3.Text = "Кол-во созданных кнопок " + btns.Count;
        }

        private int counter = 2;
        public void moveBtn(object sender, MouseEventArgs e)
        {
            Button btn = sender as Button;
            Point pos = btn.PointToClient(Cursor.Position);
            Point center = new Point(((pos.X - (btn.Size.Width / 2)) + Math.Sign(pos.X - (btn.Size.Width / 2))), ((pos.Y - (btn.Size.Height / 2)) + Math.Sign(pos.Y - (btn.Size.Height / 2))));
            const int offset = 5;
            if (pos.X < btn.Width + offset && pos.X > 0 - offset && pos.Y > 0 - offset && pos.Y < btn.Height + offset)
            {


                if ((float)Math.Abs(center.X) / (btn.Size.Width / 2) > (float)Math.Abs(center.Y) / (btn.Size.Height / 2))
                {
                    btn.Location = new Point(btn.Location.X + center.X + Math.Sign(-center.X) * (btn.Size.Width / 2), btn.Location.Y);
                }
                else
                {
                    if ((float)Math.Abs(center.X) / (btn.Size.Width / 2) == (float)Math.Abs(center.Y) / (btn.Size.Height / 2))
                    {
                        btn.Location = new Point(btn.Location.X + center.X + Math.Sign(-center.X) * (btn.Size.Width / 2), btn.Location.Y + center.Y + Math.Sign(-center.Y) * (btn.Size.Height / 2));
                    }
                    else
                    {
                        btn.Location = new Point(btn.Location.X, btn.Location.Y + center.Y + Math.Sign(-center.Y) * (btn.Size.Height / 2));
                    }
                }

                Form ActiveForm = (sender as Button).Parent as Form;
                try
                {
                    if (btn.Location.X > ActiveForm.Size.Width - 2 * btn.Size.Width)
                    {
                        btn.Location = new Point(ActiveForm.Size.Width - 2 * btn.Size.Width, btn.Location.Y);
                    }
                    if (btn.Location.X < 0 + btn.Size.Width)
                    {
                        btn.Location = new Point(0 + btn.Size.Width, btn.Location.Y);
                    }
                    if (btn.Location.Y > ActiveForm.Size.Height - 2 * btn.Size.Height)
                    {
                        btn.Location = new Point(btn.Location.X, ActiveForm.Size.Height - 2 * btn.Size.Height);
                    }
                    if (btn.Location.Y < 0 + btn.Size.Height)
                    {
                        btn.Location = new Point(btn.Location.X, 0 + btn.Size.Height);
                    }
                }
                catch
                {
                    //idk
                }
                label1.Text = Process.GetCurrentProcess().Threads.Count.ToString();
            }

        }
        public Thread addButton()
        {
            Thread thr = new Thread(new ThreadStart(createButton));
            return thr;
        }

        public delegate Point PointDelegate(Button btn);
        public Point GetPoint(Button btn)
        {
            return btn.PointToClient(Cursor.Position);
        }

        public delegate void SetPointDelegate(Button btn, Point pos);
        public void SetPoint(Button btn, Point pos)
        {
            btn.Location = pos;
        }

        public void createButton()
        {
            Button button1 = new System.Windows.Forms.Button();
            var nDeleg = new PointDelegate(GetPoint);
            var nDelegSet = new SetPointDelegate(SetPoint);
            Random rnd = new Random();
            button1.Name = "button_" + (++counter);
            int size = rnd.Next(15, 100);
            button1.Size = new System.Drawing.Size(size, size);
            Point btnpos = new Point(rnd.Next(0 + 2 * button1.Size.Width, ActiveForm.Size.Width - 2 * button1.Size.Width), rnd.Next(0 + 2 * button1.Size.Height, ActiveForm.Size.Height - 2 * button1.Size.Height));
            button1.Location = btnpos;
            button1.TabIndex = 0;
            button1.BackColor = Color.FromArgb(rnd.Next(256), rnd.Next(256), rnd.Next(256));
            button1.UseVisualStyleBackColor = false; //!!!
            button1.Text = "";
            this.Invoke((MethodInvoker)(() => Controls.Add(button1)));
            //button1.MouseMove += new System.Windows.Forms.MouseEventHandler(moveBtn);
            //button1.Click += new System.EventHandler(createButton);
            //while (true) { Thread.Sleep(1000); }
            btns.Add(button1);
            while (true)
            {
                try
                {
                    Button btn = button1;
                    //Point pos = btn.PointToClient(Cursor.Position);
                    Point pos = (Point)Invoke(nDeleg, btn);
                    Point center = new Point(((pos.X - (btn.Size.Width / 2)) + Math.Sign(pos.X - (btn.Size.Width / 2))), ((pos.Y - (btn.Size.Height / 2)) + Math.Sign(pos.Y - (btn.Size.Height / 2))));
                    const int offset = 0;
                    if (pos.X < btn.Width + offset && pos.X > 0 - offset && pos.Y > 0 - offset && pos.Y < btn.Height + offset)
                    {


                        if ((float)Math.Abs(center.X) / (btn.Size.Width / 2) > (float)Math.Abs(center.Y) / (btn.Size.Height / 2))
                        {
                            Invoke(nDelegSet, btn, new Point(btn.Location.X + center.X + Math.Sign(-center.X) * (btn.Size.Width / 2), btn.Location.Y));
                        }
                        else
                        {
                            if ((float)Math.Abs(center.X) / (btn.Size.Width / 2) == (float)Math.Abs(center.Y) / (btn.Size.Height / 2))
                            {
                                Invoke(nDelegSet, btn, new Point(btn.Location.X + center.X + Math.Sign(-center.X) * (btn.Size.Width / 2), btn.Location.Y + center.Y + Math.Sign(-center.Y) * (btn.Size.Height / 2)));
                            }
                            else
                            {
                                Invoke(nDelegSet, btn, new Point(btn.Location.X, btn.Location.Y + center.Y + Math.Sign(-center.Y) * (btn.Size.Height / 2)));
                            }
                        }

                        Form ActiveForm = button1.Parent as Form;
                        try
                        {
                            if (btn.Location.X > ActiveForm.Size.Width - 2 * btn.Size.Width)
                            {
                                Invoke(nDelegSet, btn, new Point(ActiveForm.Size.Width - 2 * btn.Size.Width, btn.Location.Y));
                            }
                            if (btn.Location.X < 0 + btn.Size.Width)
                            {
                                Invoke(nDelegSet, btn, new Point(0 + btn.Size.Width, btn.Location.Y));
                            }
                            if (btn.Location.Y > ActiveForm.Size.Height - 2 * btn.Size.Height)
                            {
                                Invoke(nDelegSet, btn, new Point(btn.Location.X, ActiveForm.Size.Height - 2 * btn.Size.Height));
                            }
                            if (btn.Location.Y < 0 + btn.Size.Height)
                            {
                                Invoke(nDelegSet, btn, new Point(btn.Location.X, 0 + btn.Size.Height));
                            }
                        }
                        catch
                        {
                            //idk
                        }
                    }
                    Thread.Sleep(delay);
                }
                catch
                {

                }
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (Thread thr in threads)
            {
                thr.Abort();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            delay = (int)(sender as NumericUpDown).Value;
        }
    }

}