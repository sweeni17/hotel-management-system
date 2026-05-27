using HotelManagement.API.Data;
using HotelManagement.API.Models;
using HotelManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.API.Repositories.Implementations;

public class ReservationRepository : IReservationRepository
{
    private readonly HotelDbContext _dbContext;

    public ReservationRepository(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Reservation>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reservations
            .AsNoTracking()
            .Include(reservation => reservation.Room)
            .Include(reservation => reservation.User)
            .Where(reservation => reservation.IsDeleted != true)
            .OrderByDescending(reservation => reservation.CreatedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<Reservation?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reservations
            .Include(reservation => reservation.Room)
            .Include(reservation => reservation.User)
            .Include(reservation => reservation.Payments)
            .FirstOrDefaultAsync(
                reservation => reservation.ReservationId == id && reservation.IsDeleted != true,
                cancellationToken);
    }

    public async Task<Reservation> AddAsync(Reservation entity, CancellationToken cancellationToken = default)
    {
        entity.CreatedAt ??= DateTime.UtcNow;
        entity.IsDeleted ??= false;
        entity.ReservationStatus ??= "Confirmed";

        await _dbContext.Reservations.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Reservation?> UpdateAsync(Reservation entity, CancellationToken cancellationToken = default)
    {
        var existingReservation = await _dbContext.Reservations
            .FirstOrDefaultAsync(
                reservation => reservation.ReservationId == entity.ReservationId && reservation.IsDeleted != true,
                cancellationToken);

        if (existingReservation is null)
        {
            return null;
        }

        existingReservation.GuestName = entity.GuestName;
        existingReservation.GuestEmail = entity.GuestEmail;
        existingReservation.GuestPhone = entity.GuestPhone;
        existingReservation.CheckInDate = entity.CheckInDate;
        existingReservation.CheckOutDate = entity.CheckOutDate;
        existingReservation.RoomId = entity.RoomId;
        existingReservation.ReservationStatus = entity.ReservationStatus;
        existingReservation.UserId = entity.UserId;
        existingReservation.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return existingReservation;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var reservation = await _dbContext.Reservations
            .FirstOrDefaultAsync(
                reservation => reservation.ReservationId == id && reservation.IsDeleted != true,
                cancellationToken);

        if (reservation is null)
        {
            return false;
        }

        reservation.IsDeleted = true;
        reservation.UpdatedAt = DateTime.UtcNow;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reservations
            .AnyAsync(
                reservation => reservation.ReservationId == id && reservation.IsDeleted != true,
                cancellationToken);
    }

    public async Task<bool> HasOverlappingReservationAsync(
        int roomId,
        DateOnly checkInDate,
        DateOnly checkOutDate,
        int? excludedReservationId = null,
        CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reservations
            .AnyAsync(
                reservation =>
                    reservation.RoomId == roomId &&
                    reservation.IsDeleted != true &&
                    reservation.ReservationStatus != "Cancelled" &&
                    (!excludedReservationId.HasValue || reservation.ReservationId != excludedReservationId.Value) &&
                    reservation.CheckInDate < checkOutDate &&
                    checkInDate < reservation.CheckOutDate,
                cancellationToken);
    }

    public async Task<Reservation?> GetWithRoomTypeAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Reservations
            .Include(reservation => reservation.Room)
            .ThenInclude(room => room!.RoomType)
            .FirstOrDefaultAsync(
                reservation => reservation.ReservationId == reservationId && reservation.IsDeleted != true,
                cancellationToken);
    }
}
