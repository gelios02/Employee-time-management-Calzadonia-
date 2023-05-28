using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Calzadonia_1111111.Employee;
using Calzadonia_1111111.Table;


using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Calzadonia_1111111.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminLoginPage : ContentPage
    {
        
            private string adminUsername = "admin";
            private string adminPassword = "password123";

            public AdminLoginPage()
            {
                InitializeComponent();
            }

            private async void LoginButton_Clicked(object sender, EventArgs e)
            {
                string username = UsernameEntry.Text;
                string password = PasswordEntry.Text;

                if (username == adminUsername && password == adminPassword)
                {
                    await Navigation.PushAsync(new EmployeesPage());
                }
                else
                {
                    await DisplayAlert("Ошибка", "Пароль неверный", "OK");
                }
            }

            private async void EmployeeLoginButton_Clicked(object sender, EventArgs e)
            {
                await Navigation.PushAsync(new MainPage());
            }
    }
}
