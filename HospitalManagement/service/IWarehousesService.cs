using HospitalManagement.dto.request;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using System.Collections.Generic;

namespace HospitalManagement.service
{
    public interface IWarehousesService
    {
        // DTO methods
        List<WarehouseResponse> GetAllWarehouses();
        List<WarehouseResponse> GetAllActiveWarehouses();
        WarehouseResponse? GetWarehouseByCode(string code);
        List<WarehouseResponse> SearchWarehouses(string keyword);

        // Entity methods
        Warehouse? GetWarehouseEntityByCode(string code);

        // CRUD
        void CreateWarehouse(WarehouseRequest request);
        void UpdateWarehouse(string code, WarehouseRequest request);
        void DeleteWarehouse(string code); // soft delete
    }
}