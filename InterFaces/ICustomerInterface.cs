using WebApi.Models;

namespace WepApi.InterFaces
{
    public interface ICustomerInterface
    {
        ICollection<Customer> GetCustomers();
        Customer GetCustomer(int id);
        ICollection<Product> GetCustomersProducts(int id);
        bool CustomerExists(string email);
        bool CreateCustomer(int id, string firstname, string lastname, int phonenumber, string email, string password);
        bool CreateCustomer(Customer Customer);
        bool Save();
    }
}
