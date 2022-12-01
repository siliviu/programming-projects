using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Game_Idea {
	public partial class Form1 : Form {
		public PictureBox[] Objects = new PictureBox[10];
		public PictureBox[] Obstacle = new PictureBox[50];
		public int objectcount = 1, obstaclecount = 1, objectsize = 50, obstaclesize = 100, currentpositionx = 0, currentpositiony = 0, isdragging = 0;
		public Control selecteditem;
		Random rnd = new Random();
		public Form1() {
			InitializeComponent();
			Height = 1080;
			Width = 1920;
			for (int i = 1; i <= 5; i++) {
				SpawnObjects(i);
				SpawnObstacles(i);
			}
		}

		public void SpawnObjects(int nr) {
			Objects[nr] = new PictureBox();
			Controls.Add(Objects[nr]);
			int x, y;
			do {
				x = rnd.Next(Width / 3, 2 * Width / 3);
				y = Height / 2;
				//y = rnd.Next(Height / 3, 2 * Height / 3);
			}
			while (CheckForDistinctObjects(x, "X_Object") /*|| CheckForDistinctObjects(y, "Y_Object")*/);
			Objects[nr].Location = new Point(x, y);
			Objects[nr].Size = new Size(objectsize, objectsize);
			Objects[nr].BackColor = Color.Green;
			Objects[nr].MouseDown += (sender, args) => {
				currentpositionx = args.X;
				currentpositiony = args.Y;
				isdragging = 1;
			};
			Objects[nr].MouseMove += (sender, args) => {
				if (isdragging == 1) {
					int dist1 = Math.Abs(args.X - currentpositionx), dist2 = Math.Abs(args.Y - currentpositiony);
					double dist = Math.Sqrt(dist1 * dist1 + dist2 * dist2);
					double md = objectsize*Math.Sqrt(objectsize) / Math.Log(objectsize);
					double mv = Math.Sqrt(objectsize * Math.Log(Math.Log(objectsize))), mct = 5, mlt = 1;
					double m = mlt*(dist >= md ? 1 : Math.Exp((md-dist) / (-mct* dist)));
					double v1 = (args.X - currentpositionx) * m, v2 = (args.Y - currentpositiony) * m, fact = Math.Sqrt(v1 * v1 + v2 * v2);
					if (fact > mv) {
						v1 *= mv / fact;
						v2 *= mv / fact;
					}
					Objects[nr].Left -= (int)v1;
					Objects[nr].Top -= (int)v2;
				}
			};
			Objects[nr].MouseUp += (sender, args) => {
				isdragging = 0;
			};
			objectcount++;
		}

		public void SpawnObstacles(int nr) {
			Obstacle[nr] = new PictureBox();
			Controls.Add(Obstacle[nr]);
			int x, y;
			do {
				x = rnd.Next(0, Width);
				y = rnd.Next(0, Height);
			}
			while (CheckForDistinctObjects(x, "X_Obstacle") || CheckForDistinctObjects(y, "Y_Obstacle") || (Width / 3 <= x && x <= 2 * Width / 3) || (Height / 3 <= y && y <= 2 * Height / 3));
			Obstacle[nr].Location = new Point(x, y);
			Obstacle[nr].Size = new Size(obstaclesize, obstaclesize);
			Obstacle[nr].BackColor = Color.Blue;
			obstaclecount++;
		}

		public bool CheckForDistinctObjects(int c, string mode) {
			switch (mode) {
				case "X_Object":
					for (int i = 1; i < objectcount; i++)
						if (Math.Abs(Objects[i].Left - c) <= objectsize)
							return true;
					return false;
				case "Y_Object":
					for (int i = 1; i < objectcount; i++)
						if (Math.Abs(Objects[i].Top - c) <= objectsize)
							return true;
					return false;
				case "X_Obstacle":
					for (int i = 1; i < obstaclecount; i++)
						if (Math.Abs(Obstacle[i].Left - c) <= objectsize)
							return true;
					return false;
				case "Y_Obstacle":
					for (int i = 1; i < obstaclecount; i++)
						if (Math.Abs(Obstacle[i].Top - c) <= objectsize)
							return true;
					return false;
			}
			return false;
		}
	}
}
