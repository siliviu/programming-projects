using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;


namespace WindowsFormsApp1
{
    public partial class Settings : Form
    {
        Form1 frm = null;
        public Settings(Form1 frm)
        {
            this.frm = frm;
            InitializeComponent();
            checkBox1.Checked = Program.showWarnings;
            checkBox2.Checked = Program.resetValues;
            comboBox1.Text = Program.currencySymbol;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                Program.showWarnings = true;
                Program.UpdateSetting("showWarnings", "true");
            }
            else
            {
                Program.showWarnings = false;
                Program.UpdateSetting("showWarnings", "false");
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                Program.resetValues = true;
                Program.UpdateSetting("resetValues", "true");
            }
            else
            {
                Program.resetValues = false;
                Program.UpdateSetting("resetValues", "false");
            }

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text == "Left")
            {
                Program.currencySymbol = "Left";
                Program.UpdateSetting("currencySymbol", "Left");
                frm.SwapPositionsLeft();
            }
            else
            {
                Program.currencySymbol = "Right";
                Program.UpdateSetting("currencySymbol", "Right");
                frm.SwapPositionsRight();
            }
        }
    }
}
