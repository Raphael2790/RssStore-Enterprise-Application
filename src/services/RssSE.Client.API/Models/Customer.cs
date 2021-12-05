using RssSE.Core.DomainObjects.BaseEntity;
using RssSE.Core.DomainObjects.Interfaces;
using RssSE.Core.DomainObjects.ValueObjects;
using System;

namespace RssSE.Client.API.Models
{
    public class Customer : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public Email Email { get; private set; }
        public Cpf Cpf { get; private set; }
        public bool Deleted { get; private set; }
        public Address Address { get; private set; }

        public Customer(Guid id, string name, string email, string cpf)
        {
            Id = id;
            Name = name;
            Email = new Email(email);
            Cpf = new Cpf(cpf);
            Deleted = false;
        }

        protected Customer() {}

        public void ChangeEmail(string email) => Email = new Email(email);
        public void ChangeAddress(Address address) => Address = address;
    }
}
