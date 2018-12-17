using System;
using System.Collections.Generic;
using System.Linq;
using Logic.Common;
using Logic.Entities.MovieEntities;

namespace Logic.Entities.CustomerEntities
{
    public class Customer : Entity
    {
        private string _name;
        public virtual CustomerName Name
        {
            get => (CustomerName)_name;
            set => _name = value;
        }

        private readonly string _email;
        public virtual CustomerEmail Email => (CustomerEmail)_email;

        public virtual CustomerStatus Status { get; protected set; }

        private decimal _moneySpent;
        public virtual Money MoneySpent
        {
            get => (Money)_moneySpent;
            protected set => _moneySpent = value;
        }

        private IList<PurchasedMovie> _purchasedMovies;
        public virtual IReadOnlyList<PurchasedMovie> PurchasedMovies => _purchasedMovies.ToList();
        
        protected Customer()
        {
            _purchasedMovies = new List<PurchasedMovie>();
        }
        
        public Customer(CustomerName name, CustomerEmail email) : this()
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _email = email ?? throw new ArgumentNullException(nameof(email));
            MoneySpent = (Money) 0;
            Status = CustomerStatus.Regular;
        }

        public virtual bool AlreadyPurchased(Movie movie) => PurchasedMovies.Any(x => x.Movie == movie && x.ExpirationDate.IsExpired);

        public virtual void PurchaseMovie(Movie movie)
        {
            if (AlreadyPurchased(movie))
                throw new InvalidOperationException();

            ExpirationDate expirationDate = movie.GetExpirationDate();
            Money price = movie.CalculatePrice(Status);
            var purchasedMovie = new PurchasedMovie(movie, this, price, expirationDate);
            _purchasedMovies.Add(purchasedMovie);
            MoneySpent += price;
        }

        public virtual Result Promote()
        {
            if (Status.IsAdvanced)
                return Result.Fail("The customer already has the Advanced status");

            var expirationDateResult = ExpirationDate.Create(DateTime.UtcNow.AddYears(1));

            if (expirationDateResult.IsFailure)
                return Result.Fail("Cannot promote the customer");
            
            if (PurchasedMovies.Count(x => x.ExpirationDate == ExpirationDate.Infinite
                || x.ExpirationDate.Value >= DateTime.UtcNow.AddDays(-30)) < 2)
                return Result.Fail("Customer must have at least 2 active movies during the last 30 days");
            
            if (PurchasedMovies.Where(x => x.PurchaseDate > DateTime.UtcNow.AddYears(-1)).Sum(x => x.Price) < 100m)
                return Result.Fail("Customer must spend at least 100 dollars spent during the last year");

            Status = Status.Promote((ExpirationDate)DateTime.UtcNow.AddYears(1));
            return Result.Ok();
        }
    }
}
