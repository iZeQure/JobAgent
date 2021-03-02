using Pocos;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SecurityAccess.Acess
{
    public interface IAccess
    {
        string GenerateAccess(User user);

        ClaimsIdentity GetClaimsIdentity(User user);
    }
}
