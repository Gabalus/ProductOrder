using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Abstracts
{

    public interface IRPCServer
    {
 
        void Start<TRequest, TResponse>(string hostName, string queueName, Func<TRequest, Task<TResponse>> func);

    }
}
