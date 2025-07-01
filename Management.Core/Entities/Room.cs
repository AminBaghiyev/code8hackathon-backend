using Management.Core.Entities.Base;
using Management.Core.Enums;

namespace Management.Core.Entities;

public class Room : BaseEntity
{
    public int Number { get; set; }
    public RoomType Type { get; set; }
    public decimal PricePerNight { get; set; }
    public RoomStatus Status { get; set; }
    public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}
