using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealEstates.Models
{
    public class PropertyType
    {
        public PropertyType()
        {
            this.Properties = new HashSet<RealEstateProperty>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<RealEstateProperty> Properties { get; set; }
    }
}
