using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommon.Commands
{

    public class DeleteProductCommand : IRequest<ServiceCommon.ServiceResult>
    {
        public required int Id { get; set; }

    }
}
