using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Xamarin.Forms;

using System.Collections.ObjectModel;

using Firebase.Database.Query;

using Xamarin.Forms.Xaml;
using Calzadonia_1111111.Table;
using System.Windows.Input;


namespace Calzadonia_1111111.Employee
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SchedulePage : ContentPage
    {
        private readonly FirebaseClient firebaseClient;
        private ObservableCollection<string> scheduleList;

        public SchedulePage(string selectedEmployeeFullName)
        {
            InitializeComponent();
            Admin1 admin1 = new Admin1();
            string link4 = admin1.link;
            // Initialize FirebaseClient
            firebaseClient = new FirebaseClient(link4);

            scheduleList = new ObservableCollection<string>();
            ScheduleListView.ItemsSource = scheduleList;

            LoadSchedule(selectedEmployeeFullName);
        }

        private async void LoadSchedule(string selectedEmployeeFullName)
        {
            try
            {
                var schedules = await firebaseClient
                    .Child("schedules")
                    .Child(selectedEmployeeFullName.Replace(".", ""))
                    .OnceSingleAsync<Dictionary<string, Dictionary<string, string>>>();

                // Clear previous schedule list
                scheduleList.Clear();

                // Populate the list with months, dates, and times
                foreach (var monthSchedule in schedules)
                {
                    string month = monthSchedule.Key;
                    var dateAndTime = monthSchedule.Value;

                    scheduleList.Add(month);

                    foreach (var item in dateAndTime)
                    {
                        string date = item.Key;
                        string time = item.Value;
                        scheduleList.Add($"- {date}: {time}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading schedule: {ex.Message}");
            }
        }
    }





}





