using Cross.Pie.Forms.Sample;
using Foundation;
using UIKit;
using NControl.iOS;

namespace Cross.Pie.Forms.iOS
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			global::Xamarin.Forms.Forms.Init ();
			NControlViewRenderer.Init ();

			// Code for starting up the Xamarin Test Cloud Agent
			#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
			#endif

			LoadApplication (new App ());

			return base.FinishedLaunching (app, options);
		}
	}
}

