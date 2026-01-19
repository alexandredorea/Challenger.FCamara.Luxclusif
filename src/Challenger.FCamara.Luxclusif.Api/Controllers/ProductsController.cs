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
}