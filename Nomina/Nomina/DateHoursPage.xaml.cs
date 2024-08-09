using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;
using System;

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

        private void CalcularButtonClick(object sender, RoutedEventArgs e)
        {
            double totalHoras = 0;
            int totalDias = 0;

            if (MondayCheckBox.IsChecked == true)
            {
                totalDias++;
                totalHoras += (MondayEndTimePicker.Time - MondayStartTimePicker.Time).TotalHours;
            }
            if (TuesdayCheckBox.IsChecked == true)
            {
                totalDias++;
                totalHoras += (TuesdayEndTimePicker.Time - TuesdayStartTimePicker.Time).TotalHours;
            }
            if (WednesdayCheckBox.IsChecked == true)
            {
                totalDias++;
                totalHoras += (WednesdayEndTimePicker.Time - WednesdayStartTimePicker.Time).TotalHours;
            }
            if (ThursdayCheckBox.IsChecked == true)
            {
                totalDias++;
                totalHoras += (ThursdayEndTimePicker.Time - ThursdayStartTimePicker.Time).TotalHours;
            }
            if (FridayCheckBox.IsChecked == true)
            {
                totalDias++;
                totalHoras += (FridayEndTimePicker.Time - FridayStartTimePicker.Time).TotalHours;
            }
            if (SaturdayCheckBox.IsChecked == true)
            {
                totalDias++;
                totalHoras += (SaturdayEndTimePicker.Time - SaturdayStartTimePicker.Time).TotalHours;
            }
            if (SundayCheckBox.IsChecked == true)
            {
                totalDias++;
                totalHoras += (SundayEndTimePicker.Time - SundayStartTimePicker.Time).TotalHours;
            }

            double totalPago = totalHoras * EmployeeViewModel.SueldoPorHora;
            double horasRedondeadas = Math.Round(totalHoras, 1); // Redondea a un decimal

            ContentDialog resultDialog = new ContentDialog()
            {
                Title = "Resultado de la nómina",
                Content = $"El empleado {EmployeeViewModel.Nombre} {EmployeeViewModel.Apellido} obtuvo {totalPago:C}. Trabajó un total de {horasRedondeadas} hora(s). Trabajó {totalDias} día(s).",
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            resultDialog.ShowAsync();
        }

    }
}
