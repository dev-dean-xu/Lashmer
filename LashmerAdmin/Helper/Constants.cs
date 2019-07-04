using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LashmerAdmin.Helper
{
    public class Constants
    {
        public static class JwtClaimIdentifiers
        {
            public const string Rol = "rol", Id = "id";
        }

        public static class JwtClaims
        {
            public const string ApiAccess = "api_access";
        }
    }
}
