using System;
using UIKit;
using System.Collections.Generic;
using NGraphics;
using CoreGraphics;
using System.Linq;
using Foundation;

namespace Cross.Pie.iOS
{
	public partial class CrossPie : UIView
	{
		List<PieItem> ListItems { get; set; } = new List<PieItem>();

		#region Constructor
		public CrossPie () : base () {}
		public CrossPie(CGRect frame) : base (frame) { }
		public CrossPie(NSCoder coder) : base(coder) { }
		public CrossPie(NSObjectFlag t) : base(t) { }
		public CrossPie (IntPtr handle) : base (handle) { }
		#endregion

		#region Properties
		public bool IsEnabled { get; set; } = true;
		public NGraphics.Color BackgroundColor { get; set; } = Colors.White;
		public NGraphics.Font Font { get; set; } = new NGraphics.Font();

		public NGraphics.Color TextColor { get; set; } = Colors.Black;

		public NGraphics.Color PercentColor { get; set; } = Colors.Black;

		public NGraphics.Color ValueColor { get; set; } = Colors.Black;

		public PieItem SelectedItem { get; set; }

		public string Title { get; set; } = string.Empty;


		public NGraphics.Color TitleColor { get; set; } = Colors.Black;

		public bool IsSingleSelectable { get; set; } = true;

		public bool IsTitleOnTop { get; set; } = true;

		public bool IsPercentVisible { get; set; } = true;

		public bool IsNameVisible { get; set; } = true;

		public bool IsValueVisible { get; set; } = true;

		public double StartAngle { get; set; }

		public double StartRadian
		{
			get
			{
				return (StartAngle / 180.0) * Math.PI;
			}
		}

		private double _pullRatio = 0.1;
		public double PullRatio {
			get{ return _pullRatio; }
			set
			{
				if (value < 0.0)
				{
					value = 0.0;
				}
				if (value > 1.0)
				{
					value = 1.0;
				}
				_pullRatio = value; 
			}
		}

		#endregion
		#region Draw
		public override void Draw (CoreGraphics.CGRect rect)
		{
			base.Draw (rect);
			using (CGContext context = UIGraphics.GetCurrentContext ()) 
			{
				context.SetAllowsAntialiasing(true);
				context.SetShouldAntialias(true);
				context.SetShouldSmoothFonts(true);

				var canvas = new CGContextCanvas (context);

				Draw (canvas, new NGraphics.Rect (rect.Left, rect.Top, rect.Width, rect.Height));
			}

		}
		void Draw(ICanvas canvas,NGraphics.Rect rect)
		{
			DrawBackground (canvas, rect);	

			LastRect = rect;
			// 0.65
			RadiusAll = Math.Min (rect.Width, rect.Height-textHeight) / 2.0;

			if (IsTitleOnTop)
			{
				Center = new NGraphics.Point (rect.Center.X, rect.Center.Y + textHeight);
			}
			else
			{
				Center = new NGraphics.Point (rect.Center.X, rect.Center.Y - textHeight);
			}

			double pullLength = RadiusAll.Value * PullRatio;
			double radius = RadiusAll.Value - (RadiusAll.Value *PullRatio);

			double sumRadian = StartRadian;
			foreach (var eachSegment in ListItems)
			{
				sumRadian = eachSegment.DrawSegment (canvas,rect.Center,radius, sumRadian,pullLength,Font,TextColor,IsNameVisible,PercentColor,IsPercentVisible,ValueColor,IsValueVisible);
			}

			Rect rectText;
			if (IsTitleOnTop)
			{
				rectText = new Rect (rect.Center.X, rect.Center.Y - RadiusAll.Value - textHeight / 4.0,20,20);
			}
			else
			{
				rectText = new Rect (rect.Center.X, rect.Center.Y + RadiusAll.Value + textHeight / 4.0, 20, 20);
			}
			canvas.DrawText (Title ?? "", rectText, new NGraphics.Font (), NGraphics.TextAlignment.Center, new Pen (TitleColor), new SolidBrush (TitleColor));

		}
		#endregion

		#region methods
		public void Clear()
		{
			ListItems.Clear ();
		}

