using Logic.Common;
using System;

namespace Logic.Entities.CustomerEntities
{
    public class CustomerName : ValueObject<CustomerName>
    {
        private CustomerName(string value)
        {
            Value = value;
        }

        public string Value { get; }

        public static Result<CustomerName> Create(string name)
        {
            name = (name ?? string.Empty).Trim();

            if(name.Length == 0)
                return Result.Fail<CustomerName>("Customer name cannot be empty");

            if (name.Length > 100)
                return Result.Fail<CustomerName>("Customer name is too long");
            
            return Result.Ok(new CustomerName(name));
        }

        protected override bool EqualsCore(CustomerName obj)
        {
            return Value.Equals(obj.Value, StringComparison.CurrentCultureIgnoreCase);
        }

        public static implicit operator string(CustomerName customerName)
        {
            return customerName.Value;
        }

        public static explicit operator CustomerName(string customerName)
        {
            return Create(customerName).Value;
        }
    }
}
