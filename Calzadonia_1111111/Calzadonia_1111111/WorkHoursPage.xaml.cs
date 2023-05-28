using Calzadonia_1111111.Employee;
using Firebase.Database;
using Firebase.Database.Query;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calzadonia_1111111.Admin;
using Calzadonia_1111111.Table;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Calzadonia_1111111
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorkHoursPage : ContentPage
    {
        private string selectedEmployeeFullName;
        private DateTime fromDate;
        private DateTime toDate;


        public WorkHoursPage(string employeeFullName)
        {
            InitializeComponent();

            selectedEmployeeFullName = employeeFullName;

          
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            Admin1 admin1 = new Admin1();
            string link = admin1.link;

            // Получение графика работы сотрудника из Firebase по его FullName
            var firebaseClient = new FirebaseClient(link);
            var scheduleData = await firebaseClient
                .Child("schedules")
                .Child(selectedEmployeeFullName.Replace(".", ""))
                .OnceSingleAsync<Dictionary<string, Dictionary<string, string>>>();

            // Формирование списка для отображения в ListView
            var scheduleList = new List<string>();
            foreach (var monthEntry in scheduleData)
            {
                var month = monthEntry.Key;
                foreach (var dateEntry in monthEntry.Value)
                {
                    var date = dateEntry.Key;
                    var time = dateEntry.Value;

                    var scheduleItem = $"{date}, {time} ({month})";
                    scheduleList.Add(scheduleItem);
                }
            }

            // Привязка списка к ListView
            ScheduleListView.ItemsSource = scheduleList;
        }

        private void ScheduleListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            // Обработка выбранного элемента списка (при необходимости)
        }

        private void FromDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            fromDate = e.NewDate;
        }

        private void ToDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            toDate = e.NewDate;

            // Выполнить вычисления и отобразить результат
            CalculateTotalHoursAsync();
        }

        private async Task CalculateTotalHoursAsync()
        {
            Admin1 admin1 = new Admin1();
            string link1 = admin1.link;
            // Получение графика работы сотрудника из Firebase по его FullName
            var firebaseClient = new FirebaseClient(link1);
            var scheduleData = await firebaseClient
                .Child("schedules")
                .Child(selectedEmployeeFullName.Replace(".", ""))
                .OnceSingleAsync<Dictionary<string, Dictionary<string, string>>>();

            // Вычисление общего количества часов
            double totalHours = 0;
            foreach (var monthEntry in scheduleData)
            {
                foreach (var dateEntry in monthEntry.Value)
                {
                    var date = DateTime.ParseExact(dateEntry.Key, "dd-MM-yyyy", null);
                    var time = TimeSpan.Parse(dateEntry.Value);

                    // Проверка, входит ли дата в заданный промежуток
                    if (date >= fromDate && date <= toDate)
                    {
                        totalHours += time.TotalHours;
                    }
                }
            }

            // Отобразить общее количество часов
            TotalHoursLabel.Text = $"Общее количество часов: {totalHours}";
        }
    }
}






