using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using ObjectLibrary.Common;

namespace SecurityLibrary.Interfaces
{
    public interface IAccess
    {
        string GenerateAccessToken(User user);

        ClaimsIdentity GetClaimsIdentity(User user);
    }
}
