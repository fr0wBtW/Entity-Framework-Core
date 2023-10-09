namespace BookShop
{
    using Data;
    using Initializer;
    using System;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var context = new BookShopContext())
            //  DbInitializer.ResetDatabase(context);
            {
                // string command = Console.ReadLine().ToLower();
                // Console.WriteLine(GetBooksByAgeRestriction(context, command));
                // Console.WriteLine(GetGoldenBooks(context));
                // Console.WriteLine(GetBooksByPrice(context));
                 int year = int.Parse(Console.ReadLine());
                 Console.WriteLine(GetBooksNotReleasedIn(context, year));
            }
        }
        //2.Age Restriction
        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            return string.Join(Environment.NewLine,
                context.Books.ToList().Where(b => b.AgeRestriction.ToString().ToLower() == command)
                .Select(b => b.Title).OrderBy(bt => bt).ToList());
        }

        //3.Golden Books
        public static string GetGoldenBooks(BookShopContext context)
        {
            var goldenBooks = context.Books.AsEnumerable().Where(b => b.EditionType.ToString() == "Gold" && b.Copies < 5000).OrderBy(b => b.BookId).Select(b => b.Title).ToList();

            return string.Join(Environment.NewLine, goldenBooks);
        }

        //4.Books by Price
        public static string GetBooksByPrice(BookShopContext context)
        {
            var booksByPrice = context.Books.Where(b => b.Price > 40).OrderByDescending(b => b.Price)
                .Select(b => $"{b.Title} - ${b.Price}");

            return string.Join(Environment.NewLine, booksByPrice);
        }

        //5.Not Released In
        public static string GetBooksNotReleasedIn(BookShopContext context, int year)
        {
            var booksNotReleasedIn = context.Books.Where(b => b.ReleaseDate.Value.Year != year).OrderBy(b => b.BookId)
                .Select(b => b.Title).ToList();

            return string.Join(Environment.NewLine, booksNotReleasedIn);
        }
    }
}

