using System;
using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
    public partial class Form1 : Form
    {
        public int scale = 50, Length = 1, width = 20, length = 20, speed = 75, mode = 1;
        public string Direction = null;
        public PictureBox[] Snake = new PictureBox[100];
        public PictureBox Apple = new PictureBox();
        Random rnd = new Random();

        public Form1()
        {
            InitializeComponent();
            KeyDown += ControlSnake;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadGame();
            SpawnApple();
            Width = length * scale + 36;
            Height = width * scale + 59;
            timer1.Interval = speed;
        }

        private void LoadGame()
        {
            Snake[0] = new PictureBox();
            Controls.Add(Snake[0]);
            int x, y;
            do
            {
                x = 10 + scale * rnd.Next(length / 4, 3 * length / 4);
                y = 10 + scale * rnd.Next(width / 4, 3 * width / 4);
            }
            while (Snake[0].Location == new Point(x, y));
            Snake[0].Location = new Point(x, y); Snake[0].Size = new Size(scale, scale);
            Snake[0].BackColor = Color.Red;
            Controls.Add(Apple);
            Apple.BackColor = Color.Green;
            Apple.Size = new Size(scale, scale);
        }

        private void SpawnApple()
        {
            int x, y;
            do
            {
                x = 10 + scale * rnd.Next(1, length);
                y = 10 + scale * rnd.Next(1, width);
            }
            while (IsInsideSnake(x, y));
            Apple.Location = new Point(x, y);
        }

        private void ControlSnake(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                case Keys.W:
                    if (Direction != "Down")
                        Direction = "Up";
                    break;
                case Keys.Right:
                case Keys.D:
                    if (Direction != "Left")
                        Direction = "Right";
                    break;
                case Keys.Down:
                case Keys.S:
                    if (Direction != "Up")
                        Direction = "Down";
                    break;
                case Keys.Left:
                case Keys.A:
                    if (Direction != "Right")
                        Direction = "Left";
                    break;
            }
        }

        private bool IsInsideSnake(int x, int y)
        {
            for (int i = 0; i < Length; i++)
                if (Snake[i].Location == new Point(x, y))
                    return true;
            return false;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            for (int i = Length - 1; i > 0; i--)
                Snake[i].Location = Snake[i - 1].Location;
            switch (Direction)
            {
                case "Up":
                    Snake[0].Top -= scale;
                    break;
                case "Down":
                    Snake[0].Top += scale;
                    break;
                case "Right":
                    Snake[0].Left += scale;
                    break;
                case "Left":
                    Snake[0].Left -= scale;
                    break;
            }
            if (Snake[0].Top < 10 || Snake[0].Top >= 10 + scale * width || Snake[0].Left >= 10 + scale * length || Snake[0].Left < 10)
                if (mode == 1)
                    EndGame();
                else
                    switch (Direction)
                    {
                        case "Up":
                            Snake[0].Top = 10 + scale * (width-1);
                            break;
                        case "Down":
                            Snake[0].Top = 10;
                            break;
                        case "Right":
                            Snake[0].Left = 10;
                            break;
                        case "Left":
                            Snake[0].Left = 10 + scale * (length-1);
                            break;
                    }
            if (Snake[0].Location == Apple.Location)
            {
                SpawnApple();
                Snake[Length] = new PictureBox();
                Controls.Add(Snake[Length]);
                switch (Direction)
                {
                    case "Up":
                        Snake[Length].Location = new Point(Snake[Length - 1].Left, Snake[Length - 1].Top + scale);
                        break;
                    case "Down":
                        Snake[Length].Location = new Point(Snake[Length - 1].Left, Snake[Length - 1].Top - scale);
                        break;
                    case "Right":
                        Snake[Length].Location = new Point(Snake[Length - 1].Left - scale, Snake[Length - 1].Top);
                        break;
                    case "Left":
                        Snake[Length].Location = new Point(Snake[Length - 1].Left + scale, Snake[Length - 1].Top);
                        break;
                }
                Snake[Length].Size = new Size(scale, scale);
                Snake[Length].BackColor = Color.Red;
                Length++;
            }
            for (int i = 1; i < Length; i++)
                if (Snake[0].Location == Snake[i].Location)
                    EndGame();
        }

        public void EndGame()
        {
            Direction = null;
            for (int i = 1; i < Length; i++)
                Controls.Remove(Snake[i]);
            Length = 1;
            int x, y;
            do
            {
                x = 10 + scale * rnd.Next(length / 4, 3 * length / 4);
                y = 10 + scale * rnd.Next(width / 4, 3 * width / 4);
            }
            while (Snake[0].Location == new Point(x, y));
            Snake[0].Location = new Point(x, y);
            MessageBox.Show("Game Over");
        }
    }
}