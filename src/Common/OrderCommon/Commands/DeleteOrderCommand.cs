using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCommon.Commands
{

    public class DeleteOrderCommand : IRequest<ServiceCommon.ServiceResult>
    {
        public required int Id { get; set; }

    }
}
