using ComputeShopApi.Models;
using ComputeShopApi.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace ComputeShopApi.Controllers
{
    [Route("osystems")]
    [ApiController]
    public class SystemController : ControllerBase
    {
        public ComputerShopDbContext _context = new();
        [HttpPost]
        public object AddNewOsystem([FromBody]AddNewOsystemDto addNewOsystemDto)
        {
            var osystem = new Osystem
            {
                Id = Guid.NewGuid(),
                Name = addNewOsystemDto.Name,
                Version = addNewOsystemDto.Version,
                RegisterTime = DateTime.Now,
                UpdateTime = DateTime.Now
            };

            if (osystem != null)
            {
                _context.Osystems.Add(osystem);
                _context.SaveChanges();

                return StatusCode(201, new { message = "Sikeres feltöltés.", result = addNewOsystemDto });
            }

            return StatusCode(404, new { message = "Sekertelen feltöltés.", result = addNewOsystemDto });
        }

        [HttpGet]
        public object GetAllOsystem()
        {
            var result = _context.Osystems.ToList();

            if (result.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres letöltés.", result = result });
            }

            return StatusCode(400, new { message = "Sekertelen letöltés.", result = "" });
        }

        [HttpGet("{id}")]
        public object GetOsystemById(Guid id)
        {
            var result = _context.Osystems.FirstOrDefault(o => o.Id == id);

            if (result != null)
            {
                return StatusCode(200, new { message = "Sikeres letöltés.", result = result });
            }

            return StatusCode(404, new { message = "Nincs ilyen operációs rendszer.", result = "" });
        }

        [HttpDelete]
        public object DeleteOsystem([FromQuery]Guid id)
        {
            var osystem = _context.Osystems.FirstOrDefault(o => o.Id == id);

            if (osystem != null)
            {
                _context.Osystems.Remove(osystem);
                _context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres törlés.", result = osystem });
            }

            return StatusCode(404, new { message = "Sikertelen törlés.", result = "" });
        }

        [HttpPut]
        public object UpdateOsystem([FromQuery]Guid id, UpdateOsystemDto updateOsystemDto)
        {
            var osystem = _context.Osystems.Find(id);

            if (osystem != null)
            {
                osystem.Name = updateOsystemDto.Name;
                osystem.Version = updateOsystemDto.Version;
                osystem.UpdateTime = DateTime.Now;

                _context.Osystems.Update(osystem);
                _context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres frissítés." });
            }

            return StatusCode(404, new { message = "Sikertelen frissítés." });
        }
    }
}
