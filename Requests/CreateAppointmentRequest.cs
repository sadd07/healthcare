using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Healthcare.Enums;

namespace Healthcare.Requests;

public class CreateAppointmentRequest
{
    [Required(ErrorMessage = "Doctor Id is required")]
    public int DoctorId { get; set; } = 30;
    
    
    [Required(ErrorMessage = "Patient Id is required")]
    public int PatientId { get; set; } = 30;

    [Required(ErrorMessage = "Day is required")]
    [RegularExpression(@"^(SU|MO|TU|WE|TH|FR|SA)$", 
        ErrorMessage = "Day must be SU, MO, TU, WE, TH, FR, or SA")]
    public string Day { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start time is required")]
    [RegularExpression(@"^([01][0-9]|2[0-3]):[0-5][0-9]$")]
    public string Start { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Duration is required")]
    public int Duration { get; set; } = 30;

    [JsonIgnore]
    public Days DayOfWeek => Day.ToUpper() switch
    {
        "SU" => Days.Sunday,
        "MO" => Days.Monday,
        "TU" => Days.Tuesday,
        "WE" => Days.Wednesday,
        "TH" => Days.Thursday,
        "FR" => Days.Friday,
        "SA" => Days.Saturday,
        _ => throw new ArgumentException("Day must be SU, MO, TU, WE, TH, FR, or SA")
    };
    
    [JsonIgnore]
    public TimeOnly StartTime
    {
        get
        {
            if (string.IsNullOrEmpty(Start))
                return default; 
                
            return TimeOnly.ParseExact(Start, "HH:mm");
        }
    }
}