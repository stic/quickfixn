using System.Collections.Generic;
using DDTool.Structures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Structures;

[TestClass]
public class DDFieldTests {

    [TestMethod]
    public void UtcTimestampMapsToUtcDateTimeField() {
        var field = new DDField(52, "SendingTime", new List<EnumValue>(), "UTCTIMESTAMP");

        Assert.AreEqual("UtcDateTimeField", field.CsClass);
        Assert.AreEqual("DateTime", field.BaseType);
    }

    [TestMethod]
    public void TzTimestampMapsToDateTimeField() {
        var field = new DDField(1132, "TZTransactTime", new List<EnumValue>(), "TZTIMESTAMP");

        Assert.AreEqual("DateTimeField", field.CsClass);
        Assert.AreEqual("DateTime", field.BaseType);
    }

    [TestMethod]
    public void TimeMapsToDateTimeField() {
        var field = new DDField(273, "MDEntryTime", new List<EnumValue>(), "TIME");

        Assert.AreEqual("DateTimeField", field.CsClass);
        Assert.AreEqual("DateTime", field.BaseType);
    }
}
