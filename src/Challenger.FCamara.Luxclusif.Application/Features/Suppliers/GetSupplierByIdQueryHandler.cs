using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.Common.Persistences;
using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace Challenger.FCamara.Luxclusif.Application.Features.Suppliers;

public sealed class GetSupplierByIdQueryHandler(IApplicationDbContext context) : IQueryHandler<GetSupplierByIdQuery, SupplierDetailDto>
{
    public async Task<Result<SupplierDetailDto>> Handle(
        GetSupplierByIdQuery request,
        CancellationToken cancellationToken)
    {
        var supplier = await context.Suppliers.SingleOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (supplier is null)
            return Result<SupplierDetailDto>.Fail(
                "Fornecedor não encontrado",
                "NOT_FOUND");

        return Result<SupplierDetailDto>.Ok(
            supplier,
            "Fornecedor recuperado com sucesso");
    }
}