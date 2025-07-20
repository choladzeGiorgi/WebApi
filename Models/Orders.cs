using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApi.Data;

namespace WebApi.Models
{
    public class Orders
    {
      
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CustomerId { get; set; }


        [JsonIgnore]
        public Product Product { get; set; } 
        [JsonIgnore]
        public Customer Customer { get; set; } 



    }
}
