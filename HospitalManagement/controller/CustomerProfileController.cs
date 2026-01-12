using System;
using System.Collections.Generic;
using HospitalManagement.dto.response;
using HospitalManagement.entity;
using HospitalManagement.entity.enums;
using HospitalManagement.repository.impl;
using HospitalManagement.service.impl;

namespace HospitalManagement.Controller
{
    public class CustomerProfileController
    {
        private readonly CustomerProfileServiceImpl _service;

        public CustomerProfileController(CustomerProfileServiceImpl service) // ✅ nhận service
        {
            _service = service;
        }

        /* ==================== 1) Get all ==================== */
        public List<CustomerProfileResponse> GetAll()
        {
            return _service.GetAll();
        }

        /* ==================== 2) Get by Profile ID ==================== */
        public CustomerProfileResponse? GetByProfileId(int profileId)
        {
            return _service.GetByProfileId(profileId);
        }

        /* ==================== 3) Get by Code ==================== */
        public CustomerProfileResponse? GetByCode(string code)
        {
            return _service.GetByCode(code);
        }

        /* ==================== 4) Create ==================== */
        public bool Create(CustomerProfile profile)
        {
            try
            {
                _service.Create(profile);
                Console.WriteLine($"[Controller] Customer profile created: profile_id={profile.ProfileId}");
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Controller] Error creating customer profile: {ex.Message}");
                return false;
            }
        }

        /* ==================== 5) Update ==================== */
        public bool Update(CustomerProfileResponse model)
        {
            try
            {
                var result = _service.Update(model);
                Console.WriteLine($"[Controller] Customer profile updated: profile_id={model.ProfileId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Controller] Error updating customer profile: {ex.Message}");
                return false;
            }
        }

        /* ==================== 6) Soft delete ==================== */
        public bool SoftDelete(int profileId)
        {
            try
            {
                var result = _service.SoftDelete(profileId);
                Console.WriteLine($"[Controller] Customer profile soft deleted: profile_id={profileId}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Controller] Error deleting customer profile: {ex.Message}");
                return false;
            }
        }
    }
}
