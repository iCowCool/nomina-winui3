public class EmployeeViewModel
{
    public string Nombre { get; set; }
    public string Apellido { get; set; }
    public string NumeroEmpleado { get; set; }
    public double SueldoPorHora { get; set; }

    public double LunesHoras { get; set; }
    public double MartesHoras { get; set; }
    public double MiercolesHoras { get; set; }
    public double JuevesHoras { get; set; }
    public double ViernesHoras { get; set; }
    public double SabadoHoras { get; set; }
    public double DomingoHoras { get; set; }

    public double LunesSueldo { get; set; }
    public double MartesSueldo { get; set; }
    public double MiercolesSueldo { get; set; }
    public double JuevesSueldo { get; set; }
    public double ViernesSueldo { get; set; }
    public double SabadoSueldo { get; set; }
    public double DomingoSueldo { get; set; }
    public double TotalSueldo { get; set; }
    public double TotalHoras { get; set; }
}
