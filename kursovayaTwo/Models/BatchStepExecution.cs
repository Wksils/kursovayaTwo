using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class BatchStepExecution
    {
        public int ExecutionId { get; set; }

        public int BatchId { get; set; }

        public int StepId { get; set; }

        public string Status { get; set; } = null!;

        public DateTime? StartedAt { get; set; }

        public DateTime? CompletedAt { get; set; }

        public int? StartedBy { get; set; }

        public int? CompletedBy { get; set; }

        public string? ActualParams { get; set; }

        public string? Notes { get; set; }
    }
}
