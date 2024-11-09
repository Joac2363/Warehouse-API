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
        private readonly IMapper _mapper;

        public WarehouseController(IWarehouseRepository warehouseRepository, IMapper mapper)
        {
            _warehouseRepository = warehouseRepository;
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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetAllProducts(int warehouseId)
        {
            if (!_warehouseRepository.WarehouseExists(warehouseId))
            {
                ModelState.AddModelError("Id", "A warehouse with that id doesnt exist");
                return NotFound(ModelState);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ICollection<WarehouseDTO> warehouses = _mapper.Map<ICollection<WarehouseDTO>>(_warehouseRepository.GetAllProducts(warehouseId));

            return Ok(warehouses);
        }

        [HttpGet("{warehouseId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetWarehouse(int warehouseId)
        {
            if (!_warehouseRepository.WarehouseExists(warehouseId))
            {
                ModelState.AddModelError("Id", "A warehouse with that id doesnt exist");
                return NotFound(ModelState);
            }

            WarehouseDTO warehouse = _mapper.Map<WarehouseDTO>(_warehouseRepository.GetWarehouse(warehouseId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(warehouse);
        }
        
        
        [HttpGet("{warehouseId}/capacity")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetCapacity(int warehouseId)
        {
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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetTotalStock(int warehouseId)
        {
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
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetTotalValueOfStock(int warehouseId)
        {
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
            if (warehouseCreate == null)
            {
                return BadRequest(ModelState);
            }

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
        public IActionResult DeleteProduct(int warehouseId)
        {
            if (!_warehouseRepository.WarehouseExists(warehouseId))
            {
                ModelState.AddModelError("Id", "A warehouse with that id doesnt exist");
                return NotFound();
            }

            // Here you would also check if any other data is tied to this category and handle it

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
            if (updatedWarehouse == null)
            {
                return BadRequest(ModelState);
            }

            if (warehouseId != updatedWarehouse.WarehouseId)
            {
                ModelState.AddModelError("Id", "The query id doesnt match body id.");
                return BadRequest(ModelState);
            }

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

            if (!_warehouseRepository.UpdateWarehouse(warehouseMap))
            {
                ModelState.AddModelError("", "Something went wrong updating warehouse");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully updated warehouse");
        }
    }
}
