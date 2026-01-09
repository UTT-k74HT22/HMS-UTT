namespace HospitalManagement.dto.request
{
    /// <summary>
    /// Request DTO để cập nhật ngưỡng tồn kho
    /// </summary>
    public class UpdateInventoryThresholdRequest
    {
        public int? MinThreshold { get; set; }
        public int? MaxThreshold { get; set; }
    }
}
