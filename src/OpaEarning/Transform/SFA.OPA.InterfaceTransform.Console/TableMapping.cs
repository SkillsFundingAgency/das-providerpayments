using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace SFA.OPA.InterfaceTransform.Console
{
    public class TableMapping
    {
        public string Source { get; set; }
        public string Destination { get; set; }
        public int Ordinal { get; set; }
        public List<string> Columns { get; set; }

        public XElement Mapping
        {
            get
            {
                return new XElement("TableMapping",
                    new XElement("Source", Source),
                    new XElement("Destination", Destination),
                    new XElement("Ordinal", Ordinal),
                    new XElement("ColumnMappings",
                        Columns.Select(
                            c =>
                                new XElement("ColumnMapping",
                                    new XElement("Source", c),
                                    new XElement("Destination", c)
                                )
                            )
                        )
                    );
            }
        }  
    }
}