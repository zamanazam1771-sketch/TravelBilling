using TravelBilling.Application.Abstractions;

namespace TravelBilling.Infrastructure.Time;

public sealed class SystemClock : IClock
{
    public DateTime UtcNow => DateTime.UtcNow;
}
