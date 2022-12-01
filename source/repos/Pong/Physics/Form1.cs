using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Physics {
	public partial class Form1 : Form {
		List<PhysicsObject> AllObjects = new List<PhysicsObject>();
		const double TAU = 2 * Math.PI;
		public static double Left = 0, Right = 0;
		int Collisions = 0;
		Label col;
		PhysicsObject b, p1;
		public Form1() {
			InitializeComponent();
			WindowState = FormWindowState.Maximized;
			Size = new Size(1920, 1080);
			System.Windows.Forms.Timer t = new System.Windows.Forms.Timer {
				Enabled = true,
				Interval = 1
			};
			Label text = new Label {
				Location = new Point(1500, 200),
				AutoSize = true
			};
			Controls.Add(text);
			col = new Label {
				Location = new Point(1500, 300),
				AutoSize = true
			};
			Controls.Add(col);
			KeyUp += (sender, e) => {
				switch (e.KeyCode) {
					case Keys.W:
						Left -= 30;
						break;
					case Keys.S:
						Left += 30;
						break;
					case Keys.E:
						Right -= 30;
						break;
					case Keys.D:
						Right += 30;
						break;
				}
			};
			PhysicsObject p1 = new PhysicsObject(50, (1080 - 200) / 2, ref AllObjects) {
				BackColor = Color.Blue,
				Speed = 0,
				Angle = TAU / 4,
				Movable = false,
				Size = new Size(20, 200)
			};
			PhysicsObject p2 = new PhysicsObject(1920 - 50 - 20, (1080 - 200) / 2, ref AllObjects) {
				BackColor = Color.Blue,
				Speed = 0,
				Angle = TAU / 4,
				Movable = false,
				Size = new Size(20, 200)
			};

			PhysicsObject wl = new PhysicsObject(3, (1080 - 1080) / 2, ref AllObjects) {
				BackColor = Color.Black,
				Speed = 0,
				Angle = TAU / 4,
				Movable = false,
				Size = new Size(5, 1080)
			};
			PhysicsObject wr = new PhysicsObject(1920 - 3 - 5, (1080 - 1080) / 2, ref AllObjects) {
				BackColor = Color.Black,
				Speed = 0,
				Angle = TAU / 4,
				Movable = false,
				Size = new Size(5, 1080)
			};
			PhysicsObject wu = new PhysicsObject(0, 3, ref AllObjects) {
				BackColor = Color.Black,
				Speed = 0,
				Angle = 0,
				Movable = false,
				Size = new Size(1920, 5)
			};
			PhysicsObject wd = new PhysicsObject(0, 1080 - 3 - 5 - 30, ref AllObjects) {
				BackColor = Color.Black,
				Speed = 0,
				Angle = 0,
				Movable = false,
				Size = new Size(1920, 5)
			};
			Random rnd = new Random();
			PhysicsObject b = new PhysicsObject((1920 - 50) / 2, (1080 - 50) / 2, ref AllObjects) {
				BackColor = Color.Red,
				Speed = 15,
				Angle = TAU / 4 * rnd.Next(-75000, 75001) / 100000d,
				Mass = 10,
				Movable = true,
				Size = new Size(50, 50)
			};

			Controls.Add(p1);
			Controls.Add(p2);
			Controls.Add(b);
			Controls.Add(wl);
			Controls.Add(wr);
			Controls.Add(wu);
			Controls.Add(wd);

			t.Tick += (sender, e) => {
				Focus();
				double curt = 0;
				bool ok = true;
				while (ok) {
					List<Tuple<int, int>> ToDo = new List<Tuple<int, int>>();
					p1.Y += Left;
					p2.Y += Right;
					Left = Right = 0;
					double mint = 1 - curt;
					for (int i = 0; i < AllObjects.Count; ++i) {
						if (!Controls.Contains(AllObjects[i].Test))
							Controls.Add(AllObjects[i].Test);
						for (int j = 0; j < i; ++j) {
							double t = WillCollide(AllObjects[i], AllObjects[j]);
							if (t <= 1 - curt && t > 0 && t <= mint) {
								if (t < mint)
									ToDo.Clear();
								mint = t;
								ToDo.Add(Tuple.Create(i, j));
							}
						}
					}
					if (ToDo.Count > 0)
						curt += mint;
					else
						ok = false;
					for (int i = 0; i < AllObjects.Count; ++i) {
						AllObjects[i].Y -= mint * AllObjects[i].Speed * Math.Sin(AllObjects[i].Angle);
						AllObjects[i].X += mint * AllObjects[i].Speed * Math.Cos(AllObjects[i].Angle);
					}
					if (ok)
						foreach (var x in ToDo)
							DoCollision(AllObjects[x.Item1], AllObjects[x.Item2]);
					double KineticEnergy = 0;
					for (int i = 0; i < AllObjects.Count; ++i)
						KineticEnergy += AllObjects[i].Mass / 2 * AllObjects[i].Speed * AllObjects[i].Speed;
					text.Text = "Total Kinetic Energy : " + KineticEnergy;
				}
			};

		}
		public double WillCollide(PhysicsObject A, PhysicsObject B) {
			double vx = A.Speed * Math.Cos(A.Angle) - B.Speed * Math.Cos(B.Angle), vy = A.Speed * Math.Sin(A.Angle) - B.Speed * Math.Sin(B.Angle);
			double la = A.X, ra = A.X + A.Width, lb = B.X, rb = B.X + B.Width, t1x, t2x;
			if (lb < la) {
				(la, lb) = (lb, la);
				(ra, rb) = (rb, ra);
				vx *= -1;
			}
			t1x = (lb - ra) / vx;
			t2x = (rb - la) / vx;
			vy *= -1;
			double ua = A.Y, da = A.Y + A.Height, ub = B.Y, db = B.Y + B.Height, t1y, t2y;
			if (ub < ua) {
				(ua, ub) = (ub, ua);
				(da, db) = (db, da);
				vy *= -1;
			}
			t1y = (ub - da) / vy;
			t2y = (db - ua) / vy;
			if (t2x < t1x)
				(t1x, t2x) = (t2x, t1x);
			if (t2y < t1y)
				(t1y, t2y) = (t2y, t1y);
			if (t1x > t1y) {
				(t1x, t1y) = (t1y, t1x);
				(t2x, t2y) = (t2y, t2x);
			}
			if (t1y > t2x)
				return -1;
			return t1y;
		}
		public void DoCollision(PhysicsObject A, PhysicsObject B) {
			if (!A.Movable && !B.Movable) return;
			Random rnd = new Random();
			if (!B.Movable) {
				A.Angle = 2 * B.Angle - A.Angle;
				if (rnd.Next(0, 5) == 0)
					A.Speed *= 1.0125;
				if (A.Angle >= TAU) A.Angle -= TAU;
				else if (A.Angle < 0) A.Angle += TAU;
			}
			else if (!A.Movable) {
				B.Angle = 2 * A.Angle - B.Angle;
				if (rnd.Next(0, 5) == 0)
					B.Speed *= 1.0125;
				if (B.Angle >= TAU) B.Angle -= TAU;
				else if (B.Angle < 0) B.Angle += TAU;
			}
			else {
				double nnx = A.X - B.X + (A.Width - B.Width) / 2, nny = B.Y - A.Y + (B.Height - A.Height) / 2;
				double NormalX = nnx / Math.Sqrt(nnx * nnx + nny * nny), NormalY = nny / Math.Sqrt(nnx * nnx + nny * nny);
				double VelocityAX = A.Speed * Math.Cos(A.Angle), VelocityAY = A.Speed * Math.Sin(A.Angle), VelocityBX = B.Speed * Math.Cos(B.Angle), VelocityBY = B.Speed * Math.Sin(B.Angle);
				double RelativeVelocityX = VelocityBX - VelocityAX, RelativeVelocityY = VelocityBY - VelocityAY, NormalVelocity = NormalX * RelativeVelocityX + NormalY * RelativeVelocityY;
				double TempTemp = (-2 * NormalVelocity / (1 / A.Mass + 1 / B.Mass)), TempX = TempTemp * NormalX, TempY = TempTemp * NormalY;
				VelocityAX -= TempX / A.Mass;
				VelocityAY -= TempY / A.Mass;
				VelocityBX += TempX / B.Mass;
				VelocityBY += TempY / B.Mass;
				A.Speed = Math.Sqrt(VelocityAX * VelocityAX + VelocityAY * VelocityAY);
				A.Angle = Math.Atan2(VelocityAY, VelocityAX);
				if (A.Angle < 0) A.Angle += TAU;
				B.Speed = Math.Sqrt(VelocityBX * VelocityBX + VelocityBY * VelocityBY);
				B.Angle = Math.Atan2(VelocityBY, VelocityBX);
				if (B.Angle < 0) B.Angle += TAU;
			}
			++Collisions;
			col.Text = Collisions.ToString();
		}

	}
	public class Object : PhysicsObject {
		public Object(double x, double y, ref List<PhysicsObject> L) : base(x, y, ref L) { }
	}
	public class PhysicsObject : Control {
		public TextBox Test;
		public bool Movable;
		public double _X, _Y, Mass, Speed, Angle;
		public double X {
			get => _X;
			set {
				_X = value;
				Left = (int)_X;
			}
		}
		public double Y {
			get => _Y;
			set {
				_Y = value;
				Top = (int)_Y;
			}
		}
		public PhysicsObject(double x, double y, ref List<PhysicsObject> L) {
			X = x; Y = y; L.Add(this);
			KeyDown += (sender, e) => {
				switch (e.KeyCode) {
					case Keys.W:
						Form1.Left -= 30;
						break;
					case Keys.S:
						Form1.Left += 30;
						break;
					case Keys.E:
						Form1.Right -= 30;
						break;
					case Keys.D:
						Form1.Right += 30;
						break;
				}
			};
		}
	}
}