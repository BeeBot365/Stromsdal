
using StromsdalKok.Domain.Entities;
using StromsdalKok.Domain.Enums;

namespace Domain.Tests;

public class AttendanceTests
{
    [Fact]
    public void ClockInNow_SetsProperties()
    {
        var before = DateTime.UtcNow;

        var attendance = Attendance.ClockInNow(userId: 1, tabletDeviceId: 2, AttendanceMethod.Pin);

        var after = DateTime.UtcNow;

        Assert.Equal(1, attendance.UserId);
        Assert.Equal(2, attendance.TabletDeviceId);
        Assert.Equal(AttendanceMethod.Pin, attendance.Method);
        Assert.InRange(attendance.ClockIn, before, after);
        Assert.Null(attendance.ClockOut);
        Assert.Null(attendance.TotalMinutes);
    }

    [Fact]
    public void ClockOutNow_SetsClockOutAndTotalMinutes()
    {
        var attendance = Attendance.ClockInNow(userId: 1, tabletDeviceId: 2, AttendanceMethod.Face);

        var before = DateTime.UtcNow;
        attendance.ClockOutNow();
        var after = DateTime.UtcNow;

        Assert.NotNull(attendance.ClockOut);
        Assert.InRange(attendance.ClockOut!.Value, before, after);
        Assert.NotNull(attendance.TotalMinutes);
        Assert.True(attendance.TotalMinutes >= 0);
    }

    [Fact]
    public void ClockOutNow_TotalMinutesCalculatedCorrectly()
    {
        var attendance = Attendance.ClockInNow(userId: 1, tabletDeviceId: 2, AttendanceMethod.Pin);

        attendance.ClockOutNow();

        var expected = (int)(attendance.ClockOut!.Value - attendance.ClockIn).TotalMinutes;
        Assert.Equal(expected, attendance.TotalMinutes);
    }

    [Fact]
    public void ClockOutNow_WhenAlreadyClockedOut_ThrowsInvalidOperationException()
    {
        var attendance = Attendance.ClockInNow(userId: 1, tabletDeviceId: 2, AttendanceMethod.Face);
        attendance.ClockOutNow();

        var ex = Assert.Throws<InvalidOperationException>(() => attendance.ClockOutNow());
        Assert.Equal("Redan utstämplad.", ex.Message);
    }

    [Theory]
    [InlineData(AttendanceMethod.Face)]
    [InlineData(AttendanceMethod.Pin)]
    public void ClockInNow_AllMethods_SetsMethodCorrectly(AttendanceMethod method)
    {
        var attendance = Attendance.ClockInNow(userId: 1, tabletDeviceId: 1, method);

        Assert.Equal(method, attendance.Method);
    }

    [Fact]
    public void InternalConstructor_SetsAllProperties()
    {
        var clockIn = new DateTime(2025, 1, 1, 8, 0, 0, DateTimeKind.Utc);
        var clockOut = new DateTime(2025, 1, 1, 16, 30, 0, DateTimeKind.Utc);

        var attendance = new Attendance(
            id: 42,
            userId: 5,
            tabletDeviceId: 3,
            clockIn: clockIn,
            clockOut: clockOut,
            method: AttendanceMethod.Face,
            totalMinutes: 510);

        Assert.Equal(42, attendance.Id);
        Assert.Equal(5, attendance.UserId);
        Assert.Equal(3, attendance.TabletDeviceId);
        Assert.Equal(clockIn, attendance.ClockIn);
        Assert.Equal(clockOut, attendance.ClockOut);
        Assert.Equal(AttendanceMethod.Face, attendance.Method);
        Assert.Equal(510, attendance.TotalMinutes);
    }

    [Fact]
    public void InternalConstructor_WithNullClockOutAndTotalMinutes_SetsNulls()
    {
        var clockIn = new DateTime(2025, 6, 15, 9, 0, 0, DateTimeKind.Utc);

        var attendance = new Attendance(
            id: 1,
            userId: 2,
            tabletDeviceId: 3,
            clockIn: clockIn,
            clockOut: null,
            method: AttendanceMethod.Pin,
            totalMinutes: null);

        Assert.Null(attendance.ClockOut);
        Assert.Null(attendance.TotalMinutes);
    }
}
