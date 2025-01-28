using Microsoft.Data.Sqlite;
using LHF.Solid.Business.Models; 
namespace LHF.Solid.Business.Services.BadServices
{
    public class ProductBadService
    {
        private readonly string _connectionString = "Data Source=products.db";

        public void AddProduct(Product product)
        {
            if (product == null || string.IsNullOrWhiteSpace(product.Name) || product.Price <= 0)
            {
                throw new ArgumentException("Invalid product data.");
            }

            product.Id = Guid.NewGuid();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Products (Id, Name, Price) VALUES (@id, @name, @price)";
                    command.Parameters.AddWithValue("@id", product.Id);
                    command.Parameters.AddWithValue("@name", product.Name);
                    command.Parameters.AddWithValue("@price", product.Price);

                    command.ExecuteNonQuery();
                }
            }
        }

        public List<Product> GetAllProducts()
        {
            var products = new List<Product>();

            using (var connection = new SqliteConnection(_connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, Name, Price FROM Products"; 

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var product = new Product
                            {
                                Id = reader.GetGuid(0),
                                Name = reader.GetString(1),
                                Price = reader.GetDecimal(2)
                            };
                            products.Add(product);
                        }
                    }
                }
            }

            return products;
        }
    }
}