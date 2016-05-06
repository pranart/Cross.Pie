using System;

using UIKit;
using CoreGraphics;

namespace Cross.Pie.iOS.Sample
{
	public partial class SecondViewController : UIViewController
	{
		public SecondViewController (IntPtr handle)
			: base (handle)
		{
		}
		CrossPie Pie { get; set; }

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();	

			Pie = new CrossPie (new CGRect (0.0, 100.0, 380.0, 380.0));
			this.View.AddSubview (Pie);

			Initialize ();
		}
		void Initialize()
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

		public override void DidReceiveMemoryWarning ()
		{
			base.DidReceiveMemoryWarning ();
			// Release any cached data, images, etc that aren't in use.
		}
	}
}

