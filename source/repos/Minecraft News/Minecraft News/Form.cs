using CefSharp.WinForms;
using HtmlAgilityPack;
using Minecraft_News.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Minecraft_News
{
    public partial class Form : System.Windows.Forms.Form
    {
        public HtmlWeb Web = new HtmlWeb();
        public Section[] Section = new Section[14];
        public SectionInfo[] SectionInfo = new SectionInfo[1024];
        public static int currentsection = 0, currentsectioninfo = 0, firstsectionvisible = 0, sectionsvisible, maxnews = 25;
        public static bool linksOpenInSystemBrowser = false, sectionscroll;
        public static List<Server> ServerList = new List<Server>();

        public Form()
        {
            InitializeComponent();
            SetSettings();
            SetupSections();
        }

        private void ScrollHandler(object sender, EventArgs e)
        {
            int latestsectionvisible = VerticalScroll.Value / 600;
            if (firstsectionvisible < latestsectionvisible)
            {
                if (latestsectionvisible - firstsectionvisible > sectionsvisible)
                    firstsectionvisible = latestsectionvisible - sectionsvisible - 1;
                for (int i = firstsectionvisible + 1; i <= latestsectionvisible && i + sectionsvisible <= currentsectioninfo; i++)
                {
                    currentsection = (currentsection + 1) % sectionsvisible;
                    int lastsectionvisible = i + sectionsvisible - 1;
                    ShowNews(lastsectionvisible, currentsection);
                    Section[currentsection].SetLocation(lastsectionvisible, this);
                }
                firstsectionvisible = latestsectionvisible;
            }
            else if (firstsectionvisible > latestsectionvisible)
            {
                if (firstsectionvisible - latestsectionvisible > sectionsvisible)
                    firstsectionvisible = latestsectionvisible + sectionsvisible;
                for (int i = firstsectionvisible - 1; i >= latestsectionvisible && i + sectionsvisible < currentsectioninfo; i--)
                {
                    ShowNews(i, currentsection);
                    Section[currentsection].SetLocation(i, this);
                    currentsection = currentsection == 0 ? sectionsvisible - 1 : currentsection - 1;
                }
                firstsectionvisible = latestsectionvisible;
            }
        }

        private void SetupSections()
        {
            sectionsvisible = Screen.PrimaryScreen.Bounds.Height / 600 + 3;
            if (!ServerList.Any())
            {
                AddGUI();
                MessageBox.Show("You aren't following any servers");
                return;
            }
            for (currentsection = 0; currentsection < sectionsvisible; currentsection++)
                Section[currentsection] = new Section(this, currentsection);
            SetNews(ServerList);
            for (int i = 0; i < currentsection; i++)
                ShowNews(i, i);
            currentsection--;
        }

        private void SetNews(List<Server> Servers)
        {
            int i = 0;
            foreach (string News in OrderAllNews(GetAllNews(Servers)))
            {
                i++;
                if (i > maxnews) break;
                GetSectionInformation(News);
            }
            AddGUI();
        }

        private string[,] GetNews(Server Server)
        {
            string[,] news = new string[64, 2];
            int i = 0;
            HtmlAgilityPack.HtmlDocument home = Web.Load(Server.NewsPage);
            HtmlNodeCollection nodes = home.DocumentNode.SelectNodes(Server.NewsLinkNode);
            if (nodes == null)
                return news;
            foreach (HtmlNode node in nodes)
            {
                if (node.Attributes["href"].Value.Contains("/home/")) continue;
                news[i, 0] = node.Attributes["href"].Value;
                if (!node.Attributes["href"].Value.Contains("https"))
                    news[i, 0] = Server.HomePage + news[i, 0];
                i++;
                if (i >= maxnews) break;
            }
            i = 0;
            nodes = home.DocumentNode.SelectNodes(Server.NewsTimeNode);
            foreach (HtmlNode node in nodes)
            {
                if (Server.NewsTimeAttribute == null)
                    news[i, 1] = Convert.ToDateTime(node.InnerText.Replace("at", "")).ToString(@"MM-dd-yyyy");
                else
                    news[i, 1] = Convert.ToDateTime(node.Attributes[Server.NewsTimeAttribute].Value).ToString(@"MM-dd-yyyy");
                i++;
                if (i >= maxnews) return news;
            }
            return news;
        }

        private string[,] GetAllNews(List<Server> ServerList)
        {
            int k = 0;
            string[,] News = new string[1024, 2];
            foreach (Server Server in ServerList)
            {
                int ok = 1;
                if (Server == null)
                    break;
                string[,] temp = GetNews(Server);
                for (int i = 0; i < 1000; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        if (temp[i, j] == null)
                        {
                            ok = 0;
                            break;
                        }
                        News[k, j] = temp[i, j];
                    }
                    if (ok == 0)
                        break;
                    k++;
                }
            }
            return News;
        }

        private string[] OrderAllNews(string[,] News)
        {
            int i;
            for (i = 0; i < 1024; i++)
            {
                if (News[i, 1] == null)
                    break;
                string CurrentLink = News[i, 0], CurrentDate = News[i, 1];
                int j = i - 1;
                while (j >= 0 && DateTime.ParseExact(News[j, 1], @"MM-dd-yyyy", CultureInfo.InvariantCulture) < DateTime.ParseExact(CurrentDate, @"MM-dd-yyyy", CultureInfo.InvariantCulture))
                {
                    News[j + 1, 1] = News[j, 1];
                    News[j + 1, 0] = News[j, 0];
                    j--;
                }
                News[j + 1, 0] = CurrentLink;
                News[j + 1, 1] = CurrentDate;
            }
            string[] OrderedNews = new string[i];
            for (int j = 0; j < i; j++)
            {
                OrderedNews[j] = News[j, 0];
            }
            return OrderedNews;
        }

        private SectionInfo GetSectionInformation(string Link)
        {
            Server Server = DetectServer(Link);
            SectionInfo[currentsectioninfo] = new SectionInfo
            {
                Server = Server.Name,
                Link = Link
            };
            HtmlAgilityPack.HtmlDocument document = Web.Load(Link);
            string title = document.DocumentNode.SelectSingleNode(Server.TitleNode).InnerText;
            string date;
            if (Server.Name == "MCGamer")
            {
                if (document.DocumentNode.SelectSingleNode(@"//*[@id='pageDescription']/a[3]/abbr") != null)
                    date = Convert.ToDateTime(document.DocumentNode.SelectSingleNode(Server.DateNode1).Attributes[Server.DateAttribute].Value).ToString("MMM d, yyyy");
                else
                    date = Convert.ToDateTime(document.DocumentNode.SelectSingleNode(Server.DateNode2).InnerText.Replace("at", "")).ToString("MMM d, yyyy");
            }
            else if (Server.DateAttribute != null)
                Convert.ToDateTime(date = document.DocumentNode.SelectSingleNode(Server.DateNode1).Attributes[Server.DateAttribute].Value).ToString("MMM d, yyyy");
            else
            {
                if (document.DocumentNode.SelectSingleNode(Server.DateNode1) != null)
                    date = Convert.ToDateTime(document.DocumentNode.SelectSingleNode(Server.DateNode1).InnerText.Replace("at", "")).ToString("MMM d, yyyy");
                else
                    date = Convert.ToDateTime(document.DocumentNode.SelectSingleNode(Server.DateNode2).InnerText.Replace("at", "")).ToString("MMM d, yyyy");
            }
            string author = document.DocumentNode.SelectSingleNode(Server.AuthorNode).InnerText.Replace("\n", string.Empty);
            SectionInfo[currentsectioninfo].Title = title.FixTitle();
            SectionInfo[currentsectioninfo].Info = "Posted on " + date + " by " + author;
            SectionInfo[currentsectioninfo].HTML = document.DocumentNode.SelectSingleNode(Server.HTMLNode).InnerHtml.FixHTML();
            currentsectioninfo++;
            return SectionInfo[currentsectioninfo - 1];
        }

        private Server DetectServer(string Link)
        {
            foreach (Server Server in Servers.AllServers)
                if (Link.Contains(Server.HomePage))
                    return Server;
            return null;
        }

        private void ShowNews(int newsnr, int sectionnr)
        {
            Section[sectionnr].Title.Text = SectionInfo[newsnr].Title;
            if (Section[sectionnr].Title.Width > 900)
            {
                while (Section[sectionnr].Title.Width > 900)
                    Section[sectionnr].Title.Text = Section[sectionnr].Title.Text.Truncate(Section[sectionnr].Title.Text.Length - 1);
                Section[sectionnr].Title.Text += "...";
            }
            Section[sectionnr].Info.Text = SectionInfo[newsnr].Info;
            Section[sectionnr].Read.Text = "Continue reading on " + SectionInfo[newsnr].Server;
            Section[sectionnr].Link = SectionInfo[newsnr].Link;
            linksOpenInSystemBrowser = false;
            if (!sectionscroll) SectionInfo[newsnr].HTML = "<style>body{ overflow: hidden; }</style>" + SectionInfo[newsnr].HTML;
            Section[sectionnr].Browser.Load("data:text/html;base64," + Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(SectionInfo[newsnr].HTML)));
            linksOpenInSystemBrowser = true;
        }

        private void AddGUI()
        {
            PictureBox buttom = new PictureBox
            {
                Size = new Size(950, 50),
                Location = new Point(0, currentsectioninfo * 600 + 10),
                BorderStyle = BorderStyle.None,
                BackColor = Color.Navy
            };

            Label top = new Label
            {
                AutoSize = true,
                Font = new Font("Arial Rounded MT Bold", 20F, FontStyle.Bold, GraphicsUnit.Point, 238),
                Location = new Point(0, currentsectioninfo * 600 + 15),
                BackColor = Color.Navy,
                Text = "Top"
            };
            PictureBox settings = new PictureBox
            {
                Size = new Size(30, 30),
                Location = new Point(1000, 30),
                BorderStyle = BorderStyle.None,
                BackgroundImage = Resources.Settings,
                BackgroundImageLayout = ImageLayout.Stretch
            };
            settings.Click += (object sender, EventArgs e) =>
            {
                Settings s = new Settings();
                s.ShowDialog();
            };
            top.MouseDown += (object sender, MouseEventArgs e) => { VerticalScroll.Value = 0; VerticalScroll.Value = 0; };
            Controls.Add(top);
            Controls.Add(buttom);
            Controls.Add(settings);
        }

        private void SetSettings()
        {
            foreach (string Server in Properties.Settings.Default.Servers)
                foreach (Server RealServer in Servers.AllServers)
                    if (RealServer.ID == Server)
                        ServerList.Add(RealServer);
            sectionscroll = Properties.Settings.Default.SectionScroll;
            maxnews = Properties.Settings.Default.MaxNews;
        }

    }

    public static class StringOperations
    {
        public static string Truncate(this string value, int maxLength)
        {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }

        public static string FixTitle(this string value)
        {
            value = value.Replace("amp;", "&");
            value = value.Replace("#039;", "'");
            value = value.Replace("quot;", "\"");
            value = value.Replace("Important&nbsp;", "");
            value = value.Replace("Official&nbsp;", "");
            return value;
        }
        public static string FixHTML(this string value)
        {
            value = value.Replace("title=\"Click to reveal or hide spoiler\"", "data-title=\"Click to reveal or hide spoiler\" value=\"Show\" onclick=\"this.value=this.value=='Show'?'Hide':'Show';\"");
            value = Resources.CSS + value;
            value = value.Replace("alt=\":mc1:\"", "");
            value = value.Replace("alt=\":mc2:\"", "");
            value = value.Replace("alt=\":mc3:\"", "");
            value = value.Replace("alt=\":mc4:\"", "");
            value = value.Replace("alt=\":mc5:\"", "");
            value = value.Replace("styles/default/xenforo/clear.png", "");
            value = value.Replace("<img src=\"\" class=\"mceSmilieSprite mceSmilie1\" alt=\"", "");
            value = value.Replace("\" title=\"Smile    :)\">", "");
            value = value.Replace("<img src=\"\" class=\"mceSmilieSprite mceSmilie2\" alt=\"", "");
            value = value.Replace("\" title=\"Wink    ;)\">", "");
            value = value.Replace("&amp;", "&");
            value = value.Replace("/proxy.php?image=", "https://www.vanitymc.co/proxy.php?image=");
            value = value.Replace(" <div", "<div");
            value = value.Replace(" <li", "<li");
            value = value.Replace(" <br> ", "<br>");
            value = value.Replace("target=\"_blank\"", "");
            value = value.Replace("class=\"link link--external\"", "");
            value = value.Replace("class=\"bbImage\"", "");
            value = value.Replace("font-size: ", "font-size:");
            value = value.Replace("margin-left: ", "margin-left:");
            value = value.Replace("text-decoration: ", "text-decoration:");
            value = value.Replace("text-align: ", "text-align:");
            value = value.Replace("class=\"lbContainer lbContainer--inline\"", "");
            value = Regex.Replace(value, @"\s+", " ");
            return value;
        }
    }

    public class Section
    {
        public Label Title = new Label
        {
            AutoSize = true,
            Font = new Font("Arial Rounded MT Bold", 36F, FontStyle.Bold, GraphicsUnit.Point, 238),
            BackColor = Color.Navy
        };
        public Label Info = new Label
        {
            AutoSize = true,
            Font = new Font("Arial Rounded MT Bold", 15.75F, FontStyle.Regular, GraphicsUnit.Point, 238),
            BackColor = Color.Navy
        };
        public Label Read = new Label
        {
            AutoSize = true,
            Font = new Font("Arial Rounded MT Bold", 20.25F, FontStyle.Regular, GraphicsUnit.Point, 238),
            BackColor = Color.Blue
        };
        public PictureBox BackgroundInfo = new PictureBox
        {
            BorderStyle = BorderStyle.None,
            BackColor = Color.Navy,
            Size = new Size(950, 95)
        };
        public PictureBox BackgroundContent = new PictureBox
        {
            BorderStyle = BorderStyle.None,
            BackColor = Color.Blue,
            Size = new Size(950, 50)
        };
#pragma warning disable CS0618
        public ChromiumWebBrowser Browser = new ChromiumWebBrowser
        {
            Size = new Size(950, 450),
            AllowDrop = false,
            TabStop = false,
        };

        public string Link;

        public Section(System.Windows.Forms.Form Form1, int nr)
        {
            Title.Location = new Point(-2, nr * 600 + 10);
            Info.Location = new Point(4, nr * 600 + 64);
            Read.Location = new Point(2, nr * 600 + 555);
            BackgroundInfo.Location = new Point(0, nr * 600);
            BackgroundContent.Location = new Point(0, nr * 600 + 545);
            Browser.Location = new Point(0, nr * 600 + 95);
            Browser.RequestHandler = new CustomIRequestHandler();
            Browser.LifeSpanHandler = new CustomILifeSpanHandler();
            Browser.MenuHandler = new CustomIContextMenuHandler();
            Read.MouseDown += (object sender, MouseEventArgs e) => { Process.Start(Link); };
            Form1.Controls.Add(Title);
            Form1.Controls.Add(Info);
            Form1.Controls.Add(Read);
            Form1.Controls.Add(BackgroundContent);
            Form1.Controls.Add(BackgroundInfo);
            Form1.Controls.Add(Browser);
        }

        public void SetLocation(int nr, Form form)
        {
            Title.Location = new Point(-2, nr * 600 + 10 - form.VerticalScroll.Value);
            Info.Location = new Point(4, nr * 600 + 64 - form.VerticalScroll.Value);
            Read.Location = new Point(2, nr * 600 + 555 - form.VerticalScroll.Value);
            BackgroundInfo.Location = new Point(0, nr * 600 - form.VerticalScroll.Value);
            BackgroundContent.Location = new Point(0, nr * 600 + 545 - form.VerticalScroll.Value);
            Browser.Location = new Point(0, nr * 600 + 95 - form.VerticalScroll.Value);
            Browser.TabStop = false;
        }

    }

    public class SectionInfo
    {
        public string Link, Server, Title, Info, HTML;
    }

    public class Server
    {
        public string ID, Name, HomePage, NewsPage, NewsLinkNode, NewsTimeNode, NewsTimeAttribute, TitleNode, DateNode1, DateNode2, DateAttribute, AuthorNode, HTMLNode;
        public Server()
        {
            Servers.AllServers.Add(this);
        }
    }
}
