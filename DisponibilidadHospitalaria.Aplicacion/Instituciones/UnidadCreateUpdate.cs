using AutoMapper;
using Dominio.Entities;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Persistencia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aplicacion.Instituciones
{
    public class UnidadCreateUpdate
    {
        public class RequestModel : IRequest
        {
            public int Id { get; set; }
            public int InstitucionId { get; set; }
            public int TipoDeUnidadId { get; set; }
            public string Denominacion { get; set; }
            public int Capacidad { get; set; }
        }

        public class RequestValidator : AbstractValidator<RequestModel>
        {
            public RequestValidator(ApplicationDbContext context)
            {
                RuleFor(x => x.Id)
                    .NotEmpty().WithMessage("No se ha especificado el campo Id")
                    .MustAsync(async (id, cancellation) =>
                    {
                        return id == -1 || (await context.Unidades.AnyAsync(x => x.Id == id, cancellationToken: cancellation));
                    }).WithMessage(x => $"No existe una unidad con Id={x.Id}");


                RuleFor(x => x.TipoDeUnidadId)
                    .NotEmpty().WithMessage("No de ha especificado el tipo de unidad")
                    .MustAsync(async (tipoDeUnidadId, cancellation) =>
                    {
                        return (await context.TiposDeUnidad.AnyAsync(x => x.Id == tipoDeUnidadId, cancellationToken: cancellation));
                    }).WithMessage(x => $"Tipo de unidad no admisible");

                RuleFor(x => x.InstitucionId)
                    .NotEmpty().WithMessage("No se ha especificado la institución")
                    .MustAsync(async (institucionId, cancellation) =>
                    {
                        return (await context.Instituciones.AnyAsync(x => x.Id == institucionId, cancellationToken: cancellation));
                    }).WithMessage(x => $"La institución no se ha definido correctamente");

                RuleFor(x => x.Denominacion)
                    .NotEmpty().WithMessage("El nombre no puede estar vacío")
                    .MaximumLength(100).WithMessage("El nombre debe tener máximo 100 caracteres");

                RuleFor(x => x)
                    .MustAsync(async (model, CancellationToken) =>
                    {
                        return !await context.Unidades.AnyAsync(x => (x.Id != model.Id) && (x.InstitucionId == model.InstitucionId) && (x.Denominacion == model.Denominacion), cancellationToken: CancellationToken);
                    }).WithMessage(x => $"Ya está definida la unidad [{x.Denominacion}] en la institución [{(context.Ciudades.FirstOrDefault(p => p.Id == x.InstitucionId))?.Nombre}]");
            }
        }

        public class Handler : IRequestHandler<RequestModel>
        {
            private readonly ApplicationDbContext _context;
            private readonly IMapper _mapper;
            private readonly ILogger<Handler> _logger;

            public Handler(ApplicationDbContext context, IMapper mapper, ILogger<Handler> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            public async Task<Unit> Handle(RequestModel request, CancellationToken cancellationToken)
            {
                var data = _mapper.Map<Unidad>(request);

                if (request.Id == -1)
                {
                    var model = new Unidad();
                    _mapper.Map<Unidad, Unidad>(data, model);
                    model.Id = 0;
                    await _context.AddAsync(model, cancellationToken);

                }
                else
                {
                    var model = await _context.Unidades.FindAsync(request.Id);
                    _mapper.Map<Unidad, Unidad>(data, model);
                    _context.Unidades.Update(model);
                }

                var result = await _context.SaveChangesAsync(cancellationToken);

                if (result > 0)
                {
                    _logger.LogInformation($"Unidad {(request.Id == -1 ? "Inserted" : "Updated")} [{(request.Id == -1 ? request.Denominacion : request.Id)}]");
                    return Unit.Value;
                }

                throw new AppException(System.Net.HttpStatusCode.Conflict, "No se realizaron los cambios");
            }
        }
    }
}
