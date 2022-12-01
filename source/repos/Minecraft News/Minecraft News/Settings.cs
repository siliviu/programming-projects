using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace Minecraft_News
{
    public partial class Settings : System.Windows.Forms.Form
    {
        public Settings()
        {
            InitializeComponent();
            int i = 0;
            Label SettingsText = new Label
            {
                AutoSize = true,
                Location = new Point(10, 10),
                Font = new Font("Arial Rounded MT Bold", 36F, FontStyle.Bold, GraphicsUnit.Point, 238),
                Text = "Settings"

            };
            Label ServersText = new Label
            {
                AutoSize = true,
                Location = new Point(10, 100),
                Font = new Font("Arial Rounded MT Bold", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 238),
                Text = "Servers"
            };
            Label OthersText = new Label
            {
                AutoSize = true,
                Location = new Point(410, 100),
                Font = new Font("Arial Rounded MT Bold", 20.25F, FontStyle.Bold, GraphicsUnit.Point, 238),
                Text = "Others"
            };
            CheckBox SectionScroll = new CheckBox
            {
                AutoSize = true,
                Text = "Section scroll",
                Location = new Point(410, 150)
            };
            Label MaximumNews = new Label
            {
                AutoSize = true,
                Text = "Maximum News",
                Location = new Point(410, 180)
            };
            ComboBox ListNumbers = new ComboBox
            {
                Size = new Size(35, 25),
                Location = new Point(500, 175),
            };
            SectionScroll.Checked = Form.sectionscroll;
            SectionScroll.CheckedChanged += (object sender, EventArgs e) => { Form.sectionscroll = !Form.sectionscroll; };
            ListNumbers.Items.AddRange(new object[] { 10, 15, 20, 25, 30, 35, 40, 45, 50 });
            ListNumbers.SelectedIndex = Form.maxnews / 5 - 2;
            ListNumbers.SelectedValueChanged += (object sender, EventArgs e) => { Form.maxnews = (int)ListNumbers.SelectedItem; };
            Controls.Add(SettingsText);
            Controls.Add(ServersText);
            Controls.Add(OthersText);
            Controls.Add(SectionScroll);
            Controls.Add(MaximumNews);
            Controls.Add(ListNumbers);
            CheckBox[] Checkbox = new CheckBox[32];
            foreach (Server Server in Servers.AllServers)
            {
                Checkbox[i] = new CheckBox
                {
                    AutoSize = true,
                    Text = Server.Name,
                    Location = new Point(10, i * 30 + 150),
                };
                foreach (Server ServerListed in Form.ServerList)
                {
                    if (Server == ServerListed)
                        Checkbox[i].Checked = true;
                }
                int j = i;
                Checkbox[i].CheckedChanged += (object sender, EventArgs e) =>
                {
                    if (Checkbox[j].Checked)
                        Form.ServerList.Add(Server);
                    else
                        Form.ServerList.Remove(Server);
                };
                Controls.Add(Checkbox[i]);
                i++;
            }
            FormClosed += (object sender, FormClosedEventArgs e) =>
            {
                List<string> ConvertedList = new List<string>();
                foreach (Server Server in Form.ServerList)
                    ConvertedList.Add(Server.ID);
                Properties.Settings.Default.Servers = ConvertedList;
                Properties.Settings.Default.SectionScroll = Form.sectionscroll;
                Properties.Settings.Default.MaxNews = Form.maxnews;
                Properties.Settings.Default.Save();
            };

        }
    }
}
