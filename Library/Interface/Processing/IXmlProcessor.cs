using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Library.Interface.Processing
{
    public interface IXmlProcessor
    {
        XDocument LoadXDocument(string xmlFile);
        IEnumerable<XElement> GetOrderNodes(XDocument xDocument);
        void ProcessOrderNodes(IEnumerable<XElement> orderNodes, string templateFile, string outputDir);
        string ProcessOrderNode(XElement orderNode, string templateFile);
    }
}
