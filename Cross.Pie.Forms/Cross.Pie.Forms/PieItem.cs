using System;
using NGraphics;
using Xamarin.Forms;

namespace Cross.Pie.Forms
{
	public class PieItem : BindableObject
	{
		
		public static readonly BindableProperty IndexProperty = BindableProperty.Create<PieItem,int> (a=>a.Index, 0);
		public int Index {
			get{ return (int)GetValue (IndexProperty); }
			set{ SetValue (IndexProperty, value); }
		}

		public static readonly BindableProperty ValueProperty = BindableProperty.Create<PieItem,double> (a=>a.Value, 0.0);
		public double Value {
			get{ return (double)GetValue (ValueProperty); }
			set{ SetValue (ValueProperty, value); }
		}

		public static readonly BindableProperty ColorProperty = BindableProperty.Create<PieItem,Xamarin.Forms.Color?> (a=>a.Color, null);
		public Xamarin.Forms.Color? Color {
			get{ return (Xamarin.Forms.Color?)GetValue (ColorProperty); }
			set{ SetValue (ColorProperty, value); }
		}

		public static readonly BindableProperty TitleColorProperty = BindableProperty.Create<PieItem,Xamarin.Forms.Color?> (a=>a.TitleColor, null);
		public Xamarin.Forms.Color? TitleColor {
			get{ return (Xamarin.Forms.Color?)GetValue (TitleColorProperty); }
			set{ SetValue (TitleColorProperty, value); }
		}

		public static readonly BindableProperty PercentColorProperty = BindableProperty.Create<PieItem,Xamarin.Forms.Color?> (a=>a.PercentColor, null);
		public Xamarin.Forms.Color? PercentColor {
			get{ return (Xamarin.Forms.Color?)GetValue (PercentColorProperty); }
			set{ SetValue (PercentColorProperty, value); }
		}

		public static readonly BindableProperty ValueColorProperty = BindableProperty.Create<PieItem,Xamarin.Forms.Color?> (a=>a.ValueColor, null);
		public Xamarin.Forms.Color? ValueColor {
			get{ return (Xamarin.Forms.Color?)GetValue (ValueColorProperty); }
			set{ SetValue (ValueColorProperty, value); }
		}

		public static readonly BindableProperty TitleProperty = BindableProperty.Create<PieItem,string> (a=>a.Title, string.Empty);
		public string Title {
			get{ return (string)GetValue (TitleProperty); }
			set{ SetValue (TitleProperty, value); }
		}

		public static readonly BindableProperty IsPullProperty = BindableProperty.Create<PieItem,bool> (a=>a.IsPull, false);
		public bool IsPull {
			get{ return (bool)GetValue (IsPullProperty); }
			set{ SetValue (IsPullProperty, value); }
		}

		public static readonly BindableProperty IsSelectedProperty = BindableProperty.Create<PieItem,bool> (a=>a.IsSelected, false);
		public bool IsSelected {
			get{ return (bool)GetValue (IsSelectedProperty); }
			set{ SetValue (IsSelectedProperty, value); }
		}

		public static readonly BindableProperty IsBoldProperty = BindableProperty.Create<PieItem,bool> (a=>a.IsBold, false);
		public bool IsBold {
			get{ return (bool)GetValue (IsBoldProperty); }
			set{ SetValue (IsBoldProperty, value); }
		}

		public static readonly BindableProperty TagProperty = BindableProperty.Create<PieItem,object> (a=>a.Tag, null);
		public object Tag {
			get{ return (object)GetValue (TagProperty); }
			set{ SetValue (TagProperty, value); }
		}

		public static readonly BindableProperty PercentProperty = BindableProperty.Create<PieItem,double> (a=>a.Percent, 0.0);
		public double Percent {
			get{ return (double)GetValue (PercentProperty); }
			set{ SetValue (PercentProperty, value); }
		}

		public static readonly BindableProperty RadianProperty = BindableProperty.Create<PieItem,double> (a=>a.Radian, 0.0);
		public double Radian {
			get{ return (double)GetValue (RadianProperty); }
			set{ SetValue (RadianProperty, value); }
		}

		public static readonly BindableProperty PenWidthProperty = BindableProperty.Create<PieItem,double> (a=>a.PenWidth, 1.0);
		public double PenWidth {
			get{ return (double)GetValue (PenWidthProperty); }
			set{ SetValue (PenWidthProperty, value); }
		}

