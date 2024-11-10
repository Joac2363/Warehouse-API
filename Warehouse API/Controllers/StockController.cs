using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Warehouse_API.DTO;
using Warehouse_API.Helpers;
using Warehouse_API.Interfaces;
using Warehouse_API.Models;
using Warehouse_API.Repositories;

namespace Warehouse_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StockController : Controller
    {
        private readonly IStockRepository _stockRepository;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public StockController(IStockRepository stockRepository, IWarehouseRepository warehouseRepository, IProductRepository productRepository, IMapper mapper)
        {
            _stockRepository = stockRepository;
            _warehouseRepository = warehouseRepository;
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet("all/")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetAllStock()
        {
            ICollection<StockDTO> stocks = _mapper.Map<ICollection<StockDTO>>(_stockRepository.GetAllStock());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(stocks);
        }

        [HttpGet("{warehouseId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult GetAllStockAtWarehouse(int warehouseId)
        {
            ICollection<StockDTO> stocks = _mapper.Map<ICollection<StockDTO>>(_stockRepository.GetAllStockAtWarehouse(warehouseId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(stocks);
        }

        [HttpGet("{warehouseId}/{productId}")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public IActionResult GetStock(int warehouseId, int productId)
        {
            if (!_stockRepository.StockExists(productId, warehouseId))
            {
                ModelState.AddModelError("Id", "A stock with thoose ids doesnt exist");
                return NotFound(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            StockDTO stock = _mapper.Map<StockDTO>(_stockRepository.GetStock(productId, warehouseId));

            return Ok(stock);
        }


        [HttpPost()]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(409)]
        [ProducesResponseType(422)]
        [ProducesResponseType(500)]
        public IActionResult CreateStock([FromBody] StockDTO stock)
        {
            if (_stockRepository.StockExists(stock.ProductId, stock.WarehouseId))
            {
                ModelState.AddModelError("Id", "A stock with thoose ids already exists");
                return Conflict(ModelState);
            }

            if (!_warehouseRepository.WarehouseExists(stock.WarehouseId))
            {
                ModelState.AddModelError("Id", "A warehouse with that id doesnt exists");
                return NotFound(ModelState);
            }
            
            if (!_productRepository.ProductExists(stock.ProductId))
            {
                ModelState.AddModelError("Id", "A product with that id doesnt exists");
                return NotFound(ModelState);
            }

            // Check if warehouse has enough capacity
            if (_warehouseRepository.GetTotalStock(stock.WarehouseId) + stock.Amount > _warehouseRepository.GetWarehouseCapacity(stock.WarehouseId))
            {
                ModelState.AddModelError("Amount", "The warehouse doesnt have enough capacity");
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            //Stock stocka = new Stock()
            //{
            //    ProductId = productId,
            //    Product = _productRepository.GetProduct(productId),
            //    WarehouseId = warehouseId,
            //    Warehouse = _warehouseRepository.GetWarehouse(warehouseId),
            //    Amount = amount,
            //    MinAcceptableStock = minAcceptableStock
            //};

            Stock stockMap = _mapper.Map<Stock>(stock);

            if (!_stockRepository.CreateStock(stockMap))
            {
                ModelState.AddModelError("", "Something went wrong while saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Succesfully created stock");
        }

        [HttpDelete("{warehouseId}/{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult DeleteStock(int warehouseId, int productId)
        {
            if (!_stockRepository.StockExists(productId, warehouseId))
            {
                ModelState.AddModelError("Id", "A stock with thoose ids doesnt exist");
                return NotFound(ModelState);
            }

            // Here you would also check if any other data is tied to this category and handle it

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_stockRepository.DeleteStock(_stockRepository.GetStock(productId, warehouseId)))
            {
                ModelState.AddModelError("", "Something went wrong deleting stock");
            }

            return Ok("Succesfully deleted stock");

        }

        [HttpPut("{warehouseId}/{productId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult UpdateStock(int warehouseId, int productId, [FromBody] StockDTO stock)
        {
            if (stock == null)
            {
                return BadRequest(ModelState);
            }
            if (stock.ProductId != productId || stock.WarehouseId != warehouseId)
            {
                ModelState.AddModelError("Id", "The supplied ids doesnt match body ids.");
                return BadRequest(ModelState);
            }

            if (!_stockRepository.StockExists(productId, warehouseId))
            {
                ModelState.AddModelError("Id", "A stock with thoose ids doesnt exist");
                return NotFound(ModelState);
            }

            // Check if warehouse has enough capacity
            int amountDifference = stock.Amount - _stockRepository.GetStock(productId, warehouseId).Amount;
            if (_warehouseRepository.GetTotalStock(stock.WarehouseId) + amountDifference > _warehouseRepository.GetWarehouseCapacity(stock.WarehouseId))
            {
                ModelState.AddModelError("Amount", "The warehouse doesnt have enough capacity");
                return BadRequest(ModelState);
            }

            // Here you would also check if any other data is tied to this category and handle it

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Stock stockMap = _mapper.Map<Stock>(stock);
            Stock existingStock = _stockRepository.GetStock(productId, warehouseId);
            existingStock.MinAcceptableStock = stockMap.MinAcceptableStock;
            existingStock.Amount = stockMap.Amount;

            if (!_stockRepository.UpdateStock(existingStock))
            {
                ModelState.AddModelError("", "Something went wrong updating stock");
            }

            return Ok("Succesfully updated stock");

        }
        
        [HttpPut("{fromWarehouseId}/{productId}/move/{toWarehouseId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        public IActionResult MoveStock(int productId, int toWarehouseId, int fromWarehouseId, [FromQuery] int amount)
        {
            if (!_stockRepository.StockExists(productId, fromWarehouseId))
            {
                ModelState.AddModelError("Id", "A stock with thoose ids doesnt exist");
                return NotFound(ModelState); 
            }

            Stock stock = _stockRepository.GetStock(productId, fromWarehouseId);

            if (amount == 0)
            {
                amount = stock.Amount;
            }

            if (stock.Amount < amount)
            {
                ModelState.AddModelError("Amount", $"Stock amount was {stock.Amount} whilst amount was {amount}");
                return BadRequest(ModelState);
            }

            // Check if warehouse has enough capacity
            if (_warehouseRepository.GetTotalStock(toWarehouseId) + amount > _warehouseRepository.GetWarehouseCapacity(toWarehouseId))
            {
                ModelState.AddModelError("Amount", "The warehouse doesnt have enough capacity");
                return BadRequest(ModelState);
            }

            // Here you would also check if any other data is tied to this category and handle it

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Warehouse toWarehouse = _warehouseRepository.GetWarehouse(toWarehouseId);

            if (!_stockRepository.MoveStock(stock, toWarehouse, amount))
            {
                ModelState.AddModelError("", "Something went wrong moving stock");
            }

            return Ok("Succesfully moved stock");

        }
    }
}
