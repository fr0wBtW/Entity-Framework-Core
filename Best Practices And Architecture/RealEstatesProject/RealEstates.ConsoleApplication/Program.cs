using Microsoft.EntityFrameworkCore;
using RealEstates.Data;
using RealEstates.Services;
using System;
using System.Runtime.CompilerServices;
using System.Text;

namespace RealEstates.ConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var db = new RealEstateDbContext();
            db.Database.Migrate();

            //IPropertiesService propertiesService = new PropertiesService(db);
            //Console.Write("Min price: ");
            //int minPrice = int.Parse(Console.ReadLine());
            //Console.Write("Max price: ");
            //int maxPrice = int.Parse(Console.ReadLine());

            //var properties = propertiesService.SearchByPrice(minPrice, maxPrice);
            //foreach (var property in properties)
            //{
            //    Console.WriteLine($"{property.District}, етаж:{property.Floor}, {property.Size}m², {property.Year}, {property.Price}€, {property.PropertyType}, {property.BuildingType}");
            //}


            //IDistrictsService districtsService = new DistrictsService(db);
            //var districts = districtsService.GetTopDistrictsByAveragePrice();

            //foreach (var district in districts)
            //{
            //    Console.WriteLine($"{district.Name} => Price: {district.AveragePrice:0.00} ({district.MinPrice}-{district.MaxPrice} => {district.PropertiesCount} properties");
            //}
            //  IPropertiesService propertiesService = new PropertiesService(db);
            //   propertiesService.Create("Дианабд", 100, 2019, 356000, "4-СТАЕН", "ЕПК", 20, 20);
            //   propertiesService.UpdateTags(1);
        }
    }
}