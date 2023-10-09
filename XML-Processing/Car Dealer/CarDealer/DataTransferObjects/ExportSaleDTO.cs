using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects
{
    [XmlType("sale")]
    public class ExportSaleDTO
    {
        [XmlElement("car")]
        public ExportCarDTO Car { get; set; }
        [XmlElement("discount")]
        public decimal Discount { get; set; }
        [XmlElement("customerName")]
        public string CustomerName { get; set; }
        [XmlElement("price")]
        public decimal Price { get; set; }
        [XmlElement("price-with-discount")]
        public decimal PriceWithDiscount { get; set; }
    }
    [XmlType("car")]
    public class ExportCarDTO
    {
        [XmlElement("make")]
        public string Make { get; set; }
        [XmlElement("model")]
        public string Model { get; set; }
        [XmlElement("travelled-distance")]
        public long TravelDistance { get; set; }
    }
}
