using System.Data;
using HospitalManagement.dto.request.Product;
using HospitalManagement.dto.response;
using HospitalManagement.dto.response.Product;
using HospitalManagement.entity.enums;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.repository.impl;

public class ProductRepositoryImpl : IProductRepository
{
    private readonly string _connectionString;

    public ProductRepositoryImpl(string connectionString)
    {
        _connectionString = connectionString;
    }
    
        // ================= INSERT =================
        public long Insert(CreateProductRequest request)
        {
            string sql = @"
                INSERT INTO products
                (category_id, manufacturer_id, code, barcode, name,
                 dosage_form, unit, description,
                 standard_price, requires_prescription, status)
                VALUES
                (@categoryId, @manufacturerId, @code, @barcode, @name,
                 @dosageForm, @unit, @description,
                 @standardPrice, @requiresPrescription, @status);

                SELECT SCOPE_IDENTITY();
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@categoryId", request.CategoryId);
            cmd.Parameters.AddWithValue("@manufacturerId",
                (object?)request.ManufacturerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@code", request.Code);
            cmd.Parameters.AddWithValue("@barcode", request.Barcode);
            cmd.Parameters.AddWithValue("@name", request.Name);
            cmd.Parameters.AddWithValue("@dosageForm", request.DosageForm);
            cmd.Parameters.AddWithValue("@unit", request.Unit);
            cmd.Parameters.AddWithValue("@description", request.Description);
            cmd.Parameters.AddWithValue("@standardPrice", request.StandardPrice);
            cmd.Parameters.AddWithValue("@requiresPrescription", request.RequiresPrescription);
            cmd.Parameters.AddWithValue("@status", request.Status.ToString());

            conn.Open();
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        // ================= UPDATE =================
        public void UpdateByCode(string code, UpdateProductRequest request)
        {
            string sql = @"
                UPDATE products SET
                    category_id = @categoryId,
                    manufacturer_id = @manufacturerId,
                    barcode = @barcode,
                    name = @name,
                    dosage_form = @dosageForm,
                    unit = @unit,
                    description = @description,
                    standard_price = @standardPrice,
                    requires_prescription = @requiresPrescription,
                    status = @status
                WHERE code = @code
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@categoryId", request.CategoryId);
            cmd.Parameters.AddWithValue("@manufacturerId",
                (object?)request.ManufacturerId ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@barcode", request.Barcode);
            cmd.Parameters.AddWithValue("@name", request.Name);
            cmd.Parameters.AddWithValue("@dosageForm", request.DosageForm);
            cmd.Parameters.AddWithValue("@unit", request.Unit);
            cmd.Parameters.AddWithValue("@description", request.Description);
            cmd.Parameters.AddWithValue("@standardPrice", request.StandardPrice);
            cmd.Parameters.AddWithValue("@requiresPrescription", request.RequiresPrescription);
            cmd.Parameters.AddWithValue("@status", request.Status.ToString());
            cmd.Parameters.AddWithValue("@code", code);

            conn.Open();
            cmd.ExecuteNonQuery();
        }
        public List<ProductResponse> FindByName(string name)
        {
                    const string sql = @"
                SELECT
                    id,
                    code,
                    name,
                    standard_price,
                    requires_prescription,
                    status
                FROM products
                WHERE name LIKE @name
                ORDER BY name
            ";

            var list = new List<ProductResponse>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@name", $"%{name}%");

            conn.Open();
            using var rs = cmd.ExecuteReader();

            while (rs.Read())
            {
                list.Add(new ProductResponse
                {
                    Id = Convert.ToInt64(rs["id"]),
                    Code = rs["code"].ToString(),
                    Name = rs["name"].ToString(),
                    StandardPrice = (decimal)rs["standard_price"],
                    RequiresPrescription = (bool)rs["requires_prescription"],
                    Status = Enum.Parse<ProductStatus>(rs["status"].ToString())
                });
            }

            return list;
        }

        // ================= SOFT DELETE =================
        public void SoftDeleteByCode(string code)
        {
            string sql = "UPDATE products SET status = @status WHERE code = @code";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@status", ProductStatus.DISCONTINUED.ToString());
            cmd.Parameters.AddWithValue("@code", code);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ================= EXISTS =================
        public bool ExistsByCode(string code)
        {
            string sql = "SELECT 1 FROM products WHERE code = @code";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@code", code);

            conn.Open();
            using var reader = cmd.ExecuteReader();
            return reader.Read();
        }

        // ================= GET ALL =================
        public List<ProductResponse> GetAll()
        {
            string sql = @"
                SELECT p.id, p.code, p.name,
                       c.name AS categoryName,
                       m.name AS manufacturerName,
                       p.standard_price,
                       p.requires_prescription,
                       p.status
                FROM products p
                JOIN categories c ON p.category_id = c.id
                LEFT JOIN manufacturers m ON p.manufacturer_id = m.id
                ORDER BY p.id DESC
            ";

            var list = new List<ProductResponse>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            conn.Open();
            using var rs = cmd.ExecuteReader();

            while (rs.Read())
            {
                list.Add(MapToProductResponse(rs));
            }

            return list;
        }

        // ================= FIND BY CODE =================
        public ProductResponse FindByCode(string code)
        {
            string sql = @"
                SELECT p.id, p.code, p.name,
                       c.name AS categoryName,
                       m.name AS manufacturerName,
                       p.standard_price,
                       p.requires_prescription,
                       p.status
                FROM products p
                JOIN categories c ON p.category_id = c.id
                LEFT JOIN manufacturers m ON p.manufacturer_id = m.id
                WHERE p.code = @code
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@code", code);

            conn.Open();
            using var rs = cmd.ExecuteReader();

            return rs.Read() ? MapToProductResponse(rs) : null;
        }

        // ================= FIND DETAIL =================
        public ProductDetailResponse FindDetailByCode(string code)
        {
            string sql = @"
                SELECT
                    p.id, p.code, p.barcode, p.name,
                    p.dosage_form, p.unit, p.description, p.image_url,
                    p.standard_price, p.requires_prescription, p.status,
                    c.id AS categoryId, c.name AS categoryName,
                    m.id AS manufacturerId, m.code AS manufacturerCode,
                    m.name AS manufacturerName, m.country AS manufacturerCountry
                FROM products p
                JOIN categories c ON p.category_id = c.id
                LEFT JOIN manufacturers m ON p.manufacturer_id = m.id
                WHERE p.code = @code
            ";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@code", code);

            conn.Open();
            using var rs = cmd.ExecuteReader();

            if (!rs.Read()) return null;

            return new ProductDetailResponse
            {
                Id = Convert.ToInt64(rs["id"]),
                Code = rs["code"]?.ToString(),
                Barcode = rs["barcode"]?.ToString(),
                Name = rs["name"]?.ToString(),
                DosageForm = rs["dosage_form"]?.ToString(),
                Unit = rs["unit"]?.ToString(),
                Description = rs["description"]?.ToString(),
                ImageUrl = rs["image_url"]?.ToString(),

                StandardPrice = (decimal)rs["standard_price"],
                RequiresPrescription = (bool)rs["requires_prescription"],
                Status = Enum.Parse<ProductStatus>(rs["status"].ToString()),

                CategoryId = Convert.ToInt64(rs["categoryId"]),
                CategoryName = rs["categoryName"]?.ToString(),

                ManufacturerId = rs["manufacturerId"] == DBNull.Value
                    ? null
                    : Convert.ToInt64(rs["manufacturerId"]),
                ManufacturerCode = rs["manufacturerCode"]?.ToString(),
                ManufacturerName = rs["manufacturerName"]?.ToString(),
                ManufacturerCountry = rs["manufacturerCountry"]?.ToString()
            };
    
        }

        // ================= MAP =================
        private ProductResponse MapToProductResponse(IDataReader rs)
        {
            return new ProductResponse
            {
                Id = Convert.ToInt64(rs["id"]),
                Code = rs["code"]?.ToString(),
                Name = rs["name"]?.ToString(),
                CategoryName = rs["categoryName"]?.ToString(),
                ManufacturerName = rs["manufacturerName"]?.ToString(),
                StandardPrice = (decimal)rs["standard_price"],
                RequiresPrescription = (bool)rs["requires_prescription"],
                Status = Enum.Parse<ProductStatus>(rs["status"].ToString())
            };
        }


        // ================= UNUSED (but required by interface) =================
        public ProductResponse FindById(long id) => null;
        public List<BatchResponse> FindBatchesByProduct(long productId)
        {
            const string sql = @"
        SELECT
            id,
            batch_code,
            expiry_date
        FROM batches
        WHERE product_id = @productId
        ORDER BY expiry_date ASC
    ";

            var list = new List<BatchResponse>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@productId", productId);

            conn.Open();
            using var rs = cmd.ExecuteReader();

            while (rs.Read())
            {
                list.Add(new BatchResponse
                {
                    Id = Convert.ToInt64(rs["id"]),
                    BatchCode = rs["batch_code"]?.ToString(),
                    ExpiryDate = rs["expiry_date"] == DBNull.Value
                        ? null
                        : Convert.ToDateTime(rs["expiry_date"]).Date
                });
            }

            return list;
        }

        public List<ProductResponse> FindByCategory(long categoryId)
        {
            const string sql = @"
        SELECT
            p.id,
            p.code,
            p.name,
            p.standard_price,
            p.status
        FROM products p
        WHERE p.category_id = @categoryId
          AND p.status = @status
        ORDER BY p.name
    ";

            var list = new List<ProductResponse>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@categoryId", categoryId);
            cmd.Parameters.AddWithValue("@status", ProductStatus.ACTIVE.ToString());

            conn.Open();
            using var rs = cmd.ExecuteReader();

            while (rs.Read())
            {
                list.Add(new ProductResponse
                {
                    Id = Convert.ToInt64(rs["id"]),
                    Code = rs["code"]?.ToString(),
                    Name = rs["name"]?.ToString(),
                    StandardPrice = (decimal)rs["standard_price"],
                    Status = Enum.Parse<ProductStatus>(rs["status"].ToString())
                });
            }

            return list;
        }
    }