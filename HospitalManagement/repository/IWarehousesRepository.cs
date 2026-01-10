using HospitalManagement.entity;
using System.Collections.Generic;

namespace HospitalManagement.repository
{
    public interface IWarehousesRepository
    {
        List<Warehouse> GetAll();
        List<Warehouse> GetAllActive();
        Warehouse? GetByCode(string code);

        void Create(Warehouse warehouse);
        void Update(Warehouse warehouse);

        // Soft delete
        void SoftDelete(string code);

        

        // Kiểm tra code tồn tại
        bool ExistsByCode(string code);
    }
}