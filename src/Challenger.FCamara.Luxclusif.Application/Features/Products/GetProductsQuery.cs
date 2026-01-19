using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.DTOs;

namespace Challenger.FCamara.Luxclusif.Application.Features.Products;

public sealed record GetProductsQuery(int Page, int PageSize = 10) : IQuery<PagedResult<ProductDto>>;