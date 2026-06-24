using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models.ReportRow
{
    public class RecipeReportRow
    {
        public string ProductName { get; set; }
        public string RecipeVersion { get; set; }
        public int BatchCount { get; set; }
        public decimal TotalVolume { get; set; }
    }
}
