using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}", Name = "GetById")]
        public IActionResult GetById(int Id)
        {
            var celestialObject = _context.CelestialObjects.Find(Id);
            if (celestialObject == null) return NotFound();

            celestialObject.Satellites = _context.CelestialObjects.Where(obj => obj.OrbitedObjectId == celestialObject.Id).ToList();

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string Name)
        {
            var celestialObjects = _context.CelestialObjects.Where(obj => obj.Name == Name).ToList();
            if (!celestialObjects.Any()) return NotFound();

            celestialObjects.ForEach(obj => obj.Satellites = 
                _context.CelestialObjects.Where(
                    sat => sat.OrbitedObjectId == obj.Id).ToList());

            return Ok(celestialObjects);
        }

        [HttpGet()]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects.ToList();
            if (!celestialObjects.Any()) return NotFound();

            celestialObjects.ForEach(obj => obj.Satellites =
                _context.CelestialObjects.Where(
                    sat => sat.OrbitedObjectId == obj.Id).ToList());

            return Ok(celestialObjects);
        }
    }
}
