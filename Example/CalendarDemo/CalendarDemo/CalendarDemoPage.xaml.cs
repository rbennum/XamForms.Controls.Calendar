using System;
using Xamarin.Forms;
using XamForms.Controls;

namespace CalendarDemo
{
    public partial class CalendarDemoPage : ContentPage
    {
        public CalendarDemoPage()
        {
            InitializeComponent();
            BindingContext = new CalendarDemoViewModel();

            TestCalendar.WeeklyEvents.Add(new WeeklyEvent
            {
	            DayOfWeek = DayOfWeek.Saturday
            });
            TestCalendar.WeeklyEvents.Add(new WeeklyEvent
            {
	            DayOfWeek = DayOfWeek.Sunday
            });
        }
    }
}
