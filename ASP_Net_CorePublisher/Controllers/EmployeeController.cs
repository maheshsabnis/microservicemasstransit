using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Application.Model;
using MassTransit;

namespace ASP_Net_CorePublisher.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {

         
        private readonly IBus bus;
        public EmployeeController(IBus bus)
        {
         
            this.bus = bus;            
        }
        [HttpPost]
        public async Task<IActionResult> Post(Employee emp)
        {
            var endPoint = await bus.GetSendEndpoint(
                new Uri("rabbitmq://localhost/employeequeue")
                );
            await endPoint.Send(emp);
            return Ok("Message is added in Queue");
        }

    }
}