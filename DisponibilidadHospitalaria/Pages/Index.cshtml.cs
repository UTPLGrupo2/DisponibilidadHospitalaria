using Aplicacion.Ciudades;
using DisponibilidadHospitalaria.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DisponibilidadHospitalaria.Pages
{
    public class IndexModel : PageModelBase
    {
        [BindProperty]
        public FiltroCiuidadVM FiltroCiudad { get; set; }

        [ViewData]
        public List<ProvinciaVM> Provincias { get; set; }

        [ViewData]
        public List<CiudadDto> Ciudades { get; set; }

        public CiudadDto Ciudad { get; set; }

        public async Task OnGetAsync()
        {
            var ciudades = await Mediator.Send(new GetCiudades.RequestModel());
            var primeraCiudad = ciudades?.OrderBy(x => x.Provincia_Codigo).ThenBy(x => x.Nombre).First() ?? new CiudadDto();

            Provincias = GetProvincias(ciudades);
            Ciudades = GetCiudades(ciudades, primeraCiudad.ProvinciaId);

            FiltroCiudad = new FiltroCiuidadVM()
            {
                ProvinciaId = primeraCiudad.ProvinciaId,
                CiudadId = primeraCiudad.Id
            };

            Ciudad = primeraCiudad;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var ciudades = await Mediator.Send(new GetCiudades.RequestModel());
            Provincias = GetProvincias(ciudades);
            Ciudades = GetCiudades(ciudades, FiltroCiudad.ProvinciaId);

            var ciudadId = Ciudades.Any(x => x.Id == FiltroCiudad.CiudadId) 
                ? FiltroCiudad.CiudadId 
                : Ciudades.OrderBy(x=>x.Provincia_Codigo).ThenBy(x=>x.Nombre).First().Id;

            Ciudad = ciudades.FirstOrDefault(x => x.Id == ciudadId);
            FiltroCiudad.CiudadId = ciudadId;

            return Page();
        }

        private List<ProvinciaVM> GetProvincias(List<CiudadDto> todasLasCiudades)
        {
            var provincias = new List<ProvinciaVM>();
            foreach (var c in todasLasCiudades)
                if (!provincias.Any(x => x.Id == c.ProvinciaId))
                    provincias.Add(new ProvinciaVM() { Id = c.ProvinciaId, Codigo = c.Provincia_Codigo, Nombre = c.Provincia_Nombre });

            return provincias;
        }

        private List<CiudadDto> GetCiudades(List<CiudadDto> todasLasCiudades, int provinciaId)
        {
            return todasLasCiudades
                .Where(x => x.ProvinciaId == provinciaId)
                .ToList();
        }
    }
}
