using Logic.Common;
using System;
using System.Text.RegularExpressions;

namespace Logic.Entities.CustomerEntities
{
    public class CustomerEmail : ValueObject<CustomerEmail>
    {
        private CustomerEmail(string value)
        {
            Value = value;
        }


        public string Value { get; }

        public static Result<CustomerEmail> Create(string email)
        {
            email = (email ?? string.Empty).Trim();

            if (email.Length == 0)
                return Result.Fail<CustomerEmail>("Email cannot be empty");

            if (email.Length > 100)
                return Result.Fail<CustomerEmail>("Email is too long");

            if (!Regex.IsMatch(email, @"^(.+)@(.+)$"))
                return Result.Fail<CustomerEmail>("Email is invalid");

            return Result.Ok(new CustomerEmail(email));
        }
        
        protected override bool EqualsCore(CustomerEmail obj)
        {
            return Value.Equals(obj.Value, StringComparison.CurrentCultureIgnoreCase);
        }

        public static implicit operator string(CustomerEmail customerEmail)
        {
            return customerEmail.Value;
        }

        public static explicit operator CustomerEmail(string customerEmail)
        {
            return Create(customerEmail).Value;
        }
    }
}
