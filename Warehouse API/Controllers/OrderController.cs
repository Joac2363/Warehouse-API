using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Warehouse_API.Data;
using Warehouse_API.DTO;
using Warehouse_API.Interfaces;
using Warehouse_API.Models;
using Warehouse_API.Repositories;

namespace Warehouse_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly DataContext _context;
        private readonly IOrderRepository _orderRepository;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrderController(DataContext context, IOrderRepository orderRepository,IPurchaseRepository purchaseRepository, IProductRepository productRepository, IMapper mapper)
        {
            _context = context;
            _orderRepository = orderRepository;
            _purchaseRepository = purchaseRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet("all/")]
        [ProducesResponseType(200, Type = typeof(ICollection<Order>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllOrders()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ICollection<OrderDTO> orders = _mapper.Map<ICollection<OrderDTO>>(_orderRepository.GetAllOrders());

            return Ok(orders);
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(200, Type = typeof(Order))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetOrder(int orderId)
        {
            // Validate OrderId
            if (!_orderRepository.OrderExists(orderId))
            {
                ModelState.AddModelError("Id", "An order with that id doesnt exist");
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            OrderDTO order = _mapper.Map<OrderDTO>(_orderRepository.GetOrder(orderId));

            return Ok(order);
        }

        [HttpPost("{purchaseId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateOrder(int purchaseId, [FromBody] OrderDTO orderCreate)
        {
            // Validate OrderPTO
            if (orderCreate == null)
            {
                return BadRequest(ModelState);
            }

            // Validate OrderId
            if (orderCreate.OrderId != 0)
            {
                ModelState.AddModelError("Id", "The Id field should not be provided.");
                return BadRequest(ModelState);
            }

            // Validate PurchseId
            if (!_purchaseRepository.PurchaseExists(purchaseId))
            {
                ModelState.AddModelError("Id", "A purchase with that id doesnt exist");
                return NotFound(ModelState);
            }

            // Validate ProductId
            if (!_productRepository.ProductExists(orderCreate.ProductId))
            {
                ModelState.AddModelError("Id", "A product with that id doesnt exist");
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create Order
            Order orderMap = _mapper.Map<Order>(orderCreate);
            if (!_orderRepository.CreateOrder(orderMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully created order");
        }

        [HttpDelete("{orderId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteOrder(int orderId)
        {
            // Validate OrderId
            if (!_orderRepository.OrderExists(orderId))
            {
                ModelState.AddModelError("Id", "An order with that id doesnt exist");
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Delete Order
            Order order = _orderRepository.GetOrder(orderId);
            if (!_orderRepository.DeleteOrder(order))
            {
                ModelState.AddModelError("", "Something went wrong deleting order");
            }

            return Ok("Succesfully deleted order");

        }
    }
}
