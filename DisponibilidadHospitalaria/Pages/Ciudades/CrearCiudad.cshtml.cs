using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Ciudades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Ciudades
{
    public class CrearCiudadModel : PageModelBase
    {

        [BindProperty]
        public CiudadCreateUpdate.RequestModel Input { get; set; }

        [ViewData]
        public List<ProvinciaDto> Provincias { get; set; }

        public async Task OnGetAsync()
        {
            Provincias = await Mediator.Send(new GetProvincias.RequestModel());
            Input ??= new CiudadCreateUpdate.RequestModel() { Id = -1 };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(Input);
                return Redirect("./Index");
            }
            else
            {
                await OnGetAsync();
                return Page();
            }
        }
    }
}
