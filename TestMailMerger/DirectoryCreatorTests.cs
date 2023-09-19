using Library;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using Library.Validator;
using Library.Validator.Methods;
using TestMailMerger.Helper;
using Library.Interface.Validator;

namespace TestMailMerger;


[TestClass]
public class DirectoryCreatorTests
{


    public IValidator DirectoryCreationValidator = new DirectoryCreationValidator();
    [TestInitialize]
    public void TestInitialize()
    {
    }
    [TestCleanup]
    public void TestCleanup()
    {
      

    }

    [TestMethod]
    public void ValidateOutputDirectory_CheckRandomDirectory_ReturnsTrue()
    {
        // Arrange
        string randomFileNameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        string outputDir = Path.Combine(Path.GetTempPath(), randomFileNameWithoutExtension);
        // Act
        var result= DirectoryCreationValidator.Validate(outputDir);
        // Assert
        Assert.IsTrue(result);
        // Clean up
    }
    [TestMethod]
    public void ValidateOutputDirectory_Empty_ReturnsFalse()
    {
        // Act
        var result = DirectoryCreationValidator.Validate(string.Empty);
        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidateOutputDirectory_PathLong_ReturnsFalse()
    {
        // Act
        var result = DirectoryCreationValidator.Validate(new string('a', 261));
        // Assert
        Assert.IsFalse(result);
    }



}