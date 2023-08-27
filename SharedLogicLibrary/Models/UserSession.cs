using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLogicLibrary.Models
{
    public class UserSession
    {
        public string? Username { get; set; }
        public string? Role { get; set; }
        public string? Token { get; set; }
    }
}
