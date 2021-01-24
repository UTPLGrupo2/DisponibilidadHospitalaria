using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisponibilidadHospitalaria.ViewModels
{
    public class InstitucionListaVM
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Ciudad { get; set; }
        public string Provincia { get; set; }
        public string NombreLista => $"{Nombre} - {Ciudad} - {Provincia}";
    }
}
