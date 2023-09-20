using Library.Interface.Validator;
using Library.Validator.Generator;
using System.Xml.Linq;

namespace Library.Validator.Processing
{
    public class TemplateValidator : IValidator
    {
        public string Message { get; private set; } = string.Empty;

        public bool Validate(string path)
        {
            var extension = Path.GetExtension(path).ToLower();
            if (!extension.Contains("txt"))
            {
                Message = "File extension is not valid.";
                return false;
            }

            XsdGenerator generator = new();
            string xsdFile = generator.GetXsdFile();
            try
            {
                var xmlSchemaDoc = XDocument.Load(xsdFile);
                var elementNames = xmlSchemaDoc.Descendants()
                    .Where(x => x.Name.LocalName == "element" && x.Attribute("name") != null)
                    .Select(x => x.Attribute("name").Value)
                    .ToList();
                var placeholders = elementNames.Select(name => $"%{name}%").ToArray();
                var templateContent = File.ReadAllText(path);
                if (!placeholders.Any(placeholder => templateContent.Contains(placeholder)))
                {
                    Message = "Template does not contain required placeholders.";
                    return false;
                }

                Message = "Template is valid.";
                return true;
            }
            catch (Exception ex)
            {
                Message = $"An error occurred: {ex.Message}";
                return false;
            }
        }
    }
}
