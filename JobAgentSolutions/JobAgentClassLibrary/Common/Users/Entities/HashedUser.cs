using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Common.Users.Entities
{
    public class HashedUser : IHashedUser
    {
        public string Password { get; set; }
        public string Salt { get; set; }
    }
}
