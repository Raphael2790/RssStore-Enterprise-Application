using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Security.Claims;

namespace RssSE.Identity.API.Models
{
    public class UserInfosDto
    {
        public UserInfosDto()
        {
            Claims = new List<Claim>();
            Roles = new List<string>();
        }
        public IdentityUser User { get; set; }
        public IList<Claim> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
