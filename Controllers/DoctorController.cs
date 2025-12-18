using Healthcare.Dto;
using Healthcare.Exceptions;
using Healthcare.Interfaces.Repositories;
using Healthcare.Interfaces.Services;
using Healthcare.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Healthcare.Controllers;

[ApiController]
public class DoctorController : Controller
{
    private readonly IDoctorService _doctor;
    private readonly IScheduleRepository _schedule;

    public DoctorController(
        IDoctorService doctor,
        IScheduleRepository schedule
    )
    {
        _doctor = doctor;
        _schedule = schedule;
    }

    // Get: /doctors
    [HttpGet("doctors")]
    public async Task<IActionResult> GetAllDoctors()
    {
        var doctors = await _doctor.GetAllDoctors();
        return ApiResponse("Success", doctors);
    }

    // Get: /doctors/{id}/availability
    [HttpGet("doctors/{id}/availability")]
    public async Task<IActionResult> GetAvailableDoctorById(
        int id,
        [FromQuery] GetAvailableDoctorRequest request)
    {
        var doctor = await _doctor.GetDoctorScheduleSlots(new GetDoctorScheduleSlotsDto
        {
            Id = id,
            Day = request.DayOfWeek,
            From = request.FromTime,
            To = request.ToTime,
            Slot = request.Slot,
        });
        if (doctor == null) throw new NotFoundException("Doctor not found");

        return ApiResponse("Success", doctor);
    }

    // Get: /doctor
    [HttpGet("doctor")]
    public async Task<IActionResult> GetDoctorById(int id)
    {
        var doctor = await _doctor.GetDoctorById(id);
        if (doctor == null) throw new NotFoundException("Doctor not found");

        return ApiResponse("Success", doctor);
    }

    // Get: /migrate
    [HttpGet("migrate")]
    public async Task<IActionResult> Migrate()
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        // var doctors = await _doctor.CreateBatch();
        var schedules = await _schedule.CreateBatch();
        return ApiResponse("Migration successfully.", schedules);
    }
}