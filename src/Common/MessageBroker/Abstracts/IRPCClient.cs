
using ServiceCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MessageBroker.Abstracts
{
    public interface IRPCClient
    {
   
        public Task<ServiceResult<TResponse>> GetResponse<TRequest, TResponse>(string hostname, string queuename, TRequest request);

        public Task<string> GetResponseAsString<TRequest, TResponse>(string hostname, string queuename, TRequest request);

    }
}
