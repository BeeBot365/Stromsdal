namespace StromsdalKok.Domain.Entities;

public class TimeLog
{
    public int Id { get; private set; }
    public int OrderId { get; private set; }
    public int StationId { get; private set; }
    public int UserId { get; private set; }
    public DateTime StartTime { get; private set; }
    public DateTime? EndTime { get; private set; }
    public int? TotalMinutes { get; private set; }
    public string? Note { get; private set; }

    internal TimeLog(int id, int orderId, int stationId, int userId,
        DateTime startTime, DateTime? endTime, int? totalMinutes, string? note)
    {
        Id = id;
        OrderId = orderId;
        StationId = stationId;
        UserId = userId;
        StartTime = startTime;
        EndTime = endTime;
        TotalMinutes = totalMinutes;
        Note = note;
    }

    private TimeLog(int orderId, int stationId, int userId, string? note)
    {
        OrderId = orderId;
        StationId = stationId;
        UserId = userId;
        StartTime = DateTime.UtcNow;
        Note = note;
    }

    public static TimeLog Start(int orderId, int stationId, int userId, string? note = null)
    {
        return new TimeLog(orderId, stationId, userId, note);
    }

    public void Stop(string? note = null)
    {
        if (EndTime.HasValue)
            throw new InvalidOperationException("Tidloggning är redan avslutad.");

        EndTime = DateTime.UtcNow;
        TotalMinutes = (int)(EndTime.Value - StartTime).TotalMinutes;
        if (note is not null) Note = note;
    }
}