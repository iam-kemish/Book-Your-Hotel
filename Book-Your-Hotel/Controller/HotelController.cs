﻿using AutoMapper;
using Book_Your_Hotel.Database;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Book_Your_Hotel.Controller
{
    [Route("api/HotelLists")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly IHotelRepo _IHotel;
        private readonly IMapper _IMapper;

        public HotelController(ILogger<HotelController> logger, ApplicationDbContext applicationDbContext, IMapper mapper, IHotelRepo hotelRepo)
        {
            _logger = logger;
            _IHotel = hotelRepo;
            _IMapper = mapper;  
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task <ActionResult<IEnumerable<HotelsDTO>>> GetAllHotels()
        {
            _logger.LogInformation("Getting all the hotels list");
           IEnumerable<Hotels> hotelLists = await _IHotel.GetAllAsync();
            return Ok(_IMapper.Map<List<HotelsDTO>>(hotelLists));
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<HotelsDTO>> GetHotel(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Bad request: Hotel id cannot be zero.");
                return BadRequest();
            }

            var hotel = await _IHotel.GetAsync(u => u.Id == id);
            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with id: {id} not found.");
                return NotFound();
            }

            _logger.LogInformation($"Hotel with id: {id} found: {hotel.Name}");
            return Ok(_IMapper.Map<HotelsDTO>(hotel));
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async  Task<ActionResult<HotelsDTO>> CreateHotel([FromBody] HotelCreateDTO newHotel)
        {
            if (newHotel == null)
            {
                _logger.LogError("Bad request: Hotel data is null.");
                return BadRequest("Hotel data is null");
            }

            if (await _IHotel.GetAsync(u => u.Name.ToLower() == newHotel.Name.ToLower()) != null)
            {
                _logger.LogError($"Hotel creation failed: Hotel with name '{newHotel.Name}' already exists.");
                ModelState.AddModelError("Custom", "Model already exists.");
                return BadRequest(ModelState);
            }
            //Because of automapper we can simply convert to another object like this.
            Hotels hotels = _IMapper.Map<Hotels>(newHotel);
            //Hotels hotels = new()
            //{
            //    Name = newHotel.Name,
            //    Location = newHotel.Location,
            //    ImageUrl = newHotel.ImageUrl,
            //    NumberOfRooms = newHotel.NumberOfRooms,
            //    Price = newHotel.Price,
            //    ContactNumber = newHotel.ContactNumber
            //};
           await  _IHotel.CreateAsync(hotels);
         
            _logger.LogInformation($"Hotel '{newHotel.Name}' created successfully with id: {hotels.Id}");

            return CreatedAtAction(nameof(GetHotel), new { id = hotels.Id }, hotels);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Bad request: Hotel id cannot be zero.");
                return BadRequest();
            }

            var hotel = await _IHotel.GetAsync(u => u.Id == id);
            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with id: {id} not found.");
                return NotFound();
            }

           await _IHotel.RemoveAsync(hotel);
          
            _logger.LogInformation($"Hotel with id: {id} deleted successfully.");

            return NoContent();
        }

        [HttpPut("{id:int}", Name = "UpdateHotel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelUpdateDTO toUpdateDTO)
        {
            if (id == 0 || toUpdateDTO.Id == 0)
            {
                _logger.LogError("Bad request: Hotel id or update data cannot be zero.");
                return BadRequest();
            }
            var existingHotel = await _IHotel.GetAsync(u => u.Id == id, tracked: true);

            if (existingHotel == null)
            {
                _logger.LogWarning($"Hotel with id: {id} not found for update.");
                return NotFound();
            }
          _IMapper.Map(toUpdateDTO,existingHotel);
          
            await _IHotel.UpdateAsync(existingHotel);

            _logger.LogInformation($"Hotel with id: {id} updated successfully. New Name: {toUpdateDTO.Name}");

            return NoContent();
        }
    }
}
