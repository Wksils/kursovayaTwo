using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class UnitsOfMeasure
    {
        public int UomId { get; set; }

        public string Symbol { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
