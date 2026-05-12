using StromsdalKok.Domain.Entities;
using StromsdalKok.Infrastructure.Data.Persistence;

namespace StromsdalKok.Infrastructure.Mappings;

public static class TimeLogMapper
{
    public static TimeLog ToDomain(TimeLogEntity entity)
    {
        return new TimeLog(
            entity.Id,
            entity.OrderId,
            entity.StationId,
            entity.UserId,
            entity.StartTime,
            entity.EndTime,
            entity.TotalMinutes,
            entity.Note
        );
    }

    public static TimeLogEntity ToEntity(TimeLog domain)
    {
        return new TimeLogEntity
        {
            Id = domain.Id,
            OrderId = domain.OrderId,
            StationId = domain.StationId,
            UserId = domain.UserId,
            StartTime = domain.StartTime,
            EndTime = domain.EndTime,
            TotalMinutes = domain.TotalMinutes,
            Note = domain.Note
        };
    }
}