using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Interface.Validator
{
    public interface IValidator
    {
        bool Validate(string path);
        string Message { get; }
    }
}
