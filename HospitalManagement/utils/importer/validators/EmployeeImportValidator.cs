using HospitalManagement.utils.importer.core;
using HospitalManagement.utils.importer.dto;

namespace HospitalManagement.utils.importer.validators
{
    /// <summary>
    /// Validator để validate dữ liệu EmployeeImportDto
    /// </summary>
    public class EmployeeImportValidator : IImportValidator<EmployeeImportDto>
    {
        public List<ImportError> Validate(EmployeeImportDto data, int rowIndex)
        {
            var errors = new List<ImportError>();

            // Validate ProfileId
            if (data.ProfileId <= 0)
            {
                errors.Add(new ImportError(rowIndex, "Profile ID", "Profile ID phải lớn hơn 0"));
            }

            // Validate Position
            if (string.IsNullOrWhiteSpace(data.Position))
            {
                errors.Add(new ImportError(rowIndex, "Chức vụ", "Chức vụ không được để trống"));
            }

            // Validate Department
            if (string.IsNullOrWhiteSpace(data.Department))
            {
                errors.Add(new ImportError(rowIndex, "Phòng ban", "Phòng ban không được để trống"));
            }

            // Validate HiredDate
            if (!data.HiredDate.HasValue)
            {
                errors.Add(new ImportError(rowIndex, "Ngày vào làm", "Ngày vào làm không hợp lệ"));
            }
            else if (data.HiredDate.Value > DateTime.Now)
            {
                errors.Add(new ImportError(rowIndex, "Ngày vào làm", "Ngày vào làm không được là tương lai"));
            }

            // Validate BaseSalary
            if (data.BaseSalary <= 0)
            {
                errors.Add(new ImportError(rowIndex, "Lương cơ bản", "Lương cơ bản phải lớn hơn 0"));
            }
            else if (data.BaseSalary > 1000000000) // 1 billion
            {
                errors.Add(new ImportError(rowIndex, "Lương cơ bản", "Lương cơ bản không hợp lý"));
            }

            return errors;
        }
    }
}
