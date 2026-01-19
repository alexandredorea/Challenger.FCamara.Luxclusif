using Challenger.FCamara.Luxclusif.Application.Common.Results;
using Challenger.FCamara.Luxclusif.Application.DTOs;
using Challenger.FCamara.Luxclusif.Application.Features.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Challenger.FCamara.Luxclusif.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController(IMediator mediator) : ControllerBase
{
    /// <summary>
    /// Creates a new product
    /// </summary>
    [HttpPost]
    [ProducesResponseType(typeof(Result<CreateProductResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Result<CreateProductResponse>), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(Result<CreateProductResponse>), StatusCodes.Status422UnprocessableEntity)]
    [ProducesResponseType(typeof(Result<CreateProductResponse>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateProduct(
        [FromBody] CreateProductCommand command,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(command, cancellationToken);

        if (!result.Success)
            return BadRequest(result);

        return CreatedAtAction(nameof(CreateProduct), result);
    }

    /// <summary>
    /// Lists all products
    /// </summary>
    [HttpGet]
    [ProducesResponseType(typeof(Result<PagedResult<ProductDto>>), StatusCodes.Status200OK)]
    public async Task<IActionResult> ListProducts(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var result = await mediator.Send(query, cancellationToken);

        return Ok(result);
    }

    /// <summary>
    /// Gets a product by ID
    /// </summary>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetProductById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var result = await mediator.Send(new GetProductByIdQuery(id), cancellationToken);

        if (!result.Success)
        {
            return NotFound(result);
        }

        return Ok(result);
    }

    /// <summary>
    /// Changes the status of a product
    /// </summary>
    /// <remarks>
    /// </remarks>
    [HttpPatch("{id:guid}/status")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(object), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(object), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> ChangeProductStatus(
        Guid id,
        [FromBody] ChangeProductStatusCommand command,
        CancellationToken cancellationToken)
    {
        command.SetProductId(id);
        var result = await mediator.Send(command, cancellationToken);

        if (!result.Success)
        {
            var statusCode = result.Error.Any(e => e.Code == "NOT_FOUND")
                ? StatusCodes.Status404NotFound
                : StatusCodes.Status400BadRequest;

            return StatusCode(statusCode, new
            {
                result.Success,
                result.Message,
                result.Data,
                result.Error
            });
        }

        return Ok(result);
    }
}