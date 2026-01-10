using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.service;
using System.Collections.Generic;

namespace HospitalManagement.controller
{
    /// <summary>
    /// Controller cho các thao tác liên quan đến Warehouse
    /// </summary>
    public class WarehousesController
    {
        private readonly IWarehousesService _warehousesService;

        public WarehousesController(IWarehousesService warehousesService)
        {
            _warehousesService = warehousesService;
        }

        // ==================== DTO ====================
        public List<WarehouseResponse> GetAllWarehouses()
        {
            return _warehousesService.GetAllWarehouses();
        }

        public List<WarehouseResponse> GetAllActiveWarehouses()
        {
            return _warehousesService.GetAllActiveWarehouses();
        }

        public WarehouseResponse? GetWarehouseByCode(string code)
        {
            return _warehousesService.GetWarehouseByCode(code);
        }

        public List<WarehouseResponse> SearchWarehouses(string keyword)
        {
            return _warehousesService.SearchWarehouses(keyword);
        }

        // ==================== Entity ====================
        public Warehouse? GetWarehouseEntityByCode(string code)
        {
            return _warehousesService.GetWarehouseEntityByCode(code);
        }

        // ==================== CRUD ====================
        public void CreateWarehouse(WarehouseRequest request)
        {
            _warehousesService.CreateWarehouse(request);
        }

        public void UpdateWarehouse(string code, WarehouseRequest request)
        {
            _warehousesService.UpdateWarehouse(code, request);
        }

        public void DeleteWarehouse(string code)
        {
            _warehousesService.DeleteWarehouse(code);
        }

        // ==================== GET FOR ORDER / COMBO ====================
        public List<WarehouseResponse> GetAllForOrder()
        {
            return _warehousesService.GetAllWarehouses();
        }
    }
}
