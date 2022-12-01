using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Connect4 {
	public partial class Form2 : Form {
		public Form2() {
			InitializeComponent();
			Initialise();
		}

		private void Form2_Load(object sender, EventArgs e) {

		}

		public void Initialise() {
			Button b = new Button() {
				Text = "Play singleplayer",
				AutoSize = true,
				Location = new Point(20, 20)
			};
			b.MouseDown += (obj, e) => {
				Form1 f = new Form1();
				f.ShowDialog();
			};
			Controls.Add(b);

			Button b2 = new Button() {
				Text = "Host multiplayer",
				AutoSize = true,
				Location = new Point(20, 120)
			};
			b2.MouseDown += (obj, e) => {
				Form1 f = new Form1(Colour.Yellow);
				f.ShowDialog();
				//f.Show();
			};
			Controls.Add(b2);
			TextBox t = new TextBox() {
				Size = new Size(90, b2.Size.Height),
				Location = new Point(21, 180)
			};
			t.KeyDown += (obj, e) => {
				if (e.KeyCode == Keys.Enter) {
					Form1 f = new Form1(Colour.Red, t.Text.Length == 0 ? "127.0.0.1" : t.Text);
					f.ShowDialog();
				}
			};
			Controls.Add(t);
			Button b3 = new Button() {
				Text = "Join multiplayer",
				AutoSize = true,
				Location = new Point(20, 220)
			};
			b3.MouseDown += (obj, e) => {
				Form1 f = new Form1(Colour.Red, t.Text.Length == 0 ? "127.0.0.1" : t.Text);
				f.ShowDialog();
				//f.Show();
			};
			Controls.Add(b3);
		}


	}
}
