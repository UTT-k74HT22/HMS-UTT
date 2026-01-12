using HospitalManagement.entity;

namespace HospitalManagement.dto.response;

public class CustomerProfileResponse
{
    public int ProfileId { get; set; }
    public long Id { get; set; }
    public string Code { get; set; }
    public string FullName { get; set; }
    public string Phone { get; set; }
    public string Email { get; set; }
    
    public string Address { get; set; }

    public CustomerType CustomerType { get; set; }
    public string TaxCode { get; set; }
    public ProfileStatus Status { get; set; }
}