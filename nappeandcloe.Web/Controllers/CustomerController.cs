using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using nappeandcloe.Data;

namespace nappeandcloe.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private string _connectionString;
        public CustomerController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [Route("AddCustomer")]
        [HttpPost]
        public Customer AddCustomer(Customer customer)
        {
            CostumerRepository costumerRepo = new CostumerRepository(_connectionString);
            costumerRepo.AddCustomer(customer);
            return customer;
        }

        [Route("UpdateCustomer")]
        [HttpPost]
        public Customer UpdateCustomer(Customer customer)
        {
            CostumerRepository costumerRepo = new CostumerRepository(_connectionString);
            costumerRepo.UpdateCustomer(customer);
            return customer;
        }

        [Route("GetAllCustomers")]
        [HttpGet]
        public IEnumerable<Customer> GetAllCustomers()
        {
            CostumerRepository costumerRepo = new CostumerRepository(_connectionString);
            return costumerRepo.GetAllCustomers();
        }

        [Route("GetCustomerById/{customerId}")]
        [HttpGet]
        public Customer GetCustomerById(int customerId)
        {
            CostumerRepository costumerRepo = new CostumerRepository(_connectionString);
            return costumerRepo.GetCustomerById(customerId);
        }
    }
}