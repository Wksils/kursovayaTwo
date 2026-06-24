using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace kursovayaTwo.Models
{
    public class User
    {
        public int UserId { get; set; }

        public string Login { get; set; } = null!;

        public string FullName { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string Role { get; set; } = null!;

        public string Department { get; set; } = null!;

        public bool IsActive { get; set; }
    }
}
