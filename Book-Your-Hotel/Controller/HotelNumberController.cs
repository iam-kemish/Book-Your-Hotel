using System.Net;
using AutoMapper;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Book_Your_Hotel.Controller
{
    [Route("api/HotelNumbers")]
   
    [ApiController]
    
   
    public class HotelNumberController : ControllerBase
    {
        private readonly IHotelNoRepo _IHotelNo;
        private readonly ILogger<HotelController> _logger;
        private readonly APIResponse response;
        private readonly IMapper _IMapper;
        private readonly IHotelRepo _IHotel;

        public HotelNumberController(IHotelNoRepo hotelNoRepo, ILogger<HotelController> logger, IMapper mapper, IHotelRepo iHotel)
        {
            _IHotelNo = hotelNoRepo;
            _logger = logger;
            response = new APIResponse();
            _IMapper = mapper;
            _IHotel = iHotel;
        }

        [HttpGet]
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetHotelNumbers()
        {
            try
            {
                _logger.LogInformation("Getting all the hotel number lists.");
                IEnumerable<HotelNumbers> hotelNumbers = await _IHotelNo.GetAllAsync(includeProperties: "Hotels");
                response.Result = _IMapper.Map<List<HotelNoDTO>>(hotelNumbers);
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
        [ResponseCache(Duration = 30)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> GetHotelNumber(int id)
        {
            if (id == 0)
            {
                response.HttpStatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Hotel Number not found." };
                return NotFound(response);
            }

            var hotelNo = await _IHotelNo.GetAsync(u => u.HotelNumber == id, includeProperties: "Hotels");
            if (hotelNo == null)
            {
                response.HttpStatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Invalid id" };
                return NotFound(response);
            }

            response.Result = _IMapper.Map<HotelNoDTO>(hotelNo);
            response.HttpStatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            return Ok(response);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> CreateHotelNumber([FromBody] HotelNoCreateDTO newHotelNumber)
        {
            try
            {
                if (newHotelNumber == null)
                {
                    response.HttpStatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    response.Errors = new List<string> { "Invalid inserted data." };
                    return BadRequest(response);
                }

                if (await _IHotelNo.GetAsync(u => u.HotelNumber == newHotelNumber.HotelNumber) != null)
                {
                    ModelState.AddModelError("Errors", "Hotel data already exists.");
                    response.HttpStatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    response.Errors = ModelState.Values.SelectMany(v => v.Errors)
                                                       .Select(e => e.ErrorMessage)
                                                       .ToList();
                    return BadRequest(response);
                }

                if (await _IHotel.GetAsync(u => u.Id == newHotelNumber.HotelID) == null)
                {
                    ModelState.AddModelError("Errors", "Hotel id is invalid");
                    response.HttpStatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    response.Errors = ModelState.Values.SelectMany(v => v.Errors)
                                                       .Select(e => e.ErrorMessage)
                                                       .ToList();
                    return BadRequest(response);
                }

                HotelNumbers hotelNumbers = _IMapper.Map<HotelNumbers>(newHotelNumber);
                await _IHotelNo.CreateAsync(hotelNumbers);

                response.Result = _IMapper.Map<HotelNoDTO>(hotelNumbers);
                response.HttpStatusCode = HttpStatusCode.Created;
                response.IsSuccess = true;

                return CreatedAtAction(nameof(GetHotelNumber), new { id = hotelNumbers.HotelNumber }, response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string>() { ex.ToString() };
            }

            return response;
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}", Name = "UpdateHotelNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateHotelNumber(int id, [FromBody] HotelNoUpdateDTO toUpdateNoDTO)
        {
            try
            {
                if (id != toUpdateNoDTO.HotelNumber || toUpdateNoDTO == null)
                {
                    response.HttpStatusCode = HttpStatusCode.BadRequest;
                    response.IsSuccess = false;
                    response.Errors = new List<string> { "Invalid hotel Number ID or update data" };
                    return BadRequest(response);
                }

                if (await _IHotel.GetAsync(u => u.Id == toUpdateNoDTO.HotelID) == null)
                {
                    ModelState.AddModelError("ErrorMessages", "Hotel ID is Invalid!");
                    return BadRequest(ModelState);
                }

                var model = _IMapper.Map<HotelNumbers>(toUpdateNoDTO);
                await _IHotelNo.UpdateAsync(model);
                response.IsSuccess = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Errors = new List<string>() { ex.ToString() };
            }

            return response;
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> DeleteHotelNumber(int id)
        {
            if (id == 0)
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Invalid hotel ID" };
                return BadRequest(response);
            }

            var HotelNo = await _IHotelNo.GetAsync(u => u.HotelNumber == id);
            if (HotelNo == null)
            {
                response.HttpStatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Hotel Number not found" };
                return NotFound(response);
            }

            await _IHotelNo.RemoveAsync(HotelNo);
            response.HttpStatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;

            return Ok(response);
        }
    }
}
