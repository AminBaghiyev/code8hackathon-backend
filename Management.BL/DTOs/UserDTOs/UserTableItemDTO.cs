namespace Management.BL.DTOs;

public record UserTableItemDTO
{
    public string FullName { get; set; }
    public string Email { get; set; }
    public string PhoneNumber { get; set; }
}
