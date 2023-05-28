using Calzadonia_1111111.Table;
using Calzadonia_1111111.Admin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Firebase.Database;
using Firebase.Database.Query;



namespace Calzadonia_1111111
{
    public partial class MainPage : ContentPage
    {
        private readonly FirebaseClient firebaseClient;
        public MainPage()
        {
            Admin1 admin1 = new Admin1();
            string link3 = admin1.link;
            InitializeComponent();
            firebaseClient = new FirebaseClient(link3);
        }
        private async void LoginButton_Clicked(object sender, EventArgs e)
        {
            var username = UsernameEntry.Text;
            var password = PasswordEntry.Text;

            // Perform authentication check against the Firebase database
            var employees = await firebaseClient.Child("employees").OnceAsync<Dictionary<string, string>>();
            foreach (var employee in employees)
            {
                var dbUsername = employee.Object["Username"];
                var dbPassword = employee.Object["Password"];
                var dbFullName = employee.Object["FullName"];

                if (username == dbUsername && password == dbPassword)
                {
                    await Navigation.PushAsync(new EmployeePage(dbFullName));
                    return;
                }
            }

            await DisplayAlert("Ошибка", "Неверный логин или пароль", "OK");
        }

        private async void AdminLoginButton_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AdminLoginPage());
        }
    }

   


}



