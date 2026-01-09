namespace HospitalManagement.dto.response.Category
{
    public class CategoryResponse
    {
        public long Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long? ParentId { get; set; }
        public bool Active { get; set; }
        public int? DisplayOrder { get; set; }
        public string? ParentName { get; set; }

        public override string ToString() => Name;
    }
}