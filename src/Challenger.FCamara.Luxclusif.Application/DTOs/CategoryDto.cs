using Challenger.FCamara.Luxclusif.Domain.Entities;

namespace Challenger.FCamara.Luxclusif.Application.DTOs;

public record CategoryDto(
    Guid Id,
    string Name,
    string Shortcode,
    Guid? ParentCategoryId,
    string? ParentCategoryName,
    DateTime CreatedAt
)
{
    public static implicit operator CategoryDto(Category category)
    {
        return new CategoryDto(
            category.Id,
            category.Name,
            category.Shortcode,
            category.ParentCategoryId,
            category.ParentCategory?.Name,
            category.CreatedAt
        );
    }
}