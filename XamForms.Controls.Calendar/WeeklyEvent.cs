using System;
using Xamarin.Forms;

namespace XamForms.Controls
{
	public class WeeklyEvent : SpecialDate
	{
		public WeeklyEvent()
		{
			BackgroundColor = Color.HotPink;
			TextColor = Color.White;
		}

		public DayOfWeek DayOfWeek { get; set; }
	}
}
