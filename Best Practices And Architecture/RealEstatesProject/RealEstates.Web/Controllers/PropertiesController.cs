﻿using Microsoft.AspNetCore.Mvc;
using RealEstates.Services;

namespace RealEstates.Web.Controllers
{
    public class PropertiesController : Controller
    {
        private readonly IPropertiesService propertiesService;

        public PropertiesController(IPropertiesService propertiesService)
        {
            this.propertiesService = propertiesService;
        }

        public ActionResult Search() 
        {
            return this.View();
        }
        public IActionResult DoSearch(int minPrice, int maxPrice)
        {
            var properties = this.propertiesService.SearchByPrice(minPrice, maxPrice);
            return this.View(properties);
        }
    }
}
