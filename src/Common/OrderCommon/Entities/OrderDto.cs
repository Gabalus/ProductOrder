using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderCommon.Entities
{

    public class OrderDto
    {

        public int Id { get; set; }

        public DateTime CreationDate { get; set; }

        public OrderLineDto[] Lines { get; set; }

    }
}
