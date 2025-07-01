namespace Management.BL.DTOs;

public record DashboardDTO
{
    public int FullRoomCount { get; set; }
    public int EmptyRoomCount { get; set; }
    public int CustomerCount { get; set; }
}
