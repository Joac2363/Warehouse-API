using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Warehouse_API.DTO;
using Warehouse_API.Helpers;
using Warehouse_API.Interfaces;
using Warehouse_API.Models;

namespace Warehouse_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public ProductController(IProductRepository productRepository, IStockRepository stockRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _stockRepository = stockRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet("all/")]
        [ProducesResponseType(200, Type = typeof(ICollection<ProductDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllProducts() 
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            ICollection<ProductDTO> products = _mapper.Map<ICollection<ProductDTO>>(_productRepository.GetAllProducts());
            
            return Ok(products);
        }
        
        [HttpGet("{productId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetProduct(int productId) 
        {
            // Validate ProductId
            if (!_productRepository.ProductExists(productId)) 
            {
                ModelState.AddModelError("Id", "A product with that id doesnt exist");
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            ProductDTO product = _mapper.Map<ProductDTO>(_productRepository.GetProduct(productId));

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateProduct([FromBody] ProductDTO productCreate)
        {
            // Validate ProductDTO
            if (productCreate == null)
            {
                return BadRequest(ModelState);
            }

            // Validate ProductId
            if (productCreate.ProductId != 0)
            {
                ModelState.AddModelError("Id", "The Id field should not be provided.");
                return BadRequest(ModelState);
            }
            
            // Validate SKU uniqueness
            bool SKUExists = _productRepository.GetAllProducts()
                                                .Where(p => p.SKU == productCreate.SKU)
                                                .Any();
            if (SKUExists)
            {
                ModelState.AddModelError("SKU", "Product with that SKU already exists");
                return StatusCode(409, ModelState);
            }

            // Validate SKU
            if (!SKUHelper.IsValidSKU(productCreate.SKU) && productCreate.SKU != 0) 
            {
                ModelState.AddModelError("SKU", "SKU was not 8 digits");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create new Product
            Product productMap = _mapper.Map<Product>(productCreate);
            if (!_productRepository.CreateProduct(productMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully created product");
        }

        [HttpDelete("{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteProduct(int productId)
        {
            // Validate ProductId
            if (!_productRepository.ProductExists(productId))
            {
                ModelState.AddModelError("Id", "A product with that id doesnt exist");
                return NotFound(ModelState);
            }

            // Validate Product has no orders
            if (_orderRepository.GetAllOrders().Any(s => s.ProductId == productId))
            {
                ModelState.AddModelError("", "Cannot delete product that still has orders");
                return BadRequest(ModelState);
            }

            // Validate Product has no stock
            if (_stockRepository.GetAllStock().Any(s => s.ProductId == productId))
            {
                ModelState.AddModelError("", "Cannot delete product that is still in stock");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // Delete Product
            if (!_productRepository.DeleteProduct(productId))
            {
                ModelState.AddModelError("", "Something went wrong deleting product");
            }

            return Ok("Succesfully deleted product");
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int productId, [FromBody] ProductDTO updatedProduct)
        {
            // Validate ProductPTO
            if (updatedProduct == null)
            {
                return BadRequest(ModelState);
            }

            // Validate ProductIds match
            if (productId != updatedProduct.ProductId)
            {
                ModelState.AddModelError("Id", "The query id doesnt match body id.");
                return BadRequest(ModelState);
            }

            // Validate ProductId
            if (!_productRepository.ProductExists(productId))
            {
                ModelState.AddModelError("Id", "A product with that id doenst exist");
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Product productMap = _mapper.Map<Product>(updatedProduct);

            // Update Product
            if (!_productRepository.UpdateProduct(productMap))
            {
                ModelState.AddModelError("", "Something went wrong updating product");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully updated product");
        }
    }
}
