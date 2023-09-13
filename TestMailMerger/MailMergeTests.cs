using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using TestMailMerger.Helper;

namespace TestMailMerger
{
    [TestClass]
    public class MailMergeTests
    {
        [TestMethod]
        public void ValidateMergeMail_WhenSuccessful_ReturnsTrue()
        {
            // Arrange
             var templateFile = Path.Combine(Path.GetTempPath(), "templateFile.txt");
             File.WriteAllText(templateFile, TemplateCreator.CreateMessage());
            var xmlFile = Path.Combine(Path.GetTempPath(), "xmlFile.xml");
            File.WriteAllText(xmlFile, XmlCreator.CreateValidXmlDocument());
            string randomFileNameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            string outputDir = Path.Combine(Path.GetTempPath(), randomFileNameWithoutExtension);
            MailMerge mailMerge = new MailMerge();

            try
            {
                // Act
                mailMerge.MergeMail(templateFile, xmlFile, outputDir);


                XDocument xDocument = mailMerge.LoadXDocument(xmlFile);
                IEnumerable<XElement> orderNodes = mailMerge.GetOrderNodes(xDocument);
                IEnumerable<string> orderIds = orderNodes
                    .Select(o => (string)o.Attribute("id"));

                foreach (var id in orderIds)
                {
                    string fileName = id + ".txt";
                    string fullPath = Path.Combine(outputDir, fileName);
                    bool fileExists = File.Exists(fullPath);

                    Assert.IsTrue(fileExists, $"Output file for order {id} does not exist.");
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
                File.Delete(xmlFile);
                Directory.GetFiles(outputDir)
                    .ToList()
                    .ForEach(File.Delete);
                Directory.Delete(outputDir);
            }
        }
        [TestMethod]
        public void ValidateMergeMail_WhenSuccessfulTwo_ReturnsTrue()
        {
            // Arrange
            var templateFile = Path.Combine(Path.GetTempPath(), "templateFile.txt");
            File.WriteAllText(templateFile, TemplateCreator.CreateMessage());
            var xmlFile = Path.Combine(Path.GetTempPath(), "xmlFile.xml");
            File.WriteAllText(xmlFile, XmlCreator.CreateValidXmlTwoElements());
            string randomFileNameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
            string outputDir = Path.Combine(Path.GetTempPath(), randomFileNameWithoutExtension);
            MailMerge mailMerge = new MailMerge();

            try
            {
                // Act
                mailMerge.MergeMail(templateFile, xmlFile, outputDir);


                XDocument xDocument = mailMerge.LoadXDocument(xmlFile);
                IEnumerable<XElement> orderNodes = mailMerge.GetOrderNodes(xDocument);
                IEnumerable<string> orderIds = orderNodes
                    .Select(o => (string)o.Attribute("id"));

                foreach (var id in orderIds)
                {
                    string fileName = id + ".txt";
                    string fullPath = Path.Combine(outputDir, fileName);
                    bool fileExists = File.Exists(fullPath);
                    Assert.IsTrue(fileExists);
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
                File.Delete(xmlFile);
                Directory.GetFiles(outputDir)
                    .ToList()
                    .ForEach(File.Delete);
                Directory.Delete(outputDir);
            }
        }
    }
}
