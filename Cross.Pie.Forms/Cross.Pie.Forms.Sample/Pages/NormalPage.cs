using System;

using Xamarin.Forms;


namespace Cross.Pie.Forms.Sample
{
	public class NormalPage : ContentPage
	{
		CrossPie Pie { get; set; }

		public NormalPage ()
		{
			Grid grid = new Grid ();
			grid.BackgroundColor = Color.White;

			grid.Children.Add (Pie = new CrossPie ());
			Pie.IsValueVisible = Pie.IsNameVisible = Pie.IsValueVisible = false;

			Random rander = new Random ();

			for (int i = 0; i < 5; i++)
			{
				Pie.Add (new PieItem 
				{
					Value = rander.Next (2, 7),
					IsPull = i==4,
					IsBold = true,
					Title = "Test",

				});
			}
			Pie.Title = "";//"Title";
			Pie.TitleColor = Xamarin.Forms.Color.Gray;
			Pie.IsTitleOnTop = true;
			Pie.Update ();

			Pie.ItemSelected += (object sender, PieItem e) => 
			{
				e.IsPull = !e.IsPull;
				e.IsBold = e.IsPull;
				Pie.Update();
			};

			Content = grid;
		}
	
		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			//this.DisplayAlert ("Try", "Try click on anything on Pie Chart", "OK");

		}
	}

}


