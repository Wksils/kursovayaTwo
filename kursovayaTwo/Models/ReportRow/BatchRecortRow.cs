using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace kursovayaTwo.Models.ReportRow
{
    public class BatchRecortRow
    {
        public string BatchNumber { get; set; }
        public string ProductName { get; set; }
        public string CreatedAt { get; set; }
        public string Status { get; set; }
        public string HasDeviations { get; set; }
        public string QaDecision { get; set; }
    }
}
