﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Seguridad
{
    public class GetUsuariosAsignados
    {
        public class RequestModel : IRequest<List<UsuarioAsignadoDto>>
        {
            public string Filtro { get; set; }
        }

        public class Handler : IRequestHandler<RequestModel, List<UsuarioAsignadoDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<UsuarioAsignadoDto>> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                return await _context.UsuariosAsignados
                    .ProjectTo<UsuarioAsignadoDto>(_mapper.ConfigurationProvider)
                    .Where(x => string.IsNullOrWhiteSpace(request.Filtro) || x.Email.Contains(request.Filtro, StringComparison.InvariantCultureIgnoreCase))
                    .ToListAsync(cancellationToken: cancellationToken);
            }
        }

    }
}
