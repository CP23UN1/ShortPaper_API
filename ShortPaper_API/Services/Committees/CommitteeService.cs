using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using ShortPaper_API.DTO; // Import the necessary DTOs and entities
using ShortPaper_API.Entities;
using ShortPaper_API.Helper;

namespace ShortPaper_API.Services.Committees
{
    public class CommitteeService : ICommitteeService
    {
        private readonly ShortpaperDbContext _db;
        private readonly ILogger<CommitteeService> _logger;

        public CommitteeService(ShortpaperDbContext db, ILogger<CommitteeService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<ServiceResponse<List<CommitteeDTO>>> GetCommitteesAsync()
        {
            var response = new ServiceResponse<List<CommitteeDTO>>();

            try
            {
                var committees = await _db.Committees
                    .Select(c => new CommitteeDTO
                    {
                        CommitteeId = c.CommitteeId,
                        Firstname = c.Firstname,
                        Lastname = c.Lastname,
                        Email = c.Email,
                        // Map other properties as needed
                    })
                    .ToListAsync();

                response.IsSuccess = true;
                response.Data = committees;
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "An unexpected error occurred while retrieving committees.";
                response.httpStatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }

        public async Task<ServiceResponse<List<AddCommitteeDTO>>> AddCommitteesFromCsvAsync(IFormFile csvFile)
        {
            var response = new ServiceResponse<List<AddCommitteeDTO>>();

            try
            {
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                {
                    var committeesToAdd = new List<Committee>();
                    bool isFirstRow = true;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            continue; // Skip the first row
                        }

                        var values = line.Split(',');

                        if (values.Length >= 3) // Assuming CSV format: Firstname,Lastname,Email
                        {
                            var committee = new Committee
                            {
                                Firstname = values[0].Trim(),
                                Lastname = values[1].Trim(),
                                Email = values[2].Trim(),
                                // Set other properties as needed
                            };

                            committeesToAdd.Add(committee);
                        }
                    }

                    _db.Committees.AddRange(committeesToAdd);
                    await _db.SaveChangesAsync();

                    // Convert added committees to DTOs for response
                    var addedCommittees = committeesToAdd.Select(c => new AddCommitteeDTO
                    {
                        Firstname = c.Firstname,
                        Lastname = c.Lastname,
                        AlternativeEmail = c.AlternativeEmail,
                        Email = c.Email,
                        Phonenumber = c.Phonenumber
                        // Map other properties as needed
                    }).ToList();

                    response.IsSuccess = true;
                    response.Data = addedCommittees;
                }
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "An unexpected error occurred while adding committees from CSV.";
                response.httpStatusCode = StatusCodes.Status500InternalServerError;
            }

            return response;
        }

        public async Task<ServiceResponse<List<AddCommitteeForStudentDTO>>> AddCommitteesForStudentsFromCsvAsync(IFormFile csvFile)
        {
            var response = new ServiceResponse<List<AddCommitteeForStudentDTO>>();

            try
            {
                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                {
                    var studentCommitteeMappings = new List<(string StudentId, string CommitteeName)>(); // Change CommitteeId to CommitteeName
                    bool isFirstRow = true;

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();

                        if (isFirstRow)
                        {
                            isFirstRow = false;
                            continue; // Skip the first row
                        }

                        var values = line.Split(',');

                        if (values.Length >= 2)
                        {
                            var studentId = values[0].Trim();
                            var committeeName = values[1].Trim(); // Get Committee name instead of ID

                            studentCommitteeMappings.Add((studentId, committeeName));
                        }
                    }

                    foreach (var mapping in studentCommitteeMappings)
                    {
                        try
                        {
                            // Retrieve student asynchronously
                            var student = await _db.Students.FirstOrDefaultAsync(s => s.StudentId == mapping.StudentId);
                            if (student == null)
                            {
                                // Log student not found
                                _logger.LogWarning($"Student with ID {mapping.StudentId} not found.");
                                continue;
                            }

                            // Retrieve committee asynchronously
                            var committee = await _db.Committees.FirstOrDefaultAsync(c => (c.Firstname + " " + c.Lastname) == mapping.CommitteeName);
                            if (committee == null)
                            {
                                // Log committee not found
                                _logger.LogWarning($"Committee with name {mapping.CommitteeName} not found.");
                                continue;
                            }

                            // Check if student has shortpaper
                            var shortpaperId = _db.Shortpapers.FirstOrDefault(c => c.StudentId == mapping.StudentId);
                            if (shortpaperId != null)
                            {
                                var shortpaperHasCommittee = new ShortpapersHasCommittee
                                {
                                    ShortpaperId = shortpaperId.ShortpaperId,
                                    CommitteeId = committee.CommitteeId // Use CommitteeId here
                                };

                                _db.ShortpapersHasCommittees.Add(shortpaperHasCommittee);
                            }
                            else
                            {
                                // Log or handle the case where the student has no shortpaper
                                _logger.LogWarning($"Student with ID {mapping.StudentId} has no shortpaper.");
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the exception
                            _logger.LogError($"An error occurred while processing student with ID {mapping.StudentId} and committee with name {mapping.CommitteeName}: {ex.Message}");
                        }
                    }

                    await _db.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Data = studentCommitteeMappings.Select(mapping => new AddCommitteeForStudentDTO
                    {
                        StudentId = mapping.StudentId,
                        CommitteeName = mapping.CommitteeName // Change CommitteeId to CommitteeName
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "An unexpected error occurred while adding committees for students from CSV.";
                // Log the exception
                _logger.LogError($"An unexpected error occurred while processing CSV file: {ex.Message}");
            }

            return response;
        }


    }
}
