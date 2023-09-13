using Library;
using System.Xml.Linq;


// Arrange
var templateFile = Path.Combine(Path.GetTempPath(), "templateFile.txt");
string message = "%salutation%\n" +
                 "Thank you for ordering %product% from\n" +
                 "our website. We will deliver it to\n" +
                 "%address%. The price is %price% with a\n" +
                 "%discount%% discount.";
File.WriteAllText(templateFile, message);
var xmlFile = Path.Combine(Path.GetTempPath(), "xmlFile.xml");

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
string XmlCreator = "<?xml version=\"1.0\" encoding=\"utf-8\" ?> \n" + doc.ToString();
File.WriteAllText(xmlFile, XmlCreator);


string randomFileNameWithoutExtension = Path.GetFileNameWithoutExtension(Path.GetRandomFileName());
string outputDir = Path.Combine(Path.GetTempPath(), randomFileNameWithoutExtension);
MailMerge mailMerge = new MailMerge();

mailMerge.PropertyChanged += (sender, args) =>
{
    if (args.PropertyName == nameof(MailMerge.Messages))
    {
        Console.WriteLine($"{mailMerge.Messages}");
    }
};
mailMerge.MergeMail(templateFile, xmlFile, outputDir);

