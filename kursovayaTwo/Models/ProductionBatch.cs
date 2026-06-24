using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class ProductionBatch
    {
        public int BatchId { get; set; }

        public string BatchNumber { get; set; } = null!;

        public int ProductId { get; set; }

        public int RecipeId { get; set; }

        public int CardId { get; set; }

        public decimal PlannedQty { get; set; }

        public decimal? ActualQty { get; set; }

        public int UomId { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? StartedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int? StartedBy { get; set; }

        public int? CompletedBy { get; set; }

        public string? QaDecision { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
