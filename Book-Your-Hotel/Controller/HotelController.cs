using Asp.Versioning;
using AutoMapper;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Book_Your_Hotel.Controller
{

    [Route("api/v2/HotelLists")]
    //[Route("api/v{version:apiVersion}/VillaAPI")]
    //[Route("api/HotelLists")]
    [ApiController]
    [ApiVersion(1)]
    [ApiVersion(2)]
    //[ApiVersion("1.0")]
    //[ApiVersion("2.0")]
    public class HotelController : ControllerBase
    {
        private readonly ILogger<HotelController> _logger;
        private readonly IHotelRepo _IHotel;
        private readonly IMapper _IMapper;
        private readonly APIResponse response;
        public HotelController(ILogger<HotelController> logger, IMapper mapper, IHotelRepo hotelRepo)
        {
            _logger = logger;
            _IHotel = hotelRepo;
            _IMapper = mapper;
            response = new APIResponse();
        }

        [HttpGet]

        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> GetAllHotels()
        {

            try
            {
                _logger.LogInformation("Getting all the hotels list");
                IEnumerable<Hotels> hotelLists = await _IHotel.GetAllAsync();
                response.Result = _IMapper.Map<List<HotelsDTO>>(hotelLists);
                response.HttpStatusCode = HttpStatusCode.OK;
                response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                response.HttpStatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.Errors = new List<string> { ex.Message };
            }
            return Ok(response);
        }

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetHotel(int id)
        {

            if (id == 0)
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Invalid hotel ID" };
                return BadRequest(response);
            }

            var hotel = await _IHotel.GetAsync(u => u.Id == id);
            if (hotel == null)
            {
                response.HttpStatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Hotel not found" };
                return NotFound(response);
            }

            response.Result = _IMapper.Map<HotelsDTO>(hotel);
            response.HttpStatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            return Ok(response);
        }
        [Authorize]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateHotel([FromBody] HotelCreateDTO newHotel)
        {

            if (newHotel == null)
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Hotel data is null" };
                return BadRequest(response);
            }

            if (await _IHotel.GetAsync(u => u.Name.ToLower() == newHotel.Name.ToLower()) != null)
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Hotel with this name already exists" };
                return BadRequest(response);
            }

            Hotels hotels = _IMapper.Map<Hotels>(newHotel);
            await _IHotel.CreateAsync(hotels);

            response.Result = _IMapper.Map<HotelsDTO>(hotels);
            response.HttpStatusCode = HttpStatusCode.Created;
            response.IsSuccess = true;

            return CreatedAtAction(nameof(GetHotel), new { id = hotels.Id }, response);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteHotel(int id)
        {

            if (id == 0)
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Invalid hotel ID" };
                return BadRequest(response);
            }

            var hotel = await _IHotel.GetAsync(u => u.Id == id);
            if (hotel == null)
            {
                response.HttpStatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Hotel not found" };
                return NotFound(response);
            }

            await _IHotel.RemoveAsync(hotel);
            response.HttpStatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;

            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}", Name = "UpdateHotel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateHotel(int id, [FromBody] HotelUpdateDTO toUpdateDTO)
        {

            if (id == 0 || toUpdateDTO.Id == 0)
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Invalid hotel ID or update data" };
                return BadRequest(response);
            }

            var existingHotel = await _IHotel.GetAsync(u => u.Id == id, tracked: true);
            if (existingHotel == null)
            {
                response.HttpStatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Hotel not found for update" };
                return NotFound(response);
            }

            _IMapper.Map(toUpdateDTO, existingHotel);
            await _IHotel.UpdateAsync(existingHotel);

            response.HttpStatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;

            return Ok(response);
        }
    }
}
