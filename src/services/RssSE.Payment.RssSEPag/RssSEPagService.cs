﻿namespace RssSE.Payment.RssSEPag
{
    public class RssSEPagService
    {
        public readonly string ApiKey;
        public readonly string EncryptionKey;

        public RssSEPagService(string apiKey, string encryptionKey)
        {
            ApiKey = apiKey;
            EncryptionKey = encryptionKey;
        }
    }
}