using System.Text.Json.Serialization;
using WebApi.Models;

namespace WepApi.Dto
{
    public class OrderDto
    {
        public int ProductId { get; set; }
        public int CustomerId { get; set; }

       
    }
}
