using System;
using System.ComponentModel;
using Xamarin.Forms;
using System.Dynamic;

namespace Cross.Pie.Forms.Sample
{
	public class AreaModel : INotifyPropertyChanged
	{
		#region INotifyPropertyChanged implementation

		public event PropertyChangedEventHandler PropertyChanged;

		#endregion

		string _country = string.Empty;
		public string Country
		{
			get
			{
				return _country;
			}
			set
			{
				_country = value;
				PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof(Country)));

			}
		}

		Color _color = Color.Black;
		public Color Color
		{
			get
			{
				return _color;
			}
			set
			{
				_color = value;
				PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(Color)));
			}
		}

		Color? _text_color = null;
		public Color? TextColor
		{
			get
			{
				return _text_color;
			}
			set
			{
				_text_color = value;
				PropertyChanged?.Invoke(this,new PropertyChangedEventArgs(nameof(TextColor)));
			}
		}

		double _value = 0.0;
		public double Value
		{
			get
			{
				return _value;
			}
			set
			{
				_value = value;
				PropertyChanged?.Invoke (this, new PropertyChangedEventArgs (nameof (Value)));
			}
		}
		public PieItem Pie { get; set; }
		public PieItem ToPieItem()
		{
			Pie = new PieItem { 
				Title = Country ?? "",
				Value = this.Value,
				Color = this.Color,
				Tag = this,
			};
			if (this.TextColor.HasValue)
			{
				Pie.TitleColor = Pie.PercentColor = Pie.ValueColor = this.TextColor.Value;
			}
			return Pie;
		}
	}
}

