using System.Collections.Generic;
using Xamarin.Forms;

namespace XamForms.Controls
{
	public partial class Calendar
	{
		public static readonly BindableProperty WeeklyEventsProperty =
			BindableProperty.Create(
				nameof(WeeklyEvents),
				typeof(ICollection<WeeklyEvent>),
				typeof(Calendar),
				new List<WeeklyEvent>(),
				propertyChanged: (bindable, oldValue, newValue) => (bindable as Calendar)?.ChangeCalendar(CalandarChanges.MaxMin));

		public ICollection<WeeklyEvent> WeeklyEvents
		{
			get { return (ICollection<WeeklyEvent>)GetValue(WeeklyEventsProperty); }
			set { SetValue(WeeklyEventsProperty, value); }
		}
	}
}
