using System;
using System.Collections.Generic;

namespace Boggle {
    static class WordHandler {
        static int MaxLength;
        static HashSet<string> Prefixes = new HashSet<string>();
        static HashSet<string>[] Words, Cur, Left;
        public static void Initialise() {
            MaxLength = Form1.BoardSize * Form1.BoardSize;
            Words = new HashSet<string>[MaxLength + 1];
            for (int i = 3; i <= MaxLength; ++i)
                Words[i] = new HashSet<string>();
            foreach (string word in Properties.Resources.dictionary.Split("\n")) {
                int CurLength = word.Length;
                if (CurLength > MaxLength || CurLength < 3) continue;
                Words[CurLength].Add(word);
                string cur = "";
                foreach (char x in word) {
                    cur += x;
                    Prefixes.Add(cur);
                }
            }
        }
        public static void Reset() {
            Cur = new HashSet<string>[MaxLength + 1];
            Left = new HashSet<string>[MaxLength + 1];
            for (int i = 3; i <= MaxLength; ++i) {
                Cur[i] = new HashSet<string>();
                Left[i] = new HashSet<string>();
            }
        }
        public static void Add(string s) {
            if (s.Length < 3)
                return;
            s = s.Replace("Q", "QU").ToLower();
            int l = s.Length;
            if (Words[l].Contains(s) && !Left[l].Contains(s)) {
                Left[l].Add(s);
                ++Form1.Total;
                Form1.RemainingScore += Score(s);
            }
        }
        public static bool Check(string s) {
            if (s.Length < 3)
                return false;
            s = s.Replace("Q", "QU").ToLower();
            int l = s.Length;
            if (Left[l].Contains(s)) {
                Left[l].Remove(s);
                Cur[l].Add(s);
                Form1.Score += Score(s);
                Form1.RemainingScore -= Score(s);
                ++Form1.Words;
                --Form1.RemainingWords;
                return true;
            }
            return false;
        }
        public static bool CheckPrefix(string s) {
            return Prefixes.Contains(s.Replace("Q", "QU").ToLower());
        }
        public static int Score(string s) {
            s = s.Replace("Q", "QU");
            switch (s.Length) {
                case 3:
                case 4:
                    return 1;
                case 5:
                    return 2;
                case 6:
                    return 3;
                case 7:
                    return 5;
                default:
                    return 11;
            }
        }
        public static string GetTop() {
            string ans = "\n\n\n";
            Func<HashSet<string>[], string> Get = (HashSet<string>[] H) => {
                string ans = "";
                for (int i = MaxLength; i >= 3; --i)
                    if (H[i].Count != 0) {
                        ans += i + " letters : ";
                        int nr = 1;
                        foreach (string x in H[i]) {
                            ans += x + ", ";
                            if (nr++ > 6) {
                                ans += "..., ";
                                break;
                            }
                        }
                        ans = ans.Remove(ans.Length - 2) + "\n";
                    }
                return ans + "\n";
            };
            return ans + "Your top words:\n\n" + Get(Cur) + "Possible top words:\n\n" + Get(Left);
        }
    }
}