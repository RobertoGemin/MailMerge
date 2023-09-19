using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Library.Validator.Mail;
using TestMailMerger.Helper;

namespace TestMailMerger
{
    [TestClass]
    public class ProcessOrderNodeTest
    {
        [TestMethod]
        public void TestProcessOrderNode_WhenSuccessful_ReturnsTrue()
        {
            // Arrange
            var mailMerge = new MailMerge();
            var orderNode = new XElement("Order",
                new XElement("firstname", "Jane"),
                new XElement("lastname", "Doe"),
                new XElement("salutation", "Dear %firstname%"),
                new XElement("product", "Hamburgers"),
                new XElement("price", "$5.00"),
                new XElement("discount", "5"),
                new XElement("address", "1512 Crescent Ave, Hampton"));
            var templateFile = Path.Combine(Path.GetTempPath(), "templateFile.txt");
            File.WriteAllText(templateFile, TemplateCreator.CreateMessage());
            string content = "Dear Jane\nThank you for ordering Hamburgers from\nour website. We will deliver it to\n1512 Crescent Ave, Hampton. The price is $5.00 with a\n5% discount.";

            try
            {
                // Act
                var outputContent = mailMerge.ProcessOrderNode(orderNode, templateFile);
                // Assert
                Assert.IsTrue(outputContent.Contains(content), "The output content does not contain the expected text.");
            }
            catch (Exception ex)
            {
                Assert.Fail($" {ex.Message}");
            }
            finally
            {
                File.Delete(templateFile);
            }
        }
    }
}
