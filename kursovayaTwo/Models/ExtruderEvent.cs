using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class ExtruderEvent
    {
        public int EventId { get; set; }
        public int ExecutionId { get; set; }
        public string Zone { get; set; } = null!;
        public string EventType { get; set; } = null!;
        public string? ParameterName { get; set; }
        public decimal? ActualValue { get; set; }
        public decimal? TargetValue { get; set; }
        public string? Description { get; set; }
        public int? RecordedBy { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
