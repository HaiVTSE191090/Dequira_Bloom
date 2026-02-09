using Application.Features.Products.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Products.Queries
{
    public record GetAllProductsQuery : IRequest<IReadOnlyList<ProductDto>>
    {
        public bool? IsActive { get; init; }
        public bool? IsFeatured { get; init; }
    }
}
