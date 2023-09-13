using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace TestMailMerger.Helper
{
    public static class XmlCreator
    {
        public static string CreateValidXmlDocument()
        {
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
            return "<?xml version=\"1.0\" encoding=\"utf-8\" ?> \n" + doc.ToString();
        }
        public static string CreateValidXmlTwoElements()
        {
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
                    , new XElement("Order", new XAttribute("id", "12345"),
                        new XElement("firstname", "John"),
                        new XElement("lastname", "Smith"),
                        new XElement("title", "Mr"),
                        new XElement("salutation", "Dear %title% %lastname%"),
                        new XElement("product", "Chicken Nuggets"),
                        new XElement("price", "$10.00"),
                        new XElement("discount", "12"),
                        new XElement("address", "128 Oak Drive, Corktown")
                    )
                )
            );

            return "<?xml version=\"1.0\" encoding=\"utf-8\" ?> \n" + doc.ToString();

        }
        public static string CreateValidXmlDocumentDubbleKey()
        {
            var doc = new XDocument(
                new XElement("Orders",
                    new XElement("Order", new XAttribute("id", "12345"),
                        new XElement("firstname", "Jane"),
                        new XElement("lastname", "Doe"),
                        new XElement("salutation", "Dear %firstname%"),
                        new XElement("product", "Hamburgers"),
                        new XElement("price", "$5.00"),
                        new XElement("discount", "5"),
                        new XElement("address", "1512 Crescent Ave, Hampton")

                    )
                    , new XElement("Order", new XAttribute("id", "12345"),
                        new XElement("firstname", "John"),
                        new XElement("lastname", "Smith"),
                        new XElement("title", "Mr"),
                        new XElement("salutation", "Dear %title% %lastname%"),
                        new XElement("product", "Chicken Nuggets"),
                        new XElement("price", "$10.00"),
                        new XElement("discount", "12"),
                        new XElement("address", "128 Oak Drive, Corktown")
                    )
                )
            );

            return "<?xml version=\"1.0\" encoding=\"utf-8\" ?> \n" + doc.ToString();

        }

        public static string CreateValidXmlDocumentOther()
        {
            var doc = new XDocument(
                new XElement("Orders",
                    new XElement("Order", new XAttribute("id", "12345"),
                        new XElement("firstname", "John"),
                        new XElement("lastname", "Smith"),
                        new XElement("title", "Mr"),
                        new XElement("salutation", "Dear %title% %lastname%"),
                        new XElement("product", "Chicken Nuggets"),
                        new XElement("price", "$10.00"),
                        new XElement("discount", "12"),
                        new XElement("address", "128 Oak Drive, Corktown")
                    )
                )
            );
            return "<?xml version=\"1.0\" encoding=\"utf-8\" ?> \n" + doc.ToString();
        }
        public static string CreateInValidXmlDocument()
        {
            var doc = new XDocument(
                new XElement("Orders",
                    new XElement("Order", new XAttribute("id", "12346"),
                        new XElement("firstname", "Jane"),
                        new XElement("lastname", "Doe"),
                        new XElement("opening", "Dear %firstname%"),
                        new XElement("product", "Hamburgers"),
                        new XElement("price", "$5.00"),
                        new XElement("discount", "5"),
                        new XElement("address", "1512 Crescent Ave, Hampton")
                    )
                )
            );
            return "<?xml version=\"1.0\" encoding=\"utf-8\" ?> \n" + doc.ToString();
        }
        public static string CreateInValidXmlDocumentEmptyValueInElement()
        {
            var doc = new XDocument(
                new XElement("Orders",
                    new XElement("Order", new XAttribute("id", "12346"),
                        new XElement("firstname", "Jane"),
                        new XElement("lastname", "Doe"),
                        new XElement("salutation", "Dear %firstname%"),
                        new XElement("product"),
                        new XElement("price", "$5.00"),
                        new XElement("discount", "5"),
                        new XElement("address", "1512 Crescent Ave, Hampton")
                    )
                )
            );
            return "<?xml version=\"1.0\" encoding=\"utf-8\" ?> \n" + doc.ToString();
        }   
        public static string CreateInValidXmlInjection()
        {

            return  @"<?xml version=""1.0"" encoding=""iso-8859-1""?>
                        <!DOCTYPE foo [
                            <!ELEMENT foo ANY >
                            <!ENTITY xxe SYSTEM ""file:///etc/passwd"" >
                        ]>
                        <foo>&xxe;</foo>
                        <Order id=""12345"">
                            <firstname>John</firstname>
                            <lastname>Smith</lastname>
                            <title>Mr</title>
                            <salutation>Dear %title% %lastname%</salutation>
                            <product>Chicken Nuggets</product>
                            <price>$10.00</price>
                            <discount>12</discount>
                            <address>128 Oak Drive, Corktown</address>
                        </Order>";
        }


    }
}
