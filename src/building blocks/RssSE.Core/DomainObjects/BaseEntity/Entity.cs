using RssSE.Core.Messages;
using System;
using System.Collections.Generic;

namespace RssSE.Core.DomainObjects.BaseEntity
{
    public abstract class Entity
    {
        public Guid Id { get; set; }
        private List<Event> _notifications;
        public IReadOnlyCollection<Event> Notifications => _notifications?.AsReadOnly();

        protected Entity()
        {
            Id = Guid.NewGuid();
            _notifications = new List<Event>();
        }

        public void AddEvent(Event @event) => _notifications.Add(@event);
        public void RemoveEvent(Event @event) => _notifications?.Remove(@event);
        public void ClearEvents() => _notifications?.Clear();

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
