using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetStore.Common
{
    public static class DbConfiguration
    {
        public static string DefConnectionString = @"Server=GEORGI\SQLEXPRESS;Database=PetStore;TrustServerCertificate=true;Integrated Security=True";
    }
}
