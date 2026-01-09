namespace HospitalManagement.dto.request.Category
{
    public class CreateCategoryRequest
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? ParentId { get; set; }
        public bool Active { get; set; }
        public int? DisplayOrder { get; set; }
    }
}