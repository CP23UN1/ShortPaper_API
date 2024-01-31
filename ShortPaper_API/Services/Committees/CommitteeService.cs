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

        public CommitteeService(ShortpaperDbContext db)
        {
            _db = db;
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

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
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
                // AddCommitteesForStudentsFromCsvAsync method

                using (var reader = new StreamReader(csvFile.OpenReadStream()))
                {
                    var studentCommitteeMappings = new List<(string StudentId, int CommitteeId)>();

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        var values = line.Split(','); // Consider supporting different delimiters

                        if (values.Length >= 2) // Assuming CSV format: StudentId,CommitteeId
                        {
                            var studentId = values[0].Trim();
                            var committeeIdStr = values[1].Trim();

                            if (int.TryParse(committeeIdStr, out int committeeId))
                            {
                                studentCommitteeMappings.Add((studentId, committeeId));
                            }
                        }
                    }

                    foreach (var mapping in studentCommitteeMappings)
                    {
                        var student = await _db.Students.FirstOrDefaultAsync(s => s.StudentId == mapping.StudentId);
                        var committee = await _db.Committees.FirstOrDefaultAsync(c => c.CommitteeId == mapping.CommitteeId);

                        if (student == null || committee == null)
                        {
                            // Log or handle the case where the student or committee ID is not found
                            continue;
                        }

                        try
                        {
                            var shortpaperId = student.Shortpapers.FirstOrDefault()?.ShortpaperId;
                            if (shortpaperId != null)
                            {
                                var shortpaperHasCommittee = new ShortpapersHasCommittee
                                {
                                    ShortpaperId = shortpaperId.Value,
                                    CommitteeId = committee.CommitteeId
                                };

                                _db.ShortpapersHasCommittees.Add(shortpaperHasCommittee);
                            }
                            else
                            {
                                // Log or handle the case where the student has no shortpaper
                            }
                        }
                        catch (Exception ex)
                        {
                            // Log the exception or handle it as needed
                        }
                    }

                    await _db.SaveChangesAsync();

                    response.IsSuccess = true;
                    response.Data = studentCommitteeMappings.Select(mapping => new AddCommitteeForStudentDTO
                    {
                        StudentId = mapping.StudentId,
                        CommitteeId = mapping.CommitteeId
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = "An unexpected error occurred while adding committees for students from CSV.";
                // Log the exception or handle it as needed
            }

            return response;

        }
    }
}
