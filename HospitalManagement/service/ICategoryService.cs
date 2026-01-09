using HospitalManagement.dto.response.Category;
using HospitalManagement.entity;
using System.Collections.Generic;

namespace HospitalManagement.service
{
    /// <summary>
    /// Service interface for Category
    /// </summary>
    public interface ICategoryService
    {
        // ==================== DTO ====================
        List<CategoryResponse> GetAllCategories();
        List<CategoryResponse> GetAllActiveCategories();
        List<CategoryResponse> SearchCategories(string keyword);
        CategoryResponse? GetCategoryById(long id);

        // ==================== Entity ====================
        Category? GetCategoryEntityById(long id);
        Category? GetCategoryByCode(string code);

        // ==================== CRUD ====================
        long CreateCategory(Category category);
        void UpdateCategory(Category category);
        void Delete(string code);

        // ==================== Exists ====================
        bool ExistsByCode(string code);
    }
}