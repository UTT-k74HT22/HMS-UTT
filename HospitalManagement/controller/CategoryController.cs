using HospitalManagement.dto.response.Category;
using HospitalManagement.entity;
using HospitalManagement.service;
using System.Collections.Generic;

namespace HospitalManagement.controller
{
    /// <summary>
    /// Controller cho các thao tác liên quan đến Category
    /// </summary>
    public class CategoryController
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // ==================== DTO ====================
        public List<CategoryResponse> GetAllCategories() => _categoryService.GetAllCategories();

        public List<CategoryResponse> GetAllActiveCategories() => _categoryService.GetAllActiveCategories();

        public CategoryResponse? GetCategoryById(long id) => _categoryService.GetCategoryById(id);

        public List<CategoryResponse> SearchCategories(string keyword) => _categoryService.SearchCategories(keyword);

        // ==================== Entity ====================
        public Category? GetCategoryEntityById(long id) => _categoryService.GetCategoryEntityById(id);

        public Category? GetCategoryByCode(string code) => _categoryService.GetCategoryByCode(code);

        // ==================== CRUD ====================
        public long CreateCategory(Category category) => _categoryService.CreateCategory(category);

        public void UpdateCategory(Category category) => _categoryService.UpdateCategory(category);

        public void DeleteCategory(string code) => _categoryService.Delete(code);

        // ==================== GET FOR ORDER ====================
        public List<CategoryResponse> GetAllForOrder() => _categoryService.GetAllCategories();
    }
}