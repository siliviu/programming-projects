using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Generating_complex_numbers_from_a_circle
{
	public partial class Form1 : Form
	{
		const int offset = 600, thickness = 1, radius = 50, step = 1;
		int x = 100, y = 100, first = 0;
		List<PointCollection> CurrentPoints = new List<PointCollection>();
		public Form1()
		{
			InitializeComponent();
			SetupPoints();
			Invalidate();
			MouseDown += (sender, e) => Invalidate();
		}
		private void SetupPoints()
		{
			for (int i = -radius; i <= radius; i += step)
			{
				int j = (int)Math.Sqrt(radius * radius - i * i);
				if (CheckIfUnique(i, j))
					CurrentPoints.Add(new PointCollection(offset + i, offset + j));
				if (CheckIfUnique(i, -j))
					CurrentPoints.Add(new PointCollection(offset + i, offset - j));
			}
			for (int j = -radius; j <= radius; j += step)
			{
				int i = (int)Math.Sqrt(radius * radius - j * j);
				if (CheckIfUnique(i, j))
					CurrentPoints.Add(new PointCollection(offset + i, offset + j));
				if (CheckIfUnique(-i, j))
					CurrentPoints.Add(new PointCollection(offset - i, offset + j));

			}
		}
		private bool CheckIfUnique(int x, int y)
		{
			foreach (PointCollection a in CurrentPoints)
				if (a.X == x && a.Y == y)
					return false;
			return true;
		}
		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			Pen p = new Pen(Color.Red, thickness);
			int l = CurrentPoints.Count();
			for (int i = 0; i < l; i++)
			{
				if (first != 0)
					for (int j = i; j < l; j++)
					{
						x = CurrentPoints[i].X + CurrentPoints[j].X - offset;
						y = CurrentPoints[i].Y + CurrentPoints[j].Y - offset;
						if (!CheckIfUnique(x, y)) continue;
						CurrentPoints.Add(new PointCollection(x, y));
						e.Graphics.DrawEllipse(p, x, y, thickness, thickness);
					}
				x = CurrentPoints[i].X;
				y = CurrentPoints[i].Y;
				e.Graphics.DrawEllipse(p, x, y, thickness, thickness);
				CurrentPoints.Add(new PointCollection(x, y));
			}
			first = 1;
		}
	}
	public class PointCollection
	{
		public int X, Y;
		public PointCollection(int x, int y)
		{
			X = x;
			Y = y;
		}
	}
}
