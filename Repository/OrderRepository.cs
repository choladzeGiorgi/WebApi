using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;
using WepApi.InterFaces;

namespace WepApi.Repository
{
    public class OrderRepository : IOrdersInterface
    {

        private readonly DataContext _context;
        public OrderRepository(DataContext context)
        {
            _context = context;
        }

        public bool CostumerExist(int customerId)
        {
            return _context.Customers.Any(o => o.Id == customerId);    
        }

        public bool CreateOrder(Orders order)
        {
           _context.Orders.Add(order);
            return Save();
        }

        public ICollection<Orders> GetOrders()
        {
            return _context.Orders.OrderBy(o => o.Id).ToList();
        }

        public Orders GetOrders(int id)
        {
            return _context.Orders.Where(o => o.Id == id).FirstOrDefault();

        }

        public bool ProductExist(int productId)
        {
            return _context.Products.Any(p =>  p.Id == productId); 
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }
    }
}
