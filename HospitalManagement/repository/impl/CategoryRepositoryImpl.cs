using HospitalManagement.dto.response.Category;
using HospitalManagement.entity;

using HospitalManagement.repository;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;

namespace HospitalManagement.repository.impl
{
    public class CategoryRepositoryImpl : ICategoryRepository
    {
        private readonly string _connectionString;

        public CategoryRepositoryImpl(string connectionString)
        {
            _connectionString = connectionString;
        }

        // ==================== FIND ALL ====================
        public List<CategoryResponse> FindAll()
        {
            const string sql = @"
                SELECT c.id, c.code, c.name, c.description,
                       c.parent_id, c.is_active, c.display_order,
                       p.name AS parent_name
                FROM categories c
                LEFT JOIN categories p ON c.parent_id = p.id
                ORDER BY c.display_order, c.name";

            return QueryCategoryResponses(sql, null);
        }

        // ==================== FIND BY ID ====================
        public Category? FindById(long id)
        {
            const string sql = "SELECT * FROM categories WHERE id=@id";
            return QuerySingleCategory(sql, cmd => cmd.Parameters.AddWithValue("@id", id));
        }

        // ==================== FIND BY CODE ====================
        public Category? FindByCode(string code)
        {
            const string sql = "SELECT * FROM categories WHERE code=@code";
            return QuerySingleCategory(sql, cmd => cmd.Parameters.AddWithValue("@code", code));
        }

        // ==================== SEARCH ====================
        public List<CategoryResponse> Search(string keyword)
        {
            const string sql = @"
                SELECT c.id, c.code, c.name, c.description,
                       c.parent_id, c.is_active, c.display_order,
                       p.name AS parent_name
                FROM categories c
                LEFT JOIN categories p ON c.parent_id = p.id
                WHERE c.code LIKE @kw OR c.name LIKE @kw
                ORDER BY c.display_order, c.name";

            return QueryCategoryResponses(sql, cmd =>
            {
                string kw = $"%{keyword}%";
                cmd.Parameters.AddWithValue("@kw", kw);
            });
        }

        // ==================== INSERT ====================
        public long Insert(Category category)
        {
            const string sql = @"
                INSERT INTO categories
                (code, name, description, parent_id, is_active, display_order)
                VALUES (@code, @name, @description, @parentId, @active, @displayOrder);
                SELECT CAST(SCOPE_IDENTITY() AS BIGINT);";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@code", category.Code);
            cmd.Parameters.AddWithValue("@name", category.Name);
            cmd.Parameters.AddWithValue("@description", category.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@parentId", category.ParentId.HasValue ? (object)category.ParentId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@active", category.IsActive);
            cmd.Parameters.AddWithValue("@displayOrder", category.DisplayOrder);

            conn.Open();
            return Convert.ToInt64(cmd.ExecuteScalar());
        }

        // ==================== UPDATE ====================
        public void Update(Category category)
        {
            const string sql = @"
                UPDATE categories
                SET code=@code, name=@name, description=@description,
                    parent_id=@parentId, is_active=@active, display_order=@displayOrder
                WHERE id=@id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", category.Id);
            cmd.Parameters.AddWithValue("@code", category.Code);
            cmd.Parameters.AddWithValue("@name", category.Name);
            cmd.Parameters.AddWithValue("@description", category.Description ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@parentId", category.ParentId.HasValue ? (object)category.ParentId.Value : DBNull.Value);
            cmd.Parameters.AddWithValue("@active", category.IsActive);
            cmd.Parameters.AddWithValue("@displayOrder", category.DisplayOrder);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ==================== DELETE ====================
        public void DeleteById(long id)
        {
            const string sql = "UPDATE categories SET is_active=0 WHERE id=@id";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@id", id);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // ==================== EXISTS ====================
        public bool ExistsByCode(string code)
        {
            const string sql = "SELECT 1 FROM categories WHERE code=@code";

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@code", code);

            conn.Open();
            return cmd.ExecuteScalar() != null;
        }

        // ==================== FIND ALL ACTIVE ====================
        public List<CategoryResponse> FindAllActive()
        {
            const string sql = @"
                SELECT c.id, c.code, c.name, c.description,
                       c.parent_id, c.is_active, c.display_order,
                       p.name AS parent_name
                FROM categories c
                LEFT JOIN categories p ON c.parent_id = p.id
                WHERE c.is_active=1
                ORDER BY c.display_order, c.name";

            return QueryCategoryResponses(sql, null);
        }

        // ==================== HELPERS ====================
        private List<CategoryResponse> QueryCategoryResponses(string sql, Action<SqlCommand>? paramAction)
        {
            var list = new List<CategoryResponse>();

            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            paramAction?.Invoke(cmd);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                list.Add(MapToCategoryResponse(reader));
            }

            return list;
        }

        private Category? QuerySingleCategory(string sql, Action<SqlCommand> paramAction)
        {
            using var conn = new SqlConnection(_connectionString);
            using var cmd = new SqlCommand(sql, conn);
            paramAction(cmd);

            conn.Open();
            using var reader = cmd.ExecuteReader();

            return reader.Read() ? MapToCategory(reader) : null;
        }

        // ==================== MAPPERS ====================
        private Category MapToCategory(SqlDataReader r)
        {
            return new Category
            {
                Id = Convert.ToInt32(r["id"]),
                Code = Convert.ToString(r["code"])!,
                Name = Convert.ToString(r["name"])!,
                Description = r["description"] == DBNull.Value ? null : Convert.ToString(r["description"]),
                ParentId = r["parent_id"] == DBNull.Value ? null : Convert.ToInt32(r["parent_id"]),
                IsActive = Convert.ToBoolean(r["is_active"]),
                DisplayOrder = r["display_order"] == DBNull.Value ? 0 : Convert.ToInt32(r["display_order"])
            };
        }

        private CategoryResponse MapToCategoryResponse(SqlDataReader r)
        {
            return new CategoryResponse
            {
                Id = Convert.ToInt64(r["id"]),
                Code = Convert.ToString(r["code"])!,
                Name = Convert.ToString(r["name"])!,
                Description = r["description"] == DBNull.Value ? null : Convert.ToString(r["description"]),
                ParentId = r["parent_id"] == DBNull.Value ? null : Convert.ToInt64(r["parent_id"]),
                Active = Convert.ToBoolean(r["is_active"]),
                DisplayOrder = r["display_order"] == DBNull.Value ? 0 : Convert.ToInt32(r["display_order"]),
                ParentName = r["parent_name"] == DBNull.Value ? null : Convert.ToString(r["parent_name"])
            };
        }
    }
}

