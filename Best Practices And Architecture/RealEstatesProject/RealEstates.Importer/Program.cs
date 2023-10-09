using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Runtime.Serialization.Json;
using System.Text.Json;

namespace RealEstates.Importer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var json = File.ReadAllText("imot.bg-raw-data-2021-03-18.json");
            var properties = JsonSerializer.Deserialize < IEnumerable<JsonProperty>>(json);
            var db = new RealEstateDbContext();
            IPropertiesService propertiesService = new PropertiesService(db);

            foreach (var property in properties.Where(x => x.Price > 1000))
            {
                try
                {
                    propertiesService.Create(
                        property.District,
                        property.Size,
                        property.Year,
                        property.Price,
                        property.Type,
                        property.BuildingType,
                        property.Floor,
                        property.TotalFloors);
                }

                catch
                {
                }
            }
        }
    }
}