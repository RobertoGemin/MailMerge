using Library;
using Library.Validator.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using TestMailMerger.Helper;

namespace TestMailMerger
{
    [TestClass]
    public class ProcessOrderNodesTest
    {

        [TestMethod]
        public void ValidateProcessOrderNodes_WhenSuccessful_ReturnsTrue()
        {
            // Arrange
            MailMerge mailMerge = new();
            var orderNodes = new List<XElement>
            {
                new XElement("Order", new XAttribute("id", "1")),
                new XElement("Order", new XAttribute("id", "2")),
                new XElement("Order", new XAttribute("id", "3")),
            };


            var templateFile = Path.Combine(Path.GetTempPath(), "templateFile.txt");
            File.WriteAllText(templateFile, TemplateCreator.CreateMessage());

            string randomFileNameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            string outputDir = Path.Combine(Path.GetTempPath(), randomFileNameWithoutExtension);
            try
            {
                // Act
                mailMerge.CreateOutputDirectory(outputDir);
                mailMerge.ProcessOrderNodes(orderNodes, templateFile, outputDir);
                // Assert
                foreach (var orderNode in orderNodes)
                {
                    var orderId = (string)orderNode.Attribute("id");
                    var outputFile = Path.Combine(outputDir, orderId + ".txt");
                    bool fileExists = File.Exists(outputFile);
                    Assert.IsTrue(fileExists, $"Output file for order {orderId} does not exist.");
                }
            }
            catch (Exception ex)
            {
                Assert.Fail($" {ex.Message}");
            }
            finally
            {
                // Clean up
            File.Delete(templateFile);
            foreach (var file in Directory.EnumerateFiles(outputDir))
            {
                File.Delete(file);
            }
            Directory.Delete(outputDir);
            }
        }


    }
}
