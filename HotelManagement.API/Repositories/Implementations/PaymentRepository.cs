using HotelManagement.API.Data;
using HotelManagement.API.Models;
using HotelManagement.API.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HotelManagement.API.Repositories.Implementations;

public class PaymentRepository : IPaymentRepository
{
    private readonly HotelDbContext _dbContext;

    public PaymentRepository(HotelDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<Payment>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _dbContext.Payments
            .AsNoTracking()
            .Include(payment => payment.Reservation)
            .OrderByDescending(payment => payment.PaymentDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<Payment?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Payments
            .Include(payment => payment.Reservation)
            .FirstOrDefaultAsync(payment => payment.PaymentId == id, cancellationToken);
    }

    public async Task<Payment> AddAsync(Payment entity, CancellationToken cancellationToken = default)
    {
        entity.PaymentDate ??= DateOnly.FromDateTime(DateTime.UtcNow);
        entity.PaymentStatus ??= "Pending";

        await _dbContext.Payments.AddAsync(entity, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return entity;
    }

    public async Task<Payment?> UpdateAsync(Payment entity, CancellationToken cancellationToken = default)
    {
        var existingPayment = await _dbContext.Payments
            .FirstOrDefaultAsync(payment => payment.PaymentId == entity.PaymentId, cancellationToken);

        if (existingPayment is null)
        {
            return null;
        }

        existingPayment.ReservationId = entity.ReservationId;
        existingPayment.Amount = entity.Amount;
        existingPayment.PaymentDate = entity.PaymentDate;
        existingPayment.PaymentStatus = entity.PaymentStatus;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return existingPayment;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var payment = await _dbContext.Payments
            .FirstOrDefaultAsync(payment => payment.PaymentId == id, cancellationToken);

        if (payment is null)
        {
            return false;
        }

        _dbContext.Payments.Remove(payment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return true;
    }

    public async Task<bool> ExistsAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Payments
            .AnyAsync(payment => payment.PaymentId == id, cancellationToken);
    }

    public async Task<IReadOnlyList<Payment>> GetByReservationIdAsync(int reservationId, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Payments
            .AsNoTracking()
            .Where(payment => payment.ReservationId == reservationId)
            .OrderByDescending(payment => payment.PaymentDate)
            .ToListAsync(cancellationToken);
    }
}
