using RssSE.Core.DomainObjects.BaseEntity;
using System;

namespace RssSE.Client.API.Models
{
    public class Address : Entity
    {
        public string Street { get; private set; }
        public string Number { get; private set; }
        public string Complement { get; private set; }
        public string Neighborhood { get; private set; }
        public string ZipCode { get; private set; }
        public string City { get; private set; }
        public string State { get; private set; }
        public string Country { get; private set; }
        public Guid CustomerId { get; private set; }

        public Customer Client { get; protected set; }

        public Address(string street, string number, string complement, string neighborhood, string zipCode, string city, string state,Guid customerId,string country = "Brasil")
        {
            Street = street;
            Number = number;
            Complement = complement;
            Neighborhood = neighborhood;
            ZipCode = zipCode;
            City = city;
            State = state;
            Country = country;
            CustomerId = customerId;
        }

        public Address() {}
    }
}
