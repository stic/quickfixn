using System;

namespace QuickFix.Fields;

/// <summary>
/// Base class for UTCTIMESTAMP fields (e.g. SendingTime, TransactTime). Per the FIX spec, UTCTIMESTAMP
/// values are always UTC; this class normalizes <see cref="Value"/> to <see cref="DateTimeKind.Utc"/>
/// on every assignment instead of leaving it <see cref="DateTimeKind.Unspecified"/>.
/// </summary>
/// <remarks>
/// <see cref="Value"/> is hidden (not overridden) for performance, so normalization only applies when
/// accessed through a variable/parameter statically typed as <see cref="UtcDateTimeField"/> or a
/// subclass; code that upcasts to <see cref="DateTimeField"/> before assigning bypasses it.
/// </remarks>
public class UtcDateTimeField : DateTimeField
{
    public UtcDateTimeField(int tag)
        : base(tag, DateTime.SpecifyKind(new DateTime(), DateTimeKind.Utc)) {}

    public UtcDateTimeField(int tag, DateTime dt)
        : base(tag, ToUtc(dt)) {}

    [Obsolete("Use the ctor that takes TimePrecision instead.  This ctor will be removed in 1.15.")]
    public UtcDateTimeField(int tag, DateTime dt, bool showMilliseconds)
        : base(tag, ToUtc(dt), showMilliseconds) {}

    public UtcDateTimeField(int tag, DateTime dt, TimePrecision timeFormatPrecision)
        : base(tag, ToUtc(dt), timeFormatPrecision) {}

    public new DateTime Value
    {
        get => base.Value;
        set => base.Value = ToUtc(value);
    }

    private static DateTime ToUtc(DateTime dt) => dt.Kind switch
    {
        DateTimeKind.Utc => dt,
        DateTimeKind.Local => dt.ToUniversalTime(),
        DateTimeKind.Unspecified => DateTime.SpecifyKind(dt, DateTimeKind.Utc), // Unspecified: FIX spec guarantees UTCTIMESTAMP is already UTC
        _ => throw new ArgumentOutOfRangeException(nameof(dt), "Invalid DateTimeKind")
    };
}
