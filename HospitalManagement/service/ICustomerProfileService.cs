using System.Collections.Generic;
using HospitalManagement.dto.response;
using HospitalManagement.entity;

namespace HospitalManagement.service
{
    public interface ICustomerProfileService
    {
        int Create(CustomerProfile profile);
        List<CustomerProfileResponse> GetAll();
        CustomerProfileResponse? GetByProfileId(int profileId);
        CustomerProfileResponse? GetByCode(string code);
        bool Update(CustomerProfileResponse model);
        bool SoftDelete(int profileId);
    }
}