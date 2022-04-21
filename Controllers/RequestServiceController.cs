using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using CynthiasWebApp.Data;
using CynthiasWebApp.Model;
using CynthiasWebApp.Support;
using Microsoft.AspNetCore.Authorization;

namespace CynthiasWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestServiceController : ControllerBase
    {
        private CynsDbContext _cynsDbContext;
        public RequestServiceController(CynsDbContext cynsDbContext)
        {
            _cynsDbContext = cynsDbContext;
        }

        [HttpGet(ReqParms.Action)]
        [Authorize(Roles = AccountType.Admin)]
        public IActionResult GetListOfRequests()
        {
            return Ok(_cynsDbContext.ClientAdminRequest);
        }

        [HttpGet(ReqParms.Action)]
        [Authorize(Roles = AccountType.Admin)]
        public IActionResult ApproveOrDenyRequest([FromBody]AdminDecision decision)
        {
            try
            {
                var reqs = _cynsDbContext.ClientAdminRequest;
                if(reqs == null)
                {
                    return NotFound(HttpReturnValue.NoClientRequests);
                }else
                {
                    if(decision.Approved)
                    {
                        return Ok(HttpReturnValue.AdminApprovedRequest);
                    }else
                    {
                        return Ok(HttpReturnValue.AdminDeniedRequest);
                    }
                }
            }catch(Exception e)
            {
                return BadRequest(e.StackTrace);
            }
        }

        
    }
}
