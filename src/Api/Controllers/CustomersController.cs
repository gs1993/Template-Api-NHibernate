using System.Collections.Generic;
using System.Linq;
using Logic.Common;
using Logic.Dtos.Customer;
using Logic.Dtos.Movie;
using Logic.Entities.CustomerEntities;
using Logic.Entities.MovieEntities;
using Logic.Repositories;
using Logic.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    public class CustomersController : BaseController
    {
        private readonly MovieRepository _movieRepository;
        private readonly CustomerRepository _customerRepository;

        public CustomersController(UnitOfWork unitOfWork, MovieRepository movieRepository,
            CustomerRepository customerRepository) : base(unitOfWork)
        {
            _customerRepository = customerRepository;
            _movieRepository = movieRepository;
        }

        [HttpGet]
        [Route("{id}")]
        public IActionResult Get(long id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
                return Error("Not Found");

            var dto = new CustomerDto()
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email,
                MoneySpent = customer.MoneySpent,
                Status = customer.Status.Type.ToString(),
                StatusExpirationDate = customer.Status.ExpirationDate,
                PurchasedMovies = customer.PurchasedMovies.Select(x => new PurchasedMovieDto()
                {
                    Price = x.Price.Value,
                    ExpirationDate = x.ExpirationDate,
                    PurchaseDate = x.PurchaseDate,
                    Movie = new MovieDto()
                    {
                        Id = x.Movie.Id,
                        Name = x.Movie.Name
                    }
                }).ToList()
            };

            return Ok(dto);
        }

        [HttpGet]
        public IActionResult GetList()
        {
            IReadOnlyList<Customer> customers = _customerRepository.GetList();

            var dtoList = customers.Select(c => new CustomerListDto()
            {
                Id = c.Id,
                Email = c.Email.Value,
                MoneySpent = c.MoneySpent,
                Name = c.Name.Value,
                Status = c.Status.ToString(),
                StatusExpirationDate = c.Status.ExpirationDate
            });

            return Ok(dtoList);
        }

        [HttpPost]
        public IActionResult Create([FromBody] CreateCustomerDto item)
        {
            var customerNameResult = CustomerName.Create(item.Name);
            var customerEmailResult = CustomerEmail.Create(item.Email);
            var moneySpent = Money.Create(0);

            var result = Result.Combine(customerNameResult, customerEmailResult, moneySpent);
            if (result.IsFailure)
                return Error(result.Error);

            if (_customerRepository.GetByEmail(item.Email) != null)
                return Error("Email is already in use: " + item.Email);

            var newCustomer = new Customer(customerNameResult.Value, customerEmailResult.Value);
            _customerRepository.Add(newCustomer);

            return Ok();
        }

        [HttpPut]
        [Route("{id}")]
        public IActionResult Update(long id, [FromBody] UpdateCustomerDto item)
        {
            var customerNameResult = CustomerName.Create(item.Name);
            if (customerNameResult.IsFailure)
                return Error(customerNameResult.Error);

            Customer customer = _customerRepository.GetById(id);
            if (customer == null || !customer.IsActive)
                return Error("Invalid customer id: " + id);

            customer.Name = customerNameResult.Value;

            return Ok();
        }
        
        [HttpPost]
        [Route("{id}/movies")]
        public IActionResult PurchaseMovie(long id, [FromBody] long movieId)
        {
            Movie movie = _movieRepository.GetById(movieId);
            if (movie == null)
                return Error("Invalid movie id: " + movieId);

            Customer customer = _customerRepository.GetById(id);
            if (customer == null || !customer.IsActive)
                return Error("Invalid customer id: " + id);
            
            if (customer.AlreadyPurchased(movie))
                return Error("The movie is already purchased: " + movie.Name);
            
            customer.PurchaseMovie(movie);

            return Ok();
        }
        
        [HttpPost]
        [Route("{id}/promotion")]
        public IActionResult PromoteCustomer(long id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null || !customer.IsActive)
                return Error("Invalid customer id: " + id);

            var result = customer.Promote();
            if (result.IsFailure)
                return Error(result.Error);

            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult DeactivateCustomer(long id)
        {
            Customer customer = _customerRepository.GetById(id);
            if (customer == null)
                return Error("Invalid customer id: " + id);

            if (!customer.IsActive)
                return Error("The customer has already been deleted");

            var result = customer.Deactivate();
            if (result.IsFailure)
                return Error(result.Error);

            return Ok();
        }
    }
}
