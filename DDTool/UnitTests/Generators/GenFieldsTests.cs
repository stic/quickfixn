using System.Collections.Generic;
using System.IO;
using DDTool.Generators;
using DDTool.Structures;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.Generators;

[TestClass]
public class GenFieldsTests {

    public TestContext TestContext { get; set; } = null!;

    [TestMethod]
    public void UtcTimestampFieldIsGeneratedWithUtcDateTimeFieldBaseClass() {
        string repoRoot = Path.Combine(TestContext.TestRunDirectory!, Path.GetRandomFileName());
        Directory.CreateDirectory(Path.Combine(repoRoot, "QuickFIXn", "Fields"));

        try {
            var fields = new List<DDField> {
                new(52, "SendingTime", new List<EnumValue>(), "UTCTIMESTAMP"),
                new(1132, "TZTransactTime", new List<EnumValue>(), "TZTIMESTAMP"),
            };

            string writtenPath = GenFields.WriteFile(repoRoot, fields);
            string generated = File.ReadAllText(writtenPath);

            StringAssert.Contains(generated, "public sealed class SendingTime : UtcDateTimeField");
            StringAssert.Contains(generated, "public sealed class TZTransactTime : DateTimeField");
        } finally {
            Directory.Delete(repoRoot, recursive: true);
        }
    }
}
