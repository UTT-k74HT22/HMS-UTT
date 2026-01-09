namespace HospitalManagement.dto.response;

public class CategoryResponse
{
    public long? Id { get; set; }
    public string Code { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public long? ParentId { get; set; }
    public bool Active { get; set; }
    public int? DisplayOrder { get; set; }
    public string ParentName { get; set; }

    // Giữ hành vi toString() như Java
    public override string ToString()
    {
        return Name;
    }
}