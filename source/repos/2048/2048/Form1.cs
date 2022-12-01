using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _2048
{
    public partial class Form1 : Form
    {
        public static int offset = 100, border = 10, scale = 100, length = 4, height = 4, currentpiece = 0, score = 0, previousscore = 0;
        public static bool onlytwos = false, noundos = false;
        GridSquare[,] Grid = new GridSquare[20, 20];
        Piece[] Pieces = new Piece[400];
        Piece[] PreviousPieces = new Piece[400];
        Random Rnd = new Random();
        public Label Score;
        public Form1()
        {
            InitializeComponent();
            KeyDown += ControlHandler;
            DrawLayout();
            DrawGrid();
            DrawPiece();
        }

        private void ControlHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyData)
            {
                case Keys.Up:
                    MovePieces("Up");
                    break;
                case Keys.Down:
                    MovePieces("Down");
                    break;
                case Keys.Left:
                    MovePieces("Left");
                    break;
                case Keys.Right:
                    MovePieces("Right");
                    break;
            }
        }

        public void DrawLayout()
        {
            Score = new Label
            {
                Location = new Point(50, 10),
                Text = "Score: 0",
                Font = new Font("Arial", 12f, FontStyle.Bold),
                AutoSize = true,
            };
            Controls.Add(Score);
            Label NewGame = new Label
            {
                Location = new Point(200, 10),
                Text = "New Game",
                Font = new Font("Arial", 12f, FontStyle.Bold),
                AutoSize = true,
            };
            Controls.Add(NewGame);
            NewGame.MouseDown += (object sender, MouseEventArgs e) => { StartGame(); };
            if (noundos) return;
            Label Undo = new Label
            {
                Location = new Point(350, 10),
                Text = "Undo",
                Font = new Font("Arial", 12f, FontStyle.Bold),
                AutoSize = true,
            };
            Controls.Add(Undo);
            Undo.MouseDown += (object sender, MouseEventArgs e) => { UndoLastMove(); };
        }

        public void DrawGrid()
        {
            for (int i = 0; i < length; i++)
                for (int j = 0; j < height; j++)
                    Grid[j, i] = new GridSquare(this, i, j, scale, border, offset);
            PictureBox Background = new PictureBox
            {
                Location = new Point(offset - 2 * border, offset - 2 * border),
                Size = new Size(3 * border + length * (border + scale), 3 * border + height * (border + scale)),
                BackColor = ColorTranslator.FromHtml("#bbada0")
            };
            Controls.Add(Background);
            Background.SendToBack();
        }

        public void DrawPiece()
        {
            int x, y, value;
            do
            {
                x = Rnd.Next(0, length);
                y = Rnd.Next(0, height);
            }
            while (GetPiece(x, y) != null);
            value = 2 * (Rnd.Next(1, 11) / 10 + 1);
            int i;
            for (i = 0; i <= currentpiece; i++)
                if (Pieces[i] == null || Pieces[i].Location == new Point(-1, -1))
                    break;
            if (onlytwos) value = 2;
            Pieces[i] = new Piece(this, x, y, scale, border, offset, value);
            if (i == currentpiece) currentpiece++;
        }

        public Piece GetPiece(int x, int y)
        {
            for (int i = 0; i < currentpiece; i++)
                if (Pieces[i] != null && Pieces[i].Location == Grid[y, x].Location)
                    return Pieces[i];
            return null;
        }

        public void DeletePiece(Piece Piece)
        {
            Piece.Visible = false;
            Piece.Location = new Point(-1, -1);
            Piece.Dispose();
            Piece = null;
        }

        public void MovePieces(string Direction)
        {
            if (!noundos) Copy();
            bool valid = false;
            if (Direction == "Up" || Direction == "Down")
                for (int i = 0; i < length; i++)
                {
                    if (Direction == "Up")
                    {
                        int current = 0;
                        for (int j = 0; j < height; j++)
                            if (GetPiece(i, j) != null)
                            {
                                if (j != current && !valid) valid = true;
                                MovePiece(GetPiece(i, j), i, current);
                                current++;
                            }
                    }
                    if (Direction == "Down")
                    {
                        int current = height - 1;
                        for (int j = height - 1; j >= 0; j--)
                            if (GetPiece(i, j) != null)
                            {
                                if (j != current && !valid) valid = true;
                                MovePiece(GetPiece(i, j), i, current);
                                current--;
                            }
                    }
                }
            if (Direction == "Left" || Direction == "Right")
            {
                for (int i = 0; i < height; i++)
                {
                    if (Direction == "Left")
                    {
                        int current = 0;
                        for (int j = 0; j < length; j++)
                            if (GetPiece(j, i) != null)
                            {
                                if (j != current && !valid) valid = true;
                                MovePiece(GetPiece(j, i), current, i);
                                current++;
                            }
                    }
                    if (Direction == "Right")
                    {
                        int current = length - 1;
                        for (int j = length - 1; j >= 0; j--)
                            if (GetPiece(j, i) != null)
                            {
                                if (j != current && !valid) valid = true;
                                MovePiece(GetPiece(j, i), current, i);
                                current--;
                            }
                    }
                }
            }
            if (MergePieces(Direction) || valid)
                DrawPiece();
            if (GameIsOver())
                EndGame();
        }

        public void MovePiece(Piece Piece, int x, int y)
        {
            Piece.Location = Grid[y, x].Location;
        }

        public bool MergePieces(string Direction)
        {
            bool valid = false;
            if (Direction == "Up" || Direction == "Down")
                for (int i = 0; i < length; i++)
                {
                    if (Direction == "Up")
                    {
                        for (int j = 0; j < height - 1; j++)
                            if (GetPiece(i, j) != null && GetPiece(i, j + 1) != null && GetPiece(i, j).Value == GetPiece(i, j + 1).Value)
                            {
                                if (!valid) valid = true;
                                GetPiece(i, j).UpdateValue();
                                UpdateScore(GetPiece(i, j).Value);
                                DeletePiece(GetPiece(i, j + 1));
                                for (int k = j + 2; k <= height - 1; k++)
                                {
                                    if (GetPiece(i, k) == null) break;
                                    MovePiece(GetPiece(i, k), i, k - 1);
                                }
                            }
                    }
                    if (Direction == "Down")
                    {
                        for (int j = height - 1; j >= 1; j--)
                            if (GetPiece(i, j) != null && GetPiece(i, j - 1) != null && GetPiece(i, j).Value == GetPiece(i, j - 1).Value)
                            {
                                if (!valid) valid = true;
                                GetPiece(i, j).UpdateValue();
                                UpdateScore(GetPiece(i, j).Value);
                                DeletePiece(GetPiece(i, j - 1));
                                for (int k = j - 2; k >= 0; k--)
                                {
                                    if (GetPiece(i, k) == null) break;
                                    MovePiece(GetPiece(i, k), i, k + 1);
                                }
                            }
                    }
                }
            if (Direction == "Left" || Direction == "Right")
                for (int i = 0; i < height; i++)
                {
                    if (Direction == "Left")
                    {
                        for (int j = 0; j < length - 1; j++)
                            if (GetPiece(j, i) != null && GetPiece(j + 1, i) != null && GetPiece(j, i).Value == GetPiece(j + 1, i).Value)
                            {
                                if (!valid) valid = true;
                                GetPiece(j, i).UpdateValue();
                                UpdateScore(GetPiece(j, i).Value);
                                DeletePiece(GetPiece(j + 1, i));
                                for (int k = j + 2; k <= length - 1; k++)
                                {
                                    if (GetPiece(k, i) == null) break;
                                    MovePiece(GetPiece(k, i), k - 1, i);
                                }
                            }
                    }
                    if (Direction == "Right")
                    {
                        for (int j = length - 1; j >= 1; j--)
                            if (GetPiece(j, i) != null && GetPiece(j - 1, i) != null && GetPiece(j, i).Value == GetPiece(j - 1, i).Value)
                            {
                                if (!valid) valid = true;
                                GetPiece(j, i).UpdateValue();
                                UpdateScore(GetPiece(j, i).Value);
                                DeletePiece(GetPiece(j - 1, i));
                                for (int k = j - 2; k >= 0; k--)
                                {
                                    if (GetPiece(k, i) == null) break;
                                    MovePiece(GetPiece(k, i), k + 1, i);
                                }
                            }
                    }
                }
            return valid;
        }

        public void UpdateScore(int points)
        {
            score += points;
            Score.Text = "Score: " + score;
        }

        public bool GameIsOver()
        {
            for (int i = 0; i < length; i++)
                for (int j = 0; j < height; j++)
                    if (GetPiece(i, j) == null)
                        return false;
            for (int i = 0; i < length; i++)
                for (int j = 0; j < height - 1; j++)
                    if (GetPiece(i, j).Value == GetPiece(i, j + 1).Value)
                        return false;
            for (int i = 0; i < height; i++)
                for (int j = 0; j < length - 1; j++)
                    if (GetPiece(j, i).Value == GetPiece(j + 1, i).Value)
                        return false;
            return true;
        }

        public void StartGame()
        {
            score = 0;
            Score.Text = "Score: 0";
            for (int i = 0; i < currentpiece; i++)
                DeletePiece(Pieces[i]);
            DrawPiece();
            Copy();
        }

        public void EndGame()
        {
            MessageBox.Show("Game over!");
        }

        public void Copy()
        {
            previousscore = score;
            for (int i = 0; i < currentpiece; i++)
            {
                if (Pieces[i] == null) continue;
                PreviousPieces[i] = new Piece(Pieces[i].Visible, Pieces[i].Value, Pieces[i].Location, Pieces[i].BackgroundImage);
            }
        }

        public void UndoLastMove()
        {
            score = previousscore;
            Score.Text = "Score: " + score;
            for (int i = 0; i < currentpiece; i++)
            {
                if (PreviousPieces[i] == null)
                {
                    DeletePiece(Pieces[i]);
                    continue;
                }
                Pieces[i].Value = PreviousPieces[i].Value;
                Pieces[i].BackgroundImage = PreviousPieces[i].BackgroundImage;
                Pieces[i].Location = PreviousPieces[i].Location;
                Pieces[i].Visible = PreviousPieces[i].Visible;
            }
        }
    }

    public class GridSquare : PictureBox
    {
        public GridSquare(Form Form, int x, int y, int size, int border, int offset)
        {
            BackColor = ColorTranslator.FromHtml("#ccc0b3");
            Size = new Size(size, size);
            Location = new Point(offset + (size + border) * x, offset + (size + border) * y);
            Form.Controls.Add(this);
        }
    }

    public class Piece : PictureBox
    {
        public int Value;
        public Piece(Form Form, int x, int y, int size, int border, int offset, int value)
        {
            BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("tile_" + value.ToString(), Properties.Resources.Culture);
            BackgroundImageLayout = ImageLayout.Stretch;
            Size = new Size(size, size);
            Location = new Point(offset + (size + border) * x, offset + (size + border) * y);
            Value = value;
            Form.Controls.Add(this);
            BringToFront();
        }
        public Piece(bool visible, int value, Point location, Image backgroundimage)
        {
            Visible = visible;
            Value = value;
            Location = location;
            BackgroundImage = backgroundimage;
        }
        public void UpdateValue()
        {
            Value *= 2;
            BackgroundImage = (Image)Properties.Resources.ResourceManager.GetObject("tile_" + Value.ToString(), Properties.Resources.Culture);
        }
    }
}
