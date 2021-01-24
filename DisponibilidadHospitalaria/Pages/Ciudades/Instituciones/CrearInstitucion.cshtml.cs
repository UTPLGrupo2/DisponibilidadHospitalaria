using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Ciudades;
using Aplicacion.Instituciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Ciudades.Instituciones
{
    public class CrearInstitucionModel : PageModelBase
    {
        [BindProperty]
        public InstitucionCreateUpdate.RequestModel Input { get; set; }

        [ViewData]
        public List<TipoDeInstitucionDto> TiposDeInstitucion { get; set; }

        [ViewData]
        public CiudadDto Ciudad { get; set; }

        public async Task OnGetAsync(int ciudadId)
        {
            Ciudad = await Mediator.Send(new GetCiudad.RequestModel() { Id = ciudadId });
            TiposDeInstitucion = await Mediator.Send(new GetTiposDeInstitucion.RequestModel());

            Input ??= new InstitucionCreateUpdate.RequestModel()
            {
                Id = -1,
                CiudadId = ciudadId,
                TipoDeInstitucionId = TiposDeInstitucion.First().Id
            };
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(Input);
                return Redirect($"./Index/?ciudadId={Input.CiudadId}");
            }
            else
            {
                await OnGetAsync(Input.CiudadId);
                return Page();
            }
        }

    }
}
