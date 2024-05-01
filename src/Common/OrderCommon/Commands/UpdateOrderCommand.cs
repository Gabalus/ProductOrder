using MediatR;
using OrderCommon.Entities;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCommon.Commands
{

    public class UpdateOrderCommand : IRequest<ServiceResult<OrderDto>>
    {
        public required int Id { get; set; }
        public required OrderLineDto[] Lines { get; set; }

    }
}
