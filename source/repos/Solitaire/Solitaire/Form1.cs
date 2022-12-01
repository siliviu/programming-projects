using System;
using System.Collections.Generic;
using System.Drawing;
using System.Media;
using System.Windows.Forms;

namespace Solitaire
{
    public partial class Form1 : Form
    {
        public List<Card> Deck = new List<Card>(), Stock = new List<Card>();
        public Card[] CurrentStock = new Card[4];
        public List<int> AvailableCards = new List<int>();
        public int currentstock = -1, scale = 200, time = 0;
        private string[,] Cards = new string[55, 3];
        public Random Rnd = new Random();
        public Timer t = new Timer();
        public SoundPlayer music = new SoundPlayer(Properties.Resources.card);
        public Form1()
        {
            InitializeComponent();
            SetupGame();
        }
        public void SetupGame()
        {
            Width = 1920;
            Height = 1080;
            Button Reset = new Button();
            Label timed = new Label();
            Controls.Add(Reset);
            Controls.Add(timed);
            Reset.Text = "Reset";
            Reset.MouseClick += (object sender, MouseEventArgs e) => { EndGame(); SetupTable(); };
            timed.Location = new Point(100, 0);
            timed.AutoSize = true;
            t.Enabled = false;
            SetupTable();
            t.Interval = 1000;
            t.Tick += (object sender, EventArgs e) => { timed.Text = string.Format("{0:D2}:{1:D2}", ++time / 60, time % 60); };
        }
        public void SetupTable()
        {
            t.Enabled = true;
            for (int i = 0; i < 4; ++i)
                for (int j = 0; j < 13; ++j)
                {
                    Cards[13 * i + j, 0] = (i == 0) ? "H" : (i == 1) ? "D" : (i == 2) ? "C" : "S";
                    Cards[13 * i + j, 1] = (j + 1).ToString();
                    AvailableCards.Add(13 * i + j);
                }
            for (int i = 0; i < 7; ++i)
            {
                Deck.Add(new Card(this, 2 * scale + 3 * scale / 4 * i, scale / 4, 14, "N", true, false));
                for (int j = 0; j <= i; ++j)
                {
                    int currentcard = PickCard();
                    Deck.Add(new Card(this, 2 * scale + 3 * scale / 4 * i, scale / 4 + scale / 4 * j, int.Parse(Cards[currentcard, 1]), Cards[currentcard, 0], i == j, i == j));
                    Deck[Deck.Count - 2].LinkedCard = Deck[Deck.Count - 1];
                    Deck[Deck.Count - 1].LinkParent = Deck[Deck.Count - 2];
                    Deck[Deck.Count - 1].BringToFront();
                }
            }
            for (int i = 0; i < 4; ++i)
                Deck.Add(new Card(this, Width - scale, scale / 4 + 5 * scale / 4 * i, 0, (i == 0) ? "H" : (i == 1) ? "D" : (i == 2) ? "C" : "S", true, false));
            Deck.Add(new Card(this, (int)(0.35 * scale), scale / 4, 0, "0", false, false));
            for (int i = 0; i < 24; ++i)
            {
                int currentcard = PickCard();
                Card TempCard = new Card(this, 0, 0, int.Parse(Cards[currentcard, 1]), Cards[currentcard, 0], true, false)
                {
                    Enabled = false,
                    Visible = false
                };
                Deck.Add(TempCard);
                Stock.Add(TempCard);
            }
        }
        public int PickCard()
        {
            int value = Rnd.Next(0, AvailableCards.Count), temp = AvailableCards[value];
            AvailableCards.RemoveAt(value);
            return temp;
        }
        public void EndGame()
        {
            t.Enabled = false;
            time = 0;
            currentstock = -1;
            for (int i = 0; i < Deck.Count; ++i)
                Deck[i].Dispose();
            Deck = new List<Card>();
            Stock = new List<Card>();
            CurrentStock = new Card[4];
            AvailableCards = new List<int>();
            Cards = new string[55, 3];
        }
        public void WinGame()
        {
            if (!GameIsOver()) return;
            int temptime = time;
            EndGame();
            MessageBox.Show(string.Format("Congratulations for beating the game! Your time is  {0:D2}:{1:D2}", temptime / 60, temptime % 60));
        }
        public bool GameIsOver()
        {
            int nr = 0;
            foreach (Card Card in Deck)
                if (Card.Active && Card.FaceUp)
                    nr++;
            return (nr == 52);
        }
    }
    public class Card : PictureBox
    {
        public int scale = 200, Value;
        public string Symbol, Colour;
        public bool _FaceUp, Active, Moving = false;
        public Point InitialLocation, TempLocation;
        public Form1 Form1;
        public Card LinkedCard, LinkParent;
        public bool FaceUp
        {
            get { return _FaceUp; }
            set
            {
                _FaceUp = value;
                BackgroundImage = value ? (Image)Properties.Resources.ResourceManager.GetObject(Value + Symbol, Properties.Resources.Culture) : Properties.Resources.Back;
            }
        }
        public Card(Form form, int x, int y, int value, string symbol, bool faceup, bool active)
        {
            Location = new Point(x, y);
            Value = value;
            Symbol = symbol;
            Colour = ("HD".Contains(symbol)) ? "Red" : "Black";
            BackgroundImageLayout = ImageLayout.Stretch;
            FaceUp = faceup;
            Active = active;
            form.Controls.Add(this);
            Form1 = (Form1)form;
            Size = new Size((int)(scale * 0.65), scale);
            BorderStyle = BorderStyle.None;
            BackColor = Color.Transparent;
            MouseClick += (object sender, MouseEventArgs e) => ClickHandler(e);
            MouseDown += (object sender, MouseEventArgs e) => MovementHandler1(e);
            MouseMove += (object sender, MouseEventArgs e) => MovementHandler2(e);
            MouseUp += (object sender, MouseEventArgs e) => MovementHandler3();
        }
        private void ClickHandler(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (Symbol != "0") return;
                Form1.music.Play();
                for (int i = 0; i < 3; ++i)
                {
                    if (Form1.CurrentStock[i] == null) continue;
                    Form1.CurrentStock[i].Visible = false;
                    Form1.CurrentStock[i].Enabled = false;
                    Form1.CurrentStock[i].Active = false;
                    Form1.CurrentStock[i] = null;
                }
                if (Form1.currentstock + 4 > Form1.Stock.Count)
                    Form1.currentstock -= Form1.currentstock + 4 - Form1.Stock.Count;
                if (Form1.currentstock < -1)
                    Form1.currentstock = -1;
                for (int i = 0; i < 3; ++i)
                {
                    if (++Form1.currentstock >= Form1.Stock.Count) continue;
                    Card CurrentCard = Form1.Stock[Form1.currentstock];
                    CurrentCard.Visible = true;
                    CurrentCard.Enabled = true;
                    CurrentCard.Location = new Point((int)(0.35 * scale), 3 * scale / 2 + scale / 4 * i);
                    CurrentCard.BringToFront();
                    Form1.CurrentStock[i] = CurrentCard;
                    if (i == 2)
                        Form1.CurrentStock[i].Active = true;
                    if (Form1.currentstock == Form1.Stock.Count - 1)
                        Form1.currentstock = -1;
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (Moving || !Active || LinkedCard != null) return;
                Card TempCard = Form1.Deck[35 + ((Symbol == "H") ? 0 : (Symbol == "D") ? 1 : (Symbol == "C") ? 2 : 3)];
                while (TempCard.LinkedCard != null)
                    TempCard = TempCard.LinkedCard;
                if (Value - TempCard.Value != 1) return;
                HandleMove(TempCard);
                UpdateStock();
                BringToFront();
                Form1.WinGame();
            }
        }
        private void HandleMove(Card Card)
        {
            Form1.music.Play();
            if (LinkParent != null)
            {
                if (!LinkParent.Active && LinkParent.Value != 0 && LinkParent.Value != 14)
                    LinkParent.Active = true;
                if (!LinkParent.FaceUp)
                    LinkParent.FaceUp = true;
                LinkParent.LinkedCard = null;
            }
            Card.LinkedCard = this;
            LinkParent = Card;
            Location = Card.Location;
            if (!Card.IsLinkedUpwards(Form1.Deck[35]) && !Card.IsLinkedUpwards(Form1.Deck[36]) && !Card.IsLinkedUpwards(Form1.Deck[37]) && !Card.IsLinkedUpwards(Form1.Deck[38]) && Card.Value != 14)
                Top += scale / 4;
        }
        private void UpdateStock()
        {
            if (Form1.CurrentStock[2] != this) return;
            Form1.CurrentStock[2] = null;
            for (int i = 3 - 2; i >= 0; --i)
                if (Form1.CurrentStock[i] != null)
                {
                    Form1.CurrentStock[i + 1] = Form1.CurrentStock[i];
                    Form1.CurrentStock[i + 1].Location = new Point((int)(0.35 * scale), 3 * scale / 2 + scale * (i + 1) / 4);
                    if (i + 1 == 3 - 1) Form1.CurrentStock[i + 1].Active = true;
                    Form1.CurrentStock[i] = null;
                }
            if (Form1.Stock.IndexOf(this) >= 3)
            {
                Card CurrentCard = Form1.Stock[Form1.Stock.IndexOf(this) - 3];
                CurrentCard.Enabled = true;
                CurrentCard.Visible = true;
                CurrentCard.Location = new Point((int)(0.35 * scale), 3 * scale / 2);
                Form1.CurrentStock[0] = CurrentCard;
            }
            Form1.Stock.Remove(this);
            for (int i = 0; i < 3; ++i)
                if (Form1.CurrentStock[i] != null)
                    Form1.CurrentStock[i].BringToFront();
        }
        private void MovementHandler1(MouseEventArgs e)
        {
            if (!Active || e.Button != MouseButtons.Left || Moving) return;
            InitialLocation = Location;
            Moving = true;
            TempLocation = e.Location;
        }
        private void MovementHandler2(MouseEventArgs e)
        {
            if (!Active || !Moving || e.Button != MouseButtons.Left || !Moving) return;
            Left += e.X - TempLocation.X;
            Top += e.Y - TempLocation.Y;
            BringToFront();
            Card Tempcard = LinkedCard;
            while (Tempcard != null)
            {
                Tempcard.Left += e.X - TempLocation.X;
                Tempcard.Top += e.Y - TempLocation.Y;
                Tempcard.BringToFront();
                Tempcard = Tempcard.LinkedCard;
            }
            foreach (Card Card in Form1.Deck)
                if (Math.Abs(Card.Left - Left) <= scale && Math.Abs(Card.Top - Top) <= scale)
                    Card.Refresh();
        }
        private void MovementHandler3()
        {
            if (!Active || !Moving) return;
            Moving = false;
            Card NearestCard = FindNearestCard();
            if (IsCompatibleWith(NearestCard))
            {
                HandleMove(NearestCard);
                UpdateStock();
                ArrangeCards();
            }
            else
            {
                Location = InitialLocation;
                ArrangeCards();
            }
            Form1.WinGame();
        }
        public void ArrangeCards()
        {
            Card Tempcard = LinkedCard;
            Point TempLocation = Location;
            while (Tempcard != null)
            {
                Tempcard.Location = TempLocation;
                Tempcard.Top += scale / 4;
                Tempcard.BringToFront();
                TempLocation = Tempcard.Location;
                Tempcard = Tempcard.LinkedCard;
            }
        }
        public Card FindNearestCard()
        {
            Card Card1 = this, MinCard = null;
            int mindist = 1000000000, currentvalue = -1;
            foreach (Card Card2 in Form1.Deck)
            {
                if (!Card2.Active || Card1 == Card2 || Card1.IsLinkedDownwards(Card2)) continue;
                int dist = (Card1.Top - Card2.Top) * (Card1.Top - Card2.Top) + (Card1.Left - Card2.Left) * (Card1.Left - Card2.Left);
                if (Math.Abs(Card1.Left - Card2.Left) <= scale / 4 && (dist < mindist || (dist == mindist && Card2.Value > currentvalue)))
                {
                    mindist = dist;
                    MinCard = Card2;
                    currentvalue = MinCard.Value;
                }
            }
            foreach (Card Card2 in Form1.Deck)
            {
                if ((!Card2.Active && Card2.Value != 0 && Card2.Value != 14) || Card1 == Card2 || Card1.IsLinkedDownwards(Card2)) continue;
                int dist = (Card1.Top - Card2.Top) * (Card1.Top - Card2.Top) + (Card1.Left - Card2.Left) * (Card1.Left - Card2.Left);
                if (Math.Abs(Card1.Left - Card2.Left) <= scale / 4 && dist < mindist)
                {
                    mindist = dist;
                    MinCard = Card2;
                }
            }
            return MinCard;
        }
        public bool IsCompatibleWith(Card Card2)
        {
            Card Card1 = this;
            return Card2 == null || (!Card2.Active && Card2.Value != 0 && Card2.Value != 14) || Card2 == Form1.CurrentStock[2] || Card2.LinkedCard != null ? false : (Card2.Value == 14 && Card1.Value == 13) || ((Card2.IsLinkedUpwards(Form1.Deck[35]) || Card2.IsLinkedUpwards(Form1.Deck[36]) || Card2.IsLinkedUpwards(Form1.Deck[37]) || Card2.IsLinkedUpwards(Form1.Deck[38])) && Card2.Symbol == Card1.Symbol && Card1.Value - Card2.Value == 1 && Card1.LinkedCard == null) || (Card2.Value != 14 && Card1.Colour != Card2.Colour && Card2.Value - Card1.Value == 1 && !(Card2.IsLinkedUpwards(Form1.Deck[35]) || Card2.IsLinkedUpwards(Form1.Deck[36]) || Card2.IsLinkedUpwards(Form1.Deck[37]) || Card2.IsLinkedUpwards(Form1.Deck[38])));
        }
        public bool IsLinkedDownwards(Card Card2)
        {
            Card CurrentCard = this;
            while (CurrentCard != null)
            {
                if (CurrentCard == Card2)
                    return true;
                CurrentCard = CurrentCard.LinkedCard;
            }
            return false;
        }
        public bool IsLinkedUpwards(Card Card2)
        {
            Card CurrentCard = this;
            while (CurrentCard != null)
            {
                if (CurrentCard == Card2)
                    return true;
                CurrentCard = CurrentCard.LinkParent;
            }
            return false;
        }
    }
}