		public void Prepare(double sum)
		{
			Percent = Value / sum;
			Radian = Percent * 2 * Math.PI;
		}



		public Xamarin.Forms.Color PenColor
		{
			get
			{
				Color = Color ?? Xamarin.Forms.Color.Black;

				double[] hsb = Color.Value.ToCross().ToHSB ();
					
				return NGraphics.Color.FromHSB (hsb [0], hsb [1], hsb [2]/2.0).ToForms();
			}
		}

		public double DrawSegment (NGraphics.ICanvas canvas,NGraphics.Point center,double radius, double startRadian,double pullLength,NGraphics.Font font,Xamarin.Forms.Color textColor,bool isNameVisible,Xamarin.Forms.Color percentColor,bool isPercentVisible,Xamarin.Forms.Color valueColor,bool isValueVisible)
		{
			double middleRadian = startRadian + Radian/2.0;
			if (IsPull)
			{
				center = new NGraphics.Point (
					center.X + pullLength * Math.Cos (middleRadian),
					center.Y + pullLength * Math.Sin (middleRadian));
			}


			var brush = new SolidBrush ((Color ?? Xamarin.Forms.Color.Black).ToCross());
			var pen = new Pen (PenColor.ToCross(),PenWidth);

			canvas.DrawPath((path)=>
			{
				const double shardRadian = 0.02;

				path.MoveTo(center);
				path.LineTo(center.X+Math.Cos(startRadian)*radius,center.Y+Math.Sin(startRadian)*radius);

				for(var addRadian = 0.0 ; addRadian < Radian ; addRadian += shardRadian)
				{
					path.LineTo(center.X+Math.Cos(startRadian+addRadian)*radius,center.Y+Math.Sin(startRadian+addRadian)*radius);
				}
				path.LineTo(center.X+Math.Cos(startRadian + Radian)*radius,center.Y+Math.Sin(startRadian + Radian)*radius);

				path.Close();
			},pen,brush);


			if (isNameVisible)
			{
				DrawName (canvas, center, radius, font, TitleColor ?? textColor, middleRadian);
			}
			if (isValueVisible)
			{
				DrawValue (canvas, center, radius, font, ValueColor ?? valueColor, middleRadian);
			}
			if (isPercentVisible)
			{
				DrawPercent (canvas, center, radius, font, PercentColor ?? percentColor, middleRadian);
			}

			return startRadian+Radian;
		}
		void DrawName (ICanvas canvas, NGraphics.Point center, double radius, NGraphics.Font font, Xamarin.Forms.Color textColor, double middleRadian)
		{
			var percentCenter = new NGraphics.Point (center.X, center.Y - 20);
			DrawText (Title ?? "",canvas, percentCenter, radius, font, textColor, middleRadian);
		}

		void DrawPercent (ICanvas canvas, NGraphics.Point center, double radius, NGraphics.Font font, Xamarin.Forms.Color textColor, double middleRadian)
		{
			var percentCenter = new NGraphics.Point (center.X, center.Y + 20);
			DrawText (Percent.ToString("P1"),canvas, percentCenter, radius, font, textColor, middleRadian);
		}
		void DrawValue (ICanvas canvas, NGraphics.Point center, double radius, NGraphics.Font font, Xamarin.Forms.Color textColor, double middleRadian)
		{
			var valueCenter = new NGraphics.Point (center.X, center.Y);
			DrawText (Value.ToString("N"),canvas, valueCenter, radius, font, textColor, middleRadian);
		}

		void DrawText (string text,ICanvas canvas, NGraphics.Point center, double radius, NGraphics.Font font, Xamarin.Forms.Color textColor, double middleRadian)
		{
			if (!string.IsNullOrWhiteSpace (text))
			{
				double textRadius = radius * 0.67;
				NGraphics.Point centerText = new NGraphics.Point (center.X + Math.Cos (middleRadian) * textRadius, center.Y + Math.Sin (middleRadian) * textRadius);
				const double width = 40;
				const double height = 5;
				Rect rect = new Rect (centerText.X - width / 2.0, centerText.Y, width, height);
				canvas.DrawText (text, rect, font, NGraphics.TextAlignment.Center, new Pen (textColor.ToCross()), new SolidBrush (textColor.ToCross()));
			}
		}
	}
}

