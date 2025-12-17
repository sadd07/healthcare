using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Healthcare.Controllers;

public class Controller : ControllerBase
{
    protected IActionResult ApiResponse(string message)
    {
        return Ok(new
        {
            code = 0,
            message, 
        });
    }

    protected IActionResult ApiResponse<T>(string message, T? data)
    {
        return Ok(new
        {
            code = 0,
            message, 
            data, 
        });
    }
}