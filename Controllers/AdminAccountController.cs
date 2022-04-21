using AuthenticationPlugin;
using CynthiasWebApp.Data;
using CynthiasWebApp.Model;
using CynthiasWebApp.Support;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CynthiasWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAccountController : ControllerBase
    {
        private CynsDbContext _cynsDbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public AdminAccountController(IConfiguration configuration,CynsDbContext cynsDbContext)
        {
            _cynsDbContext = cynsDbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpPost(ReqParms.Action)]
        [Authorize(Roles = AccountType.Admin)]
        public IActionResult GetListOfClients()
        {
            try
            {
                return Ok(_cynsDbContext.Clients);
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(ReqParms.Action)]
        public IActionResult CreateAdminAccount([FromBody]Admin admin)
        {
            try
            {
                if(AccountType.ValidAccounts.Contains(admin.Email))
                {
                    var newAdmin = new Admin()
                    {
                        FirstName = admin.FirstName,
                        LastName = admin.LastName,
                        Email = admin.Email,
                        Password = SecurePasswordHasherHelper.Hash(admin.Password)
                        
                    };
                    _cynsDbContext.Admins.Add(newAdmin);
                    _cynsDbContext.SaveChanges();
                    return Ok(HttpReturnValue.AdminAccount202);
                }else
                {
                    return Unauthorized(HttpReturnValue.AdminAccount401);
                }
            }catch(Exception e )
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(ReqParms.Action)]
        public IActionResult LoginAsAdmin([FromBody]Admin admin)
        {

                try
                {
                    var isAdmin = _cynsDbContext.Admins.Where(e => e.Email == admin.Email).FirstOrDefault();
                    if (isAdmin == null)
                    {
                        return Unauthorized();
                    }
                    else
                    {
                        if (!SecurePasswordHasherHelper.Verify(admin.Password,isAdmin.Password))
                        {
                            return Unauthorized();
                        }
                        else
                        {
                            var claims = new[]
                            {
                                new Claim(JwtRegisteredClaimNames.Email, admin.Email),
                                new Claim(ClaimTypes.Email, admin.Email),
                                new Claim(ClaimTypes.Role, AccountType.Admin)
                            };
                            var token = _auth.GenerateAccessToken(claims);
                        return new ObjectResult(new
                        {
                            access_token = token.AccessToken,
                            expires_in = token.ExpiresIn,
                            token_type = token.TokenType,
                            creation_Time = token.ValidFrom,
                            expiration_Time = token.ValidTo,
                            account_type = AccountType.Admin
                        }); 
                      }
                    }
                }
                catch(Exception e)
                {
                    return BadRequest();
                }
            
        }

        [HttpDelete(ReqParms.ActionEmail)]
        [Authorize(Roles = AccountType.Admin)]
        public IActionResult DeleteClient(string email)
        {
            try
            {
                var clientToDelete = _cynsDbContext.Clients.Where(c => c.Email == email).SingleOrDefault();
                _cynsDbContext.Clients.Remove(clientToDelete);
                _cynsDbContext.SaveChanges();
                return Ok(HttpReturnValue.ClientDeleted);
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
