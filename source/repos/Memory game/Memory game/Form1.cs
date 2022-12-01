using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Memory_game
{
    public partial class Form1 : Form
    {
        int size = 200, offset = 200, border = 50, CurrentGuess, CurrentValues, Level = 1, Time = 1000;
        public bool ok = true, game = true;
        List<Box> Boxes = new List<Box>();
        Random rnd = new Random();
        List<int> Values;

        public Form1()
        {
            InitializeComponent();
            KeyDown += KeyHandler;
            for (int i = 0; i < 3; i++)
                CreateNewBox();
            HandleLevel();
        }


        public void CreateNewBox()
        {
            Boxes.Add(new Box(Boxes.Count, size, border, offset, this));
        }

        public void GenerateValues()
        {
            List<int> TempValues = new List<int>();
            for (int i = 0; i < CurrentValues; i++)
                TempValues.Add(rnd.Next(0, Boxes.Count()));
            Values = TempValues;
        }

        public async void ShowValues()
        {
            ok = false;
            foreach (int Value in Values)
            {
                Boxes[Value].ShowBox(Time);
                await Task.Delay(Time*3/2);
            }
            DisplayMessage("Click the boxes in the right order", 1000);
            ok = true; 
        }

        public async void HandleGuess(int nr)
        {
            if (!(Values[CurrentGuess] == nr))
            {
                EndGame();
                return;
            }
            CurrentGuess++;
            if (CurrentGuess == CurrentValues)
            {
                Level++;
                ok = false;
                DisplayMessage("Next level", 500);
                await Task.Delay(Time);
                HandleLevel();
                return;
            }

        }

        public void HandleLevel()
        {
            ok = true;
            CurrentGuess = 0;
            DisplayMessage("Level : " + Level, 500);
            if (Level==1)
            {
                CurrentValues = 3;
            }
            if (Level % 3 == 0)
                CurrentValues++;
            if (Level % 7 == 0)
                CreateNewBox();
            GenerateValues();
            ShowValues();
        }

        public void EndGame()
        {
            game = false;
            DisplayMessage("You Lost! Press Enter to play again", 5000);
            Level = 1;
            CurrentValues = 3;
            for (int i = Boxes.Count - 1; i >= 3; i--)
            {
                Controls.Remove(Boxes[i]);
                Boxes[i].Dispose();
                Boxes[i] = null;
                Boxes.RemoveAt(i);
            }
        }

        public void KeyHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter || game) return;
            HandleLevel();
            game = true;
        }


        public async void DisplayMessage(string String, int Time)
        {
            using (TextBox temp = new TextBox())
            {
                temp.Text = String;
                temp.Font = new Font("Arial", 20f, FontStyle.Bold);
                Size size = TextRenderer.MeasureText(temp.Text, temp.Font);
                temp.Width = size.Width;
                temp.Height = size.Height;
                temp.BorderStyle = BorderStyle.None;
                temp.TextAlign = HorizontalAlignment.Center;
                temp.Location = new Point((ClientSize.Width - temp.Width)/2, (ClientSize.Height - temp.Height) / 2);
                temp.ReadOnly = true;
                temp.TabStop = false;
                Controls.Add(temp);
                temp.BringToFront();
                await Task.Delay(Time);
            }
        }
    }

    public class Box : PictureBox
    {
        int Number;
        public Box (int nr, int size, int border, int offset, Form1 form)
        {
            BackColor = Color.Yellow;
            Size = new Size(size, size);
            Number = nr;
            Location = new Point(nr * (size + border) + offset, offset);
            MouseDown += (object sender, MouseEventArgs e) =>
            {
                if (form.ok && form.game)
                {
                    form.HandleGuess(Number);
                    ShowBox(100);
                }
            };
            form.Controls.Add(this);
        }
        public async void ShowBox(int Time)
        {
            BackColor = Color.Red;
            await Task.Delay(Time);
            BackColor = Color.Yellow;
        }
    }
}
