using System;
using NGraphics;
using Java.Util;

namespace Cross.Pie.Droid
{
	public class PieItem 
	{

		public int Index { get; set; }

		public double Value { get; set; }

		public NGraphics.Color? Color { get; set; }

		public NGraphics.Color? TitleColor { get; set; }

		public NGraphics.Color? PercentColor { get; set; }

		public NGraphics.Color? ValueColor { get; set; }

		public string Title { get; set; } = string.Empty;

		public bool IsPull { get; set; }

		public bool IsSelected { get; set; }

		public bool IsBold { get; set; }

		public object Tag { get; set; }

		public double Percent { get; set; }

		public double Radian { get; set; }

		public double PenWidth { get; set; } = 1.0;

		public void Prepare(double sum)
		{
			Percent = Value / sum;
			Radian = Percent * 2 * Math.PI;
		}



		public NGraphics.Color PenColor
		{
			get
			{
				Color = Color ?? NGraphics.Colors.Black;

				double[] hsb = Color.Value.ToHSB ();

				return NGraphics.Color.FromHSB (hsb [0], hsb [1], hsb [2]/2.0);
			}
		}

		public double DrawSegment (NGraphics.ICanvas canvas,NGraphics.Point center,double radius, double startRadian,double pullLength,NGraphics.Font font,NGraphics.Color textColor,bool isNameVisible,NGraphics.Color percentColor,bool isPercentVisible,NGraphics.Color valueColor,bool isValueVisible)
		{
			double middleRadian = startRadian + Radian/2.0;
			if (IsPull)
			{
				center = new NGraphics.Point (
					center.X + pullLength * Math.Cos (middleRadian),
					center.Y + pullLength * Math.Sin (middleRadian));
			}


			var brush = new SolidBrush ((Color ?? NGraphics.Colors.Black));
			var pen = new Pen (PenColor,PenWidth);

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
		void DrawName (ICanvas canvas, NGraphics.Point center, double radius, NGraphics.Font font, NGraphics.Color textColor, double middleRadian)
		{
			var percentCenter = new NGraphics.Point (center.X, center.Y - 20);
			DrawText (Title ?? "",canvas, percentCenter, radius, font, textColor, middleRadian);
		}

		void DrawPercent (ICanvas canvas, NGraphics.Point center, double radius, NGraphics.Font font, NGraphics.Color textColor, double middleRadian)
		{
			var percentCenter = new NGraphics.Point (center.X, center.Y + 20);
			DrawText (Percent.ToString("P1"),canvas, percentCenter, radius, font, textColor, middleRadian);
		}
		void DrawValue (ICanvas canvas, NGraphics.Point center, double radius, NGraphics.Font font, NGraphics.Color textColor, double middleRadian)
		{
			var valueCenter = new NGraphics.Point (center.X, center.Y);
			DrawText (Value.ToString("N"),canvas, valueCenter, radius, font, textColor, middleRadian);
		}

		void DrawText (string text,ICanvas canvas, NGraphics.Point center, double radius, NGraphics.Font font, NGraphics.Color textColor, double middleRadian)
		{
			if (!string.IsNullOrWhiteSpace (text))
			{
				double textRadius = radius * 0.67;
				NGraphics.Point centerText = new NGraphics.Point (center.X + Math.Cos (middleRadian) * textRadius, center.Y + Math.Sin (middleRadian) * textRadius);
				const double width = 40;
				const double height = 5;
				Rect rect = new Rect (centerText.X - width / 2.0, centerText.Y, width, height);
				canvas.DrawText (text, rect, font, NGraphics.TextAlignment.Center, new Pen (textColor), new SolidBrush (textColor));
			}
		}
	}

}

