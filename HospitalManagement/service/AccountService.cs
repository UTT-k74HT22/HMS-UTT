using HospitalManagement.entity;

namespace HospitalManagement.service;

public interface AccountService
{
    /// <summary>
    /// Get all accounts
    /// </summary>
    List<Account> GetAccounts();
}