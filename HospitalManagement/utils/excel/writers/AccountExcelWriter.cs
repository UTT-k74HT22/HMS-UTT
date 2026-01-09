using HospitalManagement.entity;
using HospitalManagement.utils.excel.core;
using OfficeOpenXml;

namespace HospitalManagement.utils.excel.writers
{
    /// <summary>
    /// Excel writer cho Account entity
    /// Export danh sách tài khoản ra file Excel
    /// </summary>
    public class AccountExcelWriter : AbstractExcelWriter<Account>
    {
        public override string SheetName => "Accounts";
        public override string Title => "DANH SÁCH TÀI KHOẢN";
        public override string[] Headers => new[]
        {
            "STT",
            "Account ID",
            "Username",
            "Role",
            "Trạng thái",
            "Last Login",
            "Ngày tạo"
        };

        public override void Create(ExcelWorksheet worksheet, ExcelStyles styles, List<Account> data)
        {
            int currentRow = 1;

            // ===== Title Row =====
            worksheet.Row(currentRow).Height = 25;
            worksheet.Cells[currentRow, 1, currentRow, Headers.Length].Merge = true;
            SetCell(worksheet, currentRow, 1, Title, styles.Get(StyleKey.TITLE));
            currentRow++;

            // ===== Header Row =====
            worksheet.Row(currentRow).Height = 18;
            for (int i = 0; i < Headers.Length; i++)
            {
                SetCell(worksheet, currentRow, i + 1, Headers[i], styles.Get(StyleKey.HEADER));
            }
            currentRow++;

            // ===== Data Rows =====
            int stt = 1;
            foreach (var account in data)
            {
                worksheet.Row(currentRow).Height = 16;

                SetCell(worksheet, currentRow, 1, stt++, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 2, account.Id, styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 3, Safe(account.Username), styles.Get(StyleKey.DATA));
                SetCell(worksheet, currentRow, 4, account.Role, styles.Get(StyleKey.DATA_CENTER));

                // Status badge
                var statusText = account.IsActive ? "ACTIVE" : "INACTIVE";
                var statusStyle = account.IsActive 
                    ? styles.Get(StyleKey.BADGE_ACTIVE) 
                    : styles.Get(StyleKey.BADGE_INACTIVE);
                SetCell(worksheet, currentRow, 5, statusText, statusStyle);

                SetCell(worksheet, currentRow, 6, FormatDateTime(account.LastLoginAt), styles.Get(StyleKey.DATA_CENTER));
                SetCell(worksheet, currentRow, 7, FormatDate(account.CreatedAt), styles.Get(StyleKey.DATA_CENTER));

                currentRow++;
            }
        }
    }
}
