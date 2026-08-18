using System;
using NUnit.Framework;
using QuickFix.Fields;

namespace UnitTests.Fields;

[TestFixture]
public class UtcDateTimeFieldTests
{
    [Test]
    public void DefaultCtorTest()
    {
        UtcDateTimeField f = new(Tags.SendingTime);
        Assert.That(f.Value.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void CtorWithUnspecifiedKindTest()
    {
        DateTime dt = new(2025, 10, 31, 17, 30, 59);
        UtcDateTimeField f = new(Tags.SendingTime, dt);

        Assert.That(f.Value.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(f.Value, Is.EqualTo(DateTime.SpecifyKind(dt, DateTimeKind.Utc)));
    }

    [Test]
    public void CtorWithLocalKindTest()
    {
        DateTime dt = DateTime.SpecifyKind(new DateTime(2025, 10, 31, 17, 30, 59), DateTimeKind.Local);
        UtcDateTimeField f = new(Tags.SendingTime, dt);

        Assert.That(f.Value.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(f.Value, Is.EqualTo(dt.ToUniversalTime()));
    }

    [Test]
    public void CtorWithUtcKindTest()
    {
        DateTime dt = DateTime.SpecifyKind(new DateTime(2025, 10, 31, 17, 30, 59), DateTimeKind.Utc);
        UtcDateTimeField f = new(Tags.SendingTime, dt);

        Assert.That(f.Value.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(f.Value, Is.EqualTo(dt));
    }

    [Test]
    public void ValueSetterForcesUtcTest()
    {
        UtcDateTimeField f = new(Tags.SendingTime);
        f.Value = new DateTime(2025, 10, 31, 17, 30, 59);

        Assert.That(f.Value.Kind, Is.EqualTo(DateTimeKind.Utc));
    }

    [Test]
    public void ToStringTest()
    {
        UtcDateTimeField f = new(Tags.SendingTime, new DateTime(2009, 9, 4, 3, 44, 1));
        Assert.That(f.ToString(), Is.EqualTo("20090904-03:44:01.000"));
        Assert.That(f.ToStringField(), Is.EqualTo($"{Tags.SendingTime}=20090904-03:44:01.000"));
    }

    [Test]
    public void LegacyCtorThatTakesShowMillisecondsTest()
    {
        DateTime dt = new(2025, 10, 31, 17, 30, 59);
#pragma warning disable CS0618
        UtcDateTimeField f = new(Tags.SendingTime, dt, false);
#pragma warning restore CS0618

        Assert.That(f.Value.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(f.ToString(), Is.EqualTo("20251031-17:30:59"));
    }

    [Test]
    public void CtorWithTimePrecisionTest()
    {
        DateTime dt = new(2025, 10, 31, 17, 30, 59);
        UtcDateTimeField f = new(Tags.SendingTime, dt, TimePrecision.Second);

        Assert.That(f.Value.Kind, Is.EqualTo(DateTimeKind.Utc));
        Assert.That(f.ToString(), Is.EqualTo("20251031-17:30:59"));
    }
}
