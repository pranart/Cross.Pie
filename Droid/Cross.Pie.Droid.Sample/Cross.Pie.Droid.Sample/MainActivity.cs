using Android.App;
using Android.Widget;
using Android.OS;
using System;
using Android.Content;
using System.Security.Cryptography;

namespace Cross.Pie.Droid.Sample
{
	[Activity (Label = "Cross.Pie.Droid.Sample", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : TabActivity
	{
		protected override void OnCreate (Bundle savedInstanceState)
		{
			base.OnCreate (savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			AddTabSimplest ();
			AddTabNormal ();
			TabHost.CurrentTab = 0;



		}
		void AddTabSimplest()
		{
			Intent intentSimplest = new Intent ().SetClass (this, typeof(SimplestActivity));
			Android.Widget.TabHost.TabSpec tabSpecSimplest = TabHost.NewTabSpec ("Simplest")
				.SetIndicator ("Simplest", null)
				.SetContent (intentSimplest);

			TabHost.AddTab (tabSpecSimplest);
		}
		void AddTabNormal ()
		{
			Intent intentNormal = new Intent ().SetClass (this, typeof(NormalActivity));
			Android.Widget.TabHost.TabSpec tabSpecNormal = TabHost.NewTabSpec ("Normal")
				.SetIndicator ("Normal",null)
				.SetContent (intentNormal);

			TabHost.AddTab (tabSpecNormal);

		}
	}
}


