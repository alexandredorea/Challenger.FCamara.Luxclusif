using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.DTOs;
using Challenger.FCamara.Luxclusif.Application.Features.Categories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Challenger.FCamara.Luxclusif.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriesController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new category
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Result<CreateCategoryResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<CreateCategoryResponse>), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Result<CreateCategoryResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateCategory(
        [FromBody] CreateCategoryCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);
        return CreatedAtAction(actionName: nameof(CreateCategory), value: result);
    }

    /// <summary>
    /// Lists all categories
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<CategoryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PagedResult<CategoryDto>), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(PagedResult<CategoryDto>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListCategories(GetCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(request, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Deletes a category by ID
    /// </summary>
    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteCategory(
        Guid id,
        CancellationToken cancellationToken)
    {
        var command = new DeleteCategoryCommand(id);
        var result = await mediator.Send(command, cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}