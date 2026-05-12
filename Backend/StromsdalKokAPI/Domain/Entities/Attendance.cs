using StromsdalKok.Domain.Enums;

namespace StromsdalKok.Domain.Entities;

public class Attendance : BaseDomain
{
    public int UserId { get; private set; }
    public int TabletDeviceId { get; private set; }
    public DateTime ClockIn { get; private set; }
    public DateTime? ClockOut { get; private set; }
    public AttendanceMethod Method { get; private set; }
    public int? TotalMinutes { get; private set; }

    internal Attendance(int id, int userId, int tabletDeviceId,
     DateTime clockIn, DateTime? clockOut,
     AttendanceMethod method, int? totalMinutes)
    {
        Id = id;
        UserId = userId;
        TabletDeviceId = tabletDeviceId;
        ClockIn = clockIn;
        ClockOut = clockOut;
        Method = method;
        TotalMinutes = totalMinutes;
    }

    private Attendance(int userId, int tabletDeviceId, AttendanceMethod method)
    {
        UserId = userId;
        TabletDeviceId = tabletDeviceId;
        Method = method;
        ClockIn = DateTime.UtcNow;
    }

    public static Attendance ClockInNow(int userId, int tabletDeviceId, AttendanceMethod method)
    {
        return new Attendance(userId, tabletDeviceId, method);
    }

    public void ClockOutNow()
    {
        if (ClockOut.HasValue)
            throw new InvalidOperationException("Redan utstämplad.");

        ClockOut = DateTime.UtcNow;
        TotalMinutes = (int)(ClockOut.Value - ClockIn).TotalMinutes;
    }
}