using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bytescout.Spreadsheet;

namespace English_Test
{
    public partial class Form1 : Form
    {
        int level = 1, wronginarow = 0;
        double points = 100;
        List<Word> PreviousWords=new List<Word>();
        Spreadsheet document = new Spreadsheet();
        Worksheet a1, a2, b1, b2, c1, c2;
        Label CurrentLevel;
        FancyLabel CurrentWord;
        FancyLabel[] Options = new FancyLabel[5];
        Random rnd = new Random();

        public void DisplayGUI()
        {
            CurrentLevel = new Label
            {
                Location = new Point(10, 10),
            };
            Controls.Add(CurrentLevel);
            CurrentWord = new FancyLabel
            {
                Location = new Point(50, 50),
            };
            Controls.Add(CurrentWord);
            for (int i = 0; i < 5; i++)
            {
                Options[i] = new FancyLabel
                {
                    AutoSize = true,
                    Location = new Point(50, 100 + 100 * i),
                };
                int j = i;
                Options[i].MouseDown += (object sender, MouseEventArgs e) => HandleGuess(j);
                Controls.Add(Options[i]);
            }
        }

        public void EndGame()
        {
            MessageBox.Show("points: " + points + " (" + (points>=200?"C2" : points >= 180 ? "C1" : points >= 160 ? "B2" : points >= 140 ? "B1" : points >= 120 ? "A2" : "A1") +")");
            level = 1;
            points = 100;
            wronginarow = 0;
            HandleLevel();
        }

        public void HandleGuess(int nr)
        {
            if (Options[nr].Word == CurrentWord.Word)
            {
                points += 2;
                wronginarow = 0;
            }
            else
                wronginarow++;
            if (level == 60 || wronginarow==5) { EndGame(); return; };
            level++;
            HandleLevel();
        }

        public bool IsRepeating(Word word)
        {
            foreach (Word prevword in PreviousWords)
                if (prevword == word)
                    return true;
            return false;
        }

        public bool IsDuplicate (Word word)
        {
            for (int i = 0; i < 5; i++)
                if (Options[i].Word!=null && word.Definition == Options[i].Word.Definition && word.Title == Options[i].Word.Title && word.POFS == Options[i].Word.POFS)
                    return true;
            return false;
        }

        public bool IsRelevent(Word word)
        {
            if (word.POFS == "conjunction" && (level > 40 || level < 11)) return (word.POFS == "conjuction" || word.POFS == "noun");
            else if (word.POFS == "determiner" && ((40 < level && level < 51) || level < 11)) return (word.POFS == "determiner" || word.POFS == "conjuction" || word.POFS == "noun");
            else if (word.POFS == "modal verb" && (level < 11 || (30 < level && level < 51))) return (word.POFS == "modal verb" || word.POFS == "verb");
            else if (word.POFS == "auxiliary verb") return (word.POFS == "auxiliary verb" || word.POFS == "verb");
            else if (word.POFS == "number" && (level > 20)) return (word.POFS == "number" || word.POFS == "noun");
            else if (word.POFS == "ordinal number" && (level < 11 || level > 20)) return (word.POFS == "ordinal number" || word.POFS == "noun");
            else if (word.POFS == "predeterminer") return (word.POFS == "predeterminer" || word.POFS == "noun");
            else if (word.POFS == "pronoun" && level>40) return (word.POFS == "pronoun" || word.POFS == "noun");
            else if(word.POFS== "exclamation" && level>20) return (word.POFS == "exclamation" || word.POFS == "noun");
            else return word.POFS == CurrentWord.Word.POFS;
        }

        public void HandleLevel()
        {
            Worksheet Level;
            CurrentLevel.Text = level.ToString();
            if (level > 50)
                Level = c2;
            else if (level > 40)
                Level = c1;
            else if (level > 30)
                Level = b2;
            else if (level > 20)
                Level = b1;
            else if (level > 10)
                Level = a2;
            else
                Level = a1;
            if (level % 10 == 1) PreviousWords = new List<Word>();
            Word TempWord;
            do
            { TempWord = GetRandomWord(Level); } while(IsRepeating(TempWord));
            PreviousWords.Add(TempWord);
            CurrentWord.Word = TempWord;
            CurrentWord.Text = TempWord.Title;
            int correctanswer = rnd.Next(0, 5);
            Options[correctanswer].Word = TempWord;
            Options[correctanswer].Text = TempWord.Definition;
            for (int i = 0; i < 5; i++)
            {
                if (i == correctanswer) continue;
                Word AnotherTempWord;
                do { AnotherTempWord = GetRandomWord(Level); } while (AnotherTempWord.Title == TempWord.Title || IsDuplicate(AnotherTempWord) || !IsRelevent(AnotherTempWord));
                Options[i].Word = AnotherTempWord;
                Options[i].Text = AnotherTempWord.Definition;
            }
        }

        public void GetWords()
        {
            document.LoadFromStream(new MemoryStream(Properties.Resources.cambridge_uk_combined));
            a1 = document.Workbook.Worksheets.ByName("a1");
            a2 = document.Workbook.Worksheets.ByName("a2");
            b1 = document.Workbook.Worksheets.ByName("b1");
            b2 = document.Workbook.Worksheets.ByName("b2");
            c1 = document.Workbook.Worksheets.ByName("c1");
            c2 = document.Workbook.Worksheets.ByName("c2");
        }

        public Word GetRandomWord(Worksheet Level)
        {
            int nrofwords = 0;
            string word = "", definition = "", pofs = "";
            if (Level == a1) nrofwords = 789;
            else if (Level == a2) nrofwords = 1505;
            else if (Level == b1) nrofwords = 2712;
            else if (Level == b2) nrofwords = 3743;
            else if (Level == c1) nrofwords = 1972;
            else if (Level == c2) nrofwords = 2948;
            do
            {
                string wrd = "", phrase = "";
                int nr = rnd.Next(1, nrofwords);
                wrd = Level.Cell(nr, 0).ValueAsString;
                phrase = Level.Cell(nr, 1).ValueAsString;
                pofs = Level.Cell(nr, 2).ValueAsString;
                definition = Level.Cell(nr, 5).ValueAsString;
                if (phrase != "-")
                    word = phrase;
                else
                    word = wrd;
            }
            while (definition == "-");
            return new Word(word, definition, pofs);
        }

        public Form1()
        {
            InitializeComponent();
            GetWords();
            DisplayGUI();
            HandleLevel();
        }
    }

    public class FancyLabel : Label
    {
        public Word Word;
    }

    public class Word
    {
        public string Title, Definition, POFS;
        public Word(string title, string definition, string pofs)
        {
            Title = title;
            Definition = definition;
            POFS = pofs;
        }
    }
}
