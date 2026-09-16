using System;
using System.Collections.Generic;
using System.Text;

namespace CustomAuthenticationExtensions.API
{
    public class Consts
    {
        public struct EnvironmentVariableNames
        {
            public const string TenantId = "TenantId";
            public const string ClientId = "ClientId";
            public const string ClientSecret = "ClientSecret";
            public const string DataverseUrl = "DataverseUrl";

            public const string ClientDomainsTableName = "ClientDomainsTableName";
        }
    }
}
