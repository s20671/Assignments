using System;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace Cw4.Services
{
    public class ServerDbService : IServerDbService
    {
        public IConfiguration _configuration;
        public ServerDbService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<decimal> CheckProduct(int productID)
        {
            decimal productPrice = 0;
            using (SqlConnection con = new(_configuration.GetConnectionString("Default")))
            {
                SqlCommand com = new("SELECT Price FROM Product WHERE IdProduct = @productId", con);
                com.Parameters.AddWithValue("@productId", productID);
                await con.OpenAsync();
                var result = await com.ExecuteReaderAsync();
                while (result.Read())
                {
                    productPrice = decimal.Parse(result["Price"].ToString());
                }
            }
            Console.WriteLine(productPrice);
            return productPrice;
        }
        public async Task<bool> CheckWarehouse(int warehouseID)
        {
            using SqlConnection con = new(_configuration.GetConnectionString("Default"));
            SqlCommand com = new("SELECT 1 FROM Warehouse WHERE IdWarehouse = @warehouseId", con);
            com.Parameters.AddWithValue("@warehouseId", warehouseID);
            await con.OpenAsync();
            var result = await com.ExecuteReaderAsync();
            return result.HasRows;
        }
        public async Task<int> CheckOrder(int productID, int productAmount)
        {
            int IdOrders = 0;
            using (SqlConnection con = new(_configuration.GetConnectionString("Default")))
            {
                SqlCommand com = new("SELECT * FROM [Order] WHERE IdProduct = @productId AND Amount = @Amount AND CreatedAt < @createdAt ", con);
                com.Parameters.AddWithValue("@productId", productID);
                com.Parameters.AddWithValue("@Amount", productAmount);
                com.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString());
                await con.OpenAsync();
                var result = await com.ExecuteReaderAsync();
                while (result.Read())
                {
                    IdOrders = int.Parse(result["IdOrder"].ToString());
                }
            }
            return IdOrders;
        }
        public async Task<int> AddProductWarehouseByProcedure(int warehouseID, int productID, int productAmount) {
            using SqlConnection con = new(_configuration.GetConnectionString("Default"));
            SqlCommand command = con.CreateCommand();
            command.Connection = con;
            command.CommandText = "AddProductToWarehouse";
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("IdProduct", productID);
            command.Parameters.AddWithValue("IdWarehouse", warehouseID);
            command.Parameters.AddWithValue("Amount", productAmount);
            command.Parameters.AddWithValue("CreatedAt", DateTime.Now.ToString());
            await con.OpenAsync();
            command.ExecuteNonQuery();
            var primaryKey = int.Parse((await command.ExecuteScalarAsync()).ToString());
            return primaryKey;
        }
        public async Task<bool> CheckIfOrderCompleted(int orderID)
        {
            using SqlConnection con = new(_configuration.GetConnectionString("Default"));
            SqlCommand com = new("SELECT 1 FROM Product_warehouse WHERE IdOrder = @orderId", con);
            com.Parameters.AddWithValue("@orderId", orderID);
            await con.OpenAsync();
            var res = await com.ExecuteReaderAsync();
            return res.HasRows;
        }
        public async Task<int> InsertToProductWarehouse(int warehouseID, int productID, int orderID, int productPrice, decimal price)
        {
            using SqlConnection con = new(_configuration.GetConnectionString("Default"));
            await con.OpenAsync();
            System.Data.Common.DbTransaction trans = await con.BeginTransactionAsync();
            SqlCommand command = con.CreateCommand();
            command.Connection = con;
            command.Transaction = trans as SqlTransaction;
            try
            {
                command.CommandText = "INSERT INTO Product_Warehouse (IdWarehouse, IdProduct, IdOrder, Amount, Price, Createdat) output INSERTED.idproductwarehouse VALUES (@warehouseID, @productId2, @orderId, @amount, @price, @createdAt)";
                command.Parameters.AddWithValue("@warehouseID", warehouseID);
                command.Parameters.AddWithValue("@productId2", productID);
                command.Parameters.AddWithValue("@orderId", orderID);
                command.Parameters.AddWithValue("@amount", productPrice);
                command.Parameters.AddWithValue("@price", price * productPrice);
                command.Parameters.AddWithValue("@createdAt", DateTime.Now.ToString());
                var primaryKey = int.Parse((await command.ExecuteScalarAsync()).ToString());
                await trans.CommitAsync();
                return primaryKey;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Commit Exception Type: {0}", ex.GetType());
                Console.WriteLine("  Message: {0}", ex.Message);
                try
                {
                    await trans.RollbackAsync();
                }
                catch (Exception ex2)
                {
                    Console.WriteLine("Rollback Exception Type: {0}", ex2.GetType());
                    Console.WriteLine("  Message: {0}", ex2.Message);
                }
                return -1;
            }
        }
        public async Task<bool> UpdateFullfilledAt(int orderID)
        {
            using SqlConnection con = new(_configuration.GetConnectionString("Default"));
            SqlCommand com = new("UPDATE [Order] SET FulfilledAt = @FullfilledAt WHERE IdOrder = @orderId", con);
            com.Parameters.AddWithValue("@orderId", orderID);
            com.Parameters.AddWithValue("@FullfilledAt", DateTime.Now.ToString());
            await con.OpenAsync();
            var result = await com.ExecuteReaderAsync();
            return result.HasRows;
        }

    }
}
