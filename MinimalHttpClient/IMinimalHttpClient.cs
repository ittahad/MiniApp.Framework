using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MinimalHttpClient
{
    public interface IMinimalHttpClient
    {
        Task<TResponse> MakeHttpRequest<TResponse>(HttpRequestMessage httpRequestMessage);
    }
}
