using System;

namespace RssSE.Identity.API.Models
{
    public class UserLoginResponse
    {
        public string AccessToken { get; set; }
        public double ExpiresIn { get; set; }
        public Guid RefreshToken { get; set; }
        public UserToken UserToken { get; set; }
    }
}
