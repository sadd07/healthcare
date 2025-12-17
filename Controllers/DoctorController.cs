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

    // Get: /doctor
    [HttpGet("doctor")]
    public async Task<IActionResult> GetDoctorById(int id)
    {
        var doctor = await _doctor.GetById(id);
        if (doctor == null) throw new NotFoundException

        return ApiResponse("Success", doctor);
    }
}