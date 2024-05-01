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

    public class ListProductCommand : IRequest<ServiceResult<ProductDto[]>>
    {

        public int Count { get; set; }

        public int Page { get; set; }

    }
}
