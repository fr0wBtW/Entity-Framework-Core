﻿using Microsoft.AspNetCore.Mvc;
using RealEstates.Services;
using RealEstates.Web.Models;
using System.Diagnostics;

namespace RealEstates.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IDistrictsService districtsService;

        public HomeController(IDistrictsService districtsService)
        {
            this.districtsService = districtsService;
        }
        public IActionResult Index()
        {
            var districts = this.districtsService.GetTopDistrictsByAveragePrice(133);
            return View(districts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}