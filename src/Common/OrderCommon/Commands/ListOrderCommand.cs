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

    public class ListOrderCommand : IRequest<ServiceResult<OrderDto[]>>
    {

        public int Count { get; set; }

        public int Page { get; set; }

    }
}
