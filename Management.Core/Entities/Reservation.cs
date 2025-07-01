using Management.Core.Entities.Base;

namespace Management.Core.Entities;

public class Reservation : BaseEntity
{
    public string CustomerId { get; set; }
    public AppUser Customer { get; set; }
    public int RoomId { get; set; }
    public Room Room { get; set; }
    public DateTime CheckInDate { get; set; }
    public DateTime CheckOutDate { get; set; }
}
