using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class ExtruderTelemetry
    {
        public long TelemetryId { get; set; }
        public int ExecutionId { get; set; }
        public string Zone { get; set; } = null!;
        public string ParameterName { get; set; } = null!;
        public decimal? TargetValue { get; set; }
        public decimal ActualValue { get; set; }
        public int? UomId { get; set; }
        public DateTime RecordedAt { get; set; }
    }
}
