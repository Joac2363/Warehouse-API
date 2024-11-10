using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Warehouse_API.DTO;
using Warehouse_API.Helpers;
using Warehouse_API.Interfaces;
using Warehouse_API.Models;
using Warehouse_API.Repositories;

namespace Warehouse_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseController : Controller
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public PurchaseController(IPurchaseRepository purchaseRepository, IOrderRepository orderRepository, IMapper mapper)
        {
            _purchaseRepository = purchaseRepository;
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        [HttpGet("all/")]
        [ProducesResponseType(200, Type = typeof(ICollection<PurchaseDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllPurchases()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ICollection<PurchaseDTO> purchases = _mapper.Map<ICollection<PurchaseDTO>>(_purchaseRepository.GetAllPurchases());

            return Ok(purchases);
        }

        [HttpGet("{purchaseId}")]
        [ProducesResponseType(200, Type = typeof(PurchaseDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetPurchase(int purchaseId)
        {
            // Validate PurchaseId
            if (!_purchaseRepository.PurchaseExists(purchaseId))
            {
                ModelState.AddModelError("Id", "A purchase with that id doesnt exist");
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            PurchaseDTO purchase = _mapper.Map<PurchaseDTO>(_purchaseRepository.GetPurchase(purchaseId));

            return Ok(purchase);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreatePruchase([FromBody] PurchaseDTO purchaseCreate)
        {
            //Validate PurchaseDTO
            if (purchaseCreate == null)
            {
                return BadRequest(ModelState);
            }

            // Validate PurchaseId
            if (purchaseCreate.PurchaseId != 0)
            {
                ModelState.AddModelError("Id", "The Id field should not be provided.");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Create new Purchase
            Purchase purchaseMap = _mapper.Map<Purchase>(purchaseCreate);
            if (!_purchaseRepository.CreatePurchase(purchaseMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully created purchase");
        }

        [HttpDelete("{purchaseId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeletePurchase(int purchaseId)
        {
            // Validate PurchaseId
            if (!_purchaseRepository.PurchaseExists(purchaseId))
            {
                ModelState.AddModelError("Id", "A purchase with that id doesnt exist");
                return NotFound(ModelState);
            }

            // Validate Purchase has no orders
            if (_orderRepository.GetAllOrders().Any(o => o.PurchaseId == purchaseId))
            {
                ModelState.AddModelError("", "Cannot delete purchase that still has orders");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Delete Purchase
            Purchase purchase = _purchaseRepository.GetPurchase(purchaseId);
            if (!_purchaseRepository.DeletePurchase(purchase))
            {
                ModelState.AddModelError("", "Something went wrong deleting purchase");
            }

            return Ok("Succesfully deleted purchase");

        }

        [HttpGet("{purchaseId}/value")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        public IActionResult GetValueOfPurchase(int purchaseId)
        {
            // Validate PurchaseId
            if (!_purchaseRepository.PurchaseExists(purchaseId))
            {
                ModelState.AddModelError("Id", "A purchase with that id doesnt exist");
                return NotFound(ModelState);
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int value = _purchaseRepository.GetValueOfStock(purchaseId);

            return Ok(value);
        }
    }
}
