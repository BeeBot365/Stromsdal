using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Enums;
using StromsdalKok.Infrastructure.Data.Persistence;

namespace StromsdalKok.Infrastructure.Mappings;

public static class AttendanceMapper
{
    public static Attendance ToDomain(AttendanceEntity entity)
    {
        return new Attendance(
            entity.Id,
            entity.UserId,
            entity.TabletDeviceId,
            entity.ClockIn,
            entity.ClockOut,
            entity.Method,
            entity.TotalMinutes
        );
    }

    public static AttendanceEntity ToEntity(Attendance domain)
    {
        return new AttendanceEntity
        {
            Id = domain.Id,
            UserId = domain.UserId,
            TabletDeviceId = domain.TabletDeviceId,
            ClockIn = domain.ClockIn,
            ClockOut = domain.ClockOut,
            Method = domain.Method,
            TotalMinutes = domain.TotalMinutes
        };
    }
}