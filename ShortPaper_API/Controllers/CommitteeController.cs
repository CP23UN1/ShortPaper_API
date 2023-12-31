﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.Services.Committees;

namespace ShortPaper_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommitteeController : ControllerBase
    {
        private readonly ICommitteeService _committeeService;

        public CommitteeController(ICommitteeService committeeService)
        {
            _committeeService = committeeService;
        }

        [HttpGet("committees")]
        public async Task<IActionResult> GetCommittees()
        {
            var result = await _committeeService.GetCommitteesAsync();

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }

        [HttpPost]
        [Route("add-from-csv")]
        public async Task<IActionResult> AddCommitteesFromCsv(IFormFile csvFile)
        {
            var result = await _committeeService.AddCommitteesFromCsvAsync(csvFile);

            if (result.IsSuccess)
            {
                return Ok(result.Data);
            }

            return BadRequest(result.ErrorMessage);
        }
    }
}