		public void ClearAllPull()
		{
			foreach (var each in ListItems)
			{
				each.IsPull = false;
			}
		}

		public bool Add(PieItem add)
		{
			if (add == null)
				return false;
			if (add.Value <= 0.0)
				return false;
			add.Index = ListItems.Count; 
			if (add.Color == null)
			{
				add.Color = MakeDefaultColor ();
			}
			ListItems.Add (add);
			return true;
		}

		public bool AddRange(IEnumerable<PieItem> pieItems)
		{
			bool isSuccess = true;
			foreach (var each in pieItems)
			{
				isSuccess = isSuccess && Add (each);
			}
			return isSuccess;
		}

		NGraphics.Color MakeDefaultColor()
		{
			int index = ListItems.Count % 24;
			double Hue;
			double Saturate;
			if (index < 12)
			{
				Hue = index * 0.08;
				Saturate = 1.0;
			}
			else
			{
				Hue = index * 0.04;
				Saturate = 0.5;
			}
			Hue = Hue - (double)(int)Hue;
			return NGraphics.Color.FromHSB(Hue, Saturate, 1.0);

		}
		public bool Update()
		{
			if (ListItems == null)
				return false;
			if (ListItems.Count <= 0)
				return false;
			var sumValue = ListItems.Aggregate<PieItem,double>(0.0,(sum,eachObject)=>sum+eachObject.Value);

			foreach (var each in ListItems)
			{
				each.Prepare (sumValue);
			}
			this.SetNeedsDisplay ();

			return true;
		}

		NGraphics.Point? Center { get; set; }
		double? RadiusAll { get; set; }
		const int textHeight = 80;

		public event EventHandler<PieItem> ItemSelected;

		NGraphics.Rect LastRect { get; set; }

		void DrawBackground(ICanvas canvas, NGraphics.Rect rect)
		{
			canvas.DrawRectangle (rect, new Pen (BackgroundColor), new SolidBrush (BackgroundColor));
		}


		NGraphics.Point TouchPoint(NGraphics.Point point)
		{
			double newX = point.X * LastRect.Width / Bounds.Width;
			double newY = point.Y * LastRect.Height / Bounds.Height;
			return new NGraphics.Point (newX, newY);
		}

		public override void TouchesBegan (Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan (touches, evt);

			if (!IsEnabled) return;

			try
			{
				UITouch touch = touches.ToArray<UITouch>()[0];
				CGPoint cgPoint = touch.LocationInView(this);

				NGraphics.Point pointReal = new NGraphics.Point(cgPoint.X,cgPoint.Y);
				NGraphics.Point point;
				if(IsTitleOnTop)
				{
					point = new NGraphics.Point(pointReal.X,pointReal.Y+textHeight);
				}
				else
				{
					point = new NGraphics.Point(pointReal.X,pointReal.Y-textHeight);
				}

				if(Center.HasValue && RadiusAll.HasValue)
				{
					double selectedRadian = RadianOfPoint(point);

					double sumRadian = 0.0;
					foreach (var eachSegment in ListItems)
					{
						if(sumRadian <= selectedRadian && selectedRadian <= sumRadian+eachSegment.Radian)
						{
							eachSegment.IsSelected = true;
							SelectedItem = eachSegment;
							ItemSelected?.Invoke(this,eachSegment);
						}
						sumRadian += eachSegment.Radian;
					}
				}
			}
			// Analysis disable once EmptyGeneralCatchClause
			catch(Exception exc)
			{
				#if DEBUG
				throw;
				#endif
			}

		}

		double RadianOfPoint(NGraphics.Point point)
		{
			if (!Center.HasValue)
				return 0;
			double dx = point.X - Center.Value.X;
			double dy = point.Y - Center.Value.Y;

			double rawRadian = Math.Atan2 (dy, dx);
			double radianBeforePhase;
			if (dy >= 0.0)
			{
				radianBeforePhase = rawRadian;

			}
			else
			{
				radianBeforePhase = 2 * Math.PI + rawRadian;
			}
			double radianFinal = radianBeforePhase - StartRadian;
			if (radianFinal < 0.0)
			{
				return radianFinal + 2 * Math.PI;
			}
			else
			{
				return radianFinal;
			}
		}

		#endregion

	}
}

