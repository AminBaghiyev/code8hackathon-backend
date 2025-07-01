namespace Management.BL.DTOs;

public record ReservationTableItemDTO
{
    public int Id { get; set; }
    public string CustomerName { get; set; }
    public int RoomNumber { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}
