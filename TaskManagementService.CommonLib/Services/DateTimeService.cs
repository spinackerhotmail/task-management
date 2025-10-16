namespace TaskManagementService.CommonLib.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.UtcNow;
}
