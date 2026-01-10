using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.repository;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HospitalManagement.service.impl
{
    public class WarehousesServiceImpl : IWarehousesService
    {
        private readonly IWarehousesRepository _warehousesRepository;

        // ==================== Constructor ====================
        public WarehousesServiceImpl(IWarehousesRepository warehousesRepository)
        {
            _warehousesRepository = warehousesRepository;
        }

        // ==================== DTO ====================
        public List<WarehouseResponse> GetAllWarehouses()
        {
            return _warehousesRepository.GetAll()
                .Select(MapToResponse)
                .ToList();
        }

        public List<WarehouseResponse> GetAllActiveWarehouses()
        {
            return _warehousesRepository.GetAll()
                .Where(w => w.IsActive)
                .Select(MapToResponse)
                .ToList();
        }

        public List<WarehouseResponse> SearchWarehouses(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return GetAllWarehouses();

            keyword = keyword.Trim().ToLower();

            return _warehousesRepository.GetAll()
                .Where(w =>
                    w.Code.ToLower().Contains(keyword) ||
                    w.Name.ToLower().Contains(keyword))
                .Select(MapToResponse)
                .ToList();
        }

        public WarehouseResponse? GetWarehouseByCode(string code)
        {
            var warehouse = _warehousesRepository.GetByCode(code);
            return warehouse == null ? null : MapToResponse(warehouse);
        }

        // ==================== Entity ====================
        public Warehouse? GetWarehouseEntityByCode(string code)
        {
            return _warehousesRepository.GetByCode(code);
        }

        // ==================== CRUD ====================
        public void CreateWarehouse(WarehouseRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Code))
                throw new Exception("Warehouse code is required");

            if (string.IsNullOrWhiteSpace(request.Name))
                throw new Exception("Warehouse name is required");

            if (_warehousesRepository.ExistsByCode(request.Code))
                throw new Exception($"Warehouse code already exists: {request.Code}");

            Warehouse warehouse = new Warehouse
            {
                Code = request.Code,
                Name = request.Name,
                Address = request.Address,
                Phone = request.Phone,
                ManagerName = request.ManagerName,
                IsActive = true,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _warehousesRepository.Create(warehouse);
        }

        public void UpdateWarehouse(string code, WarehouseRequest request)
        {
            var warehouse = _warehousesRepository.GetByCode(code);
            if (warehouse == null)
                throw new Exception($"Warehouse with code '{code}' not found");

            warehouse.Name = request.Name;
            warehouse.Address = request.Address;
            warehouse.Phone = request.Phone;
            warehouse.ManagerName = request.ManagerName;
            warehouse.IsActive = request.IsActive;
            warehouse.UpdatedAt = DateTime.Now;

            _warehousesRepository.Update(warehouse);
        }

        public void DeleteWarehouse(string code)
        {
            var warehouse = _warehousesRepository.GetByCode(code);
            if (warehouse == null)
                throw new Exception($"Warehouse with code '{code}' not found");

            // soft delete
            warehouse.IsActive = false;
            warehouse.UpdatedAt = DateTime.Now;

            _warehousesRepository.Update(warehouse);
        }

        // ==================== Mapper ====================
        private WarehouseResponse MapToResponse(Warehouse w)
        {
            return new WarehouseResponse
            {
                Id = (int)w.Id,
                Code = w.Code,
                Name = w.Name,
                Address = w.Address,
                Phone = w.Phone,
                ManagerName = w.ManagerName,
                IsActive = w.IsActive,
                CreatedAt = w.CreatedAt,
                UpdatedAt = w.UpdatedAt
            };
        }
    }
}
