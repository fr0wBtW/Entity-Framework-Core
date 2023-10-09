using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.DTO;
using ProductShop.DTO.User;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static string ResultDirectoryPath = "../../../Datasets/Results";
        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            InitializeMapper();
            string json = GetUsersWithProducts(context);
            EnsureDirectoryExists(ResultDirectoryPath);

            File.WriteAllText(ResultDirectoryPath + "/users-and-products.json", json);
            Console.WriteLine(GetUsersWithProducts(context));
            //  CreatedDeletedDatabase(context);

            //2.Import Users
            // string inputJson = File.ReadAllText("../../../Datasets/users.json");
            // string result = ImportUsers(context, inputJson);
            // Console.WriteLine(result);

            //3. Import Products
            //string inputJson = File.ReadAllText("../../../Datasets/products.json");
            //string result1 = ImportProducts(context, inputJson);
            // Console.WriteLine(result);

            //4.Import Categories
            // string inputJson = File.ReadAllText("../../../Datasets/categories.json");
            // string result = ImportCategories(context, inputJson);
            // Console.WriteLine(result);

            //5.Import Categories and Products
            //string inputJson = File.ReadAllText("../../../Datasets/categories-products.json");
            // string result = ImportCategoryProducts(context, inputJson);
            //Console.WriteLine(result);

            //6. GetProductsInRange
            //string ResultDirectoryPath = "../../../Datasets/Results";
            //Directory.CreateDirectory(ResultDirectoryPath);
            //string json = GetProductsInRange(context);
            //File.WriteAllText(ResultDirectoryPath + "/categories-by-productsDTO.json", json);
            //Console.WriteLine(GetProductsInRange(context));
        }
        private static void EnsureDirectoryExists(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }
        public static void InitializeMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
        }
        public static void CreatedDeletedDatabase(ProductShopContext context)
        {
            // context.Database.EnsureDeleted();
            context.Database.EnsureCreated();
        }
        //2. Import Users
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Count}";
        }
        //2.1 ImportUsers
        public static string ImportUsers2(ProductShopContext context, string inputJson)
        {
            UserImportDTO[] usersDTO = JsonConvert.DeserializeObject<UserImportDTO[]>(inputJson);

            User[] users = usersDTO.Select(udto => Mapper.Map<User>(udto)).ToArray();
            context.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }
        //3. Import Products
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }
        //4. Import Categories
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            Category[] categories = JsonConvert.DeserializeObject<Category[]>(inputJson).Where(c => c.Name != null).ToArray();

            context.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }
        //5. Import Categories and Products
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            List<CategoryProduct> categoryProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.AddRange(categoryProducts);
            context.SaveChanges();
            return $"Successfully imported {categoryProducts.Count}";
        }
        //6. GetProductsInRange
        public static string GetProductsInRange(ProductShopContext context)
        {
            var productsInRange = context.Products.Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price).Select(p => new ProductsInRangeDTO
                {
                    Name = p.Name,
                    Price = p.Price,
                    SellerName = p.Seller.FirstName + ' ' + p.Seller.LastName
                }).ToList();

            string json = JsonConvert.SerializeObject(productsInRange, Formatting.Indented);

            return json;
        }

        //6. GetProductsInRange Solution 2
        public static string GetProductsInRange2(ProductShopContext context)
        {
            var productsInRange = context.Products.Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price).ProjectTo<ProductsInRangeDTO>().ToArray();

            string json = JsonConvert.SerializeObject(productsInRange, Formatting.Indented);

            return json;
        }
        //7. Export Successfully Sold Products
        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users.Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                //.Select(u => new
                //{
                //    firstName = u.FirstName,
                //    lastName = u.LastName,
                //    soldProducts = u.ProductsSold.Where(p => p.BuyerId != null).Select(p => new
                //    {
                //        name = p.Name,
                //        price = p.Price,
                //        buyerFirstName = p.Buyer.FirstName,
                //        buyerLastName = p.Buyer.LastName
                //    }).ToArray()
                .ProjectTo<UserWithSoldProductsDTO>().ToArray();
            // }).ToArray();

            string json = JsonConvert.SerializeObject(users, Formatting.Indented);

            return json;

        }
        //8. Export Categories by Products Count
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories.Select(c => new
            {
                category = c.Name,
                productsCount = c.CategoryProducts.Count(),
                averagePrice = c.CategoryProducts.Average(cp => cp.Product.Price).ToString("f2"),
                totalRevenue = c.CategoryProducts.Sum(cp => cp.Product.Price)
                .ToString("f2")
            }).OrderByDescending(c => c.productsCount).ToArray();

            string json = JsonConvert.SerializeObject(categories, Formatting.Indented);

            return json;
        }
        //9. Export Users and Products
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context.Users.Where(u => u.ProductsSold.Any(p => p.Buyer != null)).Select(u => new
            {
                lastName = u.LastName,
                age = u.Age,
                soldProducts = new
                {
                    count = u.ProductsSold.Count(p => p.Buyer != null),
                    products = u.ProductsSold.Where(p => p.Buyer != null).Select(p => new
                    {
                        name = p.Name,
                        price = p.Price
                    }).ToArray()
                }
            }).OrderByDescending(u => u.soldProducts.count).ToArray();

            var resultObj = new
            {
                usersCount = users.Length,
                users = users
            };

            JsonSerializerSettings settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };

            string json = JsonConvert.SerializeObject(resultObj, settings);

            return json;
        }
    }
}