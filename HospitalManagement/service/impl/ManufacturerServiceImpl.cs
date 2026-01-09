using System;
using System.Collections.Generic;
using HospitalManagement.entity;
using HospitalManagement.repository;
using HospitalManagement.configuration;
using HospitalManagement.repository.impl;

namespace HospitalManagement.service.impl
{
    public class ManufacturerServiceImpl : IManufacturerService
    {
        private readonly IManufacturerRepository _manufacturerRepository;

        public ManufacturerServiceImpl(DBConfig dbConfig)
        {
            _manufacturerRepository = new ManufacturerRepositoryImpl(dbConfig);
        }

        public List<Manufacturer> FindAll()
        {
            return _manufacturerRepository.FindAll();
        }

        public Manufacturer FindById(int id)
        {
            return _manufacturerRepository.FindById(id);
        }

        public long Create(Manufacturer manufacturer)
        {
            // ===== VALIDATION (giữ logic giống Java) =====
            if (string.IsNullOrWhiteSpace(manufacturer.Code))
            {
                throw new Exception("code ko được để trống");
            }

            if (string.IsNullOrWhiteSpace(manufacturer.Name))
            {
                throw new Exception("name ko được để trống");
            }

            return _manufacturerRepository.Insert(manufacturer);
        }

        public void Update(Manufacturer manufacturer)
        {
            _manufacturerRepository.Update(manufacturer);
        }

        public void Delete(int id)
        {
            _manufacturerRepository.DeleteById(id);
        }

        public bool ExistsByCode(string code)
        {
            return _manufacturerRepository.ExistsByCode(code);
        }
    }
}