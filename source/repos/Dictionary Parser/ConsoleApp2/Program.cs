using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Bytescout.Spreadsheet;

namespace DicParser
{
    class Program
    {
        public static List<string> GetLinks()
        {
            List<string> Links = new List<string>();
            Spreadsheet document = new Spreadsheet();
            document.LoadFromFile("input.xlsx");
            Worksheet worksheet = document.Workbook.Worksheets.ByName("Sheet1");
            for (int i = 0; worksheet.Cell(i, 0).Value != null; i++)
                Links.Add(worksheet.Cell(i, 0).ValueAsString);
            return Links;
        }

        static void Main(string[] args)
        {
            HtmlWeb Web = new HtmlWeb();
            Spreadsheet document = new Spreadsheet();
            Worksheet worksheet = document.Workbook.Worksheets.Add("Sheet1");
            int i = 1;
            worksheet.Cell(0, 0).Value = "word";
            worksheet.Cell(0, 1).Value = "phrase";
            worksheet.Cell(0, 2).Value = "part of speech";
            worksheet.Cell(0, 3).Value = "info";
            worksheet.Cell(0, 4).Value = "level";
            worksheet.Cell(0, 5).Value = "definition";
            worksheet.Cell(0, 6).Value = "examples";
            worksheet.Cell(0, 7).Value = "related words";

            foreach (string Link in GetLinks())
            {
                HtmlAgilityPack.HtmlDocument home = Web.Load(Link);
                HtmlNode node = home.DocumentNode.SelectSingleNode("//div[@class='di-body']");
                if (node == null) continue;
                HtmlNodeCollection entries;
                if (node.SelectSingleNode(".//div[@class='pr idiom-block']") != null )
                    entries = node.SelectSingleNode(".//div[@class='pr idiom-block']").ChildNodes;
                else
                    entries = node.SelectSingleNode(".//div[@class='entry-body']").ChildNodes;
                if (entries == null) continue;
                foreach (HtmlNode entry in entries)
                {
                    if (entry == null) continue;
                    HtmlNodeCollection defblocks = entry.SelectNodes(".//div[@class='def-block ddef_block ']");
                    if (defblocks != null)
                        foreach (HtmlNode defblock in defblocks)
                        {
                            if (defblock.ParentNode.Attributes["class"].Value == "phrase-body dphrase_b") continue;
                            worksheet.Cell(i, 0).Value = entry.SelectSingleNode(".//div[@class='di-title']").InnerText;
                            worksheet.Cell(i, 1).Value = "-";
                            if(entry.SelectSingleNode(".//span[@class='pos dpos']")!=null)
                            worksheet.Cell(i, 2).Value = entry.SelectSingleNode(".//span[@class='pos dpos']").InnerText;
                            string temp="";
                            if (entry.SelectSingleNode(".//div[@class='pos-header dpos-h']") != null && entry.SelectSingleNode(".//div[@class='pos-header dpos-h']").SelectSingleNode(".//span[@class='gram dgram']") != null)
                                temp += " " + entry.SelectSingleNode(".//div[@class='pos-header dpos-h']").SelectSingleNode(".//span[@class='gram dgram']").InnerText + "\r\n";
                            if (defblock.SelectSingleNode(".//span[@class='gram dgram']") != null)
                                temp += defblock.SelectSingleNode(".//span[@class='gram dgram']").InnerText + "\r\n" ;
                            if (entry.SelectSingleNode(".//div[@class='pos-header dpos-h']") != null)
                                foreach (HtmlNode headerelement in entry.SelectSingleNode(".//div[@class='pos-header dpos-h']").ChildNodes)
                                    if(headerelement.Attributes["class"] != null && !(headerelement.Attributes["class"].Value== "di-title" || headerelement.Attributes["class"].Value.Contains("posgram dpos-g hdib")))
                                    temp += headerelement.InnerText + "\r\n";
                            if(defblock.SelectSingleNode(".//span[@class='def-info ddef-info']") != null)
                                foreach (HtmlNode info in defblock.SelectSingleNode(".//span[@class='def-info ddef-info']").ChildNodes)
                                    if (info.Attributes["class"] != null && !(info.Attributes["class"].Value == "gram dgram" || info.Attributes["class"].Value.Contains("epp-xref dxref")))
                                        temp += info.InnerText + "\r\n";
                            temp = temp.Replace("Your browser doesn't support HTML5 audio", "");
                            temp = temp.Replace("  ", "");
                            worksheet.Cell(i, 3).Value = (temp == "" ? "-" : temp);
                            if (defblock.SelectSingleNode(".//span[@class='def-info ddef-info']") == null || !defblock.SelectSingleNode(".//span[@class='def-info ddef-info']").HasChildNodes || defblock.SelectSingleNode(".//span[@class='def-info ddef-info']").FirstChild.Attributes["class"]==null || !defblock.SelectSingleNode(".//span[@class='def-info ddef-info']").FirstChild.Attributes["class"].Value.Contains("epp-xref dxref"))
                                worksheet.Cell(i, 4).Value = "-";
                            else
                                worksheet.Cell(i, 4).Value =  defblock.SelectSingleNode(".//span[@class='def-info ddef-info']").FirstChild.InnerText;
                            if (defblock.SelectSingleNode(".//div[@class='def ddef_d']")!=null)
                            worksheet.Cell(i, 5).Value = defblock.SelectSingleNode(".//div[@class='def ddef_d']").InnerText;
                            if (defblock.SelectSingleNode(".//div[@class='def-body ddef_b']") != null)
                            {
                                HtmlNodeCollection exampleswords = defblock.SelectSingleNode(".//div[@class='def-body ddef_b']").ChildNodes;
                                string examples = "",words="";
                                foreach (HtmlNode exampleword in exampleswords)
                                {
                                    if(exampleword.Attributes["class"]==null || exampleword.Attributes["class"].Value== "examp dexamp")
                                        examples += exampleword.InnerText+"\r\n";
                                    else
                                        words += exampleword.InnerText + "\r\n";
                                }
                                worksheet.Cell(i,6).Value = examples == "" ? "-" : examples;
                                worksheet.Cell(i,7).Value = words == "" ? "-" : words;
                            }
                            i++;
                        }
                    HtmlNodeCollection phraseblocks = entry.SelectNodes(".//div[@class='pr phrase-block dphrase-block ' or @class='pr phrase-block dphrase-block lmb-25' ]");
                    if (phraseblocks != null)
                        foreach (HtmlNode phraseblock in phraseblocks)
                        {
                            worksheet.Cell(i, 0).Value = entry.SelectSingleNode(".//div[@class='di-title']").InnerText;
                            worksheet.Cell(i, 1).Value = phraseblock.SelectSingleNode(".//span[@class='phrase-title dphrase-title']").InnerText;
                            if (entry.SelectSingleNode(".//span[@class='pos dpos']") != null)
                                worksheet.Cell(i, 2).Value = "-";
                            string temp = "";
                            if (entry.SelectSingleNode(".//div[@class='pos-header dpos-h']") != null && entry.SelectSingleNode(".//div[@class='pos-header dpos-h']").SelectSingleNode(".//span[@class='gram dgram']") != null)
                                temp += " " + entry.SelectSingleNode(".//div[@class='pos-header dpos-h']").SelectSingleNode(".//span[@class='gram dgram']").InnerText + "\r\n";
                            if (phraseblock.SelectSingleNode(".//span[@class='gram dgram']") != null)
                                temp += phraseblock.SelectSingleNode(".//span[@class='gram dgram']").InnerText + "\r\n"; ;
                            if (entry.SelectSingleNode(".//div[@class='pos-header dpos-h']") != null)
                                foreach (HtmlNode headerelement in entry.SelectSingleNode(".//div[@class='pos-header dpos-h']").ChildNodes)
                                    if (headerelement.Attributes["class"] != null && !(headerelement.Attributes["class"].Value == "di-title" || headerelement.Attributes["class"].Value.Contains("posgram dpos-g hdib")))
                                        temp += headerelement.InnerText + "\r\n";
                            if (phraseblock.SelectSingleNode(".//span[@class='def-info ddef-info']") != null)
                                foreach (HtmlNode info in phraseblock.SelectSingleNode(".//span[@class='def-info ddef-info']").ChildNodes)
                                    if (info.Attributes["class"] != null && !(info.Attributes["class"].Value == "gram dgram" || info.Attributes["class"].Value.Contains("epp-xref dxref")))
                                        temp += info.InnerText + "\r\n";
                            temp = temp.Replace("Your browser doesn't support HTML5 audio", "");
                            temp = temp.Replace("  ", "");
                            worksheet.Cell(i, 3).Value = (temp == "" ? "-" : temp);
                            if (phraseblock.SelectSingleNode(".//span[@class='def-info ddef-info']") == null || !phraseblock.SelectSingleNode(".//span[@class='def-info ddef-info']").HasChildNodes || phraseblock.SelectSingleNode(".//span[@class='def-info ddef-info']").FirstChild.Attributes["class"] == null || !phraseblock.SelectSingleNode(".//span[@class='def-info ddef-info']").FirstChild.Attributes["class"].Value.Contains("epp-xref dxref"))
                                worksheet.Cell(i, 4).Value = "-";
                            else
                                worksheet.Cell(i, 4).Value = phraseblock.SelectSingleNode(".//span[@class='def-info ddef-info']").FirstChild.InnerText;
                            if (phraseblock.SelectSingleNode(".//div[@class='def ddef_d']") != null)
                                worksheet.Cell(i, 5).Value = phraseblock.SelectSingleNode(".//div[@class='def ddef_d']").InnerText;
                            if (phraseblock.SelectSingleNode(".//div[@class='def-body ddef_b']") != null)
                            {
                                HtmlNodeCollection exampleswords = phraseblock.SelectSingleNode(".//div[@class='def-body ddef_b']").ChildNodes;
                                string examples = "", words = "";
                                foreach (HtmlNode exampleword in exampleswords)
                                {
                                    if (exampleword.Attributes["class"] == null || exampleword.Attributes["class"].Value == "examp dexamp")
                                        examples += exampleword.InnerText + "\r\n";
                                    else
                                        words += exampleword.InnerText + "\r\n";
                                }
                                worksheet.Cell(i, 6).Value = examples == "" ? "-" : examples;
                                worksheet.Cell(i, 7).Value = words == "" ? "-" : words;
                            }
                            i++;
                        }
                    HtmlNodeCollection derivedblocks = entry.SelectNodes(".//div[@class='pr runon drunon' ]");
                    if (derivedblocks != null)
                        foreach (HtmlNode derivedblock in derivedblocks)
                        {
                            worksheet.Cell(i, 0).Value = entry.SelectSingleNode(".//div[@class='di-title']").InnerText;
                            if(derivedblock.SelectSingleNode(".//span[@class='w dw']")!=null)
                            worksheet.Cell(i, 1).Value = derivedblock.SelectSingleNode(".//span[@class='w dw']").InnerText;
                            if (derivedblock.SelectSingleNode(".//span[@class='pos dpos']") != null)
                                worksheet.Cell(i, 2).Value = derivedblock.SelectSingleNode(".//span[@class='pos dpos']").InnerText;
                            string temp = "";
                            if (derivedblock.SelectSingleNode(".//span[@class='gram dgram']") != null)
                                temp += derivedblock.SelectSingleNode(".//span[@class='gram dgram']").InnerText + "\r\n"; ;
                            if (derivedblock.SelectSingleNode(".//div[@class='pos-header dpos-h']") != null)
                                foreach (HtmlNode headerelement in derivedblock.SelectSingleNode(".//div[@class='pos-header dpos-h']").ChildNodes)
                                    if (headerelement.Attributes["class"] != null && headerelement.Attributes["class"].Value != "pos dpos")
                                        temp += headerelement.InnerText + "\r\n";
                            temp = temp.Replace("Your browser doesn't support HTML5 audio", "");
                            temp = temp.Replace("  ", "");
                            worksheet.Cell(i, 3).Value = (temp == "" ? "-" : temp);
                            if (derivedblock.SelectSingleNode(".//span[@class='def-info ddef-info']") == null || !derivedblock.SelectSingleNode(".//span[@class='def-info ddef-info']").HasChildNodes || derivedblock.SelectSingleNode(".//span[@class='def-info ddef-info']").FirstChild.Attributes["class"] == null || !derivedblock.SelectSingleNode(".//span[@class='def-info ddef-info']").FirstChild.Attributes["class"].Value.Contains("epp-xref dxref"))
                                worksheet.Cell(i, 4).Value = "-";
                            else
                                worksheet.Cell(i, 4).Value = derivedblock.SelectSingleNode(".//span[@class='def-info ddef-info']").FirstChild.InnerText;
                            if (derivedblock.SelectSingleNode(".//div[@class='def ddef_d']") != null)
                                worksheet.Cell(i, 5).Value = derivedblock.SelectSingleNode(".//div[@class='def ddef_d']").InnerText;
                            if (derivedblock.SelectSingleNode(".//div[@class='def-body ddef_b']") != null)
                            {
                                HtmlNodeCollection exampleswords = derivedblock.SelectSingleNode(".//div[@class='def-body ddef_b']").ChildNodes;
                                string examples = "", words = "";
                                foreach (HtmlNode exampleword in exampleswords)
                                {
                                    if (exampleword.Attributes["class"] == null || exampleword.Attributes["class"].Value == "examp dexamp")
                                        examples += exampleword.InnerText + "\r\n";
                                    else
                                        words += exampleword.InnerText + "\r\n";
                                }
                                worksheet.Cell(i, 6).Value = examples == "" ? "-" : examples;
                                worksheet.Cell(i, 7).Value = words == "" ? "-" : words;
                            }
                            i++;
                        }
                }
                document.SaveAs("output.xlsx");
            }
            document.SaveAs("output.xlsx");
            document.Close();
        }
    }
}
