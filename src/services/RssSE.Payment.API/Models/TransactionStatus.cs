namespace RssSE.Payment.API.Models
{
    public enum TransactionStatus
    {
        Authorized = 1,
        Payed = 2,
        Unauthorized = 3,
        ChargedBack = 4
    }
}
