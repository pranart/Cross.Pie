using System;
using Xamarin.Forms;
using Cross.Pie.Forms;
using NGraphics;


namespace Cross.Pie.Forms.Sample
{
	public class App : Application
	{

		public App ()
		{
			TabbedPage pages = new TabbedPage();
			pages.Children.Add (new SimplestPage () { Title="Simplest"}	);
			pages.Children.Add (new NormalPage () 	{ Title="Normal"  }	);
			pages.Children.Add (new AreaPage() 		{ Title="Area"    }	);

			MainPage = pages;



		}
	}
}

