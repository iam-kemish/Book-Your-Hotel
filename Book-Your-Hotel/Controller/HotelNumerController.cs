﻿using System.Net;
using AutoMapper;
using Book_Your_Hotel.Models;
using Book_Your_Hotel.Models.DTOs;
using Book_Your_Hotel.Repositary.IRepositary;
using Microsoft.AspNetCore.Mvc;

namespace Book_Your_Hotel.Controller
{
    [Route("api/HotelNumbers")]
    [ApiController]
    public class HotelNumerController : ControllerBase
    {
        private readonly IHotelNoRepo _IHotelNo;
        private readonly ILogger<HotelController> _logger;
        private readonly APIResponse response;
        private readonly IMapper _IMapper;

        public HotelNumerController(IHotelNoRepo hotelNoRepo, ILogger<HotelController> logger, IMapper mapper)
        {
            _IHotelNo = hotelNoRepo;
            _logger = logger;
            response = new APIResponse();
            _IMapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<ActionResult<APIResponse>> GetHotelNumbers()
        {
            try
            {
                _logger.LogInformation("Getting all the hotel number lists.");
                IEnumerable<HotelNumbers> hotelNumbers = await _IHotelNo.GetAllAsync();
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetHotelNumber(int id)
        {
            if (id == 0)
            {
                response.HttpStatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Hotel Number not found." };
                return NotFound(response);
            }
            var hotelNo = await _IHotelNo.GetAsync(u => u.HotelNumber == id);
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> CreateHotelNumber([FromBody] HotelNoCreateDTO newHotelNumber)
        {
            if (newHotelNumber == null)
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Hotel number data is null" };
                return BadRequest(response);
            }
            HotelNumbers hotelNumbers = _IMapper.Map<HotelNumbers>(newHotelNumber);
            await _IHotelNo.CreateAsync(hotelNumbers);

            response.Result = _IMapper.Map<HotelNoDTO>(hotelNumbers);
            response.HttpStatusCode = HttpStatusCode.Created;
            response.IsSuccess = true;

            return CreatedAtAction(nameof(GetHotelNumber), new { id = hotelNumbers.HotelNumber }, response);

        }

        [HttpPut("{id:int}", Name = "UpdateHotelNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> UpdateHotelNumber(int id, [FromBody] HotelNoUpdateDTO toUpdateNoDTO)
        {
            if (id == 0 || toUpdateNoDTO.HotelNumber == 0)
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Invalid hotel Number ID or update data" };
                return BadRequest(response);
            }
            var existingHotelNo = await _IHotelNo.GetAsync(u => u.HotelNumber == id, tracked: true);
            if (existingHotelNo == null)
            {
                response.HttpStatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Hotel number not found for update" };
                return NotFound(response);
            }
            _IMapper.Map(toUpdateNoDTO, existingHotelNo);
            await _IHotelNo.UpdateAsync(existingHotelNo);
            response.IsSuccess = true;
            return Ok(response);
        }

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> DeleteHotelNumber(int id)
        {
            if (id == 0)
            {
                response.HttpStatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.Errors = new List<string> { "Invalid hotel ID" };
                return BadRequest(response);
            }
            var HotelNo = await _IHotelNo.GetAsync(u=>u.HotelNumber == id);
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
