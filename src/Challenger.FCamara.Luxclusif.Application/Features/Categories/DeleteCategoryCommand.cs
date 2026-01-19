using Challenger.FCamara.Luxclusif.Application.Common.CQS;

namespace Challenger.FCamara.Luxclusif.Application.Features.Categories;

public sealed record DeleteCategoryCommand(Guid Id) : ICommand;