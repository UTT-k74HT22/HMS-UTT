using HospitalManagement.dto.response.Category;
using HospitalManagement.entity;
using System.Collections.Generic;

namespace HospitalManagement.repository
{
    /// <summary>
    /// Repository interface for Category
    /// </summary>
    public interface ICategoryRepository
    {
        /// <summary>
        /// Get all categories
        /// </summary>
        /// <returns>List of categories</returns>
        List<CategoryResponse> FindAll();

        /// <summary>
        /// Find category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        /// <returns>Category or null if not found</returns>
        Category? FindById(long id);

        /// <summary>
        /// Find category by code
        /// </summary>
        /// <param name="code">Category code</param>
        /// <returns>Category or null if not found</returns>
        Category? FindByCode(string code);

        /// <summary>
        /// Insert category and return generated ID
        /// </summary>
        /// <param name="category">Category to insert</param>
        /// <returns>Inserted category ID</returns>
        long Insert(Category category);

        /// <summary>
        /// Update category
        /// </summary>
        /// <param name="category">Category to update</param>
        void Update(Category category);

        /// <summary>
        /// Soft delete category by ID
        /// </summary>
        /// <param name="id">Category ID</param>
        void DeleteById(long id);

        /// <summary>
        /// Search categories by keyword (code or name)
        /// </summary>
        /// <param name="keyword">Keyword</param>
        /// <returns>List of matching categories</returns>
        List<CategoryResponse> Search(string keyword);

        /// <summary>
        /// Check if category exists by code
        /// </summary>
        /// <param name="code">Category code</param>
        /// <returns>True if exists</returns>
        bool ExistsByCode(string code);

        /// <summary>
        /// Get all active categories
        /// </summary>
        /// <returns>List of active categories</returns>
        List<CategoryResponse> FindAllActive();
    }
}
