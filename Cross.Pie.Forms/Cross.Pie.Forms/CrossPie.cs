using System;
using NControl.Abstractions;
using System.Collections.Generic;
using NGraphics;
using System.Linq;
using Xamarin.Forms;
using System.Xml.Serialization;

namespace Cross.Pie.Forms
{

	public sealed class CrossPie : NControlView
	{
		List<PieItem> ListItems { get; set; } = new List<PieItem>();

		#region BindableProperty
		public static readonly BindableProperty FontProperty = BindableProperty.Create<CrossPie,NGraphics.Font> (a=>a.Font, new NGraphics.Font());
		public NGraphics.Font Font {
			get{ return (NGraphics.Font)GetValue (FontProperty); }
			set{ SetValue (FontProperty, value); }
		}

		public static readonly BindableProperty TextColorProperty = BindableProperty.Create<CrossPie,Xamarin.Forms.Color> (a=>a.TextColor, Xamarin.Forms.Color.Black);
		public Xamarin.Forms.Color TextColor {
			get{ return (Xamarin.Forms.Color)GetValue (TextColorProperty); }
			set{ SetValue (TextColorProperty, value); }
		}

		public static readonly BindableProperty PercentColorProperty = BindableProperty.Create<CrossPie,Xamarin.Forms.Color> (a=>a.PercentColor, Xamarin.Forms.Color.Black);
		public Xamarin.Forms.Color PercentColor {
			get{ return (Xamarin.Forms.Color)GetValue (PercentColorProperty); }
			set{ SetValue (PercentColorProperty, value); }
		}

		public static readonly BindableProperty ValueColorProperty = BindableProperty.Create<CrossPie,Xamarin.Forms.Color> (a=>a.ValueColor, Xamarin.Forms.Color.Black);
		public Xamarin.Forms.Color ValueColor {
			get{ return (Xamarin.Forms.Color)GetValue (ValueColorProperty); }
			set{ SetValue (ValueColorProperty, value); }
		}

		public static readonly BindableProperty SelectedItemProperty = BindableProperty.Create<CrossPie,PieItem> (a=>a.SelectedItem, null);
		public PieItem SelectedItem {
			get{ return (PieItem)GetValue (SelectedItemProperty); }
			set{ SetValue (SelectedItemProperty, value); }
		}

		public static readonly BindableProperty TitleProperty = BindableProperty.Create<CrossPie,string> (a=>a.Title, string.Empty);
		public string Title {
			get{ return (string)GetValue (TitleProperty); }
			set{ SetValue (TitleProperty, value); }
		}


		public static readonly BindableProperty TitleColorProperty = BindableProperty.Create<CrossPie,Xamarin.Forms.Color> (a=>a.TitleColor, Xamarin.Forms.Color.Black);
		public Xamarin.Forms.Color TitleColor {
			get{ return (Xamarin.Forms.Color)GetValue (TitleColorProperty); }
			set{ SetValue (TitleColorProperty, value); }
		}
			
		public static readonly BindableProperty IsSingleSelectableProperty = BindableProperty.Create<CrossPie,bool> (a=>a.IsSingleSelectable, true);
		public bool IsSingleSelectable {
			get{ return (bool)GetValue (IsSingleSelectableProperty); }
			set{ SetValue (IsSingleSelectableProperty, value); }
		}

		public static readonly BindableProperty IsTitleOnTopProperty = BindableProperty.Create<CrossPie,bool> (a=>a.IsTitleOnTop, true);
		public bool IsTitleOnTop {
			get{ return (bool)GetValue (IsTitleOnTopProperty); }
			set{ SetValue (IsTitleOnTopProperty, value); }
		}

		public static readonly BindableProperty IsPercentVisibleProperty = BindableProperty.Create<CrossPie,bool> (a=>a.IsPercentVisible, true);
		public bool IsPercentVisible {
			get{ return (bool)GetValue (IsPercentVisibleProperty); }
			set{ SetValue (IsPercentVisibleProperty, value); }
		}

		public static readonly BindableProperty IsNameVisibleProperty = BindableProperty.Create<CrossPie,bool> (a=>a.IsNameVisible, true);
		public bool IsNameVisible {
			get{ return (bool)GetValue (IsNameVisibleProperty); }
			set{ SetValue (IsNameVisibleProperty, value); }
		}

		public static readonly BindableProperty IsValueVisibleProperty = BindableProperty.Create<CrossPie,bool> (a=>a.IsValueVisible, true);
		public bool IsValueVisible {
			get{ return (bool)GetValue (IsValueVisibleProperty); }
			set{ SetValue (IsValueVisibleProperty, value); }
		}

