using Healthcare.Dto;
using Healthcare.Exceptions;
using Healthcare.Interfaces.Services;
using Healthcare.Requests;
using Microsoft.AspNetCore.Mvc;

namespace Healthcare.Controllers;

[ApiController]
public class DoctorController : Controller
{
    private readonly IDoctorService _doctor;
    private readonly IAppointmentService _appointment;

    public DoctorController(
        IDoctorService doctor,
        IAppointmentService appointment
    )
    {
        _doctor = doctor;
        _appointment = appointment;
    }

    // Get: /doctors
    [HttpGet("doctors")]
    public async Task<IActionResult> GetAllDoctors()
    {
        var doctors = await _doctor.GetAllDoctors();
        return ApiResponse("Success", doctors);
    }

    // Get: /patients
    [HttpGet("patients")]
    public async Task<IActionResult> GetAllPatients()
    {
        var patients = await _appointment.GetAllPatients();
        return ApiResponse("Success", patients);
    }

    // Get: /appointments
    [HttpGet("appointments")]
    public async Task<IActionResult> GetAllAppointments()
    {
        var appointments = await _appointment.GetAllAppointments();
        return ApiResponse("Success", appointments);
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

    // Post: /appointments
    [HttpPost("appointments")]
    public async Task<IActionResult> CreateAppointment(CreateAppointmentRequest request)
    {
        var result = await _appointment.CreateAppointment(new CreateAppointmentDto
        {
            DoctorId = request.DoctorId,
            PatientId = request.PatientId,
            Day = request.DayOfWeek,
            Start = request.StartTime,
            Duration = request.Duration,
        });
        if (result.Code != 0)
        {
            if (result.Code == 1) throw new BadRequestException(result.Message);
            throw new ConflictException(result.Message);
        }
        
        throw new  Created(result.Message);
    }

    // Delete: /appointments/{id}
    [HttpDelete("appointments/{id}")]
    public async Task<IActionResult> DeleteAppointment(int id)
    {
        var result = await _appointment.DeleteAppointment(id);
        if (!result) throw new BadHttpRequestException("Delete appointment failed.");

        return ApiResponse("Delete appointment successfully.");
    }

    // Post: /seeds
    [HttpPost("seeds")]
    public async Task<IActionResult> Seeds()
    {
        var result = await _doctor.Seeds();
        return ApiResponse("Seeding data successfully.", result);
    }
}