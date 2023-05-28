using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calzadonia_1111111.Table;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;



namespace Calzadonia_1111111.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeeSchedulePage : ContentPage
    {
        private FirebaseClient firebaseClient;
        private Employee selectedEmployee;
        private DateTime selectedMonthStartDate;
        private string fullName;

        public EmployeeSchedulePage(Employee employee)
        {
            InitializeComponent();
            BindingContext = this;
            Admin1 admin1 = new Admin1();
            string link6 = admin1.link;

            firebaseClient = new FirebaseClient(link6);
            selectedEmployee = employee;
            selectedMonthStartDate = DateTime.Now.Date.AddDays(1 - DateTime.Now.Day);

            InitializeMonthPicker();
            InitializeWeekPicker();
        }

        public EmployeeSchedulePage(string fullName)
        {
            this.fullName = fullName;
        }

        private void InitializeMonthPicker()
        {
            for (int i = 1; i <= 12; i++)
            {
                var month = new DateTime(DateTime.Now.Year, i, 1).ToString("MMMM");
                MonthPicker.Items.Add(month);
            }
            MonthPicker.SelectedIndex = DateTime.Now.Month - 1;
        }

        private void InitializeWeekPicker()
        {
            WeekPicker.Items.Add("1 неделя");
            WeekPicker.Items.Add("2 неделя");
            WeekPicker.Items.Add("3 неделя");
            WeekPicker.Items.Add("4 неделя");
        }

        private void MonthPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWeekPicker();
            UpdateWeekDaysLayout();
        }

        private void WeekPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateWeekDaysLayout();
        }

        private void UpdateWeekPicker()
        {
            var selectedMonth = MonthPicker.SelectedIndex + 1;
            var totalWeeksInMonth = GetTotalWeeksInMonth(selectedMonth);

            WeekPicker.Items.Clear();
            for (int i = 1; i <= totalWeeksInMonth; i++)
            {
                WeekPicker.Items.Add($"{i} неделя");
            }
            WeekPicker.SelectedIndex = 0;
        }

        private int GetTotalWeeksInMonth(int month)
        {
            var firstDayOfMonth = new DateTime(DateTime.Now.Year, month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            var firstWeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(firstDayOfMonth, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var lastWeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(lastDayOfMonth, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            return lastWeekNumber - firstWeekNumber + 1;
        }

        private void UpdateWeekDaysLayout()
        {
            var selectedMonth = MonthPicker.SelectedIndex + 1;
            var selectedWeek = WeekPicker.SelectedIndex + 1;

            var startDate = GetStartDateOfWeek(selectedMonth, selectedWeek);
            var endDate = startDate.AddDays(6);

            MondayDatePicker.Date = new DateTime(startDate.Year, selectedMonth, startDate.Day);
            MondayTimePicker.Time = TimeSpan.Zero;

            TuesdayDatePicker.Date = new DateTime(startDate.Year, selectedMonth, startDate.AddDays(1).Day);
            TuesdayTimePicker.Time = TimeSpan.Zero;

            WednesdayDatePicker.Date = new DateTime(startDate.Year, selectedMonth, startDate.AddDays(2).Day);
            WednesdayTimePicker.Time = TimeSpan.Zero;

            ThursdayDatePicker.Date = new DateTime(startDate.Year, selectedMonth, startDate.AddDays(3).Day);
            ThursdayTimePicker.Time = TimeSpan.Zero;

            FridayDatePicker.Date = new DateTime(startDate.Year, selectedMonth, startDate.AddDays(4).Day);
            FridayTimePicker.Time = TimeSpan.Zero;

            SaturdayDatePicker.Date = new DateTime(startDate.Year, selectedMonth, startDate.AddDays(5).Day);
            SaturdayTimePicker.Time = TimeSpan.Zero;

            SundayDatePicker.Date = new DateTime(startDate.Year, selectedMonth, startDate.AddDays(6).Day);
            SundayTimePicker.Time = TimeSpan.Zero;

            WeekDaysLayout.IsVisible = true;
        }





        private DateTime GetStartDateOfWeek(int month, int week)
        {
            var currentDate = DateTime.Now.Date;
            var currentMonth = currentDate.Month;
            var currentYear = currentDate.Year;

            var firstDayOfMonth = new DateTime(currentYear, month, 1);
            var firstWeekNumber = CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(firstDayOfMonth, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var startDate = firstDayOfMonth;
            if (week > 1)
            {
                startDate = firstDayOfMonth.AddDays((week - firstWeekNumber) * 7);
            }

            // Проверяем, если текущий месяц отличается от месяца выбранного дня начала недели,
            // то меняем год и месяц на текущие, чтобы сохранить исходный месяц
            if (startDate.Month != currentMonth)
            {
                startDate = new DateTime(currentYear, currentMonth, startDate.Day);
            }

            while (startDate.DayOfWeek != DayOfWeek.Monday)
            {
                startDate = startDate.AddDays(1);
            }

            return startDate;
        }




        private async void SaveButton_Clicked(object sender, EventArgs e)
        {

            var selectedMonth = MonthPicker.SelectedIndex + 1;
            var selectedWeek = WeekPicker.SelectedIndex + 1;

            var yearMonth = DateTime.Now.ToString("MM-yyyy");
            var scheduleId = $"{selectedMonth}-{yearMonth}-{selectedWeek}";

            var startDate = GetStartDateOfWeek(selectedMonth, selectedWeek);
            var schedule = new Dictionary<string, string>();

            TimePicker[] timePickers = { MondayTimePicker, TuesdayTimePicker, WednesdayTimePicker, ThursdayTimePicker, FridayTimePicker, SaturdayTimePicker, SundayTimePicker };

            for (int i = 0; i < 7; i++)
            {
                var currentDate = startDate.AddDays(i);
                var formattedDate = currentDate.ToString("dd-MM-yyyy");
                var selectedTimePicker = timePickers[i];

                schedule[formattedDate] = selectedTimePicker.Time.ToString();
            }

            await firebaseClient.Child("schedules").Child(selectedEmployee.FullName.Replace(".", "")).Child(scheduleId).PutAsync(schedule);

            await DisplayAlert("Успех", "График работы сохранен", "OK");
        }
    }
}
