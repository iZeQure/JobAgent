using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Services
{
    public class AdminService
    {
        public Task<List<string>> GetAdminMenuAsync()
        {
            return Task.FromResult(new List<string>() { "Header 1", "Header 2" });
        }
    }
}
