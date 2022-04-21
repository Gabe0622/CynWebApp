using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CynthiasWebApp.Data;
using CynthiasWebApp.Support;

namespace CynthiasWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadPhotoController : ControllerBase
    {
        private CynsDbContext _cynsDbContext;
        public UploadPhotoController(CynsDbContext cynsDbContext)
        {
            _cynsDbContext = cynsDbContext;
        }
    }
}
