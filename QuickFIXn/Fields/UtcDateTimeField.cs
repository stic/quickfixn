using System;

namespace QuickFix.Fields;

/// <summary>
/// Base class for UTCTIMESTAMP fields (e.g. SendingTime, TransactTime). Per the FIX spec, UTCTIMESTAMP
/// values are always UTC; this class normalizes <see cref="Value"/> to <see cref="DateTimeKind.Utc"/>
/// on every assignment instead of leaving it <see cref="DateTimeKind.Unspecified"/>.
/// </summary>
/// <remarks>
/// Note that <see cref="QuickFix.FieldMap.GetDateTime(int)"/> still returns
/// <see cref="DateTimeKind.Unspecified"/> for a wire-parsed value, because a bare tag lookup has no
/// DataDictionary context to know the field is a UTCTIMESTAMP. Use the typed <c>GetField</c> overloads
/// or <see cref="QuickFix.FieldMap.GetUtcDateTime(int)"/> to obtain a normalized value.
/// </remarks>
public class UtcDateTimeField : DateTimeField
{
    public UtcDateTimeField(int tag)
        : base(tag, DateTime.SpecifyKind(default, DateTimeKind.Utc)) {}

    public UtcDateTimeField(int tag, DateTime dt)
        : base(tag, ToUtc(dt)) {}

    // Not [Obsolete] here, matching DateTimeField; the deprecation is applied to the generated field classes.
    public UtcDateTimeField(int tag, DateTime dt, bool showMilliseconds)
        : base(tag, ToUtc(dt), showMilliseconds) {}

    public UtcDateTimeField(int tag, DateTime dt, TimePrecision timeFormatPrecision)
        : base(tag, ToUtc(dt), timeFormatPrecision) {}

    // Deliberately an override, not `new`-hiding, even though FieldBase<T>.Value is on a hot path.
    // Hiding binds normalization at compile time, so assigning through a DateTimeField-typed reference
    // would silently emit un-normalized (e.g. local) time into a UTCTIMESTAMP field -- a wire-correctness
    // bug. The virtual cost is negligible: the generated field classes (SendingTime, TransactTime, ...)
    // are sealed so the JIT devirtualizes access through them, and any remaining call sits next to
    // DateTime formatting/parsing that costs orders of magnitude more.
    public override DateTime Value
    {
        get => base.Value;
        set => base.Value = ToUtc(value);
    }

    private static DateTime ToUtc(DateTime dt) => dt.Kind switch
    {
        DateTimeKind.Utc => dt,
        DateTimeKind.Local => dt.ToUniversalTime(),
        // Unspecified: per FIX spec a UTCTIMESTAMP is already UTC, so relabel without shifting.
        // Used as the discard arm because DateTime.Kind cannot return an undeclared value
        // (see UtcDateTimeFieldTests.DateTimeKindHasNoUnhandledMembers).
        _ => DateTime.SpecifyKind(dt, DateTimeKind.Utc)
    };
}
