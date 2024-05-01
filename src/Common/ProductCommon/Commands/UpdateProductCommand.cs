using MediatR;
using ProductCommon.Entities;
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductCommon.Commands
{
    public class UpdateProductCommand : IRequest<ServiceResult<ProductDto>>
    {
        public required int Id { get; set; }
        public required string Name { get; set; }

    }
}
