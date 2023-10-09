using ProductShop.Data;
using ProductShop.Dtos.Export;
using ProductShop.Dtos.Import;
using ProductShop.Models;
using ProductShop.XMLHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            ProductShopContext context = new ProductShopContext();

            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            //var usersXml = File.ReadAllText("../../../Datasets/users.xml");
            //var productsXml = File.ReadAllText("../../../Datasets/products.xml");
            //var categoriesXml = File.ReadAllText("../../../Datasets/categories.xml");
            //var categoryProductsXml = File.ReadAllText("../../../Datasets/categories-products.xml");

            //var users = ImportUsers(context, usersXml);
            //var products = ImportProducts(context, productsXml);
            //var categories = ImportCategories(context, categoriesXml);
            //var categoryProducts = ImportCategoryProducts(context, categoryProductsXml);

            //Console.WriteLine(users);
            //Console.WriteLine(products);
            //Console.WriteLine(categories);
            //Console.WriteLine(categoryProducts);

            // var productsInRange = GetProductsInRange(context);
            // File.WriteAllText("../../../results/productsInRange.xml", productsInRange);

            // var soldProducts = GetSoldProducts(context);
            // File.WriteAllText("../../../results/soldProducts.xml", soldProducts);

            // var categoriesByProductsCount = GetCategoriesByProductsCount(context);
            // File.WriteAllText("../../../results/getCategoriesByProductsCount.xml", categoriesByProductsCount);

            var soldProducts = GetUsersWithProducts(context);
            File.WriteAllText("../../../results/result.xml", soldProducts);
        }
        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            const string rootElement = "Users";
            var usersResult = XmlConverter.Deserializer<ImportUserDto>(inputXml, rootElement);

            /*  List<User> users = new List<User>();

              foreach (var importUserDTO in usersResult)
              {
                  var user = new User
                  {
                      FirstName = importUserDTO.FirstName,
                      LastName = importUserDTO.LastName,
                      Age = importUserDTO.Age
                  };

                  users.Add(user); 
              } */

            var users = usersResult.Select(u => new User
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                Age = u.Age
            }).ToArray();

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }
        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            const string rootElement = "Products";
            var productDtos = XmlConverter.Deserializer<ImportProductDTO>(inputXml, rootElement);

            var products = productDtos.Select(p => new Product
            {
                Name = p.Name,
                Price = p.Price,
                SellerId = p.SellerId,
                BuyerId = p.BuyerId
            }).ToList();


            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Count}";
        }
        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var deserializeCategories = XmlConverter.Deserializer<ImportCategoriesDTO>(inputXml, "Categories");

            var categories = deserializeCategories.Where(c => c.Name != null).Select(c => new Category
            {
                Name = c.Name
            }).ToList();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Count}";
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            const string rootElement = "CategoryProducts";

            var categoryProductsDTOs = XmlConverter.Deserializer<CategoryProductsDTO>(inputXml, rootElement);

            var categoryProducts = categoryProductsDTOs.Where(i => context.Categories.Any(s => s.Id == i.CategoryId) &&
            context.Products.Any(s => s.Id == i.ProductId)).Select(c => new CategoryProduct
            {
                CategoryId = c.CategoryId,
                ProductId = c.ProductId
            }).ToList();

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Count}";
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            const string rootElement = "Products";

            var products = context.Products.Where(p => p.Price >= 500 && p.Price <= 1000).Select(x => new ExportProductInfoDTO
            {
                Name = x.Name,
                Price = x.Price,
                Buyer = x.Buyer.FirstName + " " + x.Buyer.LastName
            }).OrderBy(p => p.Price).Take(10).ToList();

            var result = XmlConverter.Serialize(products, rootElement);

            return result;
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var rootElement = "Users";

            var userWithProducts = context.Users.Where(u => u.ProductsSold.Any())
                .Select(x => new ExportUserSoldProductsDTO
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(p => new UserProductDTO
                    {
                        Name = p.Name,
                        Price = p.Price
                    }).ToArray()
                })
                .OrderBy(l => l.LastName)
                .ThenBy(f => f.FirstName)
                .Take(5)
                .ToArray();

            var result = XmlConverter.Serialize(userWithProducts, rootElement);

            return result;
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            const string rootElement = "Categories";

            var categories = context.Categories.Select(c => new ExportCategoriesByProductsCountDTO
            { 
                Name = c.Name,
                Count = c.CategoryProducts.Count,
                TotalRevenue = c.CategoryProducts.Select(x => x.Product).Sum(p => p.Price),
                AveragePrice = c.CategoryProducts.Select(x => x.Product).Average(p => p.Price)
            }).OrderByDescending(c => c.Count)
            .ThenBy(t => t.TotalRevenue)
            .ToArray();

            var result = XmlConverter.Serialize(categories, rootElement);

            return result;
        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var usersAndProducts = context.Users
            //.ToArray()
                .Where(p => p.ProductsSold.Any())
                .Select(u => new ExportUserDTO
                {
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    Age = u.Age,
                    SoldProducts = new ExportProductCountDTO
                    {
                        Count = u.ProductsSold.Count,
                        Products = u.ProductsSold.Select(p => new ExportProductDTO
                        {
                            Name = p.Name,
                            Price = p.Price
                        })
                        .OrderByDescending(p => p.Price)
                        .ToArray()
                    }
                })
                .OrderByDescending(x => x.SoldProducts.Count)
                .Take(10)
                .ToArray();

            var resultsDTO = new ExportUserCountDTO
            {
                Count = context.Users.Count(p => p.ProductsSold.Any()),
                Users = usersAndProducts
            };

            var result = XmlConverter.Serialize(resultsDTO, "Users");

            return result;
        }
    }
}