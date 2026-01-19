using Challenger.FCamara.Luxclusif.Domain.Entities;

namespace Challenger.FCamara.Luxclusif.Application.DTOs;

public sealed record CreateCategoryResponse(
    Guid Id,
    string Name,
    string Shortcode,
    Guid? ParentCategoryId
)
{
    public static implicit operator CreateCategoryResponse(Category category)
    {
        return new CreateCategoryResponse(
            category.Id,
            category.Name,
            category.Shortcode,
            category.ParentCategoryId
        );
    }
}