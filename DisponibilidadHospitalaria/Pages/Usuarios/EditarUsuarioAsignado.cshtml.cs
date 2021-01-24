using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Aplicacion.Ciudades;
using Aplicacion.Seguridad;
using DisponibilidadHospitalaria.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DisponibilidadHospitalaria.Pages.Usuarios
{
    public class EditarUsuarioAsignadoModel : PageModelBase
    {

        [BindProperty]
        public UsuarioAsignadoCreateUpdate.RequestModel Input { get; set; }

        [ViewData]
        public List<InstitucionListaVM> Instituciones { get; set; }

        public async Task OnGetAsync(int id)
        {
            var ciudades = await Mediator.Send(new GetCiudades.RequestModel());

            Instituciones = ciudades
                .SelectMany(x => x.Instituciones)
                .Select(x => new InstitucionListaVM() { Id = x.Id, Nombre = x.Nombre, Ciudad = x.Ciudad, Provincia = x.Provincia })
                .OrderBy(x => x.Provincia).ThenBy(x => x.Ciudad).ThenBy(x => x.Nombre)
                .ToList();

            Input ??= Mapper.Map<UsuarioAsignadoCreateUpdate.RequestModel>(await Mediator.Send(new GetUsuarioAsignado.RequestModel { Id = id }));
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
                await OnGetAsync(Input.Id);
                return Page();
            }
        }
    }
}
