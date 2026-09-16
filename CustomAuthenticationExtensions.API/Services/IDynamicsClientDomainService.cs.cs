using System;
using System.Collections.Generic;
using System.Text;

namespace CustomAuthenticationExtensions.API.Services
{
    public interface IDynamicsClientDomainService
    {
        Task QueryData();
    }
}
