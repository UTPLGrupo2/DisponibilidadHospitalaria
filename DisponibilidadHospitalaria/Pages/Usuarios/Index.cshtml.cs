using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Seguridad;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Usuarios
{
    public class IndexModel : PageModelBase
    {
        public List<UsuarioAsignadoDto> Usuarios { get; set; }

        public async Task OnGetAsync()
        {
            Usuarios = await Mediator.Send(new GetUsuariosAsignados.RequestModel() { Filtro = "" });
        }
    }
}
