using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMailMerger
{
    [TestClass]
    public class WriteOutputFileTest
    {
        [TestMethod]
        public void TestWriteOutputFile()
        {
            // Arrange
            var mailMerge = new MailMerge();
            var outputDir = Path.GetTempPath();
            var orderId = "1";
            string content = "Dear Jane\nThank you for ordering Hamburgers from\nour website. We will deliver it to\n1512 Crescent Ave, Hampton. The price is $5.00 with a\n5% discount.";
            // Act
            mailMerge.WriteOutputFile(outputDir, orderId, content);
            // Assert
            var outputFile = Path.Combine(outputDir, orderId + ".txt");
            Assert.IsTrue(File.Exists(outputFile), "Output file was not created.");
            var outputContent = File.ReadAllText(outputFile);
            Assert.AreEqual(content, outputContent, "Output file content does not match expected content.");
        }
    }
}
