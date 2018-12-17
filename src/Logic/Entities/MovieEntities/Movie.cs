using System;
using Logic.Common;
using Logic.Entities.CustomerEntities;

namespace Logic.Entities.MovieEntities
{
    public class Movie : Entity
    {
        public virtual string Name { get; protected set; }
        public virtual LicensingModel LicensingModel { get; protected set; }

        public virtual ExpirationDate GetExpirationDate()
        {
            switch (LicensingModel)
            {
                case LicensingModel.TwoDays:
                    return (ExpirationDate)DateTime.UtcNow.AddDays(2);

                case LicensingModel.LifeLong:
                    return ExpirationDate.Infinite;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public virtual Money CalculatePrice(CustomerStatus status)
        {
            Money price;
            switch (LicensingModel)
            {
                case LicensingModel.TwoDays:
                    price = (Money)4;
                    break;

                case LicensingModel.LifeLong:
                    price = (Money)8;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            if (status.IsAdvanced)
                price = price * (1 - status.GetGiscount());

            return price;
        }
    }
}
