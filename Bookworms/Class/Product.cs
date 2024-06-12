using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bookworms.Class
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public int idCategory { get; set; }
        public string Category { get; set; }
        public string Avalilability { get; set; }
        public string Status { get; set; }
    }
}
