using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Firebase.Database;
using Firebase.Database.Query;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Calzadonia_1111111.Table;
using System.Windows.Input;

namespace Calzadonia_1111111.Employee
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmployeesPage : ContentPage
    {
        private FirebaseClient firebaseClient;
        private List<Employee> employees;

        public EmployeesPage()
        {
            InitializeComponent();
            firebaseClient = new FirebaseClient("https://hackers-df577-default-rtdb.firebaseio.com/");
            employees = new List<Employee>();
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await LoadEmployees();
        }

        private async Task LoadEmployees()
        {
            employees = (await firebaseClient.Child("employees").OnceAsync<Employee>()).Select(x => new Employee
            {
                FullName = x.Key,
                Username = x.Object.Username,
                Password = x.Object.Password,
                IsSelected = false
            }).ToList();
            EmployeesListView.ItemsSource = employees;
        }

        private async void AddEmployeeButton_Clicked(object sender, EventArgs e)
        {
            var employeeName = await DisplayPromptAsync("Добавить сотрудника", "Введите имя сотрудника", "Добавить", "Отмена");
            var employeeUsername = await DisplayPromptAsync("Добавить сотрудника", "Введите логин сотрудника", "Добавить", "Отмена");
            var employeePassword = await DisplayPromptAsync("Добавить сотрудника", "Введите пароль сотрудника", "Добавить", "Отмена");

            if (!string.IsNullOrEmpty(employeeName) && !string.IsNullOrEmpty(employeeUsername) && !string.IsNullOrEmpty(employeePassword))
            {
                var newEmployee = new Employee
                {
                    FullName = employeeName,
                    Username = employeeUsername,
                    Password = employeePassword
                };

                await firebaseClient.Child("employees").Child(newEmployee.FullName).PutAsync(newEmployee);
                employees.Add(newEmployee);
                EmployeesListView.ItemsSource = null;
                EmployeesListView.ItemsSource = employees;
            }
        }

        private async void DeleteButton_Clicked(object sender, EventArgs e)
        {
            var selectedEmployees = employees.Where(x => x.IsSelected).ToList();
            foreach (var selectedEmployee in selectedEmployees)
            {
                await firebaseClient.Child("employees").Child(selectedEmployee.FullName).DeleteAsync();
                employees.Remove(selectedEmployee);

                await firebaseClient.Child("schedules").Child(selectedEmployee.FullName).DeleteAsync();
                employees.Remove(selectedEmployee);


            }

            EmployeesListView.ItemsSource = null;
            EmployeesListView.ItemsSource = employees;
        }

        private async void ViewScheduleButton_Clicked(object sender, EventArgs e)
        {
            var button = sender as ImageButton;
            var selectedEmployee = button?.BindingContext as Employee;

            if (selectedEmployee != null)
            {
                await Navigation.PushAsync(new SchedulePage(selectedEmployee.FullName));

            }
        }


        private async void EditButton_Clicked(object sender, EventArgs e)
        {
            var selectedEmployee = employees.FirstOrDefault(x => x.IsSelected);
            if (selectedEmployee != null)
            {
                var employeeName = await DisplayPromptAsync("Изменить сотрудника", "Введите новое имя сотрудника", "Сохранить", "Отмена", selectedEmployee.FullName);
                var employeeUsername = await DisplayPromptAsync("Изменить сотрудника", "Введите новый логин сотрудника", "Сохранить", "Отмена", selectedEmployee.Username);
                var employeePassword = await DisplayPromptAsync("Изменить сотрудника", "Введите новый пароль сотрудника", "Сохранить", "Отмена", selectedEmployee.Password);

                if (!string.IsNullOrEmpty(employeeName) && !string.IsNullOrEmpty(employeeUsername) && !string.IsNullOrEmpty(employeePassword))
                {
                    selectedEmployee.FullName = employeeName;
                    selectedEmployee.Username = employeeUsername;
                    selectedEmployee.Password = employeePassword;

                    await firebaseClient.Child("employees").Child(selectedEmployee.FullName).PutAsync(selectedEmployee);
                    EmployeesListView.ItemsSource = null;
                    EmployeesListView.ItemsSource = employees;
                }
            }
        }

        private async void EmployeesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var selectedEmployee = e.SelectedItem as Employee;
            if (selectedEmployee != null)
            {
                await Navigation.PushAsync(new EmployeeSchedulePage(selectedEmployee));
                EmployeesListView.SelectedItem = null;
            }
            else
            {

            }
        }

        private async void LogoutButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopToRootAsync();
        }
    }
}