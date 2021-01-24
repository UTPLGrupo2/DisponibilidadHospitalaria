using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Instituciones;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Ciudades.Instituciones
{
    public class EditarUnidadModel : PageModelBase
    {
        [BindProperty]
        public UnidadCreateUpdate.RequestModel Input { get; set; }

        [ViewData]
        public List<TipoDeUnidadDto> TiposDeUnidad { get; set; }

        [ViewData]
        public InstitucionDto Institucion { get; set; }

        public async Task OnGetAsync(int institucionId, int id)
        {
            Institucion = await Mediator.Send(new GetInstitucion.RequestModel() { Id = institucionId });
            TiposDeUnidad = await Mediator.Send(new GetTiposDeUnidad.RequestModel());
            Input ??= Mapper.Map<UnidadCreateUpdate.RequestModel>(Institucion.Unidades.FirstOrDefault(x => x.Id == id));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (ModelState.IsValid)
            {
                await Mediator.Send(Input);
                Institucion = await Mediator.Send(new GetInstitucion.RequestModel() { Id = Input.InstitucionId });
                return Redirect($"./Index/?ciudadId={Institucion.CiudadId}");
            }
            else
            {
                await OnGetAsync(Input.InstitucionId, Input.Id);
                return Page();
            }
        }
    }
}
