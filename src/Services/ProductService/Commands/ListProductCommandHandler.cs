using MediatR;
using Microsoft.Extensions.DependencyInjection;
using ProductCommon.Commands;
using ProductCommon.Entities;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.Commands
{
    internal class ListProductCommandHandler : IRequestHandler<ListProductCommand, ServiceResult<ProductDto[]>>
    {
        ProductContext _context;

        public ListProductCommandHandler(ProductContext context)
        {
            _context = context;
        }

        public Task<ServiceResult<ProductDto[]>> Handle(ListProductCommand request, CancellationToken cancellationToken)
        {

            var products = _context
                .Products
                .OrderBy(p => p.Id)
                .Skip(request.Page * request.Count)
                .Take(request.Count);

            var result = new List<ProductDto>();

            foreach (var product in products)
            {
                result.Add(new ProductDto() { Id = product.Id, Name = product.Name });
            }

            return Task.FromResult(ServiceResult.Success(result.ToArray()));


        }
    }
}
