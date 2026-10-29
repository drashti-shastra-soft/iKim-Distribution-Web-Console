using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKimWebConsole.Infrastructure.IService
{
    public interface IAuthenticationService
    {
        Task<Domain.User> AuthenticateAsync(Domain.Credential mCredential);
    }
}
