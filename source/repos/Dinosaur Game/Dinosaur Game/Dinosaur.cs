using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dinosaur_Game
{
    public class Dinosaur
    {
        public static int JumpSpeed = 0;
        public static int AnimationState = 0;
        public static bool Jumping = false;
        public PictureBox Model = new PictureBox
        {
            BackgroundImage = Properties.Resources.Dinosaur,
            BackgroundImageLayout = ImageLayout.Stretch,
            Enabled = true,
            Visible = true,
            Size = new Size(Game.GameScale, Game.GameScale),
            Location = new Point(50, Game.YSize - 2 * Game.GameScale)
        };
        public Dinosaur(Game Game)
        {
            Game.Controls.Add(Model);
        }
        public void Update()
        {
            if (Jumping) return;
            if (AnimationState == 1)
            {
                Model.BackgroundImage = Properties.Resources.Dinosaur_2;
                AnimationState = 2;
            }
            else
            {
                Model.BackgroundImage = Properties.Resources.Dinosaur_1;
                AnimationState = 1;
            }
        }
        public void Jump()
        {
            if (!Jumping) return;
            Model.BackgroundImage = Properties.Resources.Dinosaur;
            AnimationState = 0;
            JumpSpeed -= 1;
            if (Model.Top - JumpSpeed <= Game.YSize - 2 * Game.GameScale)
                Model.Top -= JumpSpeed;
            else
            {
                Jumping = false;
                Model.Top = Game.YSize - 2 * Game.GameScale;
            }
        }
    }
}