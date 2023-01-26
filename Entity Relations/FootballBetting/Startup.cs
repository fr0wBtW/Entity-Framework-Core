using FootballBetting.Data;
using FootballBetting.Data.Models;
using System;
using System.Linq;

namespace FootballBetting
{
    public class Startup
    {
        static void Main(string[] args)
        {
            FootballBettingContext context = new FootballBettingContext();
            User user1 = new User();
            user1.Name = "TEST";
            user1.Email = "usera@testvam.bg";
            user1.Username = "TESTOVIQ";
            user1.Password = "123456";
            user1.Balance = 5000;
            context.Users.Add(user1);
            context.SaveChanges();
            var users = context.Users
                .Select(u => new
                {
                    u.Username, u.Email,
                    Name = u.Name == null ? "(No name)" : u.Name,
                    u.Balance, u.UserId
                });

            foreach (var u in users)
            {
                Console.WriteLine($"{u.Username} -> {u.Email} {u.Name} and the balance is: {u.Balance}, {u.UserId}");
            }
        }
    }
}
