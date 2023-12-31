﻿
using TestMailMerger.Helper;
using Library.Interface.Validator;
using Library.Validator.Processing;

namespace TestMailMerger
{
    [TestClass]
    public class TemplateValidatorTests
    {
        public IValidator MailMerge = new TemplateValidator();


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

                var result = MailMerge.Validate(tempFile);
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

                var result = MailMerge.Validate(tempFile);
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
