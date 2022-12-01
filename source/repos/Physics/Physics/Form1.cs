using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Physics {
	public partial class Form1 : Form {
		List<PhysicsObject> AllObjects = new List<PhysicsObject>();
		const double TAU = 2 * Math.PI;
		int Collisions = 0;
		Label col;
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
			t.Tick += (sender, e) => {
				double curt = 0;
				bool ok = true;
				while (ok) {
					List<Tuple<int, int>> ToDo = new List<Tuple<int, int>>();
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
			/// PI COLLISIONS
			//PhysicsObject o1 = new PhysicsObject(1500, 50, ref AllObjects) {
			//	BackColor = Color.Blue,
			//	Speed = 0,
			//	Angle = Math.PI / 2,
			//	Mass = 5,
			//	Movable = false,
			//	Size = new Size(20, 550)
			//};
			//PhysicsObject o2 = new PhysicsObject(950, 500, ref AllObjects) {
			//	BackColor = Color.Red,
			//	Speed = 1,
			//	Angle = 0 * TAU / 4,
			//	Mass = 1000000,
			//	Movable = true,
			//	Size = new Size(45, 45)
			//};
			//PhysicsObject o3 = new PhysicsObject(1000, 500, ref AllObjects) {
			//	BackColor = Color.Red,
			//	Speed = 0,
			//	Angle = 0 * TAU / 4,
			//	Mass = 1,
			//	Movable = true,
			//	Size = new Size(45, 45)
			//};
			//Controls.Add(o1); Controls.Add(o2); Controls.Add(o3);

			/// BOX OBJECTS
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
			PhysicsObject bb = new PhysicsObject(110, 960, ref AllObjects)
			{
				BackColor = Color.Blue,
				Speed = 0,
				Angle = 0,
				Movable = false,
				Size = new Size(1030, 30)
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
			if (!B.Movable) {
				A.Angle = 2 * B.Angle - A.Angle;
				if (A.Angle >= TAU) A.Angle -= TAU;
				else if (A.Angle < 0) A.Angle += TAU;
			}
			else if (!A.Movable) {
				B.Angle = 2 * A.Angle - B.Angle;
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
		public PhysicsObject(double x, double y, ref List<PhysicsObject> L) { X = x; Y = y; L.Add(this); }
	}
}