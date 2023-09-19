using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Interface.Service
{
    public interface IMessageService
    {
        string Messages { get; set; }
        event PropertyChangedEventHandler PropertyChanged;
    }
}
