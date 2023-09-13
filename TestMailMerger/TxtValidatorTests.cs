using Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestMailMerger.Helper;

namespace TestMailMerger
{
    [TestClass]
    public class TxtValidatorTests
    {
        public MailMerge MailMerge = new();


        [TestMethod]
        public void ValidTxt_WhenAnyPlaceholdersIsPresent_ReturnsTrue()
        {
            // Arrange
            var fileName = "file.txt";
            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, fileName);
            try
            {
                // Act
                File.WriteAllText(tempFile, TemplateCreator.CreateMessage());

                var result = MailMerge.ValidateTemplateFile(tempFile);
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
        } [TestMethod]
        public void ValidTxt_WhenNonPlaceholdersIsPresent_ReturnsFalse()
        {
            // Arrange
            var fileName = "file.txt";
            var tempPath = Path.GetTempPath();
            var tempFile = Path.Combine(tempPath, fileName);
            try
            {
                // Act
                File.WriteAllText(tempFile, TemplateCreator.CreateInvalidMessage());

                var result = MailMerge.ValidateTemplateFile(tempFile);
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
