using MediatR;
using Microsoft.EntityFrameworkCore;
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
    internal class DeleteOrderCommandHandler : IRequestHandler<DeleteOrderCommand, ServiceResult>
    {
        OrderContext _context;

        public DeleteOrderCommandHandler(OrderContext context)
        {
            _context = context;
        }

        public async Task<ServiceResult> Handle(DeleteOrderCommand request, CancellationToken cancellationToken)
        {
           

            
            var Order = await _context.Orders.FirstOrDefaultAsync(p=> p.Id == request.Id, cancellationToken);

            if(Order != null)
            {
                _context.Orders.Remove(Order);
                await _context.SaveChangesAsync();

                return ServiceResult.Success();
            }
            else
            {
                return ServiceResult.Failed(ServiceError.NotFound);                
            }
            
        }
    }
}
