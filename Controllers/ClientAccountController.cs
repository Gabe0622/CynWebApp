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
    public class ClientAccountController : ControllerBase
    {
        private CynsDbContext _cynsDbContext;
        private IConfiguration _configuration;
        private readonly AuthService _auth;
        public ClientAccountController(IConfiguration configuration,CynsDbContext cynsDbContext)
        {
            _cynsDbContext = cynsDbContext;
            _configuration = configuration;
            _auth = new AuthService(_configuration);
        }

        [HttpPost(ReqParms.Action)]
        public IActionResult CreateClientAccount([FromBody]Client client)
        {
            try
            {
                var doesClientExist = _cynsDbContext.Clients.Where(e => e.Email == client.Email).SingleOrDefault();
                if (doesClientExist != null)
                {
                    return BadRequest(HttpReturnValue.ClientAccountExists);
                }
                else
                {
                    var newClient = new Client()
                    {
                        FirstName = client.FirstName,
                        LastName = client.LastName,
                        Email = client.Email,
                        Age = client.Age,
                        Allergies = client.Allergies,
                        PassWord = SecurePasswordHasherHelper.Hash(client.PassWord)
                    };
                    _cynsDbContext.Clients.Add(newClient);
                    _cynsDbContext.SaveChanges();
                    return Ok(HttpReturnValue.ClientAccountCreated);
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        [HttpDelete(ReqParms.Action)]

        public IActionResult ClientDeleteAccount([FromBody]Client client)
        {
            try
            {
                var currClient = _cynsDbContext.Clients.Where(c => c.Email == client.Email).FirstOrDefault();
                if(currClient == null)
                {
                    return NotFound(HttpReturnValue.ClientNotFound);
                }else
                {
                    if(SecurePasswordHasherHelper.Verify(client.PassWord,currClient.PassWord))
                    {
                        _cynsDbContext.Clients.Remove(currClient);
                        _cynsDbContext.SaveChanges();
                        return Ok(HttpReturnValue.ClientDeleted);
                    }return (BadRequest(HttpReturnValue.ErrorMessage));
                }
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPut(ReqParms.Action)]
        public IActionResult ClientEditProfile([FromBody]Client client)
        {
            try
            {
                var clientProfile = _cynsDbContext.Clients.Where(e => e.Email == client.Email).SingleOrDefault();
                if(client == null)
                {
                    return NotFound(HttpReturnValue.ClientNotFound);
                }else
                {
                    if(SecurePasswordHasherHelper.Verify(client.PassWord, clientProfile.PassWord))
                    {
                        clientProfile.Allergies = client.Allergies;
                        _cynsDbContext.SaveChanges();
                        return Ok(HttpReturnValue.ClientEditedProfile);
                    }return Unauthorized(HttpReturnValue.IncorrectPassword);
                }
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(ReqParms.Action)]
        public IActionResult Login([FromBody]Client client)
        {
            try
            {
                var doesClientExist = _cynsDbContext.Clients.Where(e => e.Email == client.Email).FirstOrDefault();
                if(doesClientExist == null)
                {
                    return NotFound(HttpReturnValue.ClientNotFound);
                }else
                {
                    if (!SecurePasswordHasherHelper.Verify(client.PassWord, doesClientExist.PassWord))
                    {
                        return Unauthorized(HttpReturnValue.ErrorMessage);
                    }
                    else
                    {
                        var claims = new[]
                            {
                                new Claim(JwtRegisteredClaimNames.Email, client.Email),
                                new Claim(ClaimTypes.Email, client.Email),
                                new Claim(ClaimTypes.Role, AccountType.Client)
                            };
                        var token = _auth.GenerateAccessToken(claims);
                        return new ObjectResult(new
                        {
                            access_token = token.AccessToken,
                            expires_in = token.ExpiresIn,
                            token_type = token.TokenType,
                            creation_Time = token.ValidFrom,
                            expiration_Time = token.ValidTo,
                            account_type = AccountType.Client
                        });
                    }
                    
                }
            }catch(Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpPost(ReqParms.ActionRequest)]
        [Authorize(Roles = AccountType.Client)]
        public IActionResult ClientRequest(string request)
        {
            var clientRequest = new ClientRequest()
            {
                Request = request
            };
            _cynsDbContext.ClientAdminRequest.Add(clientRequest);
            _cynsDbContext.SaveChanges();
            return Ok("Successfully added request.");
        }

        
    }
}
