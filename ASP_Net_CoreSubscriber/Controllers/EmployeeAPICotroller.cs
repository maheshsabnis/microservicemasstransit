using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ASP_Net_CoreSubscriber.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ASP_Net_CoreSubscriber.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeAPIController : ControllerBase
    {
        private readonly mydbContext context;

        public EmployeeAPIController(mydbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var result = context.Employee.ToList();
            return Ok(result);
        }
    }
}