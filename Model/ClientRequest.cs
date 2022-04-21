using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CynthiasWebApp.Model
{
    public class ClientRequest
    {
        public int Id { get; set; }
        public string Request { get; set; }
        public ICollection<Client> ClientLIst { get; set; }
    }
}
