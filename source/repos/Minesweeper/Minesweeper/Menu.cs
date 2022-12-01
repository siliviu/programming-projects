using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Minesweeper
{
    public partial class Menu : Form
    {
        public static string GameDifficulty = "", GameSize = "", GameMode = "", text1 = "", text2 = "", text3 = "";

        Game frm = null;
        public Menu(Game frm)
        {
            InitializeComponent();
            this.frm = frm;
        }

        private void Menu_Load(object sender, EventArgs e)
        {
            checkBox1.Checked = Game.playsounds;
            checkBox2.Checked = Game.safefirst;
            comboBox1.SelectedItem = Game.mode;
            if(comboBox1.SelectedItem==null)
                comboBox1.SelectedItem = "Normal";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (GameDifficulty != "" && GameMode != "" && GameSize != "")
            {
                frm.label1.Visible = true;
                frm.label2.Visible = true;
                frm.label3.Visible = true;
                frm.label6.Visible = true;
                frm.label7.Visible = true;
                frm.label8.Visible = true;
                frm.label1.Text = GameSize + (GameSize == "Custom" ? " (" + Game.length.ToString() + "x" + Game.width.ToString() + ")" : "");
                frm.label3.Text = GameSize + (GameSize == "Custom" ? " (" + Game.bombs.ToString() + ")" : "");
                frm.label8.Text = GameMode;
                switch (GameDifficulty)
                {
                    case "Easy":
                        Game.bombs = (int)(0.12 * Game.length * Game.width);
                        break;
                    case "Medium":
                        Game.bombs = (int)(0.18 * Game.length * Game.width);
                        break;
                    case "Hard":
                        Game.bombs = (int)(0.24 * Game.length * Game.width);
                        break;
                    case "Hardcore":
                        Game.bombs = (int)(0.30 * Game.length * Game.width);
                        break;
                }
                frm.StartGame();
                this.Close();
                frm.button1.Enabled = false;
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Reset1();
            pictureBox1.BorderStyle = BorderStyle.FixedSingle;
            GameDifficulty = "Easy";
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Reset1();
            pictureBox2.BorderStyle = BorderStyle.FixedSingle;
            GameDifficulty = "Medium";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Reset1();
            pictureBox3.BorderStyle = BorderStyle.FixedSingle;
            GameDifficulty = "Hard";
        }

        private void pictureBox4_Click(object sender, EventArgs e)
        {
            Reset1();
            pictureBox4.BorderStyle = BorderStyle.FixedSingle;
            GameDifficulty = "Hardcore";
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Reset1();
            pictureBox5.BorderStyle = BorderStyle.FixedSingle;
            GameDifficulty = "Custom";
            label5.Visible = true;
            textBox3.Visible = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox3.Text, out int a) && !(textBox3.Text == ""))
            {
                textBox3.Text = text3;
                textBox3.SelectionStart = textBox3.Text.Length;
            }
            else
            {
                if (Int32.TryParse(textBox3.Text, out a))
                    Game.bombs = Convert.ToInt32(textBox3.Text);
                text3 = textBox3.Text;
            }
        }

        public void Reset1()
        {
            pictureBox1.BorderStyle = BorderStyle.None;
            pictureBox2.BorderStyle = BorderStyle.None;
            pictureBox3.BorderStyle = BorderStyle.None;
            pictureBox4.BorderStyle = BorderStyle.None;
            pictureBox5.BorderStyle = BorderStyle.None;
            label5.Visible = false;
            textBox3.Visible = false;
        }

        private void pictureBox10_Click(object sender, EventArgs e)
        {
            Reset2();
            Game.length = 12;
            Game.width = 9;
            pictureBox10.BorderStyle = BorderStyle.FixedSingle;
            GameSize = "Small";
        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {
            Reset2();
            Game.length = 16;
            Game.width = 12;
            pictureBox9.BorderStyle = BorderStyle.FixedSingle;
            GameSize = "Medium";
        }

        private void pictureBox8_Click(object sender, EventArgs e)
        {
            Reset2();
            Game.length = 24;
            Game.width = 16;
            pictureBox8.BorderStyle = BorderStyle.FixedSingle;
            GameSize = "Large";
        }

        private void pictureBox7_Click(object sender, EventArgs e)
        {
            Reset2();
            Game.length = 36;
            Game.width = 20;
            pictureBox7.BorderStyle = BorderStyle.FixedSingle;
            GameSize = "Insanity";
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
                Game.playsounds = true;
            else
                Game.playsounds = false;
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
                Game.safefirst = true;
            else
                Game.safefirst = false;
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Reset2();
            pictureBox6.BorderStyle = BorderStyle.FixedSingle;
            GameSize = "Custom";
            label3.Visible = true;
            label4.Visible = true;
            label7.Visible = true;
            label7.Text = "The maximum is " + (int)(Screen.PrimaryScreen.Bounds.Width - 20 - (GameMode == "Hexagon" ? ((int)(Screen.PrimaryScreen.Bounds.Height - 60) / 32 - 1) * 16 : 0)) / 32 + "x" + (int)(Screen.PrimaryScreen.Bounds.Height - 60) / 32;
            textBox1.Visible = true;
            textBox2.Visible = true;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int a) && !(textBox1.Text == ""))
            {
                textBox1.Text = text1;
                textBox1.SelectionStart = textBox1.Text.Length;
            }
            else
            {
                if (Int32.TryParse(textBox1.Text, out a))
                    Game.length = Convert.ToInt32(textBox1.Text);
                text1 = textBox1.Text;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox2.Text, out int a) && !(textBox2.Text == ""))
            {
                textBox2.Text = text2;
                textBox2.SelectionStart = textBox2.Text.Length;
            }
            else
            {
                if (Int32.TryParse(textBox2.Text, out a))
                    Game.width = Convert.ToInt32(textBox2.Text);
                text2 = textBox2.Text;
            }
        }
        

        public void Reset2()
        {
            pictureBox6.BorderStyle = BorderStyle.None;
            pictureBox7.BorderStyle = BorderStyle.None;
            pictureBox8.BorderStyle = BorderStyle.None;
            pictureBox9.BorderStyle = BorderStyle.None;
            pictureBox10.BorderStyle = BorderStyle.None;
            label3.Visible = false;
            label4.Visible = false;
            label7.Visible = false;
            textBox1.Visible = false;
            textBox2.Visible = false;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameMode = comboBox1.SelectedItem.ToString();
            Game.mode = GameMode;
            if (GameMode == "Hexagon" && GameSize == "Custom")
                label7.Text = "The maximum is " + (int)(Screen.PrimaryScreen.Bounds.Width - 20 - (GameMode == "Hexagon" ? ((int)(Screen.PrimaryScreen.Bounds.Height - 60) / 32 - 1) * 16 : 0)) / 32 + "x" + (int)(Screen.PrimaryScreen.Bounds.Height - 60) / 32;
            else
                label7.Text = "The maximum is " + (int)(Screen.PrimaryScreen.Bounds.Width - 20 - (GameMode == "Hexagon" ? ((int)(Screen.PrimaryScreen.Bounds.Height - 60) / 32 - 1) * 16 : 0)) / 32 + "x" + (int)(Screen.PrimaryScreen.Bounds.Height - 60) / 32;
            frm.SetSquares(GameMode);
        }
    }
}
