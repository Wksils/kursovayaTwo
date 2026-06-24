using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class RawMaterial
    {
        public int MaterialId { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Category { get; set; } = null!;

        public int UomId { get; set; }

        public int? ShelfLifeDays { get; set; }

    }
}
