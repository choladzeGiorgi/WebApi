
using System.Text.Json.Serialization;

namespace WepApi.Dto
{
    public class ProductImageDto
    {
        public int Id { get; set; }
        [JsonIgnore]
        public byte[]? ImageData { get; set; }
        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
        public DateTime UploadDate { get; set; }
    }
}