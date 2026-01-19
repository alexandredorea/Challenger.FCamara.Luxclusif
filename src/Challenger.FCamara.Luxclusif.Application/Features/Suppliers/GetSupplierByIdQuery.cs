using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.DTOs;

namespace Challenger.FCamara.Luxclusif.Application.Features.Suppliers;

public record GetSupplierByIdQuery(Guid Id) : IQuery<SupplierDetailDto>;