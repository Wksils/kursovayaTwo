using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }

        public string Code { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string Department { get; set; } = null!;

        public string Status { get; set; } = null!;
    }
}
