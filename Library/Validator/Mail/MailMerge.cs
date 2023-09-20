using Microsoft.VisualBasic.FileIO;
using System.Configuration;
using System.Text;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.ComponentModel;
using Library.Interface.Mail;

namespace Library.Validator.Mail
{
    public class MailMerge : IMailMerge
    {

        private string _messages = string.Empty;

        public string Messages
        {
            get => _messages;
            set
            {
                if (_messages != value)
                {
                    _messages = value;
                    OnPropertyChanged(nameof(Messages));
                }
            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        public void MergeMail(string templateFile, string xmlFile, string outputDir)
        {
            CreateOutputDirectory(outputDir);
            var xLoadXDocument = LoadXDocument(xmlFile);
            var orderNodes = GetOrderNodes(xLoadXDocument);
            ProcessOrderNodes(orderNodes, templateFile, outputDir);
        }

        public bool CreateOutputDirectory(string outputDir)
        {
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                    Messages = "An directory creating.";

                }

                return true;
            }
            catch
            {
                Messages = "An error occurred creating the output directory.";
            }


            return false;
        }


        public XDocument LoadXDocument(string xmlFile)
        {
            var xDocument = XDocument.Load(xmlFile);
            return xDocument;
        }

        public IEnumerable<XElement> GetOrderNodes(XDocument xDocument)
        {
            return xDocument.Descendants("Order");
        }

        public void ProcessOrderNodes(IEnumerable<XElement> orderNodes, string templateFile, string outputDir)
        {
            foreach (var orderNode in orderNodes)
            {
                string orderId = (string)orderNode.Attribute("id");
                if (!string.IsNullOrEmpty(orderId))
                {
                    var outputContent = ProcessOrderNode(orderNode, templateFile);
                    WriteOutputFile(outputDir, orderId, outputContent);
                }
            }
        }

        public string ProcessOrderNode(XElement orderNode, string templateFile)
        {
            var templateContent = File.ReadAllText(templateFile);
            var outputContent = templateContent;
            foreach (var childNode in orderNode.Elements())
            {
                var keyword = "%" + childNode.Name + "%";
                var replacement = (string)childNode;
                outputContent = outputContent.Replace(keyword, replacement);
            }

            while (outputContent.Contains('%'))
            {
                var start = outputContent.IndexOf('%');
                var end = outputContent.IndexOf('%', start + 1);
                if (end == -1) break;
                var keyword = outputContent.Substring(start, end - start + 1);
                var key = keyword.Trim('%');
                var replacement = (string)orderNode.Element(key) ?? string.Empty;
                outputContent = outputContent.Replace(keyword, replacement);
            }

            return outputContent;
        }

        public void WriteOutputFile(string outputDir, string orderId, string content)
        {
            var outputFile = Path.Combine(outputDir, orderId + ".txt");
            try
            {
                File.WriteAllText(outputFile, content);
                Messages = $"File with ID {orderId} has been successfully written to {outputFile}.";
            }
            catch (Exception e)
            {
                Messages = $"An error occurred while writing the file: {e.Message}";
            }
        }
    }
}
