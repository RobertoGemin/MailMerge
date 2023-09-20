using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.Validator;
using TestMailMerger.Helper;
using Library.Interface.Validator;
using Library.Validator.Processing;

namespace TestMailMerger
{
    [TestClass]
    public class XmlValidatorTests
    {
        public IValidator MailMerge = new XmlValidator();

        [TestInitialize]
        public void TestInitialize()
        {
            
        }
        [TestMethod]
        public void ValidateXml_ValidXmlFile_ReturnsTrue()
        {
            // Arrange
            string tempPath = Path.GetTempPath();
            var validXmlFile = Path.Combine(tempPath, "file1.xml");
            File.WriteAllText(validXmlFile, XmlCreator.CreateValidXmlDocument());


            // Act
            var result = MailMerge.Validate(validXmlFile);
            // Assert

            Assert.IsTrue(result);
            // Clean up
            File.Delete(validXmlFile);
        }  
        [TestMethod]
        public void ValidateXml_ValidXmlFileOther_ReturnsTrue()
        {
            // Arrange
            string tempPath = Path.GetTempPath();
            var validXmlFile = Path.Combine(tempPath, "file2.xml");
            File.WriteAllText(validXmlFile, XmlCreator.CreateValidXmlDocumentOther());


            // Act
            var result = MailMerge.Validate(validXmlFile);
            // Assert

            Assert.IsTrue(result);
            // Clean up
            File.Delete(validXmlFile);
        }

        [TestMethod]
        public void ValidateXml_ValidXmlFileMore_ReturnsTrue()
        {
            // Arrange
            string tempPath = Path.GetTempPath();
            var validXmlFile = Path.Combine(tempPath, "file4.xml");
            File.WriteAllText(validXmlFile, XmlCreator.CreateValidXmlTwoElements());


            // Act
            var result = MailMerge.Validate(validXmlFile);
            // Assert

            Assert.IsTrue(result);
            // Clean up
            File.Delete(validXmlFile);
        }
        [TestMethod]

        public void ValidateXml_InValidXmlFileSameKey_ReturnsFalse()
        {
            // Arrange
            string tempPath = Path.GetTempPath();
            var validXmlFile = Path.Combine(tempPath, "file5.xml");
            File.WriteAllText(validXmlFile, XmlCreator.CreateValidXmlDocumentDubbleKey());


            // Act
            var result = MailMerge.Validate(validXmlFile);
            // Assert

            Assert.IsFalse(result);
            // Clean up
            File.Delete(validXmlFile);
        }


        [TestMethod]
        public void ValidateXml_InValidXmlFile_ReturnsFalse()
        {
            // Arrange
            string tempPath = Path.GetTempPath();
            var validXmlFile = Path.Combine(tempPath, "file5.xml");
            File.WriteAllText(validXmlFile, XmlCreator.CreateInValidXmlDocument());

            // Act
            var result = MailMerge.Validate(validXmlFile);
            // Assert

            Assert.IsFalse(result);
            // Clean up
            File.Delete(validXmlFile);
        }

        [TestMethod]
        public void ValidateXml_InValidXmlFileEmptyValueInElement_ReturnsFalse()
        {
            // Arrange
            string tempPath = Path.GetTempPath();
            var validXmlFile = Path.Combine(tempPath, "file6.xml");
            File.WriteAllText(validXmlFile, XmlCreator.CreateInValidXmlDocumentEmptyValueInElement());

            // Act
            var result = MailMerge.Validate(validXmlFile);
            // Assert

            Assert.IsFalse(result);
            // Clean up
            File.Delete(validXmlFile);
        }
        [TestMethod]
        public void ValidateXml_InValidXmlFileInjection_ReturnsFalse()
        {
            // Arrange
            string tempPath = Path.GetTempPath();
            var validXmlFile = Path.Combine(tempPath, "file6.xml");
            File.WriteAllText(validXmlFile, XmlCreator.CreateInValidXmlInjection());

            // Act
            var result = MailMerge.Validate(validXmlFile);
            // Assert

            Assert.IsFalse(result);
            // Clean up
            File.Delete(validXmlFile);
        }
    }
}
