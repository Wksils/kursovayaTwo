using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string ProductType { get; set; } = null!;

        public string ReleaseForm { get; set; } = null!;

        public string Status { get; set; } = null!;

        public DateTime CreatedAt { get; set; }
    }
}
