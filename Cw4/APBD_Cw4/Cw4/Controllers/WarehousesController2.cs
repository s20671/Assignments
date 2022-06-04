using Cw4.Models;
using Cw4.Services;
using Microsoft.AspNetCore.Mvc;
namespace Cw4.Controllers
{
    [Route("api/warehouses2")]
    [ApiController]
    public class WarehousesController2 : ControllerBase
    {
        private readonly IServerDbService _serverDbService;
        public WarehousesController2(IServerDbService serverDbService)
        {
            _serverDbService = serverDbService;
        }
       
        [HttpPost]
        public IActionResult PostWarehouse(Warehouse warehouse)
        {
            int res = AddProductProcedure(warehouse);
            return Ok(res);
        }

        private int AddProductProcedure(Warehouse warehouse)
        {
            return _serverDbService.AddProductWarehouseByProcedure(warehouse.IdWarehouse, warehouse.IdProduct, warehouse.Amount).Result;
        }
    }
}
