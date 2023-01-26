using FootballBetting.Data;
using FootballBetting.Data.Models;
using FootballBetting.Web.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FootballBetting.Web.Controllers
{
    public class UsersController : Controller
    {
        public readonly FootballBettingContext footballBettingContext;

        public UsersController(FootballBettingContext footballBettingContext)
        {
            this.footballBettingContext = footballBettingContext;
        }
        public IActionResult Index()
        {
            return this.RedirectToRoute("All");
        }

        [HttpGet]
        public IActionResult All()
        {
            List<User> users = footballBettingContext.Users.ToList();

            return this.View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Create(CreateUserViewModel model)
        {
            if(!ModelState.IsValid)
            {
                return this.View("Error");
            }

            User user = new User()
            {
                Username = model.Username,
                Password = model.Password,
                Email = model.Email,
                Name = model.Name,
                Balance = model.Balance
            };

            this.footballBettingContext.Users.Add(user);
            this.footballBettingContext.SaveChanges();

            return this.RedirectToAction("All");
        }
    }
}
