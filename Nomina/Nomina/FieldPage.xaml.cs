using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Animation;

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
    }
}
