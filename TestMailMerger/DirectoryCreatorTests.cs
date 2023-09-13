using Library;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using TestMailMerger.Helper;


namespace TestMailMerger;


[TestClass]
public class DirectoryCreatorTests
{


    public MailMerge MailMerge = new();
    [TestInitialize]
    public void TestInitialize()
    {
        MailMerge = new MailMerge();
    }
    [TestCleanup]
    public void TestCleanup()
    {
      

    }

    [TestMethod]
    public void ValidateOutputDirectory_CreateRandomDirectory_ReturnsTrue()
    {
        // Arrange
        string randomFileNameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
        string outputDir = Path.Combine(Path.GetTempPath(), randomFileNameWithoutExtension);
        // Act
        var result= MailMerge.CreateOutputDirectory(outputDir);
        // Assert
        Assert.IsTrue(result);
        Assert.IsTrue(Directory.Exists(outputDir));
        // Clean up
        Directory.Delete(outputDir);
    }
    [TestMethod]
    public void ValidateOutputDirectory_Empty_ReturnsFalse()
    {
        // Act
        var result = MailMerge.CreateOutputDirectory(string.Empty);
        // Assert
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void ValidateOutputDirectory_PathLong_ReturnsFalse()
    {
        // Act
        var result = MailMerge.CreateOutputDirectory(new string('a', 261));
        // Assert
        Assert.IsFalse(result);
    }



}