using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Bytescout.Spreadsheet;
using System.Windows.Controls;

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
			worksheet.Cell(0, 0).Value = "Test crt";
			int i = -1, j = 0, nr = 0;
			foreach (string Link in GetLinks())
			{
				HtmlAgilityPack.HtmlDocument home = new HtmlDocument();
				home.LoadHtml(Link);
				var nodes = home.DocumentNode.FirstChild.ChildNodes;
				foreach (var Node in nodes)
				{
					if (++i % 4 == 0)
					{
						++nr;
						j = 0;
					}
					foreach (var NodeNode in Node.ChildNodes)
						worksheet.Cell(nr, j++).Value += NodeNode.InnerText;
				}
				document.SaveAs("output.xlsx");
			}
			document.SaveAs("output.xlsx");
			document.Close();
		}
	}
}
