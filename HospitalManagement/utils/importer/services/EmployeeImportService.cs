// using HospitalManagement.controller;
// using HospitalManagement.utils.importer.core;
// using HospitalManagement.utils.importer.dto;
// using HospitalManagement.utils.importer.mappers;
// using HospitalManagement.utils.importer.validators;
//
// namespace HospitalManagement.utils.importer.services
// {
//     /// <summary>
//     /// Service để import Employee từ file Excel
//     /// </summary>
//     public class EmployeeImportService : AbstractImportService<EmployeeImportDto>
//     {
//         private readonly EmployeeController _controller;
//
//         public EmployeeImportService(EmployeeController controller)
//         {
//             _controller = controller;
//         }
//
//         protected override IImportMapper<EmployeeImportDto> GetMapper()
//         {
//             return new EmployeeImportMapper();
//         }
//
//         protected override IImportValidator<EmployeeImportDto> GetValidator()
//         {
//             return new EmployeeImportValidator();
//         }
//
//         protected override void SaveData(List<EmployeeImportDto> validData)
//         {
//             foreach (var dto in validData)
//             {
//                 // TODO: Implement create employee logic
//                 // _controller.CreateEmployee(dto);
//                 
//                 // Temporary: Just log or skip
//                 Console.WriteLine($"Would create employee for ProfileId: {dto.ProfileId}");
//             }
//         }
//     }
// }
