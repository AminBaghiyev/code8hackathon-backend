using Management.Core.Enums;

namespace Management.BL.DTOs;

public record RoomTableDTO
{
    public int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public RoomStatus Status { get; set; }
}

