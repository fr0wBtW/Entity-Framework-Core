using CarDealer.Data;
using CarDealer.DataTransferObjects;
using CarDealer.Models;
using ProductShop.XMLHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext context = new CarDealerContext();

            context.Database.EnsureCreated();

            //  var suppliersXML = File.ReadAllText("../../../Datasets/suppliers.xml");
            // ImportSuppliers(context, suppliersXML);

            //  var partsXML = File.ReadAllText("../../../Datasets/parts.xml");
            //  ImportParts(context, partsXML);

            // var carsXML = File.ReadAllText("../../../Datasets/cars.xml");
            // ImportCars(context, carsXML);

            var customersXML = File.ReadAllText("../../../Datasets/customers.xml");
            // ImportCustomers(context, customersXML);

            var salesXML = File.ReadAllText("../../../Datasets/sales.xml");
            ImportSales(context, salesXML);

            var result = GetSalesWithAppliedDiscount(context);
            File.WriteAllText("../../../result.xml", result);
        }
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var supplierDTOS = XmlConverter.Deserializer<ImportSupplierDTO>(inputXml, "Suppliers");

            var result = supplierDTOS.Select(s => new Supplier
            {
                Name = s.Name,
                IsImporter = s.IsImporter
            }).ToArray();

            context.Suppliers.AddRange(result);
            context.SaveChanges();

            return $"Successfully improted {result.Length}";
        }
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var partsDTOs = XmlConverter.Deserializer<ImportPartsDTO>(inputXml, "Parts");

            var parts = partsDTOs.Where(i => context.Suppliers.Any(s => s.Id == i.SupplierId)).Select(x => new Part
            {
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity,
                SupplierId = x.SupplierId
            }).ToArray();

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Length}";
        }
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var carsDTO = XmlConverter.Deserializer<ImportCarsDTO>(inputXml, "Cars");

            List<Car> cars = new List<Car>();

            foreach (var carDto in carsDTO)
            {
                var uniqueParts = carDto.Parts.Select(s => s.Id).Distinct().ToArray();
                var realParts = uniqueParts.Where(id => context.Parts.Any(i => i.Id == id));

                var car = new Car
                {
                    Make = carDto.Make,
                    Model = carDto.Model,
                    TravelledDistance = carDto.TravelDistance,
                    PartCars = realParts.Select(id => new PartCar
                    {
                        PartId = id
                    })
                    .ToArray()
                };

                cars.Add(car);
            }
            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var customersDTOs = XmlConverter.Deserializer<ImportCustomerDTO>(inputXml, "Customers");

            var customers = customersDTOs.Select(x => new Customer
            {
                Name = x.Name,
                IsYoungDriver = x.IsYoungDriver,
                BirthDate = DateTime.Parse(x.BirthDate)
            }).ToArray();

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Length}";
        }
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var salesDTOs = XmlConverter.Deserializer<ImportSalesDTO>(inputXml, "Sales");

            var sales = salesDTOs.Where(i => context.Cars.Any(x => x.Id == i.CarId)).Select(c => new Sale
            {
                CarId = c.CarId,
                CustomerId = c.CustomerId,
                Discount = c.Discount
            }).ToArray();

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Length}";
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var result = context.Sales.Select(s => new ExportSaleDTO
            {
                Car = new ExportCarDTO
                {
                    Make = s.Car.Make,
                    Model = s.Car.Model,
                    TravelDistance = s.Car.TravelledDistance
                },
                Discount = s.Discount,
                CustomerName = s.Customer.Name,
                Price = s.Car.PartCars.Sum(p => p.Part.Price),
                PriceWithDiscount = s.Car.PartCars.Sum(p => p.Part.Price) - s.Car.PartCars.Sum(p => p.Part.Price) * s.Discount / 100,
            }).ToArray();
            var xmlResult = XmlConverter.Serialize(result, "sales");

            return xmlResult;
        }
    }
}