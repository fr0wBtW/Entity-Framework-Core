﻿using System.ComponentModel.DataAnnotations;

namespace FastFood.Core.ViewModels.Orders
{
    public class CreateOrderEmployeeViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}
