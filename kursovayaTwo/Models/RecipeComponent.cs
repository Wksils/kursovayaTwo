using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class RecipeComponent
    {
        public int ComponentId { get; set; }

        public int RecipeId { get; set; }

        public int MaterialId { get; set; }

        public decimal Quantity { get; set; }

        public int UomId { get; set; }

        public decimal Percentage { get; set; }

        public bool IsCritical { get; set; }
    }
}
