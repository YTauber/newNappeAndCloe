using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nappeandcloe.Data
{
    public class CostumerRepository
    {
        private string _connectionString;
        public CostumerRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public Customer AddCustomer(Customer customer)
        {
            if (string.IsNullOrEmpty(customer.Name))
            {
                return null;
            }
            using (MyContext context = new MyContext(_connectionString))
            {
                context.Customers.Add(customer);
                context.SaveChanges();
                return customer;
            }
        }

        public void UpdateCustomer(Customer customer)
        {
            using (var context = new MyContext(_connectionString))
            {
                context.Customers.Attach(customer);
                context.Entry(customer).State = EntityState.Modified;
                context.SaveChanges();
            }
        }


        public IEnumerable<Customer> GetAllCustomers()
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Customers.ToList();
            }
        }

        public Customer GetCustomerById(int customerId)
        {
            using (MyContext context = new MyContext(_connectionString))
            {
                return context.Customers.FirstOrDefault(c => c.Id == customerId);
            }
        }
    }
}
