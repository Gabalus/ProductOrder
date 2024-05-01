using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderCommon.Commands;
using OrderCommon.Entities;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Commands
{
    internal class ListOrderCommandHandler : IRequestHandler<ListOrderCommand, ServiceResult<OrderDto[]>>
    {
        OrderContext _context;

        public ListOrderCommandHandler(OrderContext context)
        {
            _context = context;
        }

        public Task<ServiceResult<OrderDto[]>> Handle(ListOrderCommand request, CancellationToken cancellationToken)
        {

            var Orders = _context
                .Orders
                .Include(o => o.Lines)
                .OrderBy(p => p.Id)
                .Skip(request.Page * request.Count)
                .Take(request.Count);

            var result = new List<OrderDto>();

            foreach (var Order in Orders)
            {
                result.Add(new OrderDto()
                {
                    Id = Order.Id,
                    CreationDate = Order.CreationDate,
                    Lines = Order.Lines?.Select(ol => new OrderLineDto() { ProductId = ol.ProductId, Quantity = ol.Quantity }).ToArray()

                });
            }

            return Task.FromResult(ServiceResult.Success(result.ToArray()));


        }
    }
}
