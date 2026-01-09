using System.Collections.Generic;
using HospitalManagement.entity;

namespace HospitalManagement.service
{
    public interface IManufacturerService
    {
        List<Manufacturer> FindAll();

        Manufacturer FindById(int id);

        long Create(Manufacturer manufacturer);

        void Update(Manufacturer manufacturer);

        void Delete(int id);

        bool ExistsByCode(string code);
    }
}