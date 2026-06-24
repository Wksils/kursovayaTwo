using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models.ReportRow
{
    public class LabBlockReportRow
    {
        public string BatchNumber { get; set; }
        public string ProductName { get; set; }
        public string DecisionAt { get; set; }
        public string ResultsText { get; set; }
        public string ResponsibleName { get; set; }
    }
}
