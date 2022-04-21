using CynthiasWebApp.Data;
using CynthiasWebApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CynthiasWebApp.Support
{
    public class Locate
    {
        private CynsDbContext _clientDbContext;
        public string[] emails = { "admin@aol.com", "admin@gmail.com" };

        public Locate()
        {
        }

        public Locate(CynsDbContext clientDbContext)
        {
            _clientDbContext = clientDbContext;
        }

       

        public bool ISEmailAdmin(string email)
        {
            if(email != "admin@aol.com")
            {
                return false;
            }return true;
        }
        
    }
}
