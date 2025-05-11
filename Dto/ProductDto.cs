
using System.Text.Json.Serialization;

namespace WepApi.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public List<ProductImageDto> Images { get; set; } = new List<ProductImageDto>();
        public string? PrimaryImageUrl { get; set; }
    }
}

