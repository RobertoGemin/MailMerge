using System.Xml.Linq;
using Library;
using Library.Interface.Validator;
using Library.Validator.Mail;
using Library.Validator;
using Library.Validator.Methods;
using Microsoft.VisualBasic.FileIO;
using TestMailMerger.Helper;

namespace TestMailMerger
{
    [TestClass]
    public class FileValidatorTests
    {
        public string TempFile = string.Empty;
        public IValidator FileValidator = new FileValidator();

        [TestInitialize]
        public void TestInitialize()
        {
            var tempPath = Path.GetTempPath();
            TempFile = Path.Combine(tempPath, "test.txt");
            File.WriteAllText(TempFile, TemplateCreator.CreateMessage());
        }

        [TestCleanup]
        public void TestCleanup()
        {
            File.Delete(TempFile);
            TempFile = string.Empty;
        }

        [TestMethod]
        public void ValidateFile_WhenFileExists_ReturnsTrue()
        {
            // Arrange
            var fileName = TempFile;
            // Act
            var result = FileValidator.Validate(fileName);
            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void ValidateFile_WhenFileDoesNotExist_ReturnsFalse()
        {
            // Arrange
            File.Delete(TempFile);
            var fileName = TempFile;
            // Act
            var result = FileValidator.Validate(fileName);
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateFile_WhenFileIsLocked_ReturnsFalse()
        {
            // Arrange
            var tempPath = Path.GetTempPath();
            var lockedFile = Path.Combine(tempPath, "lockedFile.txt");
            try
            {
                using (var fs = new FileStream(lockedFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None))
                {
                    // Act
                    var result = FileValidator.Validate(lockedFile);
                    Assert.IsFalse(result);
                }

                // Cleanup
                File.Delete(lockedFile);
            }
            catch (Exception ex)
            {
                Assert.Fail($" {ex.Message}");
            }
            finally
            {
                // Clean up
                File.Delete(lockedFile);
            }
        }

        [TestMethod]
        public void ValidateFile_WhenFileIsEmpty_ReturnsFalse()
        {
            // Arrange
            var fileName = TempFile;
            // Act
            File.WriteAllText(TempFile, null);
            var result = FileValidator.Validate(fileName);
            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void ValidateFile_WhenFileHasNoName_ReturnsFalse()
        {
            // Arrange
            var fileName = string.Empty;
            // Act
            var result = FileValidator.Validate(fileName);
            // Assert
            Assert.IsFalse(result);
        }


        [TestMethod]
        [DataRow("file1.xml")]
        [DataRow("file1.Xml")]
        [DataRow("file_1.XMl")]
        [DataRow("file 1.XML")]
        public void ValidateFile_WhenFileValidExtensionsXML_ReturnsTrue(string fileName)
        {
            // Arrange
            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, fileName);
            try
            {
                // Act
                File.WriteAllText(tempFile, XmlCreator.CreateValidXmlDocument());
                var result = FileValidator.Validate(tempFile);
                // Assert
                Assert.IsTrue(result);
            }
            catch (Exception ex)
            {
                Assert.Fail($" {ex.Message}");
            }
            finally
            {
                // Clean up
                File.Delete(tempFile);
            }
        }

        [TestMethod]
        [DataRow("file1.txt")]
        [DataRow("file1.Txt")]
        [DataRow("file_1.TXt")]
        [DataRow("file 1.TXT")]
        public void ValidateFile_WhenFileValidExtensionsTXT_ReturnsTrue(string fileName)
        {
            // Arrange
            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, fileName);
            try
            {
                // Act
                File.WriteAllText(tempFile, TemplateCreator.CreateMessage());
                var result = FileValidator.Validate(tempFile);
                // Assert
                Assert.IsTrue(result);
            }
            catch (Exception ex)
            {
                Assert.Fail($" {ex.Message}");
            }
            finally
            {
                // Clean up
                File.Delete(tempFile);
            }
        }

        [TestMethod]
        public void ValidateFile_WhenFileNoExtensions_ReturnsFalse()
        {
            // Arrange
            var fileName = "file";
            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, fileName);
            try
            {
                // Act
                File.WriteAllText(tempFile, TemplateCreator.CreateMessage());
                var result = FileValidator.Validate(tempFile);
                // Assert
                Assert.IsFalse(result);
            }
            catch (Exception ex)
            {
                Assert.Fail($" {ex.Message}");
            }
            finally
            {
                // Clean up
                File.Delete(tempFile);
            }
        }

        [TestMethod]
        public void ValidateFile_WhenFileIsLargerThan1Mb_ReturnsFalse()
        {
            // Arrange
            var fileName = "file";
            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, fileName);
            try
            {
                // Act
                long fileSizeInBytes = 5 * 1024 * 1024;
                var data = new byte[fileSizeInBytes];
                File.WriteAllBytes(tempFile, data);
                var result = FileValidator.Validate(tempFile);
                // Assert
                Assert.IsFalse(result);
            }
            catch (Exception ex)
            {
                Assert.Fail($" {ex.Message}");
            }
            finally
            {
                // Clean up
                File.Delete(tempFile);
            }
        }
    }
}