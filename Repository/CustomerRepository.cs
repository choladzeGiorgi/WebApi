using Microsoft.EntityFrameworkCore;
using WebApi;
using WebApi.Data;
using WebApi.Models;
using WepApi.InterFaces;

namespace WepApi.Repository
{
    public class CustomerRepository : ICustomerInterface
    {
        private readonly DataContext _context;
        public CustomerRepository(DataContext context) 
        {
         _context = context;
        }

       

        public bool CustomerExists(string email)
        {
            return _context.Customers.Any(x => x.Email == email);
        }

        public bool CreateCustomer(int id, string firstname, string lastname, int phonenumber, string email, string password)
        {

            if (CustomerExists(email))
            {
                return Save();
            }
            else
            {
                var lastCustomerId = _context.Customers.Last().Id;
                id = lastCustomerId + 1;
                Customer Customer = new Customer()
                {
                    Id = id,
                    FirstName = firstname,
                    LastName = lastname,
                    PhoneNumber = phonenumber,
                    Email = email,
                    Password = PasswordHelper.HashPassword(password)
                };

                _context.Add(Customer);

                return Save();
            }


        }

        public bool CreateCustomer(Customer Customer)
        {
            Customer.HashPassword();
            _context.Customers.Add(Customer);

            return Save();

        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public Customer GetCustomer(int id)
        {
            return _context.Customers.Where(c => c.Id == id).FirstOrDefault();
        }

        public ICollection<Customer> GetCustomers()
        {
            var customers = _context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.Product)
                        .ThenInclude(p => p.Images)
                .OrderBy(c => c.Id)
                .AsNoTracking() // Important for manual assignment
                .ToList();

            // Manually populate Products if absolutely needed
            var productIds = customers
                .SelectMany(c => c.Orders.Select(o => o.ProductId))
                .Distinct()
                .ToList();

            var products = _context.Products
                .Include(p => p.Images)
                .Where(p => productIds.Contains(p.Id))
                .ToDictionary(p => p.Id);

            foreach (var customer in customers)
            {
                customer.Products = customer.Orders
                    .Select(o => products.GetValueOrDefault(o.ProductId))
                    .Where(p => p != null)
                    .ToList();
            }

            return customers;
        }

        public ICollection<Product> GetCustomersProducts(int id)
        {
            var CustomerToGetProduct = _context.Customers.Where(c => c.Id == id).FirstOrDefault();

            var products = _context.Products.Include(p => p.Orders).ThenInclude(o => o.Customer).ToList();
            var orders = _context.Orders
               .Include(o => o.Product)   // Include related Product
               .Include(o => o.Customer)  // Include related Customer
               .ToList();

              CustomerToGetProduct.Products = orders
                    .Where(o => o.CustomerId == CustomerToGetProduct.Id)
                    .Select(o => products.FirstOrDefault(p => p.Id == o.ProductId))
                    .Where(p => p != null) 
                    .ToList();

            return CustomerToGetProduct.Products;


        }


    }
}

