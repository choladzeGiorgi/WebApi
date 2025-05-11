using WebApi.Models;

namespace WepApi.Dto
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int PhoneNumber { get; set; }
        public string Email {  get; set; }
        public string Password {  get; set; }
        public ICollection<Orders> Orders { get; set; }

        public ICollection<Product> Products { get; set; }


    }
}
