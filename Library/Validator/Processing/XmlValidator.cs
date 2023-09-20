using System.Text;
using System.Xml.Linq;
using System.Xml;
using Library.Validator.Generator;
using Library.Interface.Validator;

namespace Library.Validator.Processing
{
    public class XmlValidator : IValidator
    {
        public string Message { get; private set; } = string.Empty;

        public bool Validate(string path)
        {

            var extension = Path.GetExtension(path).ToLower();
            if (!extension.Contains("xml"))
            {
                Message = "File extension is not valid.";
                return false;
            }

            XsdGenerator generator = new();
            string xsdFile = generator.GetXsdFile();
            try
            {
                var settings = new XmlReaderSettings();
                settings.Schemas.Add(null, xsdFile);
                settings.ValidationType = ValidationType.Schema;
                var validationErrors = new StringBuilder();
                settings.ValidationEventHandler += (sender, e) =>
                {
                    validationErrors.AppendLine($"Validation Error: {e.Message}");
                };
                using (var reader = XmlReader.Create(path, settings))
                {
                    var doc = XDocument.Load(reader);

                    var emptyElements = doc.Descendants()
                        .Where(x => string.IsNullOrWhiteSpace(x.Value))
                        .ToList();
                    if (emptyElements.Any())
                        foreach (var element in emptyElements)
                            validationErrors.AppendLine($"Element '{element.Name}' does not have a value.");
                }

                Message = validationErrors.Length == 0 ? "xml is valid." : validationErrors.ToString();
                return validationErrors.Length == 0;
            }
            catch (Exception ex)
            {
                Message = $"XML validation error: {ex.Message}";
                return false;
            }
        }
    }
}
