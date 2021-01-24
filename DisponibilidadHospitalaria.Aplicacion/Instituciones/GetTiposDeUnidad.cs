﻿using AutoMapper;
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
    public class GetTiposDeUnidad
    {
        public class RequestModel : IRequest<List<TipoDeUnidadDto>>
        {
        }

        public class Handler : IRequestHandler<RequestModel, List<TipoDeUnidadDto>>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;

            public Handler(ApplicationDbContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            public async Task<List<TipoDeUnidadDto>> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                return _mapper.Map<List<TipoDeUnidadDto>>(await _context.TiposDeUnidad.ToListAsync());
            }
        }
    }
}
