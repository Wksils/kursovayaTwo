using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace kursovayaTwo.Models.ReportRow
{
    public class ReportItem
    {
        public string Name { get; set; }
        public ICommand Command { get; set; }
    }
}
