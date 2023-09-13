using Microsoft.VisualBasic.FileIO;
using System.Configuration;
using System.Text;
using System.Xml;
using Library.FileValidation;
using System.IO;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.ComponentModel;

namespace Library
{
    public class MailMerge : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void MergeMail(string templateFile, string xmlFile, string outputDir)
        {
            if (!(ValidateFile(templateFile) &&
                  ValidateFile(xmlFile) &&
                  CreateOutputDirectory(outputDir) &&
                  ValidateTemplateFile(templateFile) &&
                  ValidateXml(xmlFile)))
                return;

            var xLoadXDocument = LoadXDocument(xmlFile);
            var orderNodes = GetOrderNodes(xLoadXDocument);
            ProcessOrderNodes(orderNodes, templateFile, outputDir);
        }

        public bool ValidateFile(string path)
        {
            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                Messages = "File path is null, empty or file does not exist.";
                return false;
            }
            var extension = Path.GetExtension(path).ToLower();
            if (!FileHandling.ValidExtensions.Contains(extension))
            {
                Messages = "File extension is not valid.";
                return false;
            }
            try
            {
                if (!(File.Exists(path) && new FileInfo(path).Length > 0))
                {
                    Messages = "File does not exist or is empty.";
                    return false;
                }
                var fileSizeInMb = new FileInfo(path).Length / (1024 * 1024);
                if (fileSizeInMb >= 1)
                {
                    Messages = "File size is greater than or equal to 1 MB.";
                    return false;
                }
                Messages = "File is valid.";
                return true;
            }
            catch (IOException)
            {
                Messages = "An I/O error occurred.";
                return false;
            }


            return false;
        }

        public string GetXsdFile()
        {
            var xsdFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Orders.xsd");

            if (!File.Exists(xsdFile))
            {
                var xmlSchema = @"
                  <xs:schema xmlns:xs='http://www.w3.org/2001/XMLSchema'>
                    <xs:element name='Orders'>
                      <xs:complexType>
                        <xs:sequence>
                          <xs:element name='Order' maxOccurs='unbounded'>
                            <xs:complexType>
                              <xs:sequence>
                                <xs:element name='firstname' type='xs:string'/>
                                <xs:element name='lastname' type='xs:string'/>
                                <xs:element name='title' type='xs:string' minOccurs='0'/>
                                <xs:element name='salutation' type='xs:string'/>
                                <xs:element name='product' type='xs:string'/>
                                <xs:element name='price' type='xs:string'/>
                                <xs:element name='discount' type='xs:string'/>
                                <xs:element name='address' type='xs:string'/>
                              </xs:sequence>
                              <xs:attribute name='id' type='xs:string' use='required'/>
                            </xs:complexType>
                          </xs:element>
                        </xs:sequence>
                      </xs:complexType>
                <xs:unique name='UniqueOrderId'>
                    <xs:selector xpath='Order'/>
                    <xs:field xpath='@id'/>
                  </xs:unique>
                    </xs:element>
                  </xs:schema>";

                File.WriteAllText(xsdFile, xmlSchema);
            }

            return xsdFile;
        }

        public bool ValidateTemplateFile(string templateFile)
        {
            try
            {
                var xmlSchemaDoc = XDocument.Load(GetXsdFile());
                var elementNames = xmlSchemaDoc.Descendants()
                    .Where(x => x.Name.LocalName == "element" && x.Attribute("name") != null)
                    .Select(x => x.Attribute("name").Value)
                    .ToList();
                var placeholders = elementNames.Select(name => $"%{name}%").ToArray();
                var templateContent = File.ReadAllText(templateFile);
                if (!placeholders.Any(placeholder => templateContent.Contains(placeholder)))
                {
                    Messages = "Template does not contain required placeholders.";
                    return false;
                }
                Messages = "Template is valid.";
                return true;
            }
            catch (Exception ex)
            {
                Messages = $"An error occurred: {ex.Message}";
                return false;
            }
        }
    

        public bool ValidateXml(string xmlFile)
        {
            try
            {
                var settings = new XmlReaderSettings();
                settings.Schemas.Add(null, GetXsdFile());
                settings.ValidationType = ValidationType.Schema;
                var validationErrors = new StringBuilder();
                settings.ValidationEventHandler += (sender, e) =>
                {
                    validationErrors.AppendLine($"Validation Error: {e.Message}");
                };
                using (var reader = XmlReader.Create(xmlFile, settings))
                {
                    var doc = XDocument.Load(reader);

                    var emptyElements = doc.Descendants()
                        .Where(x => string.IsNullOrWhiteSpace(x.Value))
                        .ToList();
                    if (emptyElements.Any())
                        foreach (var element in emptyElements)
                            validationErrors.AppendLine($"Element '{element.Name}' does not have a value.");
                }

                Messages = validationErrors.ToString();
                return validationErrors.Length == 0;
            }
            catch (Exception ex)
            {
                Messages = $"XML validation error: {ex.Message}";
                return false;
            }
        }

        public bool CreateOutputDirectory(string outputDir)
        {
            if (outputDir.Length > 260)
            {
                Messages = "Output directory path length exceeds the limit of 260 characters.";
                return false;
            }

            try
            {
                if (Directory.Exists(outputDir))
                {
                    Messages = $"Output directory already exists.";
                }
                else
                {
                    Directory.CreateDirectory(outputDir);
                    Messages = $"Output directory was successfully created.";
                }
                return true;
            }
            catch
            {
                Messages = "An error occurred while validating the output directory.";
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
                var orderId = (string)orderNode.Attribute("id");
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
            // Replace child node keywords
            foreach (var childNode in orderNode.Elements())
            {
                var keyword = "%" + childNode.Name + "%";
                var replacement = (string)childNode;
                outputContent = outputContent.Replace(keyword, replacement);
            }

            // Replace recursive keywords
            while (outputContent.Contains("%"))
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
