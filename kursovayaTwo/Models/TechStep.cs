using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class TechStep
    {
        public int StepId { get; set; }

        public int CardId { get; set; }

        public int StepNumber { get; set; }

        public string Name { get; set; } = null!;

        public string? Description { get; set; }

        public int? EquipmentId { get; set; }

        public int? DurationMin { get; set; }

        public bool IsCritical { get; set; }

        public string? ParamsNote { get; set; }
    }
}
