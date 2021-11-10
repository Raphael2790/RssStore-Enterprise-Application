using System.Collections.Generic;

namespace RssSE.WebApp.MVC.Models
{
    public class UserToken
    {
        public string Id  { get; set; }
        public string Email { get; set; }
        public IEnumerable<UserClaim> UserClaims { get; set; } 
    }
}
