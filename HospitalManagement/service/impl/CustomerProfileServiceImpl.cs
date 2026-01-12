using System;
using System.Collections.Generic;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.entity.enums;
using HospitalManagement.repository.impl;
using Microsoft.Data.SqlClient;

namespace HospitalManagement.service.impl
{
    public class CustomerProfileServiceImpl : ICustomerProfileService
    {
        private readonly CustomerProfileRepositoryImpl _repository;

        public CustomerProfileServiceImpl(CustomerProfileRepositoryImpl repository)
        {
            _repository = repository;
        }

        /* ==================== 1) Create customer profile (with transaction) ==================== */
        public int Create(CustomerProfile profile)
        {
            using var conn = new SqlConnection(_repository.ConnectionString);
            conn.Open();
            using var tran = conn.BeginTransaction();

            try
            {
                // 1) Insert customer profile
                _repository.Insert(conn, tran, profile);

                // nếu cần insert thêm các bảng liên quan khác, làm trong đây

                tran.Commit();
                return profile.ProfileId; // trả về profileId vừa tạo
            }
            catch (Exception ex)
            {
                tran.Rollback();
                Console.WriteLine($"[CustomerProfileService] Create failed: {ex.Message}");
                throw;
            }
        }

        /* ==================== 2) Get all ==================== */
        public List<CustomerProfileResponse> GetAll()
        {
            return _repository.GetAll();
        }

        /* ==================== 3) Get by Profile ID ==================== */
        public CustomerProfileResponse? GetByProfileId(int profileId)
        {
            return _repository.GetByProfileId(profileId);
        }

        /* ==================== 4) Get by Code ==================== */
        public CustomerProfileResponse? GetByCode(string code)
        {
            return _repository.GetByCode(code);
        }

        /* ==================== 5) Update ==================== */
        public bool Update(CustomerProfileResponse model)
        {
            return _repository.Update(model);
        }

        /* ==================== 6) Soft delete ==================== */
        public bool SoftDelete(int profileId)
        {
            return _repository.SoftDelete(profileId);
        }
    }
}