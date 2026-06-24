using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class FilterOption
    {
        public string Value { get; set; }
        public bool IsChacked { get; set; }
        public string? Display { get; set; }
        public string Label => Display ?? Value;
    }
}
