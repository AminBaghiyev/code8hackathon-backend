using FluentValidation;

namespace Management.BL.DTOs;

public record class RoomListDTO
{
    public int Id { get; set; }
    public int Number { get; set; }
}
