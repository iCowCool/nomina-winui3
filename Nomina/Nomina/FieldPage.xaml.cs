using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;
using Microsoft.UI.Xaml.Navigation;
using System.Text.RegularExpressions;
using System;

namespace Nomina
{
    public sealed partial class FieldPage : Page
    {
        public EmployeeViewModel EmployeeViewModel { get; set; } = new EmployeeViewModel();

        public FieldPage()
        {
            this.InitializeComponent();
        }

        private void ClearButtonClick(object sender, RoutedEventArgs e)
        {
            NombreTextBox.Text = string.Empty;
            ApellidoTextBox.Text = string.Empty;
            NumeroTextBox.Text = string.Empty;
        }

        private void SiguienteButtonClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NombreTextBox.Text) ||
                string.IsNullOrWhiteSpace(ApellidoTextBox.Text) ||
                string.IsNullOrWhiteSpace(NumeroTextBox.Text))
            {
                ContentDialog missingFieldsDialog = new ContentDialog()
                {
                    Title = "Campos obligatorios faltantes",
                    Content = "Por favor, complete todos los campos obligatorios.",
                    CloseButtonText = "OK",
                    XamlRoot = this.Content.XamlRoot
                };
                missingFieldsDialog.ShowAsync();
                return;
            }

            EmployeeViewModel.Nombre = NombreTextBox.Text;
            EmployeeViewModel.Apellido = ApellidoTextBox.Text;
            EmployeeViewModel.NumeroEmpleado = NumeroTextBox.Text;

            Frame.Navigate(typeof(DateHoursPage), EmployeeViewModel, new SlideNavigationTransitionInfo() { Effect = SlideNavigationTransitionEffect.FromRight });
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is EmployeeViewModel viewModel)
            {
                EmployeeViewModel = viewModel;
                NombreTextBox.Text = EmployeeViewModel.Nombre;
                ApellidoTextBox.Text = EmployeeViewModel.Apellido;
                NumeroTextBox.Text = EmployeeViewModel.NumeroEmpleado;
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

        private void NombreTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(NombreTextBox.Text, @"^[a-zA-Z\s]+$"))
            {
                HideErrorWithAnimation(NombreErrorTextBlock);
            }
            else
            {
                ShowErrorWithAnimation(NombreErrorTextBlock);
            }
        }

        private void ApellidoTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(ApellidoTextBox.Text, @"^[a-zA-Z\s]+$"))
            {
                HideErrorWithAnimation(ApellidoErrorTextBlock);
            }
            else
            {
                ShowErrorWithAnimation(ApellidoErrorTextBlock);
            }
        }

        private void NumeroTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Regex.IsMatch(NumeroTextBox.Text, @"^\d+$"))
            {
                HideErrorWithAnimation(NumeroErrorTextBlock);
            }
            else
            {
                ShowErrorWithAnimation(NumeroErrorTextBlock);
            }
        }
    }
}
