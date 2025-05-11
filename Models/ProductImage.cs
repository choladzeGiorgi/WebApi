// Product.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

// ProductImage.cs
namespace WebApi.Models
{
    public class ProductImage
    {
        [Key]
        public int Id { get; set; }
        [JsonIgnore]
        public byte[] ImageData { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime UploadDate { get; set; } = DateTime.UtcNow;

        [ForeignKey("Product")]
        public int ProductId { get; set; }

        [JsonIgnore]
        public Product Product { get; set; }
    }
}