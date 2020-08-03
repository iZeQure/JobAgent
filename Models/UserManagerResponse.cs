using JobAgent.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JobAgent.Models
{
    public class UserManagerResponse
    {
        public User UserInfo { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public string[] Errors { get; set; }
        public DateTime? ExpireDate { get; set; }
    }
}
