using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebApi.Data;
using WebApi.Models;
using WepApi.InterFaces;
using static System.Net.Mime.MediaTypeNames;

namespace WepApi.Repository
{
    public class ProductRepository : IProductInterface
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context) => _context = context;

        public ICollection<Product> GetProducts() =>
            _context.Products.Include(p => p.Images).OrderBy(p => p.Id).ToList();

        public Product GetProduct(int id) =>
            _context.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == id);

        public ICollection<Product> GetProductsByName(string name) =>
            _context.Products.Include(p => p.Images)
                .Where(p => p.Name.ToLower().Contains(name.ToLower())).ToList();

        public bool ProductExists(int id) => _context.Products.Any(p => p.Id == id);

        public bool UpdateProduct(Product product)
        {
            _context.Products.Update(product);
            return Save();
        }

        public ICollection<ProductImage> GetProductImages(int productId) =>
            _context.ProductImages.Where(pi => pi.ProductId == productId).ToList();

        public ProductImage GetProductImage(int imageId) =>
            _context.ProductImages.FirstOrDefault(pi => pi.Id == imageId);


       

        public bool AddProductImage(int productId, ProductImage image)
        {
            if (!ProductExists(productId)) return false;
            image.ProductId = productId;
            _context.ProductImages.Add(image);
            return Save();
        }
        
        public bool AddProductImage(int productId, byte[] imageData) =>
            AddProductImage(productId, new ProductImage { ImageData = imageData });

        public bool DeleteProductImage(int imageId)
        {
            var image = _context.ProductImages.Find(imageId);
            if (image == null) return false;
            _context.ProductImages.Remove(image);
            return Save();
        }

        public bool SetPrimaryImage(int productId, int imageId)
        {
            var product = _context.Products.Include(p => p.Images).FirstOrDefault(p => p.Id == productId);
            var image = product?.Images.FirstOrDefault(i => i.Id == imageId);
            if (product == null || image == null) return false;

            foreach (var img in product.Images) img.IsPrimary = false;
            image.IsPrimary = true;
            return Save();
        }

        public bool ProductImageExists(int imageId) =>
            _context.ProductImages.Any(pi => pi.Id == imageId);

        public bool AddProductImages(int productId, IEnumerable<ProductImage> images)
        {
            if (!ProductExists(productId))
                return false;
            if (images == null || !images.Any()) return false;

            // Attach through navigation property (auto-sets ProductId)
            var product = _context.Products
                .Include(p => p.Images)
                .FirstOrDefault(p => p.Id == productId);

            if (product == null) return false;

            foreach (var image in images)
            {
                image.UploadDate = DateTime.UtcNow; // Ensure timestamp
                product.Images.Add(image); // Let EF set the ProductId
            }

            try
            {
                return _context.SaveChanges() > 0;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }
        public bool AddProductImages(int productId, IEnumerable<byte[]> imagesData) =>
            AddProductImages(productId, imagesData.Select(data => new ProductImage { ImageData = data }));



        public bool UpdateProductImage(int productId, byte[] imageData)
        {
            var product = _context.Products.Find(productId);
            if (product == null) return false;
            return Save();
        }
        // Add to WepApi.Repository.ProductRepository
        public bool CreateProduct(Product product)
        {
            _context.Products.Add(product);
            return Save();
        }
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }



    }
}