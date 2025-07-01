using Management.Core.Entities.Base;

namespace Management.Core.Entities;

public class Service : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int ReservationId { get; set; }
    public Reservation Reservation { get; set; }
}
