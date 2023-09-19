using Library.Interface.Processing;
using Library.Interface.Service;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.Interface.Mail
{
    public interface IMailMerge : IMessageService, ICreateOutputDirectory, IOutputFileWriter, IXmlProcessor
    {
        void MergeMail(string templateFile, string xmlFile, string outputDir);
    }
}
