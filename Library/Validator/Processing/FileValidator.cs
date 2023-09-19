using Library.Interface.Validator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Validator.Methods
{
    public class FileValidator : IValidator
    {
        public string Message { get; private set; } = string.Empty;

        public bool Validate(string path)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(Path.GetExtension(path).ToLower()) ||
                !File.Exists(path))
            {
                Message = "The file path is invalid or the file does not exist, or the file extension is missing.";
                return false;
            }
            try
            {
                if (!(File.Exists(path) && new FileInfo(path).Length > 0))
                {
                    Message = "File does not exist or is empty.";
                    return false;
                }
                var fileSizeInMb = new FileInfo(path).Length / (1024 * 1024);
                if (fileSizeInMb >= 1)
                {
                    Message = "File size is greater than or equal to 1 MB.";
                    return false;
                }

                Message = "File is valid.";
                return true;
            }
            catch (IOException)
            {
                Message = "An I/O error occurred.";
                return false;
            }

        }
    }
}
