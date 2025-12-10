using HospitalManagement.entity;

namespace HospitalManagement.repository
{
    public interface IAccountRepository
    {
        List<Account> FindAll();
        Account? FindByUsername(string username);
        void Save(Account account);
        void Update(Account account);
        void DeleteById(int id);
        bool ExistsByUsername(string username);
    }
}
