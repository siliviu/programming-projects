using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physics
{
    public partial class Form1 : Form
    {
        List<PhysicsObject> AllObjects = new List<PhysicsObject>();
        const double TAU = 2 * Math.PI;
        public Form1()
        {
            InitializeComponent();
            Size = new Size(1920, 1080);
            System.Windows.Forms.Timer t = new System.Windows.Forms.Timer
            {
                Enabled = true,
                Interval = 1
            };
            Label text = new Label
            {
                Location = new Point(1500, 200),
                AutoSize = true
            };
            Controls.Add(text);
            t.Tick += (sender, e) =>
            {
                double KineticEnergy = 0;
                for (int i = 0; i < AllObjects.Count; ++i)
                {
                    AllObjects[i].Y -= AllObjects[i].Speed * Math.Sin(AllObjects[i].Angle);
                    AllObjects[i].X += AllObjects[i].Speed * Math.Cos(AllObjects[i].Angle);
                    for (int j = 0; j < i; ++j)
                        if (AllObjects[i].X <= AllObjects[j].X + AllObjects[j].Width && AllObjects[j].X <= AllObjects[i].X + AllObjects[i].Width && AllObjects[i].Y <= AllObjects[j].Y + AllObjects[j].Height && AllObjects[j].Y <= AllObjects[i].Y + AllObjects[i].Height)
                            Collide(AllObjects[i], AllObjects[j]);
                    KineticEnergy += AllObjects[i].Mass * AllObjects[i].Speed * AllObjects[i].Speed / 2;
                }
                text.Text = "Total Kinetic Energy : " + KineticEnergy;
            };
            PhysicsObject b = new PhysicsObject(110, 0, ref AllObjects)
            {
                BackColor = Color.Blue,
                Speed = 0,
                Angle = 0,
                Movable = false,
                Size = new Size(1000, 30)
            };
            PhysicsObject c = new PhysicsObject(100, 0, ref AllObjects)
            {
                BackColor = Color.Blue,
                Speed = 0,
                Angle = Math.PI / 2,
                Movable = false,
                Size = new Size(30, 990)
            };
            PhysicsObject cc = new PhysicsObject(1110, 0, ref AllObjects)
            {
                BackColor = Color.Blue,
                Speed = 0,
                Angle = Math.PI / 2,
                Movable = false,
                Size = new Size(30, 970)
            };
            PhysicsObject a = new PhysicsObject(140, 40, ref AllObjects)
            {
                BackColor = Color.Red,
                Speed = 15,
                Angle = -Math.PI / 4,
                Movable = true,
                Mass = 10,
                Size = new Size(10, 10)
            };
            PhysicsObject aa = new PhysicsObject(1000, 900, ref AllObjects)
            {
                BackColor = Color.Yellow,
                Speed = 10,
                Angle = 3 * Math.PI / 4,
                Movable = true,
                Mass = 15,
                Size = new Size(12, 12)
            };
            PhysicsObject aaa = new PhysicsObject(410, 510, ref AllObjects)
            {
                BackColor = Color.Green,
                Speed = 20,
                Angle = -Math.PI / 5,
                Movable = true,
                Mass = 20,
                Size = new Size(14, 14)
            };
            PhysicsObject aaaa = new PhysicsObject(610, 320, ref AllObjects)
            {
                BackColor = Color.Orange,
                Speed = 15,
                Angle = -Math.PI / 6,
                Movable = true,
                Mass = 25,
                Size = new Size(15, 15)
            };
            PhysicsObject aaaaa = new PhysicsObject(310, 440, ref AllObjects)
            {
                BackColor = Color.Cyan,
                Speed = 10,
                Angle = -Math.PI / 3,
                Movable = true,
                Mass = 100,
                Size = new Size(20, 20)
            };
            PhysicsObject bb = new PhysicsObject(110, 960, ref AllObjects)
            {
                BackColor = Color.Blue,
                Speed = 0,
                Angle = 0,
                Movable = false,
                Size = new Size(1030, 30)
            };
            Controls.Add(b);
            Controls.Add(c);
            Controls.Add(cc);
            Controls.Add(a);
            Controls.Add(aa);
            Controls.Add(aaa);
            Controls.Add(aaaa);
            Controls.Add(aaaaa);
            Controls.Add(bb);
        }
        public void Collide(PhysicsObject A, PhysicsObject B)
        {
            if (!A.Movable && !B.Movable) return;
            if (!B.Movable)
            {
                A.Angle = 2 * B.Angle - A.Angle;
                if (A.Angle >= TAU) A.Angle -= TAU;
                else if (A.Angle < 0) A.Angle += TAU;
            }
            else if (!A.Movable)
            {
                B.Angle = 2 * A.Angle - B.Angle;
                if (B.Angle >= TAU) B.Angle -= TAU;
                else if (B.Angle < 0) B.Angle += TAU;
            }
            else
            {
                Vector2 Normal = Vector2.Normalize(new Vector2((float)(A.X - B.X + (A.Width - B.Width) / 2), (float)(B.Y - A.Y + (B.Height - A.Height) / 2)));
                Vector2 VelocityA = new Vector2((float)(A.Speed * Math.Cos(A.Angle)), (float)(A.Speed * Math.Sin(A.Angle)));
                Vector2 VelocityB = new Vector2((float)(B.Speed * Math.Cos(B.Angle)), (float)(B.Speed * Math.Sin(B.Angle)));
                Vector2 RelativeVelocity = VelocityB - VelocityA;
                double NormalVelocity = Vector2.Dot(RelativeVelocity, Normal);
                VelocityA -= (float)(-2 * B.Mass * NormalVelocity / (A.Mass + B.Mass)) * Normal;
                VelocityB += (float)(-2 * A.Mass * NormalVelocity / (A.Mass + B.Mass)) * Normal;
                A.Speed = VelocityA.Length();
                A.Angle = Math.Atan2(VelocityA.Y, VelocityA.X);
                if (A.Angle < 0) A.Angle += TAU;
                B.Speed = VelocityB.Length();
                B.Angle = Math.Atan2(VelocityB.Y, VelocityB.X);
                if (B.Angle < 0) B.Angle += TAU;
            }
        }

    }
    public class Object : PhysicsObject
    {
        public Object(double x, double y, ref List<PhysicsObject> L) : base(x, y, ref L) { }
    }
    public class PhysicsObject : Control
    {
        public bool Movable;
        public double _X, _Y, Mass, Speed, Angle;
        public double X
        {
            get => _X;
            set
            {
                _X = value;
                Left = (int)_X;
            }
        }
        public double Y
        {
            get => _Y;
            set
            {
                _Y = value;
                Top = (int)_Y;
            }
        }
        public PhysicsObject(double x, double y, ref List<PhysicsObject> L) { X = x; Y = y; L.Add(this); }
    }
}