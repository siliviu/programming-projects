using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Game : Form
    {
        public static int length, width, length_, width_, bombs, flags;
        public int[] squares;
        public static bool newgame = true, IsGameRunning = false, safefirst = true, playsounds = true;
        public PictureBox[,] Square = new PictureBox[100, 100];
        public PictureBox[,] Square2 = new PictureBox[100, 100];
        public Stopwatch stopwatch = new Stopwatch();
        public static string time, mode;

        private void button1_Click(object sender, EventArgs e)
        {
            Menu menu = Application.OpenForms["Menu"] as Menu;
            if (menu == null)
            {
                menu = new Menu(this);
                menu.Show();
            }
            else
                menu.BringToFront();
        }

        public void SquareClick(object sender, EventArgs e, int i, int j)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Right)
            {
                if (Square2[i, j].Tag != "Flagged")
                {
                    Square2[i, j].Image = Properties.Resources.flag;
                    Square2[i, j].Tag = "Flagged";
                    flags++;
                    label5.Text = (flags.ToString() + " / " + bombs.ToString());
                }
                else
                {
                    Square2[i, j].Image = null;
                    Square2[i, j].Tag = null;
                    flags--;
                    label5.Text = (flags.ToString() + " / " + bombs.ToString());
                }

            }
            else if (Square2[i, j].Tag != "Flagged")
            {
                if (newgame)
                {
                    if (safefirst)
                    {
                        while (Square[i, j].Tag != "Zero")
                        {
                            for (int k = 1; k <= width; k++)
                                for (int l = 1; l <= length; l++)
                                {
                                    Square[k, l].Tag = null;
                                    Square[k, l].Image = null;
                                    Square[k, l].BackColor = Color.White;
                                }
                            RenderImages();
                        }
                    }
                    button1.Enabled = false;
                    stopwatch.Start();
                    timer1.Enabled = true;
                }
                    Reveal(i, j);
                if (Square[i, j].Tag == "Zero")
                    Clear(i, j, 0);
                if (Square[i, j].Tag == "Bomb")
                {
                    Square[i, j].BackColor = Color.Red;
                    if (playsounds)
                    {
                        System.IO.Stream bomb = Properties.Resources.bomb_sound;
                        System.Media.SoundPlayer bomb_ = new System.Media.SoundPlayer(bomb);
                        bomb_.Play();
                    }
                    EndGame();
                    MessageBox.Show("Game over! You have lost, better luck next time!");
                }
                else if (GameHasFinished())
                {
                    EndGame();
                    if (playsounds)
                    {
                        System.IO.Stream fireworks = Properties.Resources.fireworks;
                        System.Media.SoundPlayer fireworks_ = new System.Media.SoundPlayer(fireworks);
                        fireworks_.Play();
                    }
                    MessageBox.Show("Game over! You have won, congratulations!");
                }
                else if(newgame)
                {
                    newgame = false;
                    if (playsounds)
                    {
                        System.IO.Stream pop = Properties.Resources.pop;
                        System.Media.SoundPlayer pop_ = new System.Media.SoundPlayer(pop);
                        pop_.Play();
                    }
                }
            }
        }

        public Game()
        {
            InitializeComponent();
            Resize += Form1_Resize;
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            button1.Location = new Point(843 + Convert.ToInt32(Size.Width) - 946, 15);
            label5.Location= new Point(773 + Convert.ToInt32(Size.Width) - 946, 18);
            pictureBox1.Location = new Point(744 + Convert.ToInt32(Size.Width) - 946, 15);
            label4.Location=new Point(614 + Convert.ToInt32(Size.Width) - 946, 18);
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void RenderSquares()
        {
            for (int k = 1; k <= width; k++)
                for (int l = 1; l <= length; l++)
                {
                    Square[k, l] = new PictureBox();
                    Square2[k, l] = new PictureBox();
                    Controls.Add(Square2[k, l]);
                }
            int x, y, i = 1, j = 1, offset = 0;
            for (y = 50; y < 50 + width * 32; y += 32)
            {
                for (x = 10 + offset; x < 10 + length * 32+offset; x += 32)
                {
                    Square[i, j].Location = new Point(x, y);
                    Square[i, j].Size = new Size(32, 32);
                    Square[i, j].BorderStyle = BorderStyle.FixedSingle;
                    int i_ = i, j_ = j;
                    Square2[i, j].Click += (object sender, EventArgs e) => SquareClick(sender, e, i_, j_);
                    Square2[i, j].Location = new Point(x, y);
                    Square2[i, j].Size = new Size(32, 32);
                    Square2[i, j].BorderStyle = BorderStyle.FixedSingle;
                    Square2[i, j].BackColor = Color.White;
                    Square2[i, j].BringToFront();
                    j++;
                }
                if (mode == "Hexagon")
                    offset += 16;
                j = 1;
                i++;
            }

        }

        public void RenderImages()
        {
            int[] values = new int[1048576];
            Random rnd = new Random();
            for (int i = 1; i <= bombs; i++)
            {
                values[i] = rnd.Next(1, length * width);
                bool IsDuplicate = false;
                for (int j = 1; j < i; j++)
                    if (values[i] == values[j])
                    {
                        IsDuplicate = true;
                        break;
                    }
                if (IsDuplicate == false)
                {
                    Square[values[i] / length + 1, values[i] % length == 0 ? length : values[i] % length].Image = Properties.Resources.bomb;
                    Square[values[i] / length + 1, values[i] % length == 0 ? length : values[i] % length].Tag = "Bomb";
                } 
                else
                    i--;
            }
            for (int i = 1; i <= width; i++)
                for (int j = 1; j <= length; j++)
                {
                    if (Square[i, j].Tag == "Bomb")
                        continue;
                    else
                    {
                        int nr = 0;
                        for (int k = 0; k < squares.Length; k += 2)
                            if (i + squares[k] > 0 && j + squares[k + 1] > 0 && i + squares[k] <= width && j + squares[k + 1] <= length && Square[i + squares[k], j + squares[k + 1]].Tag == "Bomb")
                            {
                                nr++;
                                if (mode == "Pawn" && squares[k] == -1)
                                    nr++;
                            }
                        switch (nr)
                        {
                            case 0:
                                Square[i, j].Tag = "Zero";
                                Square[i, j].BackColor = Color.LightGray;
                                break;
                            case 1:
                                Square[i, j].Image = Properties.Resources.one;
                                break;
                            case 2:
                                Square[i, j].Image = Properties.Resources.two;
                                break;
                            case 3:
                                Square[i, j].Image = Properties.Resources.three;
                                break;
                            case 4:
                                Square[i, j].Image = Properties.Resources.four;
                                break;
                            case 5:
                                Square[i, j].Image = Properties.Resources.five;
                                break;
                            case 6:
                                Square[i, j].Image = Properties.Resources.six;
                                break;
                            case 7:
                                Square[i, j].Image = Properties.Resources.seven;
                                break;
                            case 8:
                                Square[i, j].Image = Properties.Resources.eight;
                                break;
                        }
                    }
                }
        }

        public void StartGame()
        {
            SetSquares(mode);
            stopwatch.Reset();
            newgame = true;
            pictureBox1.Visible = true;
            label5.Visible = true;
            IsGameRunning = true;
            flags = 0;
            label5.Text = "0 / " + bombs.ToString();
            for (int i = 1; i <= width_; i++)
                for (int j = 1; j <= length_; j++)
                {
                    Controls.Remove(Square[i, j]);
                    Square[i, j] = null;
                }
            RenderSquares();
            RenderImages();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time = String.Format("{0}:{1:D2}", stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
            label4.Text = time;
        }

        void Clear(int i, int j, int generation)
        {
            if (generation == 0 && playsounds)
            {
                System.IO.Stream woosh = Properties.Resources.woosh;
                System.Media.SoundPlayer woosh_ = new System.Media.SoundPlayer(woosh);
                woosh_.Play();
            }
            generation++;
            Reveal(i, j);
            for (int k = 0; k < squares.Length; k += 2)
                if (i + squares[k] > 0 && j + squares[k + 1] > 0 && i + squares[k] <= width && j + squares[k + 1] <= length && Square2[i + squares[k], j + squares[k + 1]].Tag != "Removed")
                    {
                        if (Square[i + squares[k], j + squares[k + 1]].Tag != "Zero")
                            Reveal(i + squares[k], j + squares[k + 1]);
                        else
                            Clear(i + squares[k], j + squares[k + 1], generation);
                    }
        }

        void Reveal(int i, int j)
        {
            Controls.Remove(Square2[i,j]);
            Controls.Add(Square[i, j]);
            Square2[i, j].Tag = "Removed";
        }

        void EndGame()
        {
            length_ = length;
            width_ = width;
            stopwatch.Stop();
            timer1.Enabled = false;
            label1.Visible = false;
            label2.Visible = false;
            label3.Visible = false;
            label6.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            for (int i = 1; i <= width; i++)
                for (int j = 1; j <= length; j++) 
                {
                    Controls.Add(Square[i, j]);
                    if (Square2[i, j].Tag == "Flagged" && Square[i, j].Tag != "Bomb")
                        Square[i, j].BackColor = Color.FromArgb(60,Color.Red);
                    if (Square2[i, j].Tag == "Removed" && Square[i, j].Tag != "Bomb")
                        Square[i, j].BackColor = Color.FromArgb(50, Color.LightGreen);
                    Controls.Remove(Square2[i, j]);
                    Square2[i, j] = null;
                }
            button1.Enabled = true;
            IsGameRunning = false;
        }

        public void SetSquares(string Mode)
        {
            switch (Mode)
            {
                case "Normal":
                    squares = new int[] { -1, -1, -1, 0, -1, 1, 0, -1, 0, 1, 1, -1, 1, 0, 1, 1 };
                    break;
                case "Orth":
                    squares = new int[] { -1, 0, 0, -1, 0, 1, 1, 0 };
                    break;
                case "No Vert":
                    squares = new int[] { -1, -1, -1, 1, 0, -1, 0, 1, 1, -1, 1, 1 };
                    break;
                case "Far Orth":
                    squares = new int[] { 2, 0, 1, 0, 0, -2, 0, -1, 0, 1, 0, 2, -1, 0, -2, 0 };
                    break;
                case "No Up":
                    squares = new int[] { -1, -1, -1, 1, 0, -1, 0, 1, 1, -1, 1, 0, 1, 1 };
                    break;
                case "Horiz":
                    squares = new int[] { 0, -2, 0, -1, 0, 1, 0, 2 };
                    break;
                case "Pawn":
                    squares = new int[] {-2, 0, -1, -1, -1, 0, -1, 1 };
                    break;
                case "Knight":
                    squares = new int[] { -2, -1, -2, 1, -1, -2, -1, 2, 1, -2, 1, 2, 2, -1, 2, 1 };
                    break;
                case "Hexagon":
                    squares = new int[] { -1, 0, -1, 1, 0, -1, 0, 1, 1, -1, 1, 0 };
                    break;
            }
        }

        bool GameHasFinished()
        {
            for (int i = 1; i <= width; i++)
                for (int j = 1; j <= length; j++)
                    if (Square[i, j].Tag != "Bomb" && Square2[i, j].Tag != "Removed")
                        return false;
            return true;
        }
    }
}
