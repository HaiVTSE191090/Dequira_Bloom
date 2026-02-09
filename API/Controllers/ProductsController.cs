
using Application.Features.Products.DTOs;
using Application.Features.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAll(
            [FromQuery] bool? isActive = null,
            [FromQuery] bool? isFeatured = null)
        {
            var query = new GetAllProductsQuery
            {
                IsActive = isActive,
                IsFeatured = isFeatured
            };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
