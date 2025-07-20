using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WepApi.Dto;
using WepApi.InterFaces;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductInterface _productRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductInterface productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _mapper.Map<List<ProductDto>>(_productRepository.GetProducts());
            return Ok(products);
        }
        [HttpGet("{id}")]
        public IActionResult GetProduct(int id)
        {
            if (!_productRepository.ProductExists(id))
                return NotFound();
            return Ok(_mapper.Map<ProductDto>(_productRepository.GetProduct(id)));
        }

        [HttpGet("{id}/images")]
        public IActionResult GetProductImages(int id)
        {
            if (!_productRepository.ProductExists(id))
                return NotFound();

            var images = _productRepository.GetProductImages(id);
            var imageDtos = _mapper.Map<List<ProductImageDto>>(images);

            return Ok(imageDtos);
        }


        [HttpGet("{productId}/images/{imageId}")]
        public IActionResult GetProductImages(int productId, int imageId)
        {
            if (!_productRepository.ProductExists(productId))
                return NotFound();

            var image = _productRepository.GetProductImage(imageId);

            if (image == null || image.ImageData == null)
                return NotFound();

            var contentType = "image/jpeg";
           

            return File(image.ImageData, contentType);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromForm] ProductDto productDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Map main product properties
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Images = new List<ProductImage>()
            };

            // Process uploaded images
            if (Request.Form.Files.Count > 0)
            {
                foreach (var file in Request.Form.Files)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            await file.CopyToAsync(ms);
                            var isFirstImage = product.Images.Count == 0;

                            product.Images.Add(new ProductImage
                            {
                                ImageData = ms.ToArray(),
                                IsPrimary = isFirstImage,
                                UploadDate = DateTime.UtcNow
                            });

                            // Set first image as primary in Product.ImageData
                            if (isFirstImage)
                            {
                                product.Images.Add(new ProductImage() { ImageData =  ms.ToArray() });
                            }
                        }
                    }
                }
            }

            // Save to database
            if (!_productRepository.CreateProduct(product))
                return StatusCode(500, "Error saving product");

            // Return complete product data
            var createdProduct = _productRepository.GetProduct(product.Id);
            return Ok(_mapper.Map<ProductDto>(createdProduct));
        }

        [HttpPost("{id}/images")]
        public async Task<IActionResult> UploadImages(int id, [FromForm] List<IFormFile> files)
        {
            if (!_productRepository.ProductExists(id))
                return NotFound("Product not found");

            var images = new List<byte[]>();
            foreach (var file in files)
            {
                using var ms = new MemoryStream();
                await file.CopyToAsync(ms);
                images.Add(ms.ToArray());
            }

            if (!_productRepository.AddProductImages(id, images))
                return StatusCode(500, "Error saving images");

            return Ok(new { count = files.Count });
        }


        



        [HttpDelete("{productId}/images/{imageId}")]
        public IActionResult DeleteImage(int productId, int imageId)
        {
            if (!_productRepository.ProductExists(productId) ||
                !_productRepository.ProductImageExists(imageId))
                return NotFound();

            if (!_productRepository.DeleteProductImage(imageId))
                return StatusCode(500, "Error deleting image");

            return NoContent();
        }

        [HttpPatch("{productId}/images/{imageId}/setprimary")]
        public IActionResult SetPrimaryImage(int productId, int imageId)
        {
            if (!_productRepository.ProductExists(productId))
                return NotFound("Product not found");

            if (!_productRepository.ProductImageExists(imageId))
                return NotFound("Image not found");

            if (!_productRepository.SetPrimaryImage(productId, imageId))
                return StatusCode(500, "Error setting primary image");

            return NoContent();
        }
    }
}