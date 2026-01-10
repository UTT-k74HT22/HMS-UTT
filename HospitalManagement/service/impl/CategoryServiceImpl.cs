using HospitalManagement.dto.response.Category;
using HospitalManagement.entity;
using HospitalManagement.repository;
using System;
using System.Collections.Generic;

namespace HospitalManagement.service.impl
{
    public class CategoryServiceImpl : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryServiceImpl(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // ==================== DTO ====================
        public List<CategoryResponse> GetAllCategories() => _categoryRepository.FindAll() ?? new List<CategoryResponse>();

        public List<CategoryResponse> GetAllActiveCategories() => _categoryRepository.FindAllActive() ?? new List<CategoryResponse>();

        public List<CategoryResponse> SearchCategories(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return _categoryRepository.FindAll() ?? new List<CategoryResponse>();
            return _categoryRepository.Search(keyword.Trim()) ?? new List<CategoryResponse>();
        }

        public CategoryResponse? GetCategoryById(long id)
        {
            var category = _categoryRepository.FindById(id);
            if (category == null) return null;

            return new CategoryResponse
            {
                Id = category.Id,
                Code = category.Code,
                Name = category.Name,
                Description = category.Description,
                ParentId = category.ParentId,
                Active = category.IsActive,
                DisplayOrder = category.DisplayOrder
            };
        }

        // ==================== Entity ====================
        public Category? GetCategoryEntityById(long id) => _categoryRepository.FindById(id);

        public Category? GetCategoryByCode(string code) => _categoryRepository.FindByCode(code);

        // ==================== CRUD ====================
        public long CreateCategory(Category category)
        {
            if (_categoryRepository.ExistsByCode(category.Code))
                throw new Exception($"Category code already exists: {category.Code}");

            category.IsActive = true;
            return _categoryRepository.Insert(category);
        }

        public void UpdateCategory(Category category)
        {
            var existing = _categoryRepository.FindById(category.Id);
            if (existing == null) throw new Exception($"Category with id {category.Id} not found");

            _categoryRepository.Update(category);
        }

        public void Delete(string code)
        {
            var category = _categoryRepository.FindByCode(code);
            if (category == null) throw new Exception($"Category with code '{code}' not found");

            category.IsActive = false; // soft delete
            _categoryRepository.Update(category);
        }

        // ==================== Exists ====================
        public bool ExistsByCode(string code) => _categoryRepository.ExistsByCode(code);
    }
}
