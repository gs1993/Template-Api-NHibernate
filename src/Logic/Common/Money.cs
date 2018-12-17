using System;

namespace Logic.Common
{ 
    public class Money : ValueObject<Money>
    {
        private Money(decimal value, int decimalPlaces, string sign)
        {
            Value = value;
            DecimalPlaces = decimalPlaces;
            Sign = sign;
        }

        public decimal Value { get; }
        public int DecimalPlaces { get; }
        public string Sign { get; }

        public static Result<Money> Create(decimal value, int decimalPlaces = 2, string sign = "$")
        {
            if (value < 0)
                return Result.Fail<Money>("Value cannot be less than zero");

            if(decimalPlaces < 0)
                return Result.Fail<Money>("Decimal places cannot be less than zero");

            sign = (sign ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(sign))
                return Result.Fail<Money>("Sign cannot be empty");

            return Result.Ok(new Money(value, decimalPlaces, sign));
        }

        protected override bool EqualsCore(Money obj)
        {
            return Math.Round(Value, DecimalPlaces) == Math.Round(obj.Value, DecimalPlaces);
        }

        public static Money operator *(Money value, decimal multiplier)
        {
            return new Money(value.Value * multiplier, value.DecimalPlaces, value.Sign);
        }

        public static Money operator +(Money item1, Money item2)
        {
            if(item1.DecimalPlaces != item2.DecimalPlaces)
                throw new InvalidOperationException();

            if(item1.Sign != item2.Sign)
                throw new InvalidOperationException();

            return new Money(item1.Value + item2.Value, item1.DecimalPlaces, item1.Sign);
        }

        public override string ToString()
        {
            return $"{Math.Round(Value, DecimalPlaces)} {Sign}";
        }

        public static implicit operator decimal(Money money)
        {
            return money.Value;
        }

        public static explicit operator Money(decimal value)
        {
            return Create(value, 2, "$").Value;
        }
    }
}
