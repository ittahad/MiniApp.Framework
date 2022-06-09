using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinimalFramework
{
    public interface ITokenService
    {
        string BuildToken(
            string key, 
            string issuer, 
            string audience, 
            string userName);
    }
}
