using System.Collections.Generic;
using HospitalManagement.entity;
using HospitalManagement.dto.response;

namespace HospitalManagement.repository
{
    public interface IManufacturerRepository
    {
        List<Manufacturer> FindAll();
        Manufacturer FindById(int id);
        List<Manufacturer> SearchByCode(string code);
        long Insert(Manufacturer m);
        void Update(Manufacturer m);
        void DeleteById(int id);
        bool ExistsByCode(string code);
        List<ManufacturerResponse> FindAllActive();
    }

}