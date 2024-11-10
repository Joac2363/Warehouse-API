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
    public class WarehouseController : Controller
    {
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IStockRepository _stockRepository;
        private readonly IMapper _mapper;

        public WarehouseController(IWarehouseRepository warehouseRepository, IStockRepository stockRepository, IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
            _stockRepository = stockRepository;
            _mapper = mapper;
        }

        [HttpGet("all/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetAllWarehouses()
        {
            ICollection<WarehouseDTO> warehouses = _mapper.Map<ICollection<WarehouseDTO>>(_warehouseRepository.GetAllWarehouses());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Ok(warehouses);
        }
        
        [HttpGet("{warehouseId}/products/")]
        [ProducesResponseType(200, Type = typeof(ICollection<ProductDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetAllProducts(int warehouseId)
        {
            // Validate WarehouseId
            if (!_warehouseRepository.WarehouseExists(warehouseId))
            {
                ModelState.AddModelError("Id", "A warehouse with that id doesnt exist");
                return NotFound(ModelState);
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ICollection<Product> allProducts = _warehouseRepository.GetAllProducts(warehouseId);
            ICollection<ProductDTO> products = _mapper.Map<ICollection<ProductDTO>>(allProducts);

            return Ok(products);
        }

        [HttpGet("{warehouseId}")]
        [ProducesResponseType(200, Type = typeof(WarehouseDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetWarehouse(int warehouseId)
        {
            // Validate WarehouseId
            if (!_warehouseRepository.WarehouseExists(warehouseId))
            {
                ModelState.AddModelError("Id", "A warehouse with that id doesnt exist");
                return NotFound(ModelState);
            }

            WarehouseDTO warehouseMap = _mapper.Map<WarehouseDTO>(_warehouseRepository.GetWarehouse(warehouseId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            return Ok(warehouseMap);
        }   
        
        [HttpGet("{warehouseId}/capacity")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        public IActionResult GetCapacity(int warehouseId)
        {
            // Validate WarehouseId
            if (!_warehouseRepository.WarehouseExists(warehouseId))
            {
                ModelState.AddModelError("Id", "A warehouse with that id doesnt exist");
                return NotFound(ModelState);
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int capacity = _warehouseRepository.GetWarehouseCapacity(warehouseId);

            return Ok(capacity);
        }
        
        [HttpGet("{warehouseId}/products/total")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        public IActionResult GetTotalStock(int warehouseId)
        {
            // Validate WarehouseId
            if (!_warehouseRepository.WarehouseExists(warehouseId))
            {
                ModelState.AddModelError("Id", "A warehouse with that id doesnt exist");
                return NotFound(ModelState);
            }
            
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int stock = _warehouseRepository.GetTotalStock(warehouseId);

            return Ok(stock);
        }
        
        [HttpGet("{warehouseId}/products/total/value")]
        [ProducesResponseType(200, Type = typeof(int))]
        [ProducesResponseType(400)]
        public IActionResult GetTotalValueOfStock(int warehouseId)
        {
            // Validate WarehouseId
            if (!_warehouseRepository.WarehouseExists(warehouseId))
            {
                ModelState.AddModelError("Id", "A warehouse with that id doesnt exist");
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            int stock = _warehouseRepository.GetTotalValueOfStock(warehouseId);

            return Ok(stock);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(500)]
        public IActionResult CreateWarehouse([FromBody] WarehouseDTO warehouseCreate)
        {
            // Validate WarehouseDTO 
            if (warehouseCreate == null)
            {
                return BadRequest(ModelState);
            }

            // Validate id in WarehouseDTO
            if (warehouseCreate.WarehouseId != 0)
            {
                ModelState.AddModelError("Id", "The Id field should not be provided.");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Warehouse warehouseMap = _mapper.Map<Warehouse>(warehouseCreate);

            // Create new Warehouse
            if (!_warehouseRepository.CreateWarehouse(warehouseMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully created warehouse");
        }

        [HttpDelete("{warehouseId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteWarehouse(int warehouseId)
        {
            // Validate WarehouseId
            if (!_warehouseRepository.WarehouseExists(warehouseId))
            {
                ModelState.AddModelError("Id", "A warehouse with that id doesnt exist");
                return NotFound();
            }

            // Validate Warehouse doenst have stock
            if (_stockRepository.GetAllStock().Any(s => s.WarehouseId == warehouseId))
            {
                ModelState.AddModelError("", "Cannot delete warehouse that still has stock");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Delete Warehouse
            if (!_warehouseRepository.DeleteWarehouse(warehouseId))
            {
                ModelState.AddModelError("", "Something went wrong deleting warehouse");
            }

            return Ok("Succesfully deleted warehouse");

        }

        [HttpPut("{warehouseId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateWarehouse(int warehouseId, [FromBody] WarehouseDTO updatedWarehouse)
        {
            // Validate WarehouseDTO
            if (updatedWarehouse == null)
            {
                return BadRequest(ModelState);
            }

            // Validate WarehouseIds match
            if (warehouseId != updatedWarehouse.WarehouseId)
            {
                ModelState.AddModelError("Id", "The query id doesnt match body id.");
                return BadRequest(ModelState);
            }

            // Validate WarehouseId
            if (!_warehouseRepository.WarehouseExists(warehouseId))
            {
                ModelState.AddModelError("Id", "A warehouse with that id doenst exist");
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Warehouse warehouseMap = _mapper.Map<Warehouse>(updatedWarehouse);

            // Update Warehouse
            if (!_warehouseRepository.UpdateWarehouse(warehouseMap))
            {
                ModelState.AddModelError("", "Something went wrong updating warehouse");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully updated warehouse");
        }
    }
}
