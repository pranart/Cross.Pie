using System;

namespace Cross.Pie.Forms
{
	public static class CrossExtension
	{
		public static NGraphics.Color ToCross(this Xamarin.Forms.Color fColor)
		{
			return new NGraphics.Color (fColor.R, fColor.G, fColor.B, fColor.A);
		}
		public static Xamarin.Forms.Color ToForms(this NGraphics.Color xColor)
		{
			return new Xamarin.Forms.Color (xColor.R/255.0, xColor.G/255.0, xColor.B/255.0, xColor.A/255.0);
		}
	}
}

