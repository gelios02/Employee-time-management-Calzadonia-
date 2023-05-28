using Calzadonia_1111111.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Calzadonia_1111111
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EmployeePage : ContentPage
	{
        public string fullName;

        public EmployeePage(string fullName)
        {
            InitializeComponent();

            this.fullName = fullName;
            FullNameLabel.Text = fullName;
        }

        private void ScheduleButton_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new SchedulePageEmployee(fullName));
        }

        private void WorkHoursButton_Clicked(object sender, EventArgs e)
        {
            // Handle button click for viewing work hours
            // Implement the logic as needed
            Navigation.PushAsync(new WorkHoursPage(fullName));
        }

       
        private void Exit_Clicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new MainPage());
        }
    }
}