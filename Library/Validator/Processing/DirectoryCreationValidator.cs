using Library.Interface.Validator;

namespace Library.Validator.Processing
{
    public class DirectoryCreationValidator : IValidator
    {

        public string Message { get; private set; } = string.Empty;

        public bool Validate(string outputDir)
        {

            if (outputDir.Length > 260)
            {
                Message = "Output directory path length exceeds the limit of 260 characters.";
                return false;
            }

            try
            {
                if (Directory.Exists(outputDir))
                {
                    Message = $"Output directory already exists.";
                }
                else
                {
                    var tempFile = Path.Combine(outputDir, Path.GetRandomFileName());

                    try
                    {
                        Directory.CreateDirectory(outputDir);
                        File.Create(tempFile).Dispose();
                    }
                    finally
                    {
                        if (File.Exists(tempFile))
                        {
                            File.Delete(tempFile);
                        }

                        if (Directory.Exists(outputDir))
                        {
                            Directory.Delete(outputDir);
                        }
                    }

                    Message = $"It's possible to create the output directory.";

                }
                return true;
            }
            catch
            {
                Message = "An error occurred while validating the output directory.";
            }

            return false;
        }

    }
}
