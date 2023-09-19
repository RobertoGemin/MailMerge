using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Validator.Generator
{
    public class XsdGenerator
    {
        public  string GetXsdFile()
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
    }
}
