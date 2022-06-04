using Cw5.Models;
using Cw5.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cw5.Controllers
{
    [Route("/api/trips")]
    [ApiController]
        public class TripController : ControllerBase
        {
            private readonly IDbService _dbService;
            public TripController(IDbService dbService)
            {
                _dbService = dbService;
            }

            [HttpGet]
            public async Task<IActionResult> GetTrips()
            {
                var tripList = await _dbService.GetTripList();
                return Ok(tripList);
            }

            [HttpDelete]
            [Route("/api/clients/{idClient}")]
            public async Task<IActionResult> RemoveClient(int idClient)
            {
            await _dbService.RemoveClient(idClient);
            if (_dbService.RemoveClient(idClient).Result == 1)
            {
                return BadRequest("Unable to delete clident");

            }
                     return Ok("Client has been removed"); 
            }
            [HttpPost]
            [Route("/api/trips/{idTrip}/clients")]

            public async Task<IActionResult> AddClienttoTrip(ClientTrips clientTrip)
            {
            await _dbService.AddClienttoTrip(clientTrip);
            if (_dbService.AddClienttoTrip(clientTrip).Result == 0)
            {
                return BadRequest("Could not find this trip");
            }
            else if (_dbService.AddClienttoTrip(clientTrip).Result == 1) 
            {
                return BadRequest("Client already added to this trip");
            }
            return Ok("Successfully assigned client to this trip");
            }
        }
    }

