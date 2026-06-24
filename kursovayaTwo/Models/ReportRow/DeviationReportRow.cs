using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models.ReportRow
{
    public class DeviationReportRow
    {
        public string BatchNumber { get; set; }
        public string StepName { get; set; }
        public string ActualParams { get; set; }
        public string Status { get; set; }
    }
}
