using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tower
{
    public partial class Form1 : Form
    {
        int width, height, controlwidth = 1920, controlheight = 1080, currentblock, speed, initialspeed = 75, shift, tolerance = 5;
        bool gameisrunning;
        string Direction;
        PictureBox[] Block = new PictureBox[10000];

        Random rnd = new Random();

        private void MovePerspective(object sender, EventArgs e)
        {
            if (shift == 0) return;
            for (int i = currentblock-1; i >=0; i--)
            {
                if (Block[i].Top > Height) continue;
                Block[i].Top +=2;
            }
            shift -= 2;
        }

        private void MoveBlock(object sender, EventArgs e)
        {
            label1.Text = (currentblock - 2).ToString();
            switch(Direction)
            {
                case "Left":
                    Block[currentblock-1].Left += speed / 6;
                    if (Block[currentblock - 1].Left >= Width - width)
                        Direction = "Right";
                    break;
                case "Right":
                    Block[currentblock-1].Left -= speed / 6;
                    if (Block[currentblock - 1].Left <= 0)
                        Direction = "Left";
                        break;
            }
        }                                                                                                                                   
        public Form1()
        {
            InitializeComponent();
            InitialiseGame();
            StartGame();
        }

        private void InitialiseGame()
        {
            Width = controlwidth;
            Height = controlheight;
            label1.Left = (Width - label1.Width) / 2;
            label1.Top = 50;
            MouseClick += Drop;
        }

        private void StartGame()
        {
            shift = 0;
            currentblock = 0;
            timer1.Enabled = false;
            width = Width * 30 / 100;
            height = Height * 10 / 100;
            SpawnBlock((Width - width) / 2, Height);
            SpawnNextBlock();
            speed = initialspeed;
            timer1.Interval = 1;
            timer1.Enabled = true;   
            timer2.Enabled = true;
            gameisrunning = true;

        }

        private void Drop(object sender, MouseEventArgs e)
        {
            if (!gameisrunning) return;
            int temp = speed;
            if (currentblock > 4) shift += height;
            speed = 0;
            int offset = Math.Abs(Block[currentblock - 1].Left - Block[currentblock - 2].Left);
            if (offset >= width)
            {
                EndGame();
                return;
            }
            if (offset > width * tolerance / 100)
            {
                width -= offset;
                Block[currentblock - 1].Width = width;
                if (Block[currentblock - 1].Left < Block[currentblock - 2].Left)
                    Block[currentblock - 1].Left = Block[currentblock - 2].Left;
            }
            SpawnNextBlock();
            speed = temp;
        }

        private void SpawnBlock(int x, int y)
        {
            Block[currentblock] = new PictureBox();
            Block[currentblock].MouseClick += Drop;
            Controls.Add(Block[currentblock]);
            Block[currentblock].Location = new Point(x, y - 40 - height);
            Block[currentblock].Width = width;
            Block[currentblock].Height = height;
            Block[currentblock].BackColor = Color.Blue;
            currentblock++;
        }
        private void SpawnNextBlock()
        {
            int temp, x, y;
            temp = rnd.Next(0, 2);
            if (temp == 0)
            {
                Direction = "Left";
                x = 0;
            }
            else
            {
                Direction = "Right";
                x = Width-width;
            }
            if (currentblock <= 4)
                y = Height - currentblock * height;
            else
                y = Height - 4 * height - shift;
            SpawnBlock(x, y);
        }

        private void DestroyBlock (Control control)
        {
            Controls.Remove(control);
            control.Location = new Point(-1, -1);
            control.Dispose();
            control = null;
        }

        private void EndGame()
        {
            gameisrunning = false;
            timer1.Enabled = false;
            timer2.Enabled = false;
            for (int i = 0; i < currentblock; i++)
                DestroyBlock(Block[i]);
            MessageBox.Show("Game over!");
            StartGame();
        }
    }
}
