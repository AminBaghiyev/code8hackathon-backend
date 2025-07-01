using Management.Core.Enums;

namespace Management.BL.DTOs;

public record ReservationTableItemForUserDTO
{
    public int RoomNumber { get; set; }
    public RoomType RoomType { get; set; }
    public decimal RoomPricePerNight { get; set; }
    public RoomStatus RoomStatus { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}
