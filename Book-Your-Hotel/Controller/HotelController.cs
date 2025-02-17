using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Book_Your_Hotel.Controller
{
    [Route("api/HotelLists")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;

        public HotelController(ILogger<HotelController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<HotelsDTO> GetAllHotels()
        {
            _logger.LogInformation("Getting all the hotels list");
            return HotelStore.HotelsStores;
        }

        [HttpGet("{id:int}")]
        public ActionResult<HotelsDTO> GetHotel(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Bad request: Hotel id cannot be zero.");
                return BadRequest();
            }

            var hotel = HotelStore.HotelsStores.FirstOrDefault(u => u.Id == id);
            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with id: {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Hotel with id: {id} found: {hotel.Name}");
            return hotel;
        }

        [HttpPost]
        public ActionResult<HotelsDTO> CreateHotel([FromBody] HotelsDTO newHotel)
        {
            if (newHotel == null)
            {
                _logger.LogError("Bad request: Hotel data is null.");
                return BadRequest("Hotel data is null");
            }

            if (HotelStore.HotelsStores.FirstOrDefault(u => u.Name.ToLower() == newHotel.Name.ToLower()) != null)
            {
                _logger.LogError($"Hotel creation failed: Hotel with name '{newHotel.Name}' already exists.");
                ModelState.AddModelError("Custom", "Model already exists.");
                return BadRequest(ModelState);
            }

            HotelStore.HotelsStores.Add(newHotel);
            _logger.LogInformation($"Hotel '{newHotel.Name}' created successfully with id: {newHotel.Id}");

            return CreatedAtAction(nameof(GetHotel), new { id = newHotel.Id }, newHotel);
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteHotel(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Bad request: Hotel id cannot be zero.");
                return BadRequest();
            }

            var hotel = HotelStore.HotelsStores.FirstOrDefault(u => u.Id == id);
            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with id: {id} not found.");
                return NotFound();
            }

            HotelStore.HotelsStores.Remove(hotel);
            _logger.LogInformation($"Hotel with id: {id} deleted successfully.");

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateHotel")]
        public IActionResult UpdateHotel(int id, [FromBody] HotelsDTO toUpdateDTO)
        {
            if (id == 0 || toUpdateDTO.Id == 0)
            {
                _logger.LogError("Bad request: Hotel id or update data cannot be zero.");
                return BadRequest();
            }

            var hotel = HotelStore.HotelsStores.FirstOrDefault(u => u.Id == id);
            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with id: {id} not found for update.");
                return NotFound();
            }

            hotel.Name = toUpdateDTO.Name;
            _logger.LogInformation($"Hotel with id: {id} updated successfully. New Name: {toUpdateDTO.Name}");

            return NoContent();
        }
    }
}
