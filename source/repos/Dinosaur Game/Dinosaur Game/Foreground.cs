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
    public class Foreground
    {
        public Game Game;
        public List<PictureBox> CurrentObjects = new List<PictureBox>();
        public Random Random = new Random();
        public Foreground(Game game)
        {
            Game = game;
        }
        public void Update()
        {
            foreach (PictureBox Object in CurrentObjects)
                Object.Left -= (int)(Game.GameScale * Game._Speed * 0.02);
        }
        public void UpdateAnimation()
        {
            foreach (PictureBox Object in CurrentObjects)
                if (Object.Tag == null) continue;
                else if (Object.Tag.ToString() == "Bird1")
                {
                    Object.BackgroundImage = Properties.Resources.Bird_2;
                    Object.Tag = "Bird2";
                }
                else if (Object.Tag.ToString() == "Bird2")
                {
                    Object.BackgroundImage = Properties.Resources.Bird_1;
                    Object.Tag = "Bird1";
                }
        }
        public void GenerateNewObject()
        {
            PictureBox NewObject = new PictureBox
            {
                BackgroundImageLayout = ImageLayout.Stretch,
                Enabled = true,
                Visible = true,
                Left = Game.XSize
            };
            switch (Random.Next(1, 12))
            {
                case 1:
                case 2:
                case 3:
                    NewObject.Size = new Size(Game.GameScale / 3, 3 * Game.GameScale / 4);
                    NewObject.Top = Game.YSize - 7 * Game.GameScale / 4;
                    NewObject.BackgroundImage = Properties.Resources.Small_Cactus_1;
                    break;
                case 4:
                    NewObject.Size = new Size(2 * Game.GameScale / 3, 3 * Game.GameScale / 4);
                    NewObject.Top = Game.YSize - 7 * Game.GameScale / 4;
                    NewObject.BackgroundImage = Properties.Resources.Small_Cactus_2;
                    break;
                case 5:
                    NewObject.Size = new Size(Game.GameScale, 3 * Game.GameScale / 4);
                    NewObject.Top = Game.YSize - 7 * Game.GameScale / 4;
                    NewObject.BackgroundImage = Properties.Resources.Small_Cactus_3;
                    break;
                case 6:
                    NewObject.Size = new Size(Game.GameScale / 2, Game.GameScale);
                    NewObject.Top = Game.YSize - 2 * Game.GameScale;
                    NewObject.BackgroundImage = Properties.Resources.Big_Cactus_1;
                    break;
                case 7:
                    NewObject.Size = new Size(Game.GameScale, Game.GameScale);
                    NewObject.Top = Game.YSize - 2 * Game.GameScale;
                    NewObject.BackgroundImage = Properties.Resources.Big_Cactus_2;
                    break;
                case 8:
                    NewObject.Size = new Size(3 * Game.GameScale / 2, Game.GameScale);
                    NewObject.Top = Game.YSize - 2 * Game.GameScale;
                    NewObject.BackgroundImage = Properties.Resources.Big_Cactus_3;
                    break;
                case 9:
                    NewObject.Size = new Size(2 * Game.GameScale / 3, 3 * Game.GameScale / 4);
                    NewObject.Top = Game.YSize - 15 * Game.GameScale / 8;
                    NewObject.BackgroundImage = Properties.Resources.Bird_1;
                    NewObject.Tag = "Bird1";
                    break;
                case 10:
                    NewObject.Size = new Size(2 * Game.GameScale / 3, 3 * Game.GameScale / 4);
                    NewObject.Top = Game.YSize - 19 * Game.GameScale / 8;
                    NewObject.BackgroundImage = Properties.Resources.Bird_1;
                    NewObject.Tag = "Bird1";
                    break;
                case 11:
                    NewObject.Size = new Size(2 * Game.GameScale / 3, 3 * Game.GameScale / 4);
                    NewObject.Top = Game.YSize - 23 * Game.GameScale / 8;
                    NewObject.BackgroundImage = Properties.Resources.Bird_1;
                    NewObject.Tag = "Bird1";
                    break;
            }
            Game.Controls.Add(NewObject);
            CurrentObjects.Add(NewObject);
        }
    }
}