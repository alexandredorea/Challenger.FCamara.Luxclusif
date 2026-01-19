using Challenger.FCamara.Luxclusif.Application.Common.CQS;
using Challenger.FCamara.Luxclusif.Application.DTOs;

namespace Challenger.FCamara.Luxclusif.Application.Features.Categories;

public sealed record CreateCategoryCommand(
    string Name,
    string Shortcode,
    Guid? ParentCategoryId
) : ICommand<CreateCategoryResponse>;