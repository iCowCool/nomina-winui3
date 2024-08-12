using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using System.Collections.Generic;

namespace Nomina
{
    public sealed partial class ResultsPage : Page
    {
        public EmployeeViewModel EmployeeViewModel { get; set; }

        public ResultsPage()
        {
            this.InitializeComponent();
            this.DataContext = this; // Asegúrate de que el DataContext esté configurado correctamente
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is not null)
            {
                var parameters = e.Parameter as dynamic;
                this.EmployeeViewModel = parameters.EmployeeViewModel;

                // Asegurarte de que se pasa correctamente el sueldo por hora
                this.EmployeeViewModel.SueldoPorHora = parameters.EmployeeViewModel.SueldoPorHora;

                // Variables para los totales
                double totalHoras = 0;
                double totalSueldo = 0;

                // Asignar las horas y sueldos por día
                var daysWorked = parameters.DaysWorked as Dictionary<string, double>;
                foreach (var day in daysWorked)
                {
                    double sueldoDia = day.Value * this.EmployeeViewModel.SueldoPorHora;
                    totalHoras += day.Value;
                    totalSueldo += sueldoDia;

                    switch (day.Key)
                    {
                        case "Lunes":
                            this.EmployeeViewModel.LunesHoras = day.Value;
                            this.EmployeeViewModel.LunesSueldo = sueldoDia;
                            break;
                        case "Martes":
                            this.EmployeeViewModel.MartesHoras = day.Value;
                            this.EmployeeViewModel.MartesSueldo = sueldoDia;
                            break;
                        case "Miércoles":
                            this.EmployeeViewModel.MiercolesHoras = day.Value;
                            this.EmployeeViewModel.MiercolesSueldo = sueldoDia;
                            break;
                        case "Jueves":
                            this.EmployeeViewModel.JuevesHoras = day.Value;
                            this.EmployeeViewModel.JuevesSueldo = sueldoDia;
                            break;
                        case "Viernes":
                            this.EmployeeViewModel.ViernesHoras = day.Value;
                            this.EmployeeViewModel.ViernesSueldo = sueldoDia;
                            break;
                        case "Sábado":
                            this.EmployeeViewModel.SabadoHoras = day.Value;
                            this.EmployeeViewModel.SabadoSueldo = sueldoDia;
                            break;
                        case "Domingo":
                            this.EmployeeViewModel.DomingoHoras = day.Value;
                            this.EmployeeViewModel.DomingoSueldo = sueldoDia;
                            break;
                    }
                }

                // Asignar los totales
                this.EmployeeViewModel.TotalHoras = totalHoras;
                this.EmployeeViewModel.TotalSueldo = totalSueldo;

                // Actualizar el DataContext
                this.DataContext = this.EmployeeViewModel;
            }
        }



        private void RegresarButtonClick(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}
