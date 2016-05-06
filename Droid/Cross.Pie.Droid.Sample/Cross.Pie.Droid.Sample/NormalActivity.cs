
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

using NGraphics;
using Cross.Pie;

namespace Cross.Pie.Droid.Sample
{
	[Activity (Label = "NormalActivity")]			
	public class NormalActivity : Activity
	{
		CrossPie Pie { get; set; }

		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			SetContentView (Resource.Layout.Normal);

			Pie = FindViewById<CrossPie> (Resource.Id.myPie);

			Init ();
		}

		void Init()
		{
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
			Pie.Update ();

			Pie.ItemSelected += (object sender, PieItem e) => 
			{
				e.IsPull = !e.IsPull;
				e.IsBold = e.IsPull;
				Pie.Update();
			};
		}
	}
}

