using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Healthcare.Enums;

namespace Healthcare.Requests;

public class GetAvailableDoctorRequest
{
    [Required(ErrorMessage = "Day is required")]
    [RegularExpression(@"^(SU|MO|TU|WE|TH|FR|SA)$", 
        ErrorMessage = "Day must be SU, MO, TU, WE, TH, FR, or SA")]
    public string Day { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start time is required")]
    [RegularExpression(@"^([01][0-9]|2[0-3]):[0-5][0-9]$")]
    public string From { get; set; } = string.Empty;

    [Required(ErrorMessage = "End time is required")]
    [RegularExpression(@"^([01][0-9]|2[0-3]):[0-5][0-9]$")]
     [CustomValidation(typeof(GetAvailableDoctorRequest), nameof(ValidateTimeRange))]
    public string To { get; set; } = string.Empty;

    [RegularExpression(@"^(15|30|60)$", 
        ErrorMessage = "Day must be 15, 30, or 60)")]
    public int Slot { get; set; } = 30;

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
    public TimeOnly FromTime 
    {
        get
        {
            if (string.IsNullOrEmpty(From))
                return default; // atau throw exception
                
            return TimeOnly.ParseExact(From, "HH:mm");
        }
    }

    [JsonIgnore]
    public TimeOnly ToTime 
    {
        get
        {
            if (string.IsNullOrEmpty(To))
                return default;
                
            return TimeOnly.ParseExact(To, "HH:mm");
        }
    }

    public static ValidationResult ValidateTimeRange(string to, ValidationContext context)
    {
        var instance = (GetAvailableDoctorRequest)context.ObjectInstance;
        // Gunakan format yang benar
        string[] allowedFormats = { "HH:mm", "h:mm tt", "h:mm", "H:mm" };
        
        if (!TimeOnly.TryParseExact(instance.From, allowedFormats, out var start) ||
            !TimeOnly.TryParseExact(to, allowedFormats, out var end))
        {
            return ValidationResult.Success;
        }
        
        if (end <= start)
        {
            return new ValidationResult(
                $"End time ({end:h:mm tt}) must be after start time ({start:h:mm tt})");
        }
        
        return ValidationResult.Success;

    }
}