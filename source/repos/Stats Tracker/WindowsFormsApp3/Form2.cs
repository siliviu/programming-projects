using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class Form2 : Form
    {
        string text = "";
        double wins, matches, kills, kd, km;

        Form1 frm = null;

        public Form2(Form1 frm)
        {
            InitializeComponent();
            this.frm = frm;
            comboBox1.SelectedItem = "Winrate";
            comboBox2.SelectedItem = "Overall";
            checkBox1.Checked = true;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(textBox1.Text, out double a) && !(textBox1.Text.Length == 1 && textBox1.Text == ".") && !(textBox1.Text == ""))
            {
                textBox1.Text = text;
                textBox1.SelectionStart = textBox1.Text.Length;
            }
            else
            {
                text = textBox1.Text;
                Calculate();
            }

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(textBox2.Text, out double a) && !(textBox2.Text.Length == 1 && textBox2.Text == ".") && !(textBox2.Text == ""))
            {
                textBox2.Text = text;
                textBox2.SelectionStart = textBox2.Text.Length;
            }
            else
            {
                text = textBox2.Text;
                Calculate();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            if (!double.TryParse(textBox4.Text, out double a) && !(textBox4.Text.Length == 1 && textBox4.Text == ".") && !(textBox4.Text == ""))
            {
                textBox4.Text = text;
                textBox4.SelectionStart = textBox4.Text.Length;
            }
            else
            {
                text = textBox4.Text;
                Calculate();
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                checkBox2.Checked = false;
                textBox4.Visible = false;
            }
            if (checkBox1.Checked == false)
                checkBox2.Checked = true;
            Calculate();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                checkBox1.Checked = false;
                textBox4.Visible = true;
            }
            if (checkBox2.Checked == false)
                checkBox1.Checked = true;
            Calculate();
        }

        private void GetValues()
        {
            if (frm.label11.Text == "NUMBER") return;
            switch (comboBox2.SelectedItem)
            {
                case "Overall":
                    kills = Convert.ToDouble(frm.label12.Text);
                    kd = Convert.ToDouble(frm.label13.Text);
                    km = Convert.ToDouble(frm.label14.Text);
                    wins = Convert.ToDouble(frm.label10.Text);
                    matches = Convert.ToDouble(frm.label9.Text);
                    break;
                case "Solos":
                    kills = Convert.ToDouble(frm.label17.Text);
                    kd = Convert.ToDouble(frm.label16.Text);
                    km = Convert.ToDouble(frm.label15.Text);
                    wins = Convert.ToDouble(frm.label19.Text);
                    matches = Convert.ToDouble(frm.label20.Text);
                    break;
                case "Duos":
                    kills = Convert.ToDouble(frm.label31.Text);
                    kd = Convert.ToDouble(frm.label30.Text);
                    km = Convert.ToDouble(frm.label29.Text);
                    wins = Convert.ToDouble(frm.label33.Text);
                    matches = Convert.ToDouble(frm.label34.Text);
                    break;
                case "Squads":
                    kills = Convert.ToDouble(frm.label44.Text);
                    kd = Convert.ToDouble(frm.label43.Text);
                    km = Convert.ToDouble(frm.label42.Text);
                    wins = Convert.ToDouble(frm.label46.Text);
                    matches = Convert.ToDouble(frm.label47.Text);
                    break;
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Calculate();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch(comboBox1.SelectedItem)
            {
                case "Winrate":
                    label1.Text = "The winrate you would like to have";
                    label2.Text = "Your current average winrate";
                    label4.Visible = false;
                    checkBox1.Visible = false;
                    checkBox2.Visible = false;
                    textBox4.Visible = false;
                    break;
                case "K/D":
                    label1.Text = "The k/d ratio you would like to have";
                    label2.Text = "Your current k/d ratio average";
                    label4.Visible = true;
                    checkBox1.Visible = true;
                    checkBox2.Visible = true;
                    textBox4.Visible = true;
                    break;
                case "K/M":
                    label1.Text = "The k/m ratio you would like to have";
                    label2.Text = "Your current k/m ratio average";
                    label4.Visible = false;
                    checkBox1.Visible = false;
                    checkBox2.Visible = false;
                    textBox4.Visible = false;
                    break;
            }
            Calculate();
        }

        private void Calculate()
        {
            if (textBox1.Text == "" || textBox2.Text == "" || ( checkBox1.Visible == true && checkBox1.Checked == false && textBox4.Text == "")) return;
            GetValues();
            switch (comboBox1.SelectedItem)
            {
                case "Winrate":
                    double desiredwinrate = Convert.ToDouble(textBox1.Text) / 100, currentwinrate = Convert.ToDouble(textBox2.Text) / 100;
                    textBox3.Text = Math.Ceiling((desiredwinrate * matches - wins) / (currentwinrate - desiredwinrate)).ToString();
                    break;
                case "K/D":
                    double desiredkd = Convert.ToDouble(textBox1.Text), currentkd = Convert.ToDouble(textBox2.Text), cwinrate;
                    if (checkBox1.Checked)
                        cwinrate = wins / matches;
                    else
                        cwinrate = Convert.ToDouble(textBox4.Text)/100;
                    textBox3.Text = Math.Ceiling((desiredkd*(matches-wins)-kills)/((1-cwinrate)*(currentkd-desiredkd))).ToString();
                    break;
                case "K/M":
                    double desiredkm = Convert.ToDouble(textBox1.Text), currentkm = Convert.ToDouble(textBox2.Text);
                    textBox3.Text = Math.Ceiling((desiredkm * matches - kills) / (currentkm - desiredkm)).ToString();
                    break;
            }
        }
    }
}
