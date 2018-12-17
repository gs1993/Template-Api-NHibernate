using System;
using Logic.Common;
using Logic.Entities.CustomerEntities;

namespace Logic.Entities.MovieEntities
{
    public class PurchasedMovie : Entity
    {
        public virtual Movie Movie { get; protected set; }
        public virtual Customer Customer { get; protected set; }

        private decimal _price;
        public virtual Money Price
        {
            get => (Money)_price;
            protected set => _price = value;
        }

        public virtual DateTime PurchaseDate { get; set; }

        private DateTime? _expirationDate;
        public virtual ExpirationDate ExpirationDate {
            get => (ExpirationDate)_expirationDate;
            protected set => _expirationDate = value;
        }

        public PurchasedMovie(Movie movie, Customer customer, Money price, ExpirationDate expirationDate) : this()
        {
            if(price is null || price.Value == 0)
                throw new ArgumentException(nameof(Money));

            if(expirationDate is null || expirationDate.IsExpired)
                throw new ArgumentException(nameof(Money));

            Movie = movie ?? throw new ArgumentNullException();
            Customer = customer ?? throw new ArgumentNullException();
            ExpirationDate = expirationDate;
            Price = price;
        }

        protected PurchasedMovie()
        {
            
        }
    }
}
