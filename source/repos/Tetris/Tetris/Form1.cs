using System;
using System.Drawing;
using NAudio.Wave;
using System.IO;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        int mode = 1, level = 0, rows = 20, columns = 10, scale = 45, thickness = 1, xgrid = 500, ygrid = 50, currentpiecenumber = 0, initialspeed = 500, speed, acceleration = 10, time = 0, music, volume = 100, lines, nextset, score, pos0;
        bool gamerunning = false, sound= true;
        double combo=1;
        WaveOutEvent output = new WaveOutEvent();
        PictureBox[,] Squares = new PictureBox[30, 30];
        PictureBox[] Piece = new PictureBox[900];
        PictureBox[] PreviewPiece = new PictureBox[5];
        Random rnd = new Random();

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (GameHasEnded())
            {
                StopGame();
                StartGame();
            }
            if (IsOnTheEdge("Down") || IsTouchingABlock("Down"))
            {
                for (int i = 0; i < currentpiecenumber; i++)
                    if (Piece[i].Tag != null)
                        Piece[i].Tag = null;
                CheckForCompletedLine();
                SpawnTetronimo(nextset);

                nextset = rnd.Next(1, 8);
                PreviewNextSet(nextset);

            }
            else
                for (int j = 0; j < currentpiecenumber; j++)
                    if (Piece[j].Tag != null)
                        Piece[j].Top += scale + thickness;
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            time++;
            label11.Text = score.ToString();
            label9.Text = level.ToString();
            label3.Text = String.Format("{0:D2}:{1:D2}", time / 3600, time / 60 % 60);
            label1.Text = timer1.Interval.ToString();
            if (mode == 1)
            {
                label6.Text = decimal.Round(1000.0m / speed, 2).ToString() + " blocks/s";
                if (time != 0 && timer1.Interval > 50 && time % (speed * 2.5) == 0)
                {
                    speed -= (speed <= speed / 2 ? (speed <= speed / 4 ? 1 / 2 : 4 / 5) : 1) * acceleration;
                    timer1.Interval = speed;
                }
            }
        }
        

        public Form1()
        {
            InitializeComponent();
            RenderGrid(xgrid, ygrid);
            output.Volume = 1.0f;
            output.PlaybackStopped += Output_PlaybackStopped;
            Mechanics();
            StartGame();
        }

        private void Output_PlaybackStopped(object sender, StoppedEventArgs e)
        {
            if(gamerunning) PlayMusic();
        }

        private void StartGame()
        {
            score = 0;
            lines = 0;
            label4.Text = "0";
            time = 0;
            gamerunning = true;
            if(mode==2)
            {
                label8.Visible = true;
                label9.Visible = true;
                level = 0;
                speed = 800;
                label6.Text = "1.25 blocks/s";
            }
            else
            {
                speed = initialspeed;
                timer1.Interval = speed;
                label6.Text = decimal.Round(1000.0m / speed, 2).ToString() + " blocks/s";
                label8.Visible = false;
                label9.Visible = false;
            }
            timer1.Enabled = true;
            timer2.Enabled = true;
            PlayMusic();
            for (int i = 1; i < 5; i++)
            {
                PreviewPiece[i] = new PictureBox();
                Controls.Add(PreviewPiece[i]);
            }
            SpawnTetronimo(rnd.Next(1, 8));
            nextset = rnd.Next(1, 8);
            PreviewNextSet(nextset);

        }

        private void PlayMusic()
        {
            UnmanagedMemoryStream A = Properties.Resources.Game_Boy_Tetris_Music_A;
            WaveStream AA = new Mp3FileReader(A);
            UnmanagedMemoryStream B = Properties.Resources.Game_Boy_Tetris_Music_B;
            WaveStream BB = new Mp3FileReader(B);
            UnmanagedMemoryStream C = Properties.Resources.Game_Boy_Tetris_Music_B;
            WaveStream CC = new Mp3FileReader(C);
            music = rnd.Next(1, 4);
            switch (music)
            {
                case 1:
                    output.Init(AA);
                    output.Play();
                    break;
                case 2:
                    output.Init(BB);
                    output.Play();
                    break;
                case 3:
                    output.Init(CC);
                    output.Play();
                    break;
            }
        }

        private void StopGame()
        {
            for (int i = 0; i < currentpiecenumber; i++)
                DestroyBlock(Piece[i]);
            timer2.Enabled = false;
            gamerunning = false;
            output.Stop();
            for (int i = 1; i < 5; i++)
                Controls.Remove(PreviewPiece[i]);
            MessageBox.Show("You lost mate");
        }

        private void UpdateLevelSpeed()
        {
            if (lines <= 550)
            {
                for (int i = 0; ; i++)
                    if (5 * i * (i + 1) <= lines && lines < 5 * (i + 1) * (i + 2))
                    {
                        level = i;
                        break;
                    }
            }
            else if (lines <= 1150)
                level = 10 + (lines - 550) / 100;
            else if (lines <= 2700)
            {
                for (int j = 0; ; j++)
                {
                    if (5 * j * (j + 1) <= lines - 600 && lines - 600 < 5 * (j + 1) * (j + 2))
                    {
                        level = j + 6;
                        break;
                    }
                }
            }
            else if (lines < 3500)
                level = 26 + (lines - 2700) / 200;
            else level = 30;

            if (level <= 8)
                speed = (int)((48.0 - 5 * level) / 60 * 1000);
            else if (level == 9)
                speed = 100;
            else if (level <= 12)
                speed = 83;
            else if (level <= 15)
                speed = 66;
            else if (level <= 18)
                speed = 50;
            else if (level <= 28)
                speed = 33;
            else
                speed = 16;
            label6.Text = decimal.Round(1000.0m / speed, 2).ToString() + " blocks/s";
            timer1.Interval = speed;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Pause();
        }

        private void Pause()
        {
            if (gamerunning)
            {
                output.Pause();
                gamerunning = false;
                timer1.Enabled = false;
                timer2.Enabled = false;
                pictureBox1.BackgroundImage = Properties.Resources.play;
            }
            else
            {
                output.Play();
                gamerunning = true;
                timer1.Enabled = true;
                timer2.Enabled = true;
                pictureBox1.BackgroundImage = Properties.Resources.pause;
            }
        }

        private void RenderGrid(int x, int y)
        {
            PictureBox Background = new PictureBox();
            Controls.Add(Background);
            Background.Location = new Point(x, y);
            Background.Size = new Size((scale + thickness) * columns + thickness, (scale + thickness) * rows + thickness);
            Background.BackColor = Color.Black;
            for (int i = 0; i < rows; i++)
                for (int j = 0; j < columns; j++)
                {
                    Squares[i, j] = new PictureBox();
                    Controls.Add(Squares[i, j]);
                    Squares[i, j].Location = new Point(x + j * scale + thickness * (j + 1), y + i * scale + thickness * (i + 1));
                    Squares[i, j].Size = new Size(scale, scale);
                    Squares[i, j].BackColor = Color.Gray;
                    Squares[i, j].BringToFront();
                }
        }

        private void SpawnPiece(int x, int y, int model, int tag)
        {
            int i,currentpiece=0;
            for (i = 0; i<currentpiecenumber; i++) 
                if (Piece == null || Piece[i].Location == new Point(0, 0))
                {
                    currentpiece = i;
                    break;
                }
            if (i == currentpiecenumber)
            {
                currentpiece = currentpiecenumber;
                currentpiecenumber++;
            }
            Piece[currentpiece] = new PictureBox();
            Controls.Add(Piece[currentpiece]);
            Piece[currentpiece].Location = new Point(xgrid + (x - 1) * scale + thickness * x, ygrid + (y - 1) * scale + thickness * y);
            Piece[currentpiece].Size = new Size(scale, scale);
            switch (model)
            {
                case 1:
                    Piece[currentpiece].BackColor = Color.Cyan;
                    Piece[currentpiece].Tag = (101 + 10 * tag).ToString();
                    break;
                case 2:
                    Piece[currentpiece].BackColor = Color.Blue;
                    Piece[currentpiece].Tag = (201 + 10 * tag).ToString();
                    break;
                case 3:
                    Piece[currentpiece].BackColor = Color.Orange;
                    Piece[currentpiece].Tag = (301 + 10 * tag).ToString();

                    break;
                case 4:
                    Piece[currentpiece].BackColor = Color.Yellow;
                    Piece[currentpiece].Tag = (401 + 10 * tag).ToString();

                    break;
                case 5:
                    Piece[currentpiece].BackColor = Color.LawnGreen;
                    Piece[currentpiece].Tag = (501 + 10 * tag).ToString();

                    break;
                case 6:
                    Piece[currentpiece].BackColor = Color.Purple;
                    Piece[currentpiece].Tag = (601 + 10 * tag).ToString();

                    break;
                case 7:
                    Piece[currentpiece].BackColor = Color.Red;
                    Piece[currentpiece].Tag = (701 + 10 * tag).ToString();
                    break;
            }
            Piece[currentpiece].BringToFront();
        }

        private void SpawnTetronimo(int variation)
        {
            switch (variation)
            {
                case 1:
                    SpawnPiece((columns + 1) / 2 - 1, 1, 1, 1);
                    SpawnPiece((columns + 1) / 2, 1, 1, 2);
                    SpawnPiece((columns + 1) / 2 + 1, 1, 1, 3);
                    SpawnPiece((columns + 1) / 2 + 2, 1, 1, 4);
                    break;
                case 2:
                    SpawnPiece(columns / 2, 1, 2, 1);
                    SpawnPiece(columns / 2, 2, 2, 2);
                    SpawnPiece(columns / 2 + 1, 2, 2, 3);
                    SpawnPiece(columns / 2 + 2, 2, 2, 4);
                    break;
                case 3:
                    SpawnPiece(columns / 2, 2, 3, 1);
                    SpawnPiece(columns / 2 + 1, 2, 3, 2);
                    SpawnPiece(columns / 2 + 2, 2, 3, 3);
                    SpawnPiece(columns / 2 + 2, 1, 3, 4);
                    break;
                case 4:
                    SpawnPiece((columns + 1) / 2, 1, 4, 1);
                    SpawnPiece((columns + 1) / 2 + 1, 1, 4, 2);
                    SpawnPiece((columns + 1) / 2, 2, 4, 3);
                    SpawnPiece((columns + 1) / 2 + 1, 2, 4, 4);
                    break;
                case 5:
                    SpawnPiece(columns / 2 + 1, 1, 5, 3);
                    SpawnPiece(columns / 2 + 2, 1, 5, 4);
                    SpawnPiece(columns / 2, 2, 5, 1);
                    SpawnPiece(columns / 2 + 1, 2, 5, 2);
                    break;
                case 6:
                    SpawnPiece(columns / 2 + 1, 1, 6, 1);
                    SpawnPiece(columns / 2, 2, 6, 2);
                    SpawnPiece(columns / 2 + 1, 2, 6, 3);
                    SpawnPiece(columns / 2 + 2, 2, 6, 4);
                    break;
                case 7:
                    SpawnPiece(columns / 2, 1, 7, 1);
                    SpawnPiece(columns / 2 + 1, 1, 7, 2);
                    SpawnPiece(columns / 2 + 1, 2, 7, 3);
                    SpawnPiece(columns / 2 + 2, 2, 7, 4);
                    break;
            }
        }

        private void PreviewNextSet(int x)
        {
            switch (x)
            {
                case 1:
                    SpawnPreviewPiece(10, 380 , 1, 1);
                    SpawnPreviewPiece(10 + scale, 380 , 1, 2);
                    SpawnPreviewPiece(10 + 2 * scale, 380 , 1, 3);
                    SpawnPreviewPiece(10 + 3 * scale, 380 , 1, 4);
                    break;
                case 2:
                    SpawnPreviewPiece(10, 380, 2, 1);
                    SpawnPreviewPiece(10, 380 + scale, 2, 2);
                    SpawnPreviewPiece(10+scale, 380 + scale, 2, 3);
                    SpawnPreviewPiece(10 + 2* scale, 380 + scale, 2, 4);
                    break;
                case 3:
                    SpawnPreviewPiece(10+2*scale, 380, 3, 1);
                    SpawnPreviewPiece(10, 380 + scale, 3, 2);
                    SpawnPreviewPiece(10 + scale, 380 + scale, 3, 3);
                    SpawnPreviewPiece(10 + 2 * scale, 380 + scale, 3, 4);
                    break;
                case 4:
                    SpawnPreviewPiece(10 + scale, 380, 4, 1);
                    SpawnPreviewPiece(10 + scale, 380 + scale, 4, 2);
                    SpawnPreviewPiece(10 + 2*scale, 380, 4, 3);
                    SpawnPreviewPiece(10 + 2*scale, 380 + scale, 4, 4);
                    break;
                case 5:
                    SpawnPreviewPiece(10 + 2*scale, 380, 5, 3);
                    SpawnPreviewPiece(10 + 3 * scale, 380, 5, 4);
                    SpawnPreviewPiece(10 + scale, 380 + scale, 5, 1);
                    SpawnPreviewPiece(10 + 2 * scale, 380 + scale, 5, 2);
                    break;
                case 6:
                    SpawnPreviewPiece(10 + scale, 380, 6, 1);
                    SpawnPreviewPiece(10 , 380 + scale, 6, 2);
                    SpawnPreviewPiece(10+scale, 380 + scale, 6, 3);
                    SpawnPreviewPiece(10 + 2*scale, 380 + scale, 6, 4);
                    break;
                case 7:
                    SpawnPreviewPiece(10 + scale, 380, 7, 1);
                    SpawnPreviewPiece(10 + 2 * scale, 380, 7, 2);
                    SpawnPreviewPiece(10 + 2 * scale, 380 + scale, 7, 3);
                    SpawnPreviewPiece(10 + 3 * scale, 380 + scale, 7, 4);
                    break;
            }
        }

        private void SpawnPreviewPiece(int x, int y, int model, int nr)
        {
            PreviewPiece[nr].Location = new Point(x,y);
            PreviewPiece[nr].Size = new Size(scale, scale);
            PreviewPiece[nr].BorderStyle = BorderStyle.FixedSingle;
            switch (model)
            {
                case 1:
                    PreviewPiece[nr].BackColor = Color.Cyan;
                    break;
                case 2:
                    PreviewPiece[nr].BackColor = Color.Blue;
                    break;
                case 3:
                    PreviewPiece[nr].BackColor = Color.Orange;
                    break;
                case 4:
                    PreviewPiece[nr].BackColor = Color.Yellow;
                    break;
                case 5:
                    PreviewPiece[nr].BackColor = Color.LawnGreen;
                    break;
                case 6:
                    PreviewPiece[nr].BackColor = Color.Purple;
                    break;
                case 7:
                    PreviewPiece[nr].BackColor = Color.Red;
                    break;
            }
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            output.Volume = trackBar1.Value/100f;
            volume=trackBar1.Value;
            if (trackBar1.Value == 0)
            { pictureBox2.BackgroundImage = Properties.Resources.speakermuted;
                sound = false;
            }
            else
            {
                sound = true;
                if (trackBar1.Value > 75)
                    pictureBox2.BackgroundImage = Properties.Resources.speaker3;
                else if (trackBar1.Value > 50)
                    pictureBox2.BackgroundImage = Properties.Resources.speaker2;
                else if (trackBar1.Value > 25)
                    pictureBox2.BackgroundImage = Properties.Resources.speaker1;
                else if (trackBar1.Value > 0)
                    pictureBox2.BackgroundImage = Properties.Resources.speaker0;
            }

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if(sound)
            {
                sound = false;
                pictureBox2.BackgroundImage = Properties.Resources.speakermuted;
                output.Volume = 0f;
                trackBar1.Value = 0;
            }
            else
            {
                sound = true;
                if (volume == 0) volume = 100;
                output.Volume = volume/100f;
                trackBar1.Value = volume;
                if (volume > 75)
                    pictureBox2.BackgroundImage = Properties.Resources.speaker3;
                else if (volume > 50)
                    pictureBox2.BackgroundImage = Properties.Resources.speaker2;
                else if (volume > 25)
                    pictureBox2.BackgroundImage = Properties.Resources.speaker1;
                else if (volume > 0)
                    pictureBox2.BackgroundImage = Properties.Resources.speaker0;
            }

        }

        private void Mechanics()
        {
            KeyPreview = true;
            KeyDown += (object sender1, KeyEventArgs e1) =>
            {
                if (timer1.Enabled == false) return;
                switch (e1.KeyCode)
                {
                    case Keys.Right:
                    case Keys.D:
                        if (!IsOnTheEdge("Right") && !IsTouchingABlock("Right"))
                            for (int i = 0; i < currentpiecenumber; i++)
                                if (Piece[i].Tag != null)
                                    Piece[i].Left += scale + thickness;
                        break;
                    case Keys.Left:
                    case Keys.A:
                        if (!IsOnTheEdge("Left") && !IsTouchingABlock("Left"))
                            for (int i = 0; i < currentpiecenumber; i++)
                                if (Piece[i].Tag != null)
                                    Piece[i].Left -= scale + thickness;
                        break;
                    case Keys.S:
                    case Keys.Down:
                        timer1.Interval = speed / 5;
                        break;
                    case Keys.Space:
                    case Keys.Enter:
                        timer1.Interval = 1;
                        Timer temp = new Timer();
                        temp.Enabled = true;
                        temp.Interval = 1;
                        temp.Tick += (object sender, EventArgs e) => { if (IsOnTheEdge("Down") || IsTouchingABlock("Down")) timer1.Interval = speed; temp = null; };
                        break;
                    case Keys.P:
                    case Keys.Escape:
                        Pause();
                        break;
                }
            };
            KeyUp += (object sender1, KeyEventArgs e1) =>
            {
                if (timer1.Enabled == false) return;
                switch (e1.KeyCode)
                {
                    case Keys.S:
                    case Keys.Down:
                        timer1.Interval = speed;
                        break;
                    case Keys.W:
                    case Keys.Up:
                        Rotate();
                        break;

                }
            };
        }

        private void Rotate()
        {
            if (!TetrominoCanBeRotated()) return;
            for (int i = 0; i < currentpiecenumber; i++)
                switch (Piece[i].Tag)
                {
                    case "111":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "112";
                        break;
                    case "121":
                        Piece[i].Tag = "122";
                        break;
                    case "131":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "132";
                        break;
                    case "141":
                        Piece[i].Left -= 2 * (scale + thickness);
                        Piece[i].Top -= 2 * (scale + thickness);
                        Piece[i].Tag = "142";
                        break;
                    case "112":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "111";
                        break;
                    case "122":
                        Piece[i].Tag = "121";
                        break;
                    case "132":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "131";
                        break;
                    case "142":
                        Piece[i].Left += 2 * (scale + thickness);
                        Piece[i].Top += 2 * (scale + thickness);
                        Piece[i].Tag = "141";
                        break;
                    case "211":
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "212";
                        break;
                    case "221":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Tag = "222";
                        break;
                    case "231":
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "232";
                        break;
                    case "241":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Top -= 2 * (scale + thickness);
                        Piece[i].Tag = "242";
                        break;
                    case "212":
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "213";
                        break;
                    case "222":
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "223";
                        break;
                    case "232":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Tag = "233";
                        break;
                    case "242":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Top += 2 * (scale + thickness);
                        Piece[i].Tag = "243";
                        break;
                    case "213":
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "214";
                        break;
                    case "223":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Tag = "224";
                        break;
                    case "233":
                        Piece[i].Left -= 2 * (scale + thickness);
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "234";
                        break;
                    case "243":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Top -= 2 * (scale + thickness);
                        Piece[i].Tag = "244";
                        break;
                    case "214":
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "211";
                        break;
                    case "224":
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "221";
                        break;
                    case "234":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Top += 2 * (scale + thickness);
                        Piece[i].Tag = "231";
                        break;
                    case "244":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Top += 2 * (scale + thickness);
                        Piece[i].Tag = "241";
                        break;
                    case "311":
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "312";
                        break;
                    case "321":
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "322";
                        break;
                    case "331":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Tag = "332";
                        break;
                    case "341":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Top += 2 * (scale + thickness);
                        Piece[i].Tag = "342";
                        break;
                    case "312":
                        Piece[i].Top += 2 * (scale + thickness);
                        Piece[i].Tag = "313";
                        break;
                    case "322":
                        Piece[i].Top += scale + thickness;
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Tag = "323";
                        break;
                    case "332":
                        Piece[i].Tag = "333";
                        break;
                    case "342":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "343";
                        break;
                    case "313":
                        Piece[i].Top -= 2 * (scale + thickness);
                        Piece[i].Tag = "314";
                        break;
                    case "323":
                        Piece[i].Tag = "324";
                        break;
                    case "333":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "334";
                        break;
                    case "343":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "344";
                        break;
                    case "314":
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "311";
                        break;
                    case "324":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Tag = "321";
                        break;
                    case "334":
                        Piece[i].Left += 2 * (scale + thickness);
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "331";
                        break;
                    case "344":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Top -= 2 * (scale + thickness);
                        Piece[i].Tag = "341";
                        break;
                    case "511":
                        Piece[i].Top -= 2 * (scale + thickness);
                        Piece[i].Tag = "512";
                        break;
                    case "521":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "522";
                        break;
                    case "531":
                        Piece[i].Tag = "532";
                        break;
                    case "541":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "542";
                        break;
                    case "512":
                        Piece[i].Top += 2 * (scale + thickness);
                        Piece[i].Tag = "511";
                        break;
                    case "522":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "521";
                        break;
                    case "532":
                        Piece[i].Tag = "531";
                        break;
                    case "542":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "541";
                        break;
                    case "611":
                        Piece[i].Top += scale + thickness;
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Tag = "612";
                        break;
                    case "621":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "622";
                        break;
                    case "631":
                        Piece[i].Tag = "632";
                        break;
                    case "641":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "642";
                        break;
                    case "612":
                        Piece[i].Tag = "613";
                        break;
                    case "622":
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "623";
                        break;
                    case "632":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Tag = "633";
                        break;
                    case "642":
                        Piece[i].Tag = "643";
                        break;
                    case "613":
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "614";
                        break;
                    case "623":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Tag = "624";
                        break;
                    case "633":
                        Piece[i].Left -= 2 * (scale + thickness);
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "634";
                        break;
                    case "643":
                        Piece[i].Top -= (scale + thickness);
                        Piece[i].Tag = "644";
                        break;
                    case "614":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Tag = "611";
                        break;
                    case "624":
                        Piece[i].Tag = "621";
                        break;
                    case "634":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "631";
                        break;
                    case "644":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Tag = "641";
                        break;
                    case "711":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Top += 2 * (scale + thickness);
                        Piece[i].Tag = "712";
                        break;
                    case "721":
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "722";
                        break;
                    case "731":
                        Piece[i].Left += scale + thickness;
                        Piece[i].Tag = "732";
                        break;
                    case "741":
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "742";
                        break;
                    case "712":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Top -= 2 * (scale + thickness);
                        Piece[i].Tag = "711";
                        break;
                    case "722":
                        Piece[i].Top -= scale + thickness;
                        Piece[i].Tag = "721";
                        break;
                    case "732":
                        Piece[i].Left -= scale + thickness;
                        Piece[i].Tag = "731";
                        break;
                    case "742":
                        Piece[i].Top += scale + thickness;
                        Piece[i].Tag = "741";
                        break;
                }

        }

        private bool TetrominoCanBeRotated()
        {
            for (int i = 0; i < currentpiecenumber; i++)
            {
                if (Piece[i].Tag == null) continue;
                if (!PieceCanBeRotated(Piece[i]))
                    return false;
            }
            return true;
        }

        private bool PieceCanBeRotated(Control control)
        {
            switch (control.Tag)
            {
                case "111":
                    if (control.Top >= ygrid + (rows - 1) * scale + thickness * rows || CheckForPieceRelative(control, 1, 1))
                        return false;
                    break;
                case "131":
                    if (CheckForPieceRelative(control, -1, -1))
                        return false;
                    break;
                case "141":
                    if (control.Top <= ygrid + scale + 2 * thickness || CheckForPieceRelative(control, -2, -2))
                        return false;
                    break;
                case "112":
                    if (control.Left >= xgrid + (columns - 2) * scale + thickness * (columns - 1) || CheckForPieceRelative(control, -1, -1))
                        return false;
                    break;
                case "132":
                    if (CheckForPieceRelative(control, 1, 1))
                        return false;
                    break;
                case "142":
                    if (control.Left <= xgrid + thickness || CheckForPieceRelative(control, 2, 2))
                        return false;
                    break;
                case "211":
                    if (control.Top <= ygrid + thickness || CheckForPieceRelative(control, 0, 1))
                        return false;
                    break;
                case "221":
                    if (CheckForPieceRelative(control, 1, 0))
                        return false;
                    break;
                case "231":
                    if (CheckForPieceRelative(control, 0, -1))
                        return false;
                    break;
                case "241":
                    if (CheckForPieceRelative(control, -1, -2))
                        return false;
                    break;
                case "212":
                    if (CheckForPieceRelative(control, 0, -1))
                        return false;
                    break;
                case "222":
                    if (control.Left >= xgrid + (columns - 1) * scale + thickness * columns || CheckForPieceRelative(control, 0, -1))
                        return false;
                    break;
                case "232":
                    if (CheckForPieceRelative(control, 0, +1))
                        return false;
                    break;
                case "242":
                    if (CheckForPieceRelative(control, 1, 2))
                        return false;
                    break;
                case "213":
                    if (CheckForPieceRelative(control, 0, 1))
                        return false;
                    break;
                case "223":
                    if (CheckForPieceRelative(control, -1, 0))
                        return false;
                    break;
                case "233":
                    if (CheckForPieceRelative(control, -2, -1))
                        return false;
                    break;
                case "243":
                    if (CheckForPieceRelative(control, -1, -2))
                        return false;
                    break;
                case "214":
                    if (CheckForPieceRelative(control, 0, -1))
                        return false;
                    break;
                case "224":
                    if (CheckForPieceRelative(control, 0, 1))
                        return false;
                    break;
                case "234":
                    if (CheckForPieceRelative(control, 1, 2))
                        return false;
                    break;
                case "244":
                    if (control.Left >= xgrid + (columns - 1) * scale + thickness * columns || CheckForPieceRelative(control, 1, 2))
                        return false;
                    break;
                case "311":
                    if (control.Top >= ygrid + (rows - 1) * scale + thickness * rows || CheckForPieceRelative(control, 0, -1))
                        return false;
                    break;
                case "321":
                    if (CheckForPieceRelative(control, 0, -1))
                        return false;
                    break;
                case "331":
                    if (CheckForPieceRelative(control, -1, 0))
                        return false;
                    break;
                case "341":
                    if (CheckForPieceRelative(control, -1, 2))
                        return false;
                    break;
                case "312":
                    if (CheckForPieceRelative(control, 0, 2))
                        return false;
                    break;
                case "322":
                    if (control.Left >= xgrid + (columns - 1) * scale + thickness * columns||CheckForPieceRelative(control, -1, 1))
                        return false;
                    break;
                case "342":
                    if (CheckForPieceRelative(control, 1, -1))
                        return false;
                    break;
                case "313":
                    if (CheckForPieceRelative(control, 0, -2))
                        return false;
                    break;
                case "333":
                    if (CheckForPieceRelative(control, -1, 1))
                        return false;
                    break;
                case "343":
                    if (CheckForPieceRelative(control, -1, 1))

                        return false;
                    break;
                case "314":
                    if (CheckForPieceRelative(control, 0, 1))
                        return false;
                    break;
                case "324":
                    if (CheckForPieceRelative(control, 1, 0))
                        return false;
                    break;
                case "334":
                    if (CheckForPieceRelative(control, 2, -1))
                        return false;
                    break;
                case "344":
                    if (control.Left >= xgrid + (columns - 1) * scale + thickness * columns ||CheckForPieceRelative(control, 1, -2))
                        return false;
                    break;
                case "511":
                    if (CheckForPieceRelative(control, 0, -2))
                        return false;
                    break;
                case "521":
                    if (CheckForPieceRelative(control, -1, -1))
                        return false;
                    break;
                case "541":
                    if (control.Top <= ygrid + thickness || CheckForPieceRelative(control, -1, 1))
                        return false;
                    break;
                case "512":
                    if (CheckForPieceRelative(control, 0, 2))
                        return false;
                    break;
                case "522":
                    if (CheckForPieceRelative(control, 1, 1))
                        return false;
                    break;
                case "542":
                    if (control.Left >= xgrid + (columns - 1) * scale + thickness * columns||CheckForPieceRelative(control, 1, -1))
                        return false;
                    break;
                case "611":
                    if (CheckForPieceRelative(control, -1, 1))
                        return false;
                    break;
                case "621":
                    if (control.Top >= ygrid + (rows - 1) * scale + thickness * rows || CheckForPieceRelative(control, 1, -1))
                        return false;
                    break;
                case "641":
                    if (CheckForPieceRelative(control, -1, 1))
                        return false;
                    break;
                case "622":
                    if (control.Left >= xgrid + (columns - 1) * scale + thickness * columns || CheckForPieceRelative(control, 0, 1))
                        return false;
                    break;
                case "632":
                    if (CheckForPieceRelative(control, 1, 0))
                        return false;
                    break;
                case "613":
                    if (CheckForPieceRelative(control, 0, -1))
                        return false;
                    break;
                case "623":
                    if (CheckForPieceRelative(control, -1, 0))
                        return false;
                    break;
                case "633":
                    if (CheckForPieceRelative(control, -2, 1))
                        return false;
                    break;
                case "643":
                    if (CheckForPieceRelative(control, 0, -1))
                        return false;
                    break;
                case "614":
                    if ( CheckForPieceRelative(control, 1, 0))
                        return false;
                    break;

                case "634":
                    if (CheckForPieceRelative(control, 1, -1))
                        return false;
                    break;
                case "644":
                    if (control.Left >= xgrid + (columns - 1) * scale + thickness * columns || CheckForPieceRelative(control, 1, 0))
                        return false;
                    break;
                case "711":
                    if (CheckForPieceRelative(control, 1, 2))
                        return false;
                    break;
                case "721":
                    if (CheckForPieceRelative(control, 0, 1))
                        return false;
                    break;
                case "731":
                    if (CheckForPieceRelative(control, 1, 0))
                        return false;
                    break;
                case "741":
                    if (control.Top >= ygrid + (rows - 1) * scale + thickness * rows || CheckForPieceRelative(control, 0, -1))
                        return false;
                    break;
                case "712":
                    if (control.Left <= xgrid + thickness || CheckForPieceRelative(control, -1, -2))
                        return false;
                    break;
                case "722":
                    if (CheckForPieceRelative(control, 0, -1))
                        return false;
                    break;
                case "732":
                    if (CheckForPieceRelative(control, -1, 0))
                        return false;
                    break;
                case "742":
                    if (CheckForPieceRelative(control, 0, 1))
                        return false;
                    break;

            }
            return true;
        }

        private bool CheckForPieceRelative(Control control, int x, int y)
        {
            for (int i = 0; i < currentpiecenumber; i++)
            {
                if (Piece[i].Tag != null) continue;
                if (Piece[i].Location == new Point(control.Left + x * (thickness + scale), control.Top + y * (thickness + scale)))
                    return true;
            }
            return false;
        }

        private bool IsOnTheEdge(string Place)
        {
            for (int i = 0; i < currentpiecenumber; i++)
            {
                if (Piece[i].Tag == null) continue;
                switch (Place)
                {
                    case "Left":
                        if (Piece[i].Location.X == xgrid + thickness)
                            return true;
                        break;
                    case "Right":
                        if (Piece[i].Location.X == xgrid + (columns - 1) * scale + thickness * columns)
                            return true;
                        break;
                    case "Down":
                        if (Piece[i].Location.Y == ygrid + (rows - 1) * scale + thickness * rows)
                            return true;
                        break;
                }
            }
            return false;

        }

        private bool IsTouchingABlock(string Direction)
        {
            for (int i = 0; i < currentpiecenumber; i++)
            {
                if (Piece[i].Tag == null) continue;
                for (int j = 0; j < currentpiecenumber; j++)
                {
                    if (i == j || Piece[j].Tag != null) continue;
                    switch (Direction)
                    {
                        case "Down":
                            if (Piece[j].Top - Piece[i].Top == scale + thickness && Piece[j].Left == Piece[i].Left) return true;
                            break;
                        case "Left":
                            if (Piece[j].Top == Piece[i].Top && Piece[i].Left - Piece[j].Left ==scale+thickness) return true;
                            break;
                        case "Right":
                            if (Piece[j].Top == Piece[i].Top && Piece[j].Left - Piece[i].Left == scale + thickness) return true;
                            break;
                    }
                }
            }
            return false;
        }

        private void CheckForCompletedLine()
        {
            int nr = 0;
            double multiplier=0;
            for (int i = 1; i <= rows; i++)
            {
                int f = 0;
                for (int j = 1; j <= columns && f==0; j++)
                {
                    int ok = 0;
                    f = 1;
                    for (int k = 0; k < currentpiecenumber; k++)
                        if (Piece[k].Location == new Point(xgrid + (j - 1) * scale + thickness * j, ygrid + (i - 1) * scale + thickness * i))
                        {
                            ok++;
                            f = 0;
                            break;
                        }
                    if (ok == 0) f = 1;
                    if (j == columns && f==0)
                    {
                        nr++;
                        lines++;
                        label4.Text = lines.ToString();
                        ClearRow(i);
                        Shift(i);
                        if (mode == 2) UpdateLevelSpeed();
                    }
                }
            }
            switch(nr)
            {
                case 0:
                    multiplier = 0;
                    combo = 1;
                    break;
                case 1:
                    multiplier = 1;
                    if (combo != 1) combo *= 1.25;
                    break;
                case 2:
                    multiplier = 2.5;
                    if (combo != 1) combo *= 1.5;
                    break;
                case 3:
                    multiplier = 7.5;
                    if (combo != 1) combo *= 1.75;
                    break;
                case 4:
                    multiplier = 30;
                    if (combo != 1) combo *= 2;
                    break;
            }
            if (mode == 1)
                score += (int)(combo * multiplier * 100 * Math.Log10(time));
            else
                score += (int)(combo * multiplier * 100 * (level + 1));
        }

        private void ClearRow(int x)
        {
            for (int i = 1; i <= columns; i++)
                for (int j = 0; j < currentpiecenumber; j++)
                    if (Piece[j].Location == new Point(xgrid + (i - 1) * scale + thickness * i, ygrid + (x - 1) * scale + thickness * x))
                        DestroyBlock(Piece[j]);
        }

        private void Shift(int x)
        {
            for (int i = x - 1; i >= 1; i--)
                for (int j = 1; j <= columns; j++)
                    for (int k = 0; k < currentpiecenumber; k++)
                        if (Piece[k].Location == new Point(xgrid + (j - 1) * scale + thickness * j, ygrid + (i - 1) * scale + thickness * i))
                            Piece[k].Top += scale + thickness;

        }

        private void DestroyBlock(Control control)
        {
            Controls.Remove(control);
            control.Location = new Point(0, 0);
            control.Dispose();
            control = null;
        }

        private bool GameHasEnded()
        {
            for (int i = 1; i <= 3; i++)
                for (int j = 1; j <= columns; j++)
                {
                    int nr = 0;
                    for (int k = 0; k < currentpiecenumber; k++)
                        if (Piece[k].Location == new Point(xgrid + (j - 1) * scale + thickness * j, ygrid + (i - 1) * scale + thickness * i))
                            nr++;
                    if (nr == 2)
                        return true;
                }
            return false;
        }
    }
}
