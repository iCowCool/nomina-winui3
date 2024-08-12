using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Nomina
{
    public sealed partial class DateHoursPage : Page
    {
        public EmployeeViewModel EmployeeViewModel { get; set; }

        public DateHoursPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is EmployeeViewModel viewModel)
            {
                EmployeeViewModel = viewModel;
            }
        }

        private void AtrasButtonClick(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(FieldPage), EmployeeViewModel, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
        }

        private async void CalcularButtonClick(object sender, RoutedEventArgs e)
        {
            var daysWorked = new Dictionary<string, double>();

            if (MondayCheckBox.IsChecked == true)
            {
                double hours = await AskForHoursAsync("Lunes");
                daysWorked.Add("Lunes", hours);
            }
            if (TuesdayCheckBox.IsChecked == true)
            {
                double hours = await AskForHoursAsync("Martes");
                daysWorked.Add("Martes", hours);
            }
            if (WednesdayCheckBox.IsChecked == true)
            {
                double hours = await AskForHoursAsync("Miércoles");
                daysWorked.Add("Miércoles", hours);
            }
            if (ThursdayCheckBox.IsChecked == true)
            {
                double hours = await AskForHoursAsync("Jueves");
                daysWorked.Add("Jueves", hours);
            }
            if (FridayCheckBox.IsChecked == true)
            {
                double hours = await AskForHoursAsync("Viernes");
                daysWorked.Add("Viernes", hours);
            }
            if (SaturdayCheckBox.IsChecked == true)
            {
                double hours = await AskForHoursAsync("Sábado");
                daysWorked.Add("Sábado", hours);
            }
            if (SundayCheckBox.IsChecked == true)
            {
                double hours = await AskForHoursAsync("Domingo");
                daysWorked.Add("Domingo", hours);
            }

            // Calcular el total de horas trabajadas y la paga total
            double totalHoras = daysWorked.Values.Sum();
            double totalPago = totalHoras * EmployeeViewModel.SueldoPorHora;

            // Pasar los resultados a la nueva página
            var parameters = new
            {
                TotalHoras = totalHoras,
                TotalPago = totalPago,
                DaysWorked = daysWorked
            };

            Frame.Navigate(typeof(ResultsPage), parameters, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private async Task<double> AskForHoursAsync(string day)
        {
            TextBox inputTextBox = new TextBox();
            ContentDialog dialog = new ContentDialog()
            {
                Title = $"¿Cuántas horas trabajaste el {day}?",
                Content = inputTextBox,
                PrimaryButtonText = "OK",
                CloseButtonText = "Cancelar",
                XamlRoot = this.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary && double.TryParse(inputTextBox.Text, out double hours))
            {
                return hours;
            }
            return 0;
        }

    }
}
