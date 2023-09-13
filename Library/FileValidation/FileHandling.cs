using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.FileValidation
{
    public static class FileHandling
    { 
        public static List<string> ValidExtensions { get; } = new List<string> { ".xml", ".txt" };
    }
}
