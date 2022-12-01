using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;

namespace Boggle {
    public partial class Form1 : Form {
        public static int BoardSize = 4, Score, RemainingScore, Words, RemainingWords, Total, Time;
        public static int[] dy = { 1, 1, 0, -1, -1, -1, 0, 1 }, dx = { 0, 1, 1, 1, 0, -1, -1, -1 };
        public static int[,] seen;
        public static HashSet<Tuple<LetterBox, LetterBox>> Connected;
        public static Label LScore, LRemainingScore, LWords, LRemainingWords, LTotal, LTime;
        public static string CurWord, CurrentWord;
        public static List<LetterBox> Selected;
        public static LetterBox[,] Grid;
        public Form1() {
            WordHandler.Initialise();
            InitializeComponent();
            Resize += Scale;
            Paint += Draw;
            InitialiseGame();
            Size = new Size(800, 600);
        }
        private void Form1_Load(object sender, EventArgs e) { }
        public void InitialiseGame() {
            List<string> Dice = new List<string>{
        "AAAFRS",  "AAEEEE"  ,"AAFIRS",  "ADENNN", "AEEEEM",
        "AEEGMU",  "AEGMNN"  ,"AFIRSY",  "BJKQXZ", "CCNSTW",
        "CEIILT",  "CEILPT"  ,"CEIPST",  "DHHNOT", "DHHLOR",
        "DHLNOR",  "DDLNOR"  ,"EIIITT",  "EMOTTT", "ENSSSU",
        "FIPRSY",  "GORRVW"  ,"HIPRRY",  "NOOTUW", "OOOTTU"
        };
            Random Rnd = new Random();
            Grid = new LetterBox[BoardSize, BoardSize];
            for (int i = 0; i < BoardSize; ++i)
                for (int j = 0; j < BoardSize; ++j) {
                    int NrDice = Rnd.Next(0, Dice.Count());
                    Grid[i, j] = new LetterBox(i, j, Dice[NrDice][Rnd.Next(0, 6)], this);
                    Dice.RemoveAt(NrDice);
                    Controls.Add(Grid[i, j]);
                }
            seen = new int[BoardSize, BoardSize];
            Connected = new HashSet<Tuple<LetterBox, LetterBox>>();
            Selected = new List<LetterBox>();
            Time = 60;
            CurWord = CurrentWord = "";
            Score = RemainingScore = Words = Total = 0;
            Timer T = new Timer();
            T.Interval = 1000;
            T.Tick += (sender, e) => {
                LTime.Text = (--Time).ToString();
                if (Time == 0) {
                    T.Stop();
                    EndGame();
                }
            };
            WordHandler.Reset();
            Solve();
            RemainingWords = Total;
            InitialiseGUI();
            UpdateGUI();
            T.Start();
            Scale(null, null);
        }
        private void InitialiseGUI() {
            LScore = new Label {
                AutoSize = true
            };
            LWords = new Label {
                AutoSize = true
            };
            LRemainingScore = new Label {
                AutoSize = true
            };
            LRemainingWords = new Label {
                AutoSize = true
            };
            LTotal = new Label {
                AutoSize = true
            };
            LTime = new Label {
                AutoSize = true,
                Text = Time.ToString()
            };
            Controls.Add(LScore);
            Controls.Add(LWords);
            Controls.Add(LTotal);
            Controls.Add(LRemainingWords);
            Controls.Add(LRemainingScore);
            Controls.Add(LTime);
        }
        private void Scale(object sender, EventArgs e) {
            double dx = Size.Width / 800d, dy = Size.Height / 600d;
            double d = Math.Min(dx, dy);
            if (d < 0.2) return;
            Size = new Size((int)(800 * d), (int)(600 * d));
            LScore.Location = new Point((int)(50 * d), (int)(10 * d));
            LWords.Location = new Point((int)(200 * d), (int)(10 * d));
            LRemainingScore.Location = new Point((int)(350 * d), (int)(10 * d));
            LRemainingWords.Location = new Point((int)(500 * d), (int)(10 * d));
            LTotal.Location = new Point((int)(650 * d), (int)(10 * d));
            LTime.Location = new Point((int)(650 * d), (int)(60 * d));
            LScore.Font = LWords.Font = LRemainingScore.Font = LRemainingWords.Font = LTotal.Font = new Font("Arial", (int)(10 * d));
            LTime.Font = new Font("Arial", (int)(20 * d));
            for (int i = 0; i < BoardSize; ++i)
                for (int j = 0; j < BoardSize; ++j) {
                    Grid[i, j].size = (int)(100 * d);
                    Grid[i, j].offset = (int)(100 * d);
                    Grid[i, j].border = (int)(10 * d);
                    Grid[i, j].Size = new Size(Grid[i, j].size, Grid[i, j].size);
                    Grid[i, j].Location = new Point(Grid[i, j].offset + i * (Grid[i, j].size + Grid[i, j].border), Grid[i, j].offset + j * (Grid[i, j].size + Grid[i, j].border));
                    Grid[i, j].Font = new Font("Arial", Grid[i, j].size / (Grid[i, j].Letter != 'Q' ? 2 : 3));
                }
        }
        public static void UpdateGUI() {
            LWords.Text = "Current words :\n" + Words.ToString();
            LScore.Text = "Current score :\n" + Score.ToString();
            LRemainingWords.Text = "Remaining words :\n" + RemainingWords.ToString();
            LRemainingScore.Text = "Remaining score :\n" + RemainingScore.ToString();
            LTotal.Text = "Total found :\n" + (1f * Words / Total).ToString("P");
        }
        private void Solve() {
            for (int i = 0; i < BoardSize; ++i)
                for (int j = 0; j < BoardSize; ++j)
                    Solve(i, j);
        }
        private void Solve(int y, int x) {
            CurWord += Grid[y, x].Letter;
            seen[y, x] = 1;
            if (WordHandler.CheckPrefix(CurWord)) {
                WordHandler.Add(CurWord);
                for (int k = 0; k < 8; ++k) {
                    int yy = y + dy[k], xx = x + dx[k];
                    if (0 <= yy && yy < BoardSize && 0 <= xx && xx < BoardSize && seen[yy, xx] == 0)
                        Solve(yy, xx);
                }
            }
            CurWord = CurWord.Remove(CurWord.Length - 1);
            seen[y, x] = 0;
        }
        public static LetterBox GetPosition(Point p) {
            for (int i = 0; i < BoardSize; ++i)
                for (int j = 0; j < BoardSize; ++j) {
                    int x = Grid[i, j].Location.X + Grid[i, j].size / 2,
                        y = Grid[i, j].Location.Y + Grid[i, j].size / 2,
                        r = (Grid[i, j].size - Grid[i, j].border) / 2;
                    if ((p.X - x) * (p.X - x) + (p.Y - y) * (p.Y - y) <= r * r)
                        return Grid[i, j];
                }
            return null;
        }
        public static bool Check(LetterBox cur) {
            if (Selected.Count == 0) return true;
            LetterBox last = Selected[Selected.Count() - 1];
            return Math.Abs(last.Row - cur.Row) <= 1 && Math.Abs(last.Column - cur.Column) <= 1;
        }
        private void Draw(object sender, PaintEventArgs e) {
            for (int i = 0; i < BoardSize; ++i)
                for (int j = 0; j < BoardSize; ++j)
                    for (int k = 0; k < 8; ++k) {
                        int ii = i + dy[k], jj = j + dx[k];
                        if (ii >= 0 && ii < BoardSize && jj >= 0 && jj < BoardSize && Connected.Contains(Tuple.Create(Grid[i, j], Grid[ii, jj]))) {
                            int s = Grid[i, j].size, b = Grid[i, j].border;
                            int y0 = Grid[i, j].Location.Y + s / 2 + dx[k] * ((s - b) / 2), x0 = Grid[i, j].Location.X + s / 2 + dy[k] * ((s - b) / 2);
                            int y1 = Grid[ii, jj].Location.Y + s / 2 + dx[(k + 4) % 8] * ((s - b) / 2), x1 = Grid[ii, jj].Location.X + s / 2 + dy[(k + 4) % 8] * ((s - b) / 2);
                            e.Graphics.DrawLine(new Pen(Color.LightGray, b), new Point(x0, y0), new Point(x1, y1));
                        }
                    }
        }
        private void EndGame() {
            MessageBox.Show("Game over!" + WordHandler.GetTop());
            Controls.Remove(LScore);
            Controls.Remove(LWords);
            Controls.Remove(LTotal);
            Controls.Remove(LRemainingWords);
            Controls.Remove(LRemainingScore);
            Controls.Remove(LTime);
            LScore = LWords = LTotal = LRemainingScore = LRemainingWords = LTime = null;
            for (int i = 0; i < BoardSize; ++i)
                for (int j = 0; j < BoardSize; ++j) {
                    Controls.Remove(Grid[i, j]);
                    Grid[i, j] = null;
                }
            Invalidate();
            InitialiseGame();
        }
    }
    public class LetterBox : Label {
        public int size = 100, offset = 100, border = 10;
        bool Selected = false;
        public int Row, Column;
        public char Letter;
        Form form;
        public LetterBox(int r, int c, char t, Form Form_) {
            Row = r;
            Column = c;
            Letter = t;
            Text = t.ToString().Replace("Q", "QU");
            BackColor = Color.DarkGray;
            ForeColor = Color.Black;
            form = Form_;
            TextAlign = ContentAlignment.MiddleCenter;
            MouseDown += (sender, e) => {
                if (Form1.Selected.Count != 0) return;
                Select();
            };
            MouseMove += (sender, e) => {
                if (Form1.Selected.Count == 0) return;
                var x = Form1.GetPosition(new Point(Location.X + e.X, Location.Y + e.Y));
                if (x == null)
                    return;
                if (Form1.Selected.Count >= 2 && x == Form1.Selected[Form1.Selected.Count - 2]) {
                    using (var sp = new SoundPlayer(Properties.Resources.s2))
                        sp.Play();
                    Form1.Selected[Form1.Selected.Count - 1].Deselect();
                    return;
                }
                x.Select();
            };
            MouseUp += (sender, e) => {
                if (WordHandler.Check(Form1.CurrentWord)) {
                    using (var sp = new SoundPlayer(Properties.Resources.s1))
                        sp.Play();
                    Form1.UpdateGUI();
                }
                for (int i = Form1.CurrentWord.Length - 1; i >= 0; --i)
                    Form1.Selected[i].Deselect();
            };
        }
        public void Select() {
            if (Selected || !Form1.Check(this)) return;
            Selected = true;
            BackColor = Color.LightGray;
            ForeColor = Color.Gray;
            Form1.Selected.Add(this);
            Form1.CurrentWord += Letter;
            using (var sp = new SoundPlayer(Properties.Resources.s2))
                sp.Play();
            if (Form1.Selected.Count >= 2) {
                Form1.Connected.Add(Tuple.Create(Form1.Selected[Form1.Selected.Count - 2], Form1.Selected[Form1.Selected.Count - 1]));
                form.Invalidate();
            }
        }
        public void Deselect() {
            if (!Selected) return;
            Selected = false;
            BackColor = Color.DarkGray;
            ForeColor = Color.Black;
            if (Form1.Selected.Count >= 2) {
                Form1.Connected.Remove(Tuple.Create(Form1.Selected[Form1.Selected.Count - 2], Form1.Selected[Form1.Selected.Count - 1]));
                form.Invalidate();
            }
            Form1.Selected.RemoveAt(Form1.Selected.Count() - 1);
            Form1.CurrentWord = Form1.CurrentWord.Remove(Form1.CurrentWord.Length - 1);
        }
    }
}