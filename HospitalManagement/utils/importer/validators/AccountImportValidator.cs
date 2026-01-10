using HospitalManagement.utils.importer.core;
using HospitalManagement.utils.importer.dto;

namespace HospitalManagement.utils.importer.validators
{
    /// <summary>
    /// Validator để validate dữ liệu AccountImportDto
    /// </summary>
    public class AccountImportValidator : IImportValidator<AccountImportDto>
    {
        private readonly List<string> _validRoles = new() { "ADMIN", "EMPLOYEE", "CUSTOMER" };

        public List<ImportError> Validate(AccountImportDto data, int rowIndex)
        {
            var errors = new List<ImportError>();

            // Validate Username
            if (string.IsNullOrWhiteSpace(data.Username))
            {
                errors.Add(new ImportError(rowIndex, "Username", "Username không được để trống"));
            }
            else if (data.Username.Length < 3)
            {
                errors.Add(new ImportError(rowIndex, "Username", "Username phải có ít nhất 3 ký tự"));
            }
            else if (data.Username.Length > 50)
            {
                errors.Add(new ImportError(rowIndex, "Username", "Username không được quá 50 ký tự"));
            }

            // Validate Password
            if (string.IsNullOrWhiteSpace(data.Password))
            {
                errors.Add(new ImportError(rowIndex, "Password", "Password không được để trống"));
            }
            else if (data.Password.Length < 6)
            {
                errors.Add(new ImportError(rowIndex, "Password", "Password phải có ít nhất 6 ký tự"));
            }

            // Validate Role
            if (string.IsNullOrWhiteSpace(data.Role))
            {
                errors.Add(new ImportError(rowIndex, "Role", "Role không được để trống"));
            }
            else if (!_validRoles.Contains(data.Role.ToUpper()))
            {
                errors.Add(new ImportError(rowIndex, "Role", 
                    $"Role không hợp lệ. Phải là một trong: {string.Join(", ", _validRoles)}"));
            }

            return errors;
        }
    }
}
