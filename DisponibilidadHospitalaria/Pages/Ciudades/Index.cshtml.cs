using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Ciudades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Ciudades
{
    public class IndexModel : PageModelBase
    {
        public List<CiudadDto> DataList { get; set; }

        public async Task OnGetAsync()
        {
            DataList = await Mediator.Send(new GetCiudades.RequestModel() { Filtro = "" });
        }
    }
}
