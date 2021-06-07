using Pocos;
using System;

namespace JobAgent.Models
{
    public class RefreshTokenModel
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }
        public User User { get; set; }
    }
}
