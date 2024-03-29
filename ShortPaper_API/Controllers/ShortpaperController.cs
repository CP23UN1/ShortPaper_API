﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShortPaper_API.DTO;
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;
using ShortPaper_API.Services.Shortpapers;
using ShortPaper_API.Services.Students;

namespace ShortPaper_API.Controllers
{
    [Route("api")]
    [ApiController]
    public class ShortpaperController : ControllerBase
    {
        private readonly ShortpaperDbContext _dbContext;
        private readonly IShortpaperService _shortpaperService;

        public ShortpaperController(ShortpaperDbContext dbContext, IShortpaperService shortpaperService)

        {
            _dbContext = dbContext;
            _shortpaperService = shortpaperService;
        }

        [HttpGet]
        [Route("shortpapers")]
        public ServiceResponse<List<ShortpaperDTO>> GetShortpaper()
        {
            var shortpapers = _shortpaperService.GetShortpaper();
            return shortpapers;
        }

        [HttpGet]
        [Route("shortpaper/search-by-filter/{filterText}")]
        public ServiceResponse<List<ShortpaperDTO>> GetStudentByFilter(string filterText)
        {
            var shortpaper = _shortpaperService.GetShortpaperByFilter(filterText);
            return shortpaper;
        }

        [HttpGet]
        [Route("shortpaper/search-by-student/{studentId}")]
        public ServiceResponse<ShortpaperDTO> GetStudentByStudent(string studentId)
        {
            var shortpaper = _shortpaperService.GetShortpaperByStudent(studentId);
            return shortpaper;
        }

        [HttpPost]
        [Route("shortpaper/create")]
        public ServiceResponse<AddShortpaperDTO> AddShortpaper(AddShortpaperDTO shortpaperDTO)
        {
            var status = _shortpaperService.AddShortpaper(shortpaperDTO);
            return status;
        }

        [HttpPatch]
        [Route("shortpaper/update/{id}")]
        public ServiceResponse<UpdateShortpaperDTO> UpdateShortpaper(UpdateShortpaperDTO shortpaperDTO)
        {
            var status = _shortpaperService.UpdateShortpaper(shortpaperDTO);
            return status;
        }
    }
}
