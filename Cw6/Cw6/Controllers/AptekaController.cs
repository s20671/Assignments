using Cw6.Models;
using Cw6.Models.DTO;
using Cw6.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Cw6.Controllers
{
    [Route("/api/apteka")]
    [ApiController]
        public class AptekaController : ControllerBase
        {
            private readonly IDbService _dbService;

            public AptekaController(IDbService dbService)
            {
                _dbService = dbService;
            }

            [HttpGet]
            public async Task<IActionResult> GetDoctors()
            {
                var doctors = await _dbService.GetDoctors();
            return Ok(doctors);
            }

            [HttpGet]
            [Route("/api/apteka/{idPresciption}")]
            public async Task<IActionResult> GetPrescriptions(int idPresciption)
            {
                var prescription = await _dbService.GetPrescriptions(idPresciption);
                return Ok(prescription);
            }

            [HttpDelete]
            [Route("/api/apteka/{idDoctora}")]
            public async Task<IActionResult> RemoveDoctor(int idDoctora)
            {
            var result = _dbService.RemoveDoctor(idDoctora).Result;
            switch (result)
            {
                case 1:
                    return BadRequest("Doctor not found");
                case 2:
                    return BadRequest("Unable to delete this doctor");
            }
            return Ok("Successfully removed this doctor");

            }
            [HttpPost]
            [Route("/api/apteka")]
            public async Task<IActionResult> AddDoctor(Doctor doctor)
            {
            var result = _dbService.AddDoctor(doctor).Result;
            return result == false ? BadRequest("Unable to add this doctor") : Ok("Doctor has been added successfully");
        }
        [HttpPatch]
            [Route("/api/apteka/{idDoctora}")]

            public async Task<IActionResult> ModifyDoctor(DoctorGetter doctorGetter, int idDoctora)
            {
            var result =  _dbService.ModifyDoctor(doctorGetter, idDoctora).Result;
            return result == false ? BadRequest("Unable to modify this doctor") : (IActionResult) Ok("Doctor has been modified successfully");
        }
    }
    }

