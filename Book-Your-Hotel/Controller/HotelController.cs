using Asp.Versioning;
using AutoMapper;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Book_Your_Hotel.Controller
{
    [Route("api/HotelLists")]

    [ApiController]
    
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
        public async Task<ActionResult<APIResponse>> GetAllHotels([FromQuery] int? occupancy, int PageSize = 0, int PageNumber = 1)
        {

            try
            {
                _logger.LogInformation("Getting all the hotels list");
                List<Hotels> hotelLists;
                // = await _IHotel.GetAllAsync();
                if (occupancy > 0 && occupancy < 5) { 
                
                hotelLists = await _IHotel.GetAllAsync(u=>u.AvailableRooms > 1 && u.AvailableRooms < 5, PageSize: PageSize, PageNumber: PageNumber );
                }else if(occupancy > 5 && occupancy < 10)
                {
                    hotelLists = await _IHotel.GetAllAsync(u => u.AvailableRooms > 3 && u.AvailableRooms < 10, PageSize: PageSize, PageNumber: PageNumber);
                }
                else if(occupancy > 10)
                {
                    hotelLists = await _IHotel.GetAllAsync(u=> u.AvailableRooms > 5, PageSize: PageSize, PageNumber: PageNumber);
                }else
                {
                    hotelLists = await _IHotel.GetAllAsync( PageSize: PageSize, PageNumber: PageNumber);
                }

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
        public async Task<ActionResult<APIResponse>> CreateHotel([FromForm] HotelCreateDTO newHotel)
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

            if (newHotel.Image != null)
            {
                // Generate a unique file name using hotel ID and original file extension
                string fileName = hotels.Id + Path.GetExtension(newHotel.Image.FileName);

                // Define the relative path to save the image in the wwwroot folder
                string filePath = @"wwwroot\ResultedImages\" + fileName;

                // Combine it with the current directory to get the full physical path
                var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

                // Check if file already exists, if so, delete it
                FileInfo fileInfo = new FileInfo(directoryLocation);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                // Save the uploaded file to the server
                using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                {
                    newHotel.Image.CopyTo(fileStream);
                }

                // Generate the public URL of the image
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";

                // Save image URL and local path to the hotel record
                hotels.ImageUrl = baseUrl + "/ResultedImages/" + fileName;
                hotels.ImageLocalPath = filePath;

                // Update the hotel record with image info
                await _IHotel.UpdateAsync(hotels);
            }
            response.Result = _IMapper.Map<HotelsDTO>(hotels);
            response.HttpStatusCode = HttpStatusCode.Created;
            response.IsSuccess = true;

            return CreatedAtAction(nameof(GetHotel), new { id = hotels.Id }, response);
        }
        [Authorize(Roles = "admin")]
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
            if (!string.IsNullOrEmpty(hotel.ImageLocalPath))
            {
              
                var oldImageDirectory = Path.Combine(Directory.GetCurrentDirectory(), hotel.ImageLocalPath);
                // Check if file already exists, if so, delete it
                FileInfo fileInfo = new FileInfo(oldImageDirectory);
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }
            }
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
        [Authorize(Roles = "admin")]
        [HttpPut("{id:int}", Name = "UpdateHotel")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<APIResponse>> UpdateHotel(int id, [FromForm] HotelUpdateDTO toUpdateDTO)
        {

            if (id == 0 || toUpdateDTO.Id == 0)
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Invalid hotel ID or update data" };
                return BadRequest(response);
            }

          Hotels hotels=   _IMapper.Map<Hotels>(toUpdateDTO);
            await _IHotel.UpdateAsync(hotels);
            if (toUpdateDTO.Image != null)
            {
                if (!string.IsNullOrEmpty(toUpdateDTO.ImageLocalPath))
                {
                    //Since its update we need to retrieve already existing imageurlpath in DB.
                    var oldImageDirectory = Path.Combine(Directory.GetCurrentDirectory(), hotels.ImageLocalPath);
                    // Check if file already exists, if so, delete it
                    FileInfo fileInfo = new FileInfo(oldImageDirectory);
                    if (fileInfo.Exists)
                    {
                        fileInfo.Delete();
                    }
                }
                // Generate a unique file name using hotel ID and original file extension
                string fileName = toUpdateDTO.Id + Path.GetExtension(toUpdateDTO.Image.FileName);

                // Define the relative path to save the image in the wwwroot folder
                string filePath = @"wwwroot\ResultedImages\" + fileName;

                // Combine it with the current directory to get the full physical path
                var directoryLocation = Path.Combine(Directory.GetCurrentDirectory(), filePath);

               

                // Save the uploaded file to the server
                using (var fileStream = new FileStream(directoryLocation, FileMode.Create))
                {
                    toUpdateDTO.Image.CopyTo(fileStream);
                }

                // Generate the public URL of the image
                var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";

                // Save image URL and local path to the hotel record
                hotels.ImageUrl = baseUrl + "/ResultedImages/" + fileName;
                hotels.ImageLocalPath = filePath;

                // Update the hotel record with image info
                await _IHotel.UpdateAsync(hotels);
            }
            response.HttpStatusCode = HttpStatusCode.NoContent;
            response.IsSuccess = true;

            return Ok(response);
        }
    }
}
