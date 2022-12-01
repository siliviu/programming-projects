using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using HtmlAgilityPack;

namespace WindowsFormsApp3
{
    public partial class Form1 : Form
    {
        string username, profilepage;
        Form1[] frm = new Form1[10];
        HtmlWeb web = new HtmlWeb();
        HtmlAgilityPack.HtmlDocument overall;
        int current = 0;

        public Form1()
        {
            InitializeComponent();
            textBox1.KeyDown += (object sender1, KeyEventArgs e1) => Compare(sender1, e1);
            pictureBox1.Click += (object sender1, EventArgs e1) => Compare(sender1, null);
        }

        private void textBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) Search();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Search();
        }


        private void Search()
        {
            username = textBox1.Text;
            profilepage = String.Format("https://www.fortbuff.com/players/{0}", Uri.EscapeUriString(username));
            overall = web.Load(profilepage);
            double wins, matches, kills;
            if (overall.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[1]/td[2]") == null)
            {
                MessageBox.Show("User could not be found");
                return;
            }
            username = overall.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[1]/div/main/header/h1").InnerText;
            label1.Text = username;
            matches = Convert.ToDouble(overall.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[1]/td[2]").InnerText);
            wins = Convert.ToDouble(overall.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[8]/td[2]").InnerText);
            kills = Convert.ToDouble(overall.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[4]/td[2]").InnerText);
            label9.Text = matches.ToString();
            label10.Text = wins.ToString();
            label11.Text = string.Format("{0:F2}", 100 * wins / matches) + "%";
            label12.Text = kills.ToString();
            label13.Text = string.Format("{0:F2}", kills / (matches - wins));
            label14.Text = string.Format("{0:F2}", kills / matches);
            var solo = web.Load(profilepage + "?mode=solo");
            if (solo.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[1]/td[2]") == null)
            {
                label15.Text = "0";
                label16.Text = "0";
                label17.Text = "0";
                label18.Text = "0";
                label19.Text = "0";
                label20.Text = "0";
            }
            else
            {
                matches = Convert.ToDouble(solo.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[1]/td[2]").InnerText);
                wins = Convert.ToDouble(solo.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[8]/td[2]").InnerText);
                kills = Convert.ToDouble(solo.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[4]/td[2]").InnerText);
                label20.Text = matches.ToString();
                label19.Text = wins.ToString();
                label18.Text = string.Format("{0:F2}", 100 * wins / matches) + "%";
                label17.Text = kills.ToString();
                label16.Text = string.Format("{0:F2}", kills / (matches - wins));
                label15.Text = string.Format("{0:F2}", kills / matches);
            }
            var duos = web.Load(profilepage + "?mode=duo");
            if (duos.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[1]/td[2]") == null)
            {
                label34.Text = "0";
                label33.Text = "0";
                label32.Text = "0";
                label31.Text = "0";
                label30.Text = "0";
                label29.Text = "0";
            }
            else
            {
                matches = Convert.ToDouble(duos.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[1]/td[2]").InnerText);
                wins = Convert.ToDouble(duos.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[8]/td[2]").InnerText);
                kills = Convert.ToDouble(duos.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[4]/td[2]").InnerText);
                label34.Text = matches.ToString();
                label33.Text = wins.ToString();
                label32.Text = string.Format("{0:F2}", 100 * wins / matches) + "%";
                label31.Text = kills.ToString();
                label30.Text = string.Format("{0:F2}", kills / (matches - wins));
                label29.Text = string.Format("{0:F2}", kills / matches);
            }
            var squads = web.Load(profilepage + "?mode=squad");
            if (squads.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[1]/td[2]") == null)
            {
                label47.Text = "0";
                label46.Text = "0";
                label45.Text = "0";
                label44.Text = "0";
                label43.Text = "0";
                label42.Text = "0";
            }
            else
            {
                matches = Convert.ToDouble(squads.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[1]/td[2]").InnerText);
                wins = Convert.ToDouble(squads.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[8]/td[2]").InnerText);
                kills = Convert.ToDouble(squads.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[2]/main/aside/section/article/table/tbody/tr[4]/td[2]").InnerText);
                label47.Text = matches.ToString();
                label46.Text = wins.ToString();
                label45.Text = string.Format("{0:F2}", 100 * wins / matches) + "%";
                label44.Text = kills.ToString();
                label43.Text = string.Format("{0:F2}", kills / (matches - wins));
                label42.Text = string.Format("{0:F2}", kills / matches);
            }
            if (overall.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[1]/div/main/header/aside/div[5]/time") != null)
                label55.Text = overall.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[1]/div/main/header/aside/div[5]/time").InnerText;
            else if (overall.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[1]/div/main/header/aside/div[4]/time") != null)
                label55.Text = overall.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[1]/div/main/header/aside/div[4]/time").InnerText;
            else
                label55.Text = overall.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[1]/div/main/header/aside/div[3]/time").InnerText;
            if (overall.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[1]/div/main/header/small/time") != null)
                label56.Text = overall.DocumentNode.SelectSingleNode(@"//*[@id='app']/div/div[2]/div/div/div[1]/div/main/header/small/time").InnerText;
            else
                label56.Text = "now";
                
            MakeVisible();
        }

        private void MakeVisible()
        {
            foreach (Control Control in Controls)
            {
                if (Control.Name == "button1") continue;
                Control.Visible = true;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            if (label1.Text == "IGN")   return;
            string[] file =
            {
                "Username : " + username,
                "",
                "",
                "Overall ",
                "",
                "Matches : " + label9.Text,
                "Wins : " + label10.Text,
                "Winrate : " + label11.Text,
                "Kills : " + label12.Text,
                "K/D : " + label13.Text,
                "K/M : " + label14.Text,
                "",
                "Solo ",
                "",
                "Matches : " + label20.Text,
                "Wins : " + label19.Text,
                "Winrate : " + label18.Text,
                "Kills : " + label17.Text,
                "K/D : " + label16.Text,
                "K/M : " + label15.Text,
                "",
                "Duos ",
                "",
                "Matches : " + label34.Text,
                "Wins : " + label33.Text,
                "Winrate : " + label32.Text,
                "Kills : " + label31.Text,
                "K/D : " + label30.Text,
                "K/M : " + label29.Text,
                "",
                "Squads ",
                "",
                "Matches : " + label47.Text,
                "Wins : " + label46.Text,
                "Winrate : " + label45.Text,
                "Kills : " + label44.Text,
                "K/D : " + label43.Text,
                "K/M : " + label42.Text,
            };
            string date = DateTime.Now.ToString("yy-MM-dd hh.mm");
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\" + "Saves");
            File.WriteAllLines(AppDomain.CurrentDomain.BaseDirectory+@"\"+"Saves"+@"\"+username + " - " + date +".txt", file);
            MessageBox.Show("Statistics have successfully been saved");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(current >= 10)
            {
                MessageBox.Show("You can't open more than 10 tabs!");
                return;
            }
            frm[current] = new Form1();
            frm[current].Show();
            frm[current].button1.Visible = false;
            frm[current].textBox1.KeyDown += (object sender1, KeyEventArgs e1) => Compare(sender1, e1);
            frm[current].pictureBox1.Click += (object sender1, EventArgs e1) => Compare(sender1, null);
            current++;
        }

        private void Compare(object sender, KeyEventArgs e)
        {
            if ((e != null && e.KeyCode != Keys.Enter) || current == 0) return;
            try
            {
                CompareStats(label9);
                CompareStats(label10);
                CompareStats(label11);
                CompareStats(label12);
                CompareStats(label13);
                CompareStats(label14);
                CompareStats(label20);
                CompareStats(label19);
                CompareStats(label18);
                CompareStats(label17);
                CompareStats(label16);
                CompareStats(label15);
                CompareStats(label34);
                CompareStats(label33);
                CompareStats(label32);
                CompareStats(label31);
                CompareStats(label30);
                CompareStats(label29);
                CompareStats(label47);
                CompareStats(label46);
                CompareStats(label45);
                CompareStats(label44);
                CompareStats(label43);
                CompareStats(label42);
            }
            catch (Exception) { }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (label1.Text == "IGN") return;
            Form2 frm = new Form2(this);
            frm.Show();
        }

        private void CompareStats(Control control)
        {
            double max;
            if (control == label11 || control == label18 || control == label32 || control == label45)
            {
                max = Convert.ToDouble(control.Text.TrimEnd(new char[] { '%', ' ' }));
                control.ForeColor = Color.Black;
                int currentmaxi = 0;
                int[] maxi = new int[10];
                for (int i = 0; i < current; i++)
                    foreach (Control control1 in frm[i].Controls)
                    {
                        if (control1.Name != control.Name) continue;
                        control1.ForeColor = Color.Black;
                        if (Convert.ToDouble(control1.Text.TrimEnd(new char[] { '%', ' ' })) > max)
                        {
                            currentmaxi = 0;
                            max = Convert.ToDouble(control1.Text.TrimEnd(new char[] { '%', ' ' }));
                            maxi[currentmaxi] = i;
                            currentmaxi++;
                        }
                        else if (Convert.ToDouble(control1.Text.TrimEnd(new char[] { '%', ' ' })) == max)
                        {
                            maxi[currentmaxi] = i;
                            currentmaxi++;
                        }
                    }
                for (int i = 0; i < currentmaxi; i++)
                    foreach (Control control1 in frm[maxi[i]].Controls)
                    {
                        if (control1.Name != control.Name) continue;
                        control1.ForeColor = Color.Red;
                    }
                if (max == Convert.ToDouble(control.Text.TrimEnd(new char[] { '%', ' ' })))
                    control.ForeColor = Color.Red;
            }
            else
            {
                max = Convert.ToDouble(control.Text);
                control.ForeColor = Color.Black;
                int currentmaxi = 0;
                int[] maxi = new int[10];
                for (int i = 0; i < current; i++)
                    foreach (Control control1 in frm[i].Controls)
                    {
                        if (control1.Name != control.Name) continue;
                        control1.ForeColor = Color.Black;
                        if (Convert.ToDouble(control1.Text) > max)
                        {
                            currentmaxi = 0;
                            max = Convert.ToDouble(control1.Text);
                            maxi[currentmaxi] = i;
                            currentmaxi++;
                        }
                        else if (Convert.ToDouble(control1.Text) == max)
                        {
                            maxi[currentmaxi] = i;
                            currentmaxi++;
                        }
                    }
                for (int i = 0; i < currentmaxi; i++)
                    foreach (Control control1 in frm[maxi[i]].Controls)
                    {
                        if (control1.Name != control.Name) continue;
                        control1.ForeColor = Color.Red;
                    }
                if (max == Convert.ToDouble(control.Text))
                    control.ForeColor = Color.Red;
            }
           
        }
    }
}