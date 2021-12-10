using System;

namespace RssSE.Identity.Data.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public Guid Token { get; set; }
        public DateTime ExpirationDate { get; set; }

        public RefreshToken()
        {
            Id = Guid.NewGuid();
            Token = Guid.NewGuid();
        }
    }
}
