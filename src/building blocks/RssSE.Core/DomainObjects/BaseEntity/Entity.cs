using System;

namespace RssSE.Core.DomainObjects.BaseEntity
{
    public abstract class Entity
    {
        protected Entity()
        {
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public override bool Equals(object obj)
        {
            var compareTo = obj as Entity;

            if (ReferenceEquals(this, compareTo)) return true;
            if (ReferenceEquals(null, compareTo)) return false;

            return Id.Equals(compareTo.Id);
        }

        //Sobrescreve o operador de igualdade entre entidades
        public static bool operator ==(Entity a, Entity b)
        {
            if (ReferenceEquals(null, a) && ReferenceEquals(null, b))
                return true;

            if (ReferenceEquals(null, a) || ReferenceEquals(null, b))
                return false;

            return a.Equals(b);
        }

        //Sobrescreve o operador de diferenção entre entidades
        public static bool operator !=(Entity a, Entity b)
        {
            return !(a == b);
        }

        //Ajuda na distinção da comparação de igualdade entre os objetos
        public override int GetHashCode()
        {
            return (GetType().GetHashCode() * 907) + Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{GetType().Name} [Id={Id}]";
        }

        public virtual bool IsValid()
        {
            throw new NotImplementedException();
        }
    }
}
