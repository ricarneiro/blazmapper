namespace BlazMapper.DomainBase
{
    public abstract class AValueObject : IEquatable<AValueObject>
    {
        public bool IsValid { get; protected set; }

        public abstract bool GetValidationExpression();

        protected abstract IEnumerable<object> GetEqualityComponents();

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != GetType())
                return false;

            var other = (AValueObject)obj;
            return GetEqualityComponents().SequenceEqual(other.GetEqualityComponents());
        }

        public bool Equals(AValueObject? other)
        {
            return Equals((object?)other);
        }

        public override int GetHashCode()
        {
            return GetEqualityComponents()
                .Select(x => x?.GetHashCode() ?? 0)
                .Aggregate((x, y) => x ^ y);
        }

        public static bool operator ==(AValueObject? left, AValueObject? right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AValueObject? left, AValueObject? right)
        {
            return !Equals(left, right);
        }
    }
}
