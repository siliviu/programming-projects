using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace Dinosaur_Game
{
    public partial class Game : Form
    {
        public static int Score = 0, Time = 0, PreviousScoreTime = 0, PreviousObjectTime = 0, PreviousAnimationTime = 0, Speed = 1000, XSize = 1920, YSize = 1080, GameScale = 200;
        public static double _Speed = 1 + Math.Log2(Speed);
        public static bool GameIsOver = false;
        public Background Background;
        public Foreground Foreground;
        public Random Random = new Random();
        public Dinosaur Dinosaur;
        public Game()
        {
            InitializeComponent();
            Height = YSize;
            Width = XSize;
            System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer
            {
                Interval = 1,
                Enabled = true,
            };
            Timer.Tick += (object sender, EventArgs e) => GameTick();
            KeyDown += (object sender, KeyEventArgs e) => HandleControls(e);
            Background = new Background(this);
            Foreground = new Foreground(this);
            Dinosaur = new Dinosaur(this);
        }
        public void GameTick()
        {
            if (GameIsOver) return;
            ++Time;
            Dinosaur.Jump();
            Background.Update();
            Foreground.Update();
            Recycle();
            if (DetectCollisions())
                EndGame();
            if ((Time - PreviousAnimationTime) >= (12 - _Speed / 2))
            {
                PreviousAnimationTime = Time;
                Dinosaur.Update();
                Foreground.UpdateAnimation();
            }
            if ((Time - PreviousObjectTime) >= GameScale * (10 - _Speed / 2 + Random.Next(Math.Min((int)(_Speed / 2), 3), 4)) * 0.1)
            {
                PreviousObjectTime = Time;
                Foreground.GenerateNewObject();
            }
            if ((Time - PreviousScoreTime) >= (10 - _Speed / 2))
            {
                PreviousScoreTime = Time;
                label1.Text = (++Score).ToString();
            }
            return;
        }

        public void HandleControls(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Space:
                    if (Dinosaur.Jumping) return;
                    Dinosaur.JumpSpeed = (int)(2 * (Math.Sqrt(GameScale)));
                    Dinosaur.Jumping = true;
                    break;
            }
        }
        public bool DetectCollisions()
        {
            if (Foreground.CurrentObjects == null) return false;
            Rectangle DinosaurBox = new Rectangle(Dinosaur.Model.Left, Dinosaur.Model.Top, Dinosaur.Model.Width, Dinosaur.Model.Height);
            foreach (PictureBox Object in Foreground.CurrentObjects)
            {
                Rectangle ObjectBox = new Rectangle(Object.Left, Object.Top, Object.Width, Object.Height);
                if (DinosaurBox.IntersectsWith(ObjectBox))
                    return true;
            }
            return false;
        }

        public void Recycle()
        {
            foreach (PictureBox Object in Foreground.CurrentObjects.ToList())
                if (Object.Right < 0)
                {
                    Foreground.CurrentObjects.Remove(Object);
                    Object.Dispose();
                }
        }

        public void EndGame()
        {
            GameIsOver = true;
            MessageBox.Show("It's over mate!");
        }
    }
}