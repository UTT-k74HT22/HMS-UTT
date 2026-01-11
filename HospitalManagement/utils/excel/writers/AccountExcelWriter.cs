using HospitalManagement.dto.response;
using HospitalManagement.utils.excel.core;
using ClosedXML.Excel;

namespace HospitalManagement.utils.excel.writers
{
    /// <summary>
    /// Excel writer cho AccountResponse entity (ClosedXML version)
    /// Export danh sách tài khoản ra file Excel
    /// </summary>
    public class AccountExcelWriter : AbstractExcelWriter<AccountResponse>
    {
        public override string SheetName { get; } = "Danh sách tài khoản";
        public override string Title { get; } = "DANH SÁCH TÀI KHOẢN";

        public override string[] Headers { get; } = new string[]
        {
            "STT",
            "ID Tài khoản",
            "Tên đăng nhập",
            "ROLE",
            "Trạng thái"
        };
        
        public override void Create(IXLWorksheet worksheet, List<AccountResponse> data)
        {
            Console.WriteLine($"[AccountExcelWriter] Creating sheet for {data.Count} accounts");
            
            // Title row (merge cells A1:E1)
            Console.WriteLine("[AccountExcelWriter] Setting title...");
            var titleRange = worksheet.Range("A1:E1");
            titleRange.Merge();
            SetCell(worksheet, 1, 1, Title, ApplyTitleStyle);
            
            // Header row
            Console.WriteLine("[AccountExcelWriter] Setting headers...");
            for (int i = 0; i < Headers.Length; i++)
            {
                SetCell(worksheet, 2, i + 1, Headers[i], ApplyHeaderStyle);
            }

            // Data rows
            Console.WriteLine("[AccountExcelWriter] Writing data rows...");
            int row = 3;
            int stt = 1;
            foreach (var item in data)
            {
                SetCell(worksheet, row, 1, stt, cell => ApplyDataStyle(cell, true));
                SetCell(worksheet, row, 2, item.Id, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 3, item.Username, cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 4, item.Role.ToString(), cell => ApplyDataStyle(cell));
                SetCell(worksheet, row, 5, item.Active ? "Kích hoạt" : "Vô hiệu hóa", 
                    cell => ApplyBadgeStyle(cell, item.Active));
                
                row++;
                stt++;
            }
            
            Console.WriteLine($"[AccountExcelWriter] Completed writing {data.Count} rows");
        }
    }
}
