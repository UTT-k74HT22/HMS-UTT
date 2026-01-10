// using HospitalManagement.dto.response;
// using HospitalManagement.utils.excel.core;
// using OfficeOpenXml;
//
// namespace HospitalManagement.utils.excel.writers
// {
//     /// <summary>
//     /// Excel writer cho Employee
//     /// Export danh sách nhân viên ra file Excel
//     /// </summary>
//     public class EmployeeExcelWriter : AbstractExcelWriter<EmployeeResponse>
//     {
//         public override string SheetName => "Employees";
//         public override string Title => "DANH SÁCH NHÂN VIÊN";
//         public override string[] Headers => new[]
//         {
//             "STT",
//             "ID",
//             "Profile ID",
//             "Chức vụ",
//             "Phòng ban",
//             "Ngày vào làm",
//             "Lương cơ bản"
//         };
//
//         public override void Create(ExcelWorksheet worksheet, ExcelStyles styles, List<EmployeeResponse> data)
//         {
//             int currentRow = 1;
//
//             // ===== Title Row =====
//             worksheet.Row(currentRow).Height = 25;
//             worksheet.Cells[currentRow, 1, currentRow, Headers.Length].Merge = true;
//             SetCell(worksheet, currentRow, 1, Title, styles.Get(StyleKey.TITLE));
//             currentRow++;
//
//             // ===== Header Row =====
//             worksheet.Row(currentRow).Height = 18;
//             for (int i = 0; i < Headers.Length; i++)
//             {
//                 SetCell(worksheet, currentRow, i + 1, Headers[i], styles.Get(StyleKey.HEADER));
//             }
//             currentRow++;
//
//             // ===== Data Rows =====
//             int stt = 1;
//             foreach (var employee in data)
//             {
//                 worksheet.Row(currentRow).Height = 16;
//
//                 SetCell(worksheet, currentRow, 1, stt++, styles.Get(StyleKey.DATA_CENTER));
//                 SetCell(worksheet, currentRow, 2, employee.Id, styles.Get(StyleKey.DATA_CENTER));
//                 SetCell(worksheet, currentRow, 3, employee.ProfileId, styles.Get(StyleKey.DATA_CENTER));
//                 SetCell(worksheet, currentRow, 4, Safe(employee.Position), styles.Get(StyleKey.DATA));
//                 SetCell(worksheet, currentRow, 5, Safe(employee.Department), styles.Get(StyleKey.DATA));
//                 SetCell(worksheet, currentRow, 6, FormatDate(employee.HiredDate), styles.Get(StyleKey.DATA_CENTER));
//                 SetCell(worksheet, currentRow, 7, FormatNumber(employee.BaseSalary), styles.Get(StyleKey.DATA_CENTER));
//
//                 currentRow++;
//             }
//         }
//     }
// }
