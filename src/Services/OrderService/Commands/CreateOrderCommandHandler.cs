
using Microsoft.Extensions.DependencyInjection;
using OrderCommon.Commands;
using OrderCommon.Entities;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Commands
{
    internal class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, ServiceResult<OrderDto>>
    {
        OrderContext _context;

        public CreateOrderCommandHandler(OrderContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<OrderDto>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {


            var Order = new Order() { CreationDate = DateTime.Now };

            Order.Lines = new List<OrderLine>();

            foreach (var line in request.Lines)
            {
                Order.Lines.Add(new OrderLine() { ProductId = line.ProductId, Quantity = line.Quantity });
            }           

            await _context.Orders.AddAsync(Order, cancellationToken);

            await _context.SaveChangesAsync();

            return ServiceResult.Success(new OrderDto()
            {
                Id = Order.Id,
                CreationDate = Order.CreationDate,
                Lines = Order.Lines.Select(ol => new OrderLineDto() { ProductId = ol.ProductId, Quantity = ol.Quantity }).ToArray()

            });

        }
    }
}
