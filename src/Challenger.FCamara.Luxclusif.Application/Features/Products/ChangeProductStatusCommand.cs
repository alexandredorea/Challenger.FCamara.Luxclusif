using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Domain.Enumerators;
using System.Text.Json.Serialization;

namespace Challenger.FCamara.Luxclusif.Application.Features.Products;

public class ChangeProductStatusCommand : ICommand<ChangeProductStatusResponse>
{
    [JsonIgnore]
    public Guid ProductId { get; private set; }

    public ProductStatus NewStatus { get; init; }

    public string UserId { get; init; }

    public string UserEmail { get; init; }

    public void SetProductId(Guid productId)
    {
        ProductId = productId;
    }
}

public record ChangeProductStatusResponse(
    Guid ProductId,
    ProductStatus PreviousStatus,
    ProductStatus NewStatus,
    DateTime ChangedAt
);