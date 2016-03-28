using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewPageCreator
{
	class Program
	{
		static string typescriptFile = @"import {Page, NavController, NavParams} from 'ionic-angular';
@Page({
	templateUrl: 'build/pages/%%PAGENAME%%/%%PAGENAME%%.html'
})
export class %%PAGECLASS%% {
	constructor(private nav: NavController, navParams: NavParams) {
	}
}";
		static string htmlFile = @"<ion-navbar *navbar class='%%PAGECLASS%%'>
    <button menuToggle>
        <ion-icon name='menu'></ion-icon>
	</button>
	<ion-title> </ion-title>
</ion-navbar>
<ion-content class='%%PAGECLASS%%'>

</ion-content>";
		static void Main(string[] args)
		{
			List<string> nameParts = new List<string>();
			string pageName;
			bool properName;

			//input and verification
			do
			{
				properName = true;
				Console.WriteLine("Enter pagename like this one \"MyFirstTry\" without quotes");
				pageName = Console.ReadLine();
				List<int> indexes = new List<int>();
				int index = 0;
				foreach (Char C in pageName)
				{
					if (Char.IsUpper(C))
					{
						indexes.Add(index);
					}
					else if (index == 0)
					{
						properName = false;
						Console.WriteLine("First symbol must be uppercase");
					}
					index++;
				}
				if (indexes.Count > 0)
				{
					indexes.Add(pageName.Length);
					nameParts = new List<string>();

					for (int i = 0; i < indexes.Count - 1; i++)
					{
						nameParts.Add(pageName.Substring(indexes[i], indexes[i + 1] - indexes[i]));
					}
				}
				else
				{
					properName = false;
					Console.WriteLine("Too short name");
				}
			} while (!properName);




			string pageCssClass = String.Join("-", nameParts).ToLower();
			string pageTsClass = pageName + "Page";
			string pageTsFileName = "./pages/" + pageCssClass + "/" + pageCssClass + ".ts";
			string pageScssFileName = "./pages/" + pageCssClass + "/" + pageCssClass + ".scss";
			string pageHtmlFileName = "./pages/" + pageCssClass + "/" + pageCssClass + ".html";
			Directory.CreateDirectory("./pages/" + pageCssClass);

			string coreCssPath = "./theme/app.core.scss";
			using (StreamWriter sw = File.AppendText(coreCssPath))
			{
				sw.WriteLine("");
				sw.WriteLine("@import \"../pages/"+ pageCssClass + "/"+ pageCssClass + "\";");
			}

			typescriptFile = typescriptFile.Replace("%%PAGENAME%%", pageCssClass);
			typescriptFile = typescriptFile.Replace("%%PAGECLASS%%", pageTsClass);
			using (StreamWriter sw = File.AppendText(pageTsFileName))
			{
				sw.WriteLine(typescriptFile);
			}

			using (StreamWriter sw = File.AppendText(pageScssFileName))
			{
				sw.WriteLine("." + pageCssClass + "{");
				sw.WriteLine("");
				sw.WriteLine("}");
			}

			htmlFile = htmlFile.Replace("%%PAGECLASS%%", pageCssClass);
			using (StreamWriter sw = File.AppendText(pageHtmlFileName))
			{
				sw.WriteLine(htmlFile);
			}
		}
	}
}
