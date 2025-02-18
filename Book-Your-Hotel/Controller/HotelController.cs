using Book_Your_Hotel.Database;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Book_Your_Hotel.Controller
{
    [Route("api/HotelLists")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly ApplicationDbContext _Db;

        public HotelController(ILogger<HotelController> logger, ApplicationDbContext applicationDbContext)
        {
            _logger = logger;
            _Db = applicationDbContext;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<IEnumerable<HotelsDTO>> GetAllHotels()
        {
            _logger.LogInformation("Getting all the hotels list");
            return Ok(_Db.HotelLists.ToList());
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<HotelsDTO> GetHotel(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Bad request: Hotel id cannot be zero.");
                return BadRequest();
            }

            var hotel = _Db.HotelLists.FirstOrDefault(u => u.Id == id);
            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with id: {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Hotel with id: {id} found: {hotel.Name}");
            return Ok(hotel);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<HotelsDTO> CreateHotel([FromBody] HotelsDTO newHotel)
        {
            if (newHotel == null)
            {
                _logger.LogError("Bad request: Hotel data is null.");
                return BadRequest("Hotel data is null");
            }

            if (_Db.HotelLists.FirstOrDefault(u => u.Name.ToLower() == newHotel.Name.ToLower()) != null)
            {
                _logger.LogError($"Hotel creation failed: Hotel with name '{newHotel.Name}' already exists.");
                ModelState.AddModelError("Custom", "Model already exists.");
                return BadRequest(ModelState);
            }
            Hotels hotels = new()
            {
                Name = newHotel.Name,
                Location = newHotel.Location,
                ImageUrl = newHotel.ImageUrl,
                NumberOfRooms = newHotel.NumberOfRooms,
                Price = newHotel.Price,
                ContactNumber = newHotel.ContactNumber
            };
            _Db.HotelLists.Add(hotels);
            _Db.SaveChanges();
            _logger.LogInformation($"Hotel '{newHotel.Name}' created successfully with id: {newHotel.Id}");

            return CreatedAtAction(nameof(GetHotel), new { id = newHotel.Id }, newHotel);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteHotel(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Bad request: Hotel id cannot be zero.");
                return BadRequest();
            }

            var hotel = _Db.HotelLists.FirstOrDefault(u => u.Id == id);
            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with id: {id} not found.");
                return NotFound();
            }

            _Db.HotelLists.Remove(hotel);
            _Db.SaveChanges();
            _logger.LogInformation($"Hotel with id: {id} deleted successfully.");

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateHotel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult UpdateHotel(int id, [FromBody] HotelsDTO toUpdateDTO)
        {
            if (id == 0 || toUpdateDTO.Id == 0)
            {
                _logger.LogError("Bad request: Hotel id or update data cannot be zero.");
                return BadRequest();
            }
            var hotel = _Db.HotelLists.AsNoTracking().FirstOrDefault(u => u.Id == id);

            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with id: {id} not found for update.");
                return NotFound();
            }

            hotel.Name = toUpdateDTO.Name;
            hotel.ContactNumber = toUpdateDTO.ContactNumber;
            hotel.Price = toUpdateDTO.Price;
            hotel.Location = toUpdateDTO.Location;
            hotel.ImageUrl = toUpdateDTO.ImageUrl;
            _Db.HotelLists.Update(hotel);
            _Db.SaveChanges();

            _logger.LogInformation($"Hotel with id: {id} updated successfully. New Name: {toUpdateDTO.Name}");

            return NoContent();
        }
    }
}
