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

                if(committees.Count == 0)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = "No committees found.";
                    response.httpStatusCode = StatusCodes.Status404NotFound;

                }
                else 
                {
                    response.IsSuccess = true;
                    response.Data = committees;
                }
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
                    var studentCommitteeMappings = new List<(string StudentId, string CommitteeName, bool IsAdvisor, bool IsPrincipal, bool IsCommittee)>();
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

                        if (values.Length >= 5) // Assuming IsAdvisor, IsPrincipal, IsCommittee are boolean values in the CSV
                        {
                            var studentId = values[0].Trim();
                            var committeeName = values[1].Trim();
                            var isAdvisor = bool.Parse(values[2].Trim());
                            var isPrincipal = bool.Parse(values[3].Trim());
                            var isCommittee = bool.Parse(values[4].Trim());

                            studentCommitteeMappings.Add((studentId, committeeName, isAdvisor, isPrincipal, isCommittee));
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
                                    CommitteeId = committee.CommitteeId,
                                    IsAdvisor = mapping.IsAdvisor ? 1u : 0u,
                                    IsPrincipal = mapping.IsPrincipal ? 1u : 0u,
                                    IsCommittee = mapping.IsCommittee ? 1u : 0u
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
                        CommitteeName = mapping.CommitteeName,
                        IsAdvisor = mapping.IsAdvisor ? 1u : 0u,
                        IsPrincipal = mapping.IsPrincipal ? 1u : 0u,
                        IsCommittee = mapping.IsCommittee ? 1u : 0u
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

        public async Task<ServiceResponse<List<CommitteeRoleDTO>>> UpdateCommitteeRolesForStudentAsync(string studentId, List<CommitteeRoleDTO> committeeRoles)
        {
            var response = new ServiceResponse<List<CommitteeRoleDTO>>();

            try
            {
                var updatedCommitteeRoles = new List<CommitteeRoleDTO>();
                // Retrieve the student
                var student = await _db.Students.FirstOrDefaultAsync(s => s.StudentId == studentId);
                if (student == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = $"Student with ID {studentId} not found.";
                    return response;
                }

                // Retrieve the shortpaper for the student
                var shortpaper = await _db.Shortpapers.FirstOrDefaultAsync(sp => sp.StudentId == studentId);
                if (shortpaper == null)
                {
                    response.IsSuccess = false;
                    response.ErrorMessage = $"Shortpaper not found for student with ID {studentId}.";
                    return response;
                }

                foreach (var role in committeeRoles)
                {
                    var committeeName = role.CommitteeName;
                    var isAdvisor = role.IsAdvisor;
                    var isPrincipal = role.IsPrincipal;
                    var isCommittee = role.IsCommittee;
                    updatedCommitteeRoles.Add(role);

                    // Validate committee name
                    if (string.IsNullOrEmpty(committeeName))
                    {
                        response.IsSuccess = false;
                        response.ErrorMessage = "Committee name is empty or null.";
                        return response;
                    }

                    // Retrieve the committee
                    var committee = await _db.Committees.FirstOrDefaultAsync(c => (c.Firstname + " " + c.Lastname) == committeeName);
                    if (committee == null)
                    {
                        response.IsSuccess = false;
                        response.ErrorMessage = $"Committee with name '{committeeName}' not found.";
                        return response;
                    }

                    // Check if the student already has a role in the specified committee
                    var existingRole = await _db.ShortpapersHasCommittees.FirstOrDefaultAsync(s => s.ShortpaperId == shortpaper.ShortpaperId && s.CommitteeId == committee.CommitteeId);
                    if (existingRole != null)
                    {
                        // Check if the new roles conflict with existing roles
                        if ((isAdvisor && existingRole.IsAdvisor == 1) ||
                            (isPrincipal && existingRole.IsPrincipal == 1) ||
                            (isCommittee && existingRole.IsCommittee == 1))
                        {
                            response.IsSuccess = false;
                            response.ErrorMessage = "The specified role conflicts with the existing role for this committee.";
                            return response;
                        }

                        // Update the roles
                        existingRole.IsAdvisor = isAdvisor ? 1u : 0u;
                        existingRole.IsPrincipal = isPrincipal ? 1u : 0u;
                        existingRole.IsCommittee = isCommittee ? 1u : 0u;
                    }
                    else
                    {
                        // Create a new role entry
                        var newRole = new ShortpapersHasCommittee
                        {
                            ShortpaperId = shortpaper.ShortpaperId,
                            CommitteeId = committee.CommitteeId,
                            IsAdvisor = isAdvisor ? 1u : 0u,
                            IsPrincipal = isPrincipal ? 1u : 0u,
                            IsCommittee = isCommittee ? 1u : 0u
                        };
                        _db.ShortpapersHasCommittees.Add(newRole);
                    }
                }

                await _db.SaveChangesAsync();

                response.IsSuccess = true;
                response.Data = updatedCommitteeRoles;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.ErrorMessage = $"An unexpected error occurred while updating committee roles for student {studentId}: {ex.Message}";
                // Log the exception
                _logger.LogError(response.ErrorMessage);
            }

            return response;
        }
    }
}
