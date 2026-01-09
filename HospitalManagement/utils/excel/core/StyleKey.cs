namespace HospitalManagement.utils.excel.core
{
    /// <summary>
    /// Enum định nghĩa các loại style cho Excel
    /// </summary>
    public enum StyleKey
    {
        /// <summary>
        /// Style cho tiêu đề sheet (Dark Blue, Bold, White text)
        /// </summary>
        TITLE,
        
        /// <summary>
        /// Style cho header cột (Royal Blue, Bold, White text)
        /// </summary>
        HEADER,
        
        /// <summary>
        /// Style cho dữ liệu thường (White background, Black text)
        /// </summary>
        DATA,
        
        /// <summary>
        /// Style cho dữ liệu căn giữa
        /// </summary>
        DATA_CENTER,
        
        /// <summary>
        /// Style cho badge active (Green background, White text)
        /// </summary>
        BADGE_ACTIVE,
        
        /// <summary>
        /// Style cho badge inactive (Red background, White text)
        /// </summary>
        BADGE_INACTIVE
    }
}
