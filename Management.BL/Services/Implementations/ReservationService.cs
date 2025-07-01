using AutoMapper;
using Management.BL.DTOs;
using Management.BL.Services.Abstractions;
using Management.Core.Entities;
using Management.DL.Repositories.Abstractions;
using Management.DL.Repositories.Implementations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Management.BL.Services.Implementations
{
    public class ReservationService : IReservationService
    {
        private readonly IRepository<Reservation> _repository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IRepository<Room> _roomRepository;
        private readonly IRepository<Service> _serviceRepository;

        public ReservationService(IRepository<Reservation> repository,UserManager<AppUser> userManager,IMapper mapper,IRepository<Room> roomRepository, IRepository<Service> serviceRepository)
        {
            _repository = repository;
            _userManager = userManager;
            _mapper = mapper;
            _roomRepository = roomRepository;
            _serviceRepository=serviceRepository;
        }

        public async Task CreateRoomReservation(ReservationCreateDTO dto)
        {
            AppUser? user = await _userManager.FindByIdAsync(dto.CustomerId);
            if (user == null)
                throw new Exception("User is not found");

            Room? room = await _roomRepository.GetOneAsync(r => r.Id == dto.RoomId);
            if (room == null)
                throw new Exception("Room not found");

            var overlappingReservations = await _repository.GetAllAsync(
                r => r.RoomId == dto.RoomId &&
                (
                    (dto.CheckInDate >= r.CheckInDate && dto.CheckInDate < r.CheckOutDate) ||
                    (dto.CheckOutDate > r.CheckInDate && dto.CheckOutDate <= r.CheckOutDate) ||
                    (dto.CheckInDate <= r.CheckInDate && dto.CheckOutDate >= r.CheckOutDate)
                )
            );

            if (overlappingReservations.Any())
            {
                throw new Exception("Room is already booked for the selected dates.");
            }

            var reservation = _mapper.Map<Reservation>(dto);
            reservation.Customer = user;
            reservation.Room = room;

            if (dto.Service != null && dto.Service.Any())
            {
                var services = dto.Service.Select(s => new Service
                {
                    Name = s.Name,
                    Price = s.Price,
                    Reservation = reservation 
                }).ToList();

                reservation.Services = services;
            }

            await _repository.CreateAsync(reservation);
            await _repository.SaveChangesAsync();
        }


        public async Task<ICollection<ReservationTableItemDTO>> GetTableItemsByDateRangeAsync(DateTime startDate, DateTime endDate, int page = 0, int count = 10)
        {
            if (startDate == default) startDate = new DateTime(1900, 1, 1);
            if (endDate == default) endDate = new DateTime(2100, 1, 1);

            var reservations = await _repository.GetAllAsync(
                predicate: r => r.CheckInDate.Date >= startDate.Date && r.CheckOutDate.Date <= endDate.Date,
                includes: q => q.Include(r => r.Customer).Include(r => r.Room),
                page: page,
                count: count
            );

            var dtoList = _mapper.Map<ICollection<ReservationTableItemDTO>>(reservations);
            return dtoList;
        }


  
        public async Task<ReservationUpdateDTO> GetByIdForUpdateAsync(int id)
        {
            var reservation = await _repository.GetOneAsync(r => r.Id == id);
            if (reservation == null)
                throw new Exception("Reservation not found");

            var updateDto = _mapper.Map<ReservationUpdateDTO>(reservation);
            return updateDto;
        }

        public async Task<ICollection<ReservationListItemDTO>> GetListItemsAsync(int page = 0, int count = 0)
        {
            var reservations = await _repository.GetAllAsync(
                includes: q => q.Include(r => r.Customer)
                                .Include(r => r.Room),
                page: page,
                count: count
            );

            var dtoList = _mapper.Map<ICollection<ReservationListItemDTO>>(reservations);
            return dtoList;
        }
        //public async Task<ICollection<ReservationTableItemDTO>> GetTableItemsAsync(int page = 0, int count = 10)
        //{
        //    var reservations = await _repository.GetAllAsync(
        //        includes: q => q.Include(r => r.Customer)
        //                        .Include(r => r.Room),
        //        page: page,
        //        count: count
        //    );

        //    var dtoList = _mapper.Map<ICollection<ReservationTableItemDTO>>(reservations);
        //    return dtoList;
        //}

        public async Task DeleteAsync(int id)
        {
            var reservation = await _repository.GetOneAsync(r => r.Id == id);
            if (reservation == null)
                throw new Exception("Reservation not found");

            _repository.Delete(reservation);
            await _repository.SaveChangesAsync();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _repository.SaveChangesAsync();
        }
        public async Task UpdateAsync(ReservationUpdateDTO dto)
        {
            Reservation reservation = await _repository.GetOneAsync(e=>e.Id==dto.Id);
            if (reservation == null) return;

            _mapper.Map(dto, reservation); 
            _repository.Update(reservation);
            await _repository.SaveChangesAsync();
        }

     
    }
}











