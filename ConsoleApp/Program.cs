using Library.Interface;
using System.Xml.Linq;
using Library.Validator.Class;

var templateFile = TemplateFile(out var xmlFile, out var XmlCreator);
File.WriteAllText(xmlFile, XmlCreator);
string randomFileNameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
string outputDir = Path.Combine(Path.GetTempPath(), randomFileNameWithoutExtension);




MailMergeValidator  mailMergeValidator = MailMergeValidator.CreateMailMergeValidator();


mailMergeValidator.PropertyChanged += (sender, args) =>
{
    if (args.PropertyName == nameof(MailMergeValidator.Messages))
    {
        Console.WriteLine($"{mailMergeValidator.Messages}");
    }
};

if (mailMergeValidator.Validate(templateFile, xmlFile, outputDir))
{
    mailMergeValidator.Process();
}

Console.ReadLine();

static string TemplateFile(out string xmlFile1, out string xmlCreator)
{
    var s = Path.Combine(Path.GetTempPath(), "templateFile.txt");
    string message = "%salutation%\n" +
                     "Thank you for ordering %product% from\n" +
                     "our website. We will deliver it to\n" +
                     "%address%. The price is %price% with a\n" +
                     "%discount%% discount.";
    File.WriteAllText(s, message);
    xmlFile1 = Path.Combine(Path.GetTempPath(), "xmlFile.xml");

    var doc = new XDocument(
        new XElement("Orders",
            new XElement("Order", new XAttribute("id", "12346"),
                new XElement("firstname", "Jane"),
                new XElement("lastname", "Doe"),
                new XElement("salutation", "Dear %firstname%"),
                new XElement("product", "Hamburgers"),
                new XElement("price", "$5.00"),
                new XElement("discount", "5"),
                new XElement("address", "1512 Crescent Ave, Hampton")
            )
        )
    );
    xmlCreator = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> \n" + doc.ToString();
    return s;
}

