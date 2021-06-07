using Pocos;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SecurityAccess.Access
{
    public interface IAccess
    {
        string GenerateAccess(User user);

        ClaimsIdentity GetClaimsIdentity(User user);
    }
}
