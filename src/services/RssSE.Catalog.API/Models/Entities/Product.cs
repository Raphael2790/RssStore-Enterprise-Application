using RssSE.Core.DomainObjects.BaseEntity;
using RssSE.Core.DomainObjects.Interfaces;
using System;

namespace RssSE.Catalog.API.Models
{
    public class Product : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public bool Active { get; private set; }
        public decimal Value { get; private set; }
        public DateTime RegisterDate { get; private set; }
        public string Image { get; private set; }
        public int StockAmount { get; private set; }

        public bool IsAvailable(int productQuantity) => Active && StockAmount >= productQuantity;

        public void DebitStock(int productQuantity)
        {
            if (StockAmount >= productQuantity)
                StockAmount -= productQuantity;
        }
    }
}
