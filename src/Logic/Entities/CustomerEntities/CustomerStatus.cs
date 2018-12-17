using Logic.Common;
using System;

namespace Logic.Entities.CustomerEntities
{
    public class CustomerStatus : ValueObject<CustomerStatus>
    {
        public static CustomerStatus Regular = new CustomerStatus(ExpirationDate.Infinite, CustomerStatusType.Regular);

        private CustomerStatus(ExpirationDate expirationDate, CustomerStatusType type) : this()
        {
            _expirationDate = expirationDate ?? throw new ArgumentNullException();
            Type = type;
        }

        public CustomerStatus()
        {
            
        }

        public CustomerStatusType Type { get; }

        private DateTime? _expirationDate;
        public ExpirationDate ExpirationDate => (ExpirationDate)_expirationDate;

        public bool IsAdvanced => Type == CustomerStatusType.Advanced && !ExpirationDate.IsExpired;
        public decimal GetGiscount() => IsAdvanced ? 0.25m : 0m;
        
        protected override bool EqualsCore(CustomerStatus obj)
        {
            return Type == obj.Type && ExpirationDate == obj.ExpirationDate;
        }

        public CustomerStatus Promote(ExpirationDate expirationDate)
        {
            return new CustomerStatus(expirationDate, CustomerStatusType.Advanced);
        }
    }

    public enum CustomerStatusType
    {
        Regular = 1,
        Advanced = 2
    }
}
