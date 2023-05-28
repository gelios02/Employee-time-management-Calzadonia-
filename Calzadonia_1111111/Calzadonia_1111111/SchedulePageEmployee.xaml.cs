using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Calzadonia_1111111
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SchedulePageEmployee : ContentPage
	{
        private string employeeFullName;

        public ObservableCollection<string> Schedule { get; set; }

        public SchedulePageEmployee(string fullName)
        {
            InitializeComponent();

            employeeFullName = fullName;
            Schedule = new ObservableCollection<string>();

            BindingContext = this;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Загрузка графика работы для выбранного сотрудника из базы данных
            var schedule = await LoadEmployeeSchedule(employeeFullName);

            if (schedule != null)
            {
                foreach (var month in schedule.Keys)
                {
                    var dates = schedule[month].Keys;
                    foreach (var date in dates)
                    {
                        var time = schedule[month][date];
                        var scheduleEntry = $"{month}: {date} - {time}";
                        Schedule.Add(scheduleEntry);
                    }
                }
            }
        }

        private async Task<Dictionary<string, Dictionary<string, string>>> LoadEmployeeSchedule(string fullName)
        {
            // Загрузка графика работы сотрудника из базы данных
            var firebaseClient = new FirebaseClient("https://hackers-df577-default-rtdb.firebaseio.com/");
            var schedule = await firebaseClient.Child("schedules").Child(fullName).OnceSingleAsync<Dictionary<string, Dictionary<string, string>>>();

            return schedule;
        }

    }
}