using Healthcare.Dto;
using Healthcare.Exceptions;
using Healthcare.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Healthcare.Controllers;

[ApiController]
public class DoctorController : Controller
{
    private readonly IDoctorRepository _doctor;

    public DoctorController(IDoctorRepository doctor)
    {
        _doctor = doctor;
    }

    // Get: /doctors
    [HttpGet("doctors")]
    public async Task<IActionResult> GetAllDoctors()
    {
        var doctors = await _doctor.GetAll();
        return ApiResponse("Success", doctors);
    }

    // Get: /doctors/{id}/availability
    [HttpGet("doctors/{id}/availability")]
    public async Task<IActionResult> GetAvailableDoctorById(
        int id,
        DateTime from,
        DateTime to,
        int slot
    )
    {
        return ApiResponse("Success");
    }

    // Get: /doctor
    [HttpGet("doctor")]
    public async Task<IActionResult> GetDoctorById(int id)
    {
        var doctor = await _doctor.GetById(id);
        if (doctor == null) throw new NotFoundException("Doctor not found");

        return ApiResponse("Success", doctor);
    }

    // Get: /migrate
    [HttpGet("migrate")]
    public async Task<IActionResult> Migrate()
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var doctors = await _doctor.CreateBatch();
        return ApiResponse("Migration successfully.", doctors);
    }
}