		public static readonly BindableProperty StartAngleProperty = BindableProperty.Create<CrossPie,double> (a=>a.StartAngle, 0.0);
		public double StartAngle {
			get{ return (double)GetValue (StartAngleProperty); }
			set{ SetValue (StartAngleProperty, value); }
		}

		public double StartRadian
		{
			get
			{
				return (StartAngle / 180.0) * Math.PI;
			}
		}

		public static readonly BindableProperty PullRatioProperty = BindableProperty.Create<CrossPie,double> (a=>a.PullRatio, 0.1);
		public double PullRatio {
			get{ return (double)GetValue (PullRatioProperty); }
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
				SetValue (PullRatioProperty, value); 
			}
		}
		#endregion	

		public CrossPie ()
		{
			VerticalOptions = LayoutOptions.Fill;
			HorizontalOptions = LayoutOptions.Fill;
		}

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

		Xamarin.Forms.Color MakeDefaultColor()
		{
			#if LIMIT
			return Xamarin.Forms.Color.FromHsla (0.14*ListItems.Count, 1, 0.5,1.0);
				
			#else
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
			return Xamarin.Forms.Color.FromHsla (Hue, Saturate, 0.5,1.0);
			#endif
		}
		public bool Update()
		{
			#if TRIAL
			var mem = Plugin.Settings.CrossSettings.Current;
			const int TrialLimit = 30;
			int TrialLeft = mem.GetValueOrDefault<int> ("Cross.Pie.Forms.Pro.Trial.CountDown", TrialLimit);
			if (TrialLeft <= 0)
			{
				Xamarin.Forms.Element element = this;
				Page ifPage = null;
				do
				{
					element = element.Parent;
					ifPage = element as ContentPage;
					if(ifPage != null)
					{
						Device.BeginInvokeOnMainThread(()=>
						{
							// Analysis disable once ConvertToLambdaExpression
							ifPage.DisplayAlert("Limit",string.Format("Trial version limit to {0} times.",TrialLimit),"OK");
						});
						break;
					}
				}while(element != null);
				return false;
			}
			TrialLeft--;
			mem.AddOrUpdateValue<int>("Cross.Pie.Forms.Pro.Trial.CountDown",TrialLeft);
			#endif

			if (ListItems == null)
				return false;
			if (ListItems.Count <= 0)
				return false;
			var sumValue = ListItems.Aggregate<PieItem,double>(0.0,(sum,eachObject)=>sum+eachObject.Value);

			foreach (var each in ListItems)
			{
				each.Prepare (sumValue);
			}
			Invalidate ();

			return true;
		}

		NGraphics.Point? Center { get; set; }
		double? RadiusAll { get; set; }
		const int textHeight = 80;

		public event EventHandler<PieItem> ItemSelected;

		NGraphics.Rect LastRect { get; set; }

		void DrawBackground(ICanvas canvas, NGraphics.Rect rect)
		{
			canvas.DrawRectangle (rect, new Pen (BackgroundColor.ToCross ()), new SolidBrush (BackgroundColor.ToCross ()));
		}

		public override void Draw (NGraphics.ICanvas canvas, NGraphics.Rect rect)
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
			canvas.DrawText (Title ?? "", rectText, new NGraphics.Font (), NGraphics.TextAlignment.Center, new Pen (TitleColor.ToCross()), new SolidBrush (TitleColor.ToCross()));
		}
		NGraphics.Point TouchPoint(NGraphics.Point point)
		{
			double newX = point.X * LastRect.Width / Width;
			double newY = point.Y * LastRect.Height / Height;
			return new NGraphics.Point (newX, newY);
		}
		public override bool TouchesBegan (IEnumerable<NGraphics.Point> points)
		{
			if (!IsEnabled) return base.TouchesBegan (points);

			try
			{
				var pointReal = TouchPoint(points.First ());

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
			return base.TouchesBegan (points);
		}
		double RadianOfPoint(NGraphics.Point point)
		{
			if (!Center.HasValue)
				return 0;
			double dx = point.X - Center.Value.X;
			double dy = point.Y - Center.Value.Y;

			double rawRadian = Math.Atan2 (dy, dx);
			if (dy >= 0.0)
			{
				return rawRadian;

			}
			else
			{
				return 2 * Math.PI + rawRadian;
			}

		}


		public override bool TouchesMoved (IEnumerable<NGraphics.Point> points)
		{
			return base.TouchesMoved (points);
		}
		public override bool TouchesEnded (IEnumerable<NGraphics.Point> points)
		{
			return base.TouchesEnded (points);
		}
		public override bool TouchesCancelled (IEnumerable<NGraphics.Point> points)
		{
			return base.TouchesCancelled (points);
		}
	}
}

