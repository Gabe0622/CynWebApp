using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CynthiasWebApp.Model
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PassWord { get; set; }
        public string Allergies { get; set; }
        public int Age { get; set; }

    }
}
