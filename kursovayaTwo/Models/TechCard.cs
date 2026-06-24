using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class TechCard
    {
        public int CardId { get; set; }

        public int ProductId { get; set; }

        public string Version { get; set; } = null!;

        public string Status { get; set; } = null!;

        public bool IsActive { get; set; }

        public int? ApprovedBy { get; set; }

        public DateTime? ApprovedAt { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string? Notes { get; set; }
    }
}
