using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.DTOs;
using Challenger.FCamara.Luxclusif.Application.Features.Suppliers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Challenger.FCamara.Luxclusif.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public sealed class SuppliersController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new supplier
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Result<CreateSupplierResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<CreateSupplierResponse>), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(Result<CreateSupplierResponse>), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Result<CreateSupplierResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateSupplier(
        [FromBody] CreateSupplierCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (!result.Success)
            return Conflict(result);

        return CreatedAtAction(
            nameof(GetSupplierById),
            new { id = result.Data!.Id },
            result);
    }

    /// <summary>
    /// Lists all suppliers
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(PagedResult<SupplierDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(PagedResult<SupplierDto>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListSuppliers(GetSuppliersQuery query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);
        return Ok(result);
    }

    /// <summary>
    /// Gets a supplier by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(Result<SupplierDetailDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<SupplierDetailDto>), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSupplierById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetSupplierByIdQuery(id);
        var result = await mediator.Send(query, cancellationToken);

        if (!result.Success)
            return NotFound(result);

        return Ok(result);
    }
}