using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OrderCommon.Commands;
using OrderCommon.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderService.Commands
{
    internal class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, ServiceResult<OrderDto>>
    {
        OrderContext _context;

        public UpdateOrderCommandHandler(OrderContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult<OrderDto>> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
        {
           

            
            var Order = await _context
                .Orders
                .Include(o=>o.Lines)
                .FirstOrDefaultAsync(p=> p.Id == request.Id, cancellationToken);

            if(Order != null)
            {
                for (int i = Order.Lines.Count -1; i >=0 ; i--)
                {
                    if (!request.Lines.Any(rl => rl.ProductId == Order.Lines.ElementAt(i).ProductId))
                        Order.Lines.Remove(Order.Lines.ElementAt(i));
                }
                

                foreach (var item in request.Lines)
                {
                    var orderLine = Order.Lines.FirstOrDefault(ol => ol.ProductId == item.ProductId);
                    if (orderLine != null)
                    {
                        orderLine.Quantity = item.Quantity;
                    }
                    else
                    {
                        Order.Lines.Add(new OrderLine() { ProductId = item.ProductId, Quantity = item.Quantity });
                    }
                }
                
                await _context.SaveChangesAsync();

                return ServiceResult.Success(new OrderDto()
                {
                    Id = Order.Id,
                    CreationDate = Order.CreationDate,
                    Lines = Order.Lines.Select(ol => new OrderLineDto() { ProductId = ol.ProductId, Quantity = ol.Quantity }).ToArray()

                });
            }
            else
            {
                return ServiceResult.Failed<OrderDto>(ServiceError.NotFound);                
            }
            
        }
    }
}
