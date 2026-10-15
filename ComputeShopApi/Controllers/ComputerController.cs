using ComputeShopApi.Models;
using ComputeShopApi.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ComputeShopApi.Controllers
{
    [Route("computers")]
    [ApiController]
    public class ComputerController : ControllerBase
    {
        public ComputerShopDbContext _context = new();

        [HttpGet]
        public object GetAllComputer()
        {
            var result = _context.Computers
                .Select(c => new
                {
                    c.Id,
                    c.Brand,
                    c.Type,
                    c.Display,
                    c.Memory,
                    c.CreatedTime,
                    c.OsId,
                    OsName = c.Os.Name,
                    OsVersion = c.Os.Version
                })
                .ToList();

            if (result.Count > 0)
            {
                return StatusCode(200, new { message = "Sikeres letöltés.", result = result });
            }

            return StatusCode(400, new { message = "Sikertelen letöltés.", result = "" });
        }

        [HttpGet("{id}")]
        public object GetComputerById(Guid id)
        {
            var result = _context.Computers
                .Where(c => c.Id == id)
                .Select(c => new
                {
                    c.Id,
                    c.Brand,
                    c.Type,
                    c.Display,
                    c.Memory,
                    c.CreatedTime,
                    c.OsId,
                    OsName = c.Os.Name,
                    OsVersion = c.Os.Version
                })
                .FirstOrDefault();

            if (result != null)
            {
                return StatusCode(200, new { message = "Sikeres letöltés.", result = result });
            }

            return StatusCode(404, new { message = "Nincs ilyen számítógép.", result = "" });
        }

        [HttpPost]
        public object AddNewComputer([FromBody] AddNewComputerDto addNewComputerDto)
        {
            var osystem = _context.Osystems.Find(addNewComputerDto.OsId);

            if (osystem == null)
            {
                return StatusCode(404, new { message = "Nincs ilyen operációs rendszer.", result = addNewComputerDto });
            }

            var computer = new Computer
            {
                Id = Guid.NewGuid(),
                Brand = addNewComputerDto.Brand,
                Type = addNewComputerDto.Type,
                Display = addNewComputerDto.Display,
                Memory = addNewComputerDto.Memory,
                CreatedTime = DateTime.Now,
                OsId = addNewComputerDto.OsId
            };

            _context.Computers.Add(computer);
            _context.SaveChanges();

            return StatusCode(201, new { message = "Sikeres feltöltés.", result = addNewComputerDto });
        }

        [HttpPut]
        public object UpdateComputer([FromQuery] Guid id, UpdateComputerDto updateComputerDto)
        {
            var computer = _context.Computers.Find(id);

            if (computer == null)
            {
                return StatusCode(404, new { message = "Sikertelen frissítés." });
            }

            var osystem = _context.Osystems.Find(updateComputerDto.OsId);

            if (osystem == null)
            {
                return StatusCode(404, new { message = "Nincs ilyen operációs rendszer." });
            }

            computer.Brand = updateComputerDto.Brand;
            computer.Type = updateComputerDto.Type;
            computer.Display = updateComputerDto.Display;
            computer.Memory = updateComputerDto.Memory;
            computer.OsId = updateComputerDto.OsId;

            _context.Computers.Update(computer);
            _context.SaveChanges();

            return StatusCode(200, new { message = "Sikeres frissítés." });
        }

        [HttpDelete]
        public object DeleteComputer([FromQuery] Guid id)
        {
            var computer = _context.Computers.Find(id);

            if (computer != null)
            {
                _context.Computers.Remove(computer);
                _context.SaveChanges();

                return StatusCode(200, new { message = "Sikeres törlés.", result = id });
            }

            return StatusCode(404, new { message = "Sikertelen törlés.", result = "" });
        }
    }
}
