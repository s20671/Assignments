using System.Threading.Tasks;

namespace Cw4.Services
{
    public interface IServerDbService
    {
        Task<decimal> CheckProduct(int productId);
        Task<bool> CheckWarehouse(int warehouseId);
        Task<int> AddProductWarehouseByProcedure(int warehouseId, int productId, int Amount);
        Task<int> CheckOrder(int productId, int Amount);
        Task<bool> CheckIfOrderCompleted(int orderId);
        Task<bool> UpdateFullfilledAt(int orderId);
        Task<int> InsertToProductWarehouse(int warehouseID, int productId2, int orderId, int amount, decimal price);
    }
}
