using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer.DataTransferObjects
{
    [XmlType("Car")]
    public class ImportCarsDTO
    {
        [XmlElement("make")]
        public string Make { get; set; }
        [XmlElement("model")]
        public string Model { get; set; }
        [XmlElement("travelDistance")]
        public long TravelDistance { get; set; }
        [XmlArray("parts")]
        public ImportCarPartDTO[] Parts { get; set; }
    }
    [XmlType("partId")]
    public class ImportCarPartDTO
    {
        [XmlAttribute("id")]
        public int Id { get; set; }
    }
}
