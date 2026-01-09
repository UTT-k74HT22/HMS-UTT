using System;
using System.Collections.Generic;
using HospitalManagement.entity;
using HospitalManagement.service;

namespace HospitalManagement.Controller
{
    public class ManufacturerController
    {
        private readonly IManufacturerService _manufacturerService;

        public ManufacturerController(IManufacturerService manufacturerService)
        {
            _manufacturerService = manufacturerService;
        }

        public List<Manufacturer> GetAll()
        {
            return _manufacturerService.FindAll();
        }

        public Manufacturer FindById(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id is invalid");
            }

            return _manufacturerService.FindById(id);
        }

        public long Create(Manufacturer manufacturer)
        {
            if (manufacturer == null)
            {
                throw new ArgumentNullException(nameof(manufacturer));
            }

            return _manufacturerService.Create(manufacturer);
        }

        public void Update(Manufacturer manufacturer)
        {
            if (manufacturer == null)
            {
                throw new ArgumentNullException(nameof(manufacturer));
            }

            _manufacturerService.Update(manufacturer);
        }

        public void Delete(int id)
        {
            if (id <= 0)
            {
                throw new ArgumentException("id is invalid");
            }

            _manufacturerService.Delete(id);
        }
    }
}