using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.DTOs;

namespace Challenger.FCamara.Luxclusif.Application.Features.Suppliers;

public sealed record GetSuppliersQuery(int Page, int PageSize = 10) : IQuery<PagedResult<SupplierDto>>;