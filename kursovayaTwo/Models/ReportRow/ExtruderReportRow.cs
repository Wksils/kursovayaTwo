using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models.ReportRow
{
    public class ExtruderReportRow
    {
        public string BatchNumber { get; set; }
        public string Zone { get; set; }
        public string ParameterName { get; set; }
        public decimal? TargetValue { get; set; }
        public decimal ActualValue { get; set; }
        public string Deviation {  get; set; }
        public string RecordedAt { get; set; }
        public string EventType { get; set; }
    }
}
