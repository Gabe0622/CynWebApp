using CynthiasWebApp.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CynthiasWebApp.Data
{
    public class CynsDbContext : DbContext
    {
        public CynsDbContext(DbContextOptions<CynsDbContext> options) : base(options)
        {

        }
        public DbSet<Client> Clients { get; set; }
        public DbSet<CompletedServices> CompletedServicesDbConext { get; set; }
        public DbSet<ServiceTypes> ServiceTypes { get; set; }
        public DbSet<Admin> Admins { get; set; }
        public DbSet<ClientRequest> ClientAdminRequest { get; set; }
      
    }
}
 