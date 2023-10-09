﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop.Dtos.Export
{
    public class ExportUserCountDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }
        [XmlArray("users")]
        public ExportUserDTO[] Users { get; set; }
    }

    [XmlType("User")]
    public class ExportUserDTO
    {
        [XmlElement("firstName")]
        public string FirstName { get; set; }
        [XmlElement("lastName")]
        public string LastName { get; set; }
        [XmlElement("age")]
        public int? Age { get; set; }
        [XmlElement("soldProducts")]
        public ExportProductCountDTO SoldProducts { get; set; }
    }
    public class ExportProductCountDTO
    {
        [XmlElement("count")]
        public int Count { get; set; }
        [XmlArray("products")]
        public ExportProductDTO[] Products { get; set; }
    }
    [XmlType("Product")]
    public class ExportProductDTO
    {
        [XmlElement("name")]
        public string Name { get; set; }
        [XmlElement("price")]
        public decimal Price { get; set; }

    }
}
