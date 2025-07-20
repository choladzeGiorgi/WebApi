using WebApi.Models;
using WepApi.Dto;

namespace WepApi.InterFaces
{
    public interface IProductInterface
    {
        // Product methods
        ICollection<Product> GetProducts();
        Product GetProduct(int id);
        ICollection<Product> GetProductsByName(string name);
        bool ProductExists(int id);
        bool UpdateProduct(Product product);

        // Image methods
        ICollection<ProductImage> GetProductImages(int productId);

        ProductImage GetProductImage(int imageId);
        bool AddProductImage(int productId, ProductImage image);
        bool AddProductImage(int productId, byte[] imageData);
        bool DeleteProductImage(int imageId);
        bool SetPrimaryImage(int productId, int imageId);
        bool ProductImageExists(int imageId);
        bool AddProductImages(int productId, IEnumerable<ProductImage> images);
        bool AddProductImages(int productId, IEnumerable<byte[]> imagesData);

        bool Save();
       
        // Legacy methods
        bool UpdateProductImage(int productId, byte[] imageData);
        bool CreateProduct(Product product);
    }
}