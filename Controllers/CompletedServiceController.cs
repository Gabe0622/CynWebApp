using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CynthiasWebApp.Data;
using CynthiasWebApp.Model;
using CynthiasWebApp.Support;
using AuthenticationPlugin;
using Microsoft.AspNetCore.Authorization;

namespace CynthiasWebApp.Support
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompletedServiceController : ControllerBase
    {
        private CynsDbContext _cynsDbContext;
        public CompletedServiceController(CynsDbContext cynsDbContext)
        {
            _cynsDbContext = cynsDbContext;
        }

        [HttpPost(ReqParms.ActionEmailCost)]
        [Authorize(Roles = AccountType.Admin)]
        public IActionResult ClientCompleteService(string email, double cost)
        {
            try
            {
                var client = _cynsDbContext.Clients.Where(c => c.Email == email).FirstOrDefault();
                if (client == null)
                {
                    return NotFound(HttpReturnValue.ClientNotFound);
                }
                else
                {
                    var completeService = new CompletedServices()
                    {
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Email = client.Email,
                        Service = "Hair",
                        Cost = cost
                    };
                    _cynsDbContext.CompletedServicesDbConext.Add(completeService);
                    _cynsDbContext.SaveChanges();
                    return Ok(HttpReturnValue.ClientCompletedService);
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
