// using HospitalManagement.controller;
// using HospitalManagement.entity;
// using HospitalManagement.utils.importer.core;
// using HospitalManagement.utils.importer.dto;
// using HospitalManagement.utils.importer.mappers;
// using HospitalManagement.utils.importer.validators;
//
// namespace HospitalManagement.utils.importer.services
// {
//     /// <summary>
//     /// Service để import Account từ file Excel
//     /// </summary>
//     public class AccountImportService : AbstractImportService<AccountImportDto>
//     {
//         private readonly AccountController _controller;
//
//         public AccountImportService(AccountController controller)
//         {
//             _controller = controller;
//         }
//
//         protected override IImportMapper<AccountImportDto> GetMapper()
//         {
//             return new AccountImportMapper();
//         }
//
//         protected override IImportValidator<AccountImportDto> GetValidator()
//         {
//             return new AccountImportValidator();
//         }
//
//         protected override void SaveData(List<AccountImportDto> validData)
//         {
//             foreach (var dto in validData)
//             {
//                 // TODO: Implement create account logic
//                 // _controller.CreateAccount(dto.Username, dto.Password, dto.Role, dto.IsActive);
//                 
//                 // Temporary: Just log or skip
//                 Console.WriteLine($"Would create account: {dto.Username}");
//             }
//         }
//     }
// }
