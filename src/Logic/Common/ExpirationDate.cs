using System;

namespace Logic.Common
{
    public class ExpirationDate : ValueObject<ExpirationDate>
    {
        public static readonly ExpirationDate Infinite = new ExpirationDate(null);

        public DateTime? Value { get; }
        public bool IsExpired => Value != Infinite && Value < DateTime.UtcNow;

        private ExpirationDate(DateTime? value)
        {
            Value = value;
        }

        public static Result<ExpirationDate> Create(DateTime? value)
        {
            return Result.Ok(new ExpirationDate(value));
        }

        protected override bool EqualsCore(ExpirationDate obj)
        {
            return Value == obj.Value;
        }

        public static implicit operator DateTime?(ExpirationDate expirationDate)
        {
            return expirationDate.Value;
        }

        public static explicit operator ExpirationDate(DateTime? value)
        {
            if (value is null)
                return Infinite;

            return Create(value).Value;
        }
    }
}
