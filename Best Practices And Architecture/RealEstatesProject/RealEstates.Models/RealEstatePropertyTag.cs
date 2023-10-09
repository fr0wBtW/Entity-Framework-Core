using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Models
{
    public class RealEstatePropertyTag
    {
        public int PropertyId { get; set; }
        public virtual RealEstateProperty Property { get; set; }
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }
}
