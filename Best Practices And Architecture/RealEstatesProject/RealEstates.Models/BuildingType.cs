using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Models
{
    public class BuildingType
    {
        public BuildingType()
        {
            this.Properties = new HashSet<RealEstateProperty>();
        }
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<RealEstateProperty> Properties { get; set; }
    }
}
