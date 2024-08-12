using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
                // Restaurar estado previo
                SueldoHoraTextBox.Text = EmployeeViewModel.SueldoPorHora.ToString();
                MondayCheckBox.IsChecked = EmployeeViewModel.LunesHoras > 0;
                TuesdayCheckBox.IsChecked = EmployeeViewModel.MartesHoras > 0;
                WednesdayCheckBox.IsChecked = EmployeeViewModel.MiercolesHoras > 0;
                ThursdayCheckBox.IsChecked = EmployeeViewModel.JuevesHoras > 0;
                FridayCheckBox.IsChecked = EmployeeViewModel.ViernesHoras > 0;
                SaturdayCheckBox.IsChecked = EmployeeViewModel.SabadoHoras > 0;
                SundayCheckBox.IsChecked = EmployeeViewModel.DomingoHoras > 0;
            }
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

            double totalHoras = daysWorked.Values.Sum();
            if (double.TryParse(SueldoHoraTextBox.Text, out double sueldoPorHora))
            {
                EmployeeViewModel.SueldoPorHora = sueldoPorHora;
            }
            double totalPago = totalHoras * EmployeeViewModel.SueldoPorHora;

            // Guardar el estado
            EmployeeViewModel.LunesHoras = daysWorked.ContainsKey("Lunes") ? daysWorked["Lunes"] : 0;
            EmployeeViewModel.MartesHoras = daysWorked.ContainsKey("Martes") ? daysWorked["Martes"] : 0;
            EmployeeViewModel.MiercolesHoras = daysWorked.ContainsKey("Miércoles") ? daysWorked["Miércoles"] : 0;
            EmployeeViewModel.JuevesHoras = daysWorked.ContainsKey("Jueves") ? daysWorked["Jueves"] : 0;
            EmployeeViewModel.ViernesHoras = daysWorked.ContainsKey("Viernes") ? daysWorked["Viernes"] : 0;
            EmployeeViewModel.SabadoHoras = daysWorked.ContainsKey("Sábado") ? daysWorked["Sábado"] : 0;
            EmployeeViewModel.DomingoHoras = daysWorked.ContainsKey("Domingo") ? daysWorked["Domingo"] : 0;

            var parameters = new
            {
                TotalHoras = totalHoras,
                TotalPago = totalPago,
                DaysWorked = daysWorked,
                EmployeeViewModel = this.EmployeeViewModel
            };

            Frame.Navigate(typeof(ResultsPage), parameters, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        private async Task<double> AskForHoursAsync(string day)
        {
            TextBox inputTextBox = new TextBox();
            ContentDialog dialog = new ContentDialog()
            {
                Title = $"¿Cuántas horas trabajaste el {day.ToLower()}?",
                Content = inputTextBox,
                PrimaryButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };

            var result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary && double.TryParse(inputTextBox.Text, out double hours))
            {
                return hours;
            }
            return 0;
        }

        private void AtrasButtonClick(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.Navigate(typeof(FieldPage), EmployeeViewModel, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromLeft });
            }
        }

        private void SueldoHoraTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(SueldoHoraTextBox.Text, out _))
            {
                HideErrorWithAnimation(SueldoHoraErrorTextBlock);
            }
            else
            {
                ShowErrorWithAnimation(SueldoHoraErrorTextBlock);
            }
        }

        private void ShowErrorWithAnimation(TextBlock errorTextBlock)
        {
            errorTextBlock.Visibility = Visibility.Visible;
            Storyboard storyboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 0,
                To = 1,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut }
            };
            Storyboard.SetTarget(animation, errorTextBlock);
            Storyboard.SetTargetProperty(animation, "Opacity");
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }

        private void HideErrorWithAnimation(TextBlock errorTextBlock)
        {
            Storyboard storyboard = new Storyboard();
            DoubleAnimation animation = new DoubleAnimation()
            {
                From = 1,
                To = 0,
                Duration = new Duration(TimeSpan.FromSeconds(0.3)),
                EasingFunction = new CircleEase() { EasingMode = EasingMode.EaseInOut }
            };
            animation.Completed += (s, e) => { errorTextBlock.Visibility = Visibility.Collapsed; };
            Storyboard.SetTarget(animation, errorTextBlock);
            Storyboard.SetTargetProperty(animation, "Opacity");
            storyboard.Children.Add(animation);
            storyboard.Begin();
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            SueldoHoraTextBox.Text = string.Empty;

            MondayCheckBox.IsChecked = false;
            TuesdayCheckBox.IsChecked = false;
            WednesdayCheckBox.IsChecked = false;
            ThursdayCheckBox.IsChecked = false;
            FridayCheckBox.IsChecked = false;
            SaturdayCheckBox.IsChecked = false;
            SundayCheckBox.IsChecked = false;
        }
    }
}
