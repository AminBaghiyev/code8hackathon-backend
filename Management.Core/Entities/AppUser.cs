using Microsoft.AspNetCore.Identity;

namespace Management.Core.Entities;

public class AppUser : IdentityUser
{
    public string FullName { get; set; }
    public ICollection<Reservation> Reservations { get; set; }
}
