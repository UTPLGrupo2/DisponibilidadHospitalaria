using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instituciones
{
    public class GetInstitucionesPorCiudad
    {
        public class RequestModel : IRequest<List<InstitucionDto>>
        {
            public int CiudadId { get; set; }
        }

        public class Handler : IRequestHandler<RequestModel, List<InstitucionDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<InstitucionDto>> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                return _mapper.Map<List<InstitucionDto>>(await _context.Instituciones
                    .Where(x => x.CiudadId == request.CiudadId)
                    .Include(x => x.Ciudad)
                    .Include(x => x.TipoDeInstitucion)
                    .ToListAsync());
            }
        }
    }
}
