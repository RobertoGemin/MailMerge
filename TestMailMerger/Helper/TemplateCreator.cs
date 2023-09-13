using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestMailMerger.Helper
{
    public static class TemplateCreator
    {
        public static string CreateMessage()
        {
            string message = "%salutation%\n" +
                             "Thank you for ordering %product% from\n" +
                             "our website. We will deliver it to\n" +
                             "%address%. The price is %price% with a\n" +
                             "%discount%% discount.";
            return message;
        }
        public static string CreateInvalidMessage()
        {
            string message = "salutation\n" +
                             "Thank you for ordering product from\n" +
                             "our website. We will deliver it to\n" +
                             "address. The price is price with a\n" +
                             "discount% discount.";
            return message;
        }
    }
}
