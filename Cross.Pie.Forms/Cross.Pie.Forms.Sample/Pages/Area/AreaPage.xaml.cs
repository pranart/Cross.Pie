using System.Linq;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Cross.Pie.Forms.Sample
{
	public partial class AreaPage : ContentPage
	{
		public AreaPage ()
		{
			InitializeComponent ();

			XPie.IsValueVisible = false;

			SetupData ();

			XPie.ItemSelected += XPie_ItemSelected;

			List.ItemSelected += List_ItemSelected;
		}
		protected override void OnAppearing ()
		{
			base.OnAppearing ();

			this.DisplayAlert ("Try", "Try click on anything on List or PieChart", "OK");
		}
		void List_ItemSelected (object sender, SelectedItemChangedEventArgs e)
		{
			var model = e.SelectedItem as AreaModel;
			if (model?.Pie != null)
			{
				XPie.ClearAllPull ();
				model.Pie.IsPull = true;
				XPie.Update ();
			}
		}

		void XPie_ItemSelected (object sender, PieItem e)
		{
			var model = e?.Tag as AreaModel;
			if (model != null)
			{
				List.SelectedItem = model;
			}
		}

		void SetupData ()
		{
			var listModel = new List<AreaModel> {
				new AreaModel {
					Country = "Russia",
					Value = 27.3,
					Color = Color.Blue
				},
				new AreaModel {
					Country = "Canada",
					Value = 16.0,
					Color = Color.Red
				},
				new AreaModel {
					Country = "China",
					Value = 15.4,
					Color = Color.Yellow
				},
				new AreaModel {
					Country = "USA",
					Value = 15.4,
					Color = Color.Green
				},
				new AreaModel {
					Country = "Brazil",
					Value = 13.6,
					Color = Color.Purple
				},
				new AreaModel {
					Country = "Australia",
					TextColor = Color.Yellow,
					Value = 12.3,
					Color = Color.Navy
				},
			};
			List.ItemTemplate = new DataTemplate (typeof(AreaCell));
			List.ItemsSource = listModel;
			var listPieItem = from x in listModel
			select x.ToPieItem ();
			XPie.AddRange (listPieItem);
			XPie.Update ();
		}
	}
}

