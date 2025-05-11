using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using WebApi;
using WepApi.Dto;

namespace WebApi.Models
{
    public class Customer
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [JsonIgnore]
        public int PhoneNumber { get; set; }
        public string Email {  get; set; }
        [JsonIgnore]
        public string Password { get; set; }

        public void HashPassword()
        {
            Password = PasswordHelper.HashPassword(Password);
        }
      
       
        public ICollection<Orders> Orders { get; set; }

        public ICollection<Product> Products { get; set; } 


    }
}
