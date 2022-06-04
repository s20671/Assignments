using Cw4.Models;
using Cw4.Services;
using Microsoft.AspNetCore.Mvc;
using System;
namespace Cw4.Controllers
{
    [Route("api/warehouses")]
    [ApiController]
    public class WarehousesController : ControllerBase
    {
        private readonly IServerDbService _serverDbService;
        public WarehousesController(IServerDbService serverDbService)
        {
            _serverDbService = serverDbService;
        }
       
        [HttpPost]
        public IActionResult postWarehouse(Warehouse warehouse)
        {
            warehouse.CreatedAt = DateTime.Now;
            decimal checkProduct = _serverDbService.CheckProduct(warehouse.IdProduct).Result;
            int orderId = _serverDbService.CheckOrder(warehouse.IdProduct, warehouse.Amount).Result;
            var checkWarehouse = _serverDbService.CheckWarehouse(warehouse.IdWarehouse).Result;
            var checkOrder = _serverDbService.CheckOrder(warehouse.IdProduct, warehouse.Amount).Result;
            var checkIfOrderComplete = _serverDbService.CheckIfOrderCompleted(orderId).Result;
       
            if (checkProduct is 0 || checkWarehouse is false)
            {
                return NotFound("No results!");
            }
            if (warehouse.Amount <= 0)
            {
                return BadRequest("Value must be greater than zero");
            }
            if (checkOrder is 0)
            {
                return NotFound("No order found");
            }
            if (checkIfOrderComplete is true)
            {
                return Conflict("Order is complete");
            }
               _serverDbService.UpdateFullfilledAt(orderId);
               var res = _serverDbService.InsertToProductWarehouse(warehouse.IdWarehouse, warehouse.IdProduct, orderId, warehouse.Amount, checkProduct).Result;
               return Ok(res);
        }
    }
}
