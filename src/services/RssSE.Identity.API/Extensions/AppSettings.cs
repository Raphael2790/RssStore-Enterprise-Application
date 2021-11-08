namespace RssSE.Identity.API.Extensions
{
    public class AppSettings
    {
        public string Secret { get; set; }
        public int ExpirationInHours { get; set; }
        public string EmmitedBy { get; set; }
        public string ValidFor { get; set; }
    }
